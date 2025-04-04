using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace Gidrolock_Modbus_Scanner
{
    public partial class Datasheet : Form
    {
        public static bool isPolling = false;
        static bool isAwaitingResponse = false;
        static ModbusResponseEventArgs latestMessage = null;

        int timeout = 3000;
        static int dbc; //data byte count
        static byte slaveID;
        Device device = App.device;
        static List<Entry> entries;
        static List<EntryRow> entryRows;
        static int activeEntryIndex;   // entry index for modbus requests/responses
        static int activeRowIndex;     // index for actual rows on the panel; separate indexes needed for group requests
        static SerialPort port = Modbus.port;
        static Stopwatch stopwatch = new Stopwatch();
        bool closed = false;
        public Datasheet(byte slaveID, int baudrateIndex)
        {
            Modbus.ResponseReceived += PublishResponse;
            Datasheet.slaveID = slaveID;
            entries = device.entries;
            Console.WriteLine("Initializing datasheet");
            InitializeComponent();

            Label_DeviceName.Text = device.name;
            Label_Description.Text = device.description;
            
            cbBaudrate.Items.Add("1200");
            cbBaudrate.Items.Add("2400");
            cbBaudrate.Items.Add("4800");
            cbBaudrate.Items.Add("9600");
            cbBaudrate.Items.Add("14400");
            cbBaudrate.Items.Add("19200");
            cbBaudrate.Items.Add("38400");
            cbBaudrate.Items.Add("57600");
            cbBaudrate.Items.Add("115200");
            cbBaudrate.SelectedIndex = baudrateIndex;

            udSlaveId.Value = slaveID;

            int rowCount = 0;

            entryRows = new List<EntryRow>();
            foreach (Entry e in entries)
            {
                if (e.length > 1)
                {
                    if ((e.length == 2 && e.dataType == "uint32") || e.dataType == "string") // this is a single multi-register entry
                    {
                        entryRows.Add(new EntryRow(rowCount, e, e.name) { Width = flowLayoutPanel1.Width - 25, Height = 25, Margin = Padding.Empty });
                        rowCount++;
                    }
                    else // this is a collection of registers
                    {
                        for (int i = 0; i < e.length; i++)
                        {
                            if (i < e.labels.Count)
                                entryRows.Add(new EntryRow(rowCount, e, e.name + ": " + e.labels[i], (i == 0 ? true : false)) { Width = flowLayoutPanel1.Width - 25, Height = 25, Margin = Padding.Empty });
                            else entryRows.Add(new EntryRow(rowCount, e, (e.address + i).ToString(), (i == 0 ? true : false)) { Width = flowLayoutPanel1.Width - 25, Height = 25, Margin = Padding.Empty });

                            rowCount++;
                        }
                    }

                }
                else
                {
                    entryRows.Add(new EntryRow(rowCount, e, e.name) { Width = flowLayoutPanel1.Width - 25, Height = 25, Margin = Padding.Empty });
                    rowCount++;
                }
            }


            for (int i = 0; i < entryRows.Count; i++)
            { flowLayoutPanel1.Controls.Add(entryRows[i]); }

            this.Update();
            FormClosing += (s, e) => { closed = true; };
            Task.Run(() => AutoPoll());
        }

        public void AutoPoll()
        {
            if (!port.IsOpen)
                port.Open();
            port.ReadTimeout = timeout;

            while (!closed)
            {
                if (isPolling)
                {
                    //Console.WriteLine("Poll tick");
                    if (activeEntryIndex >= device.entries.Count || activeRowIndex >= entryRows.Count)
                    {
                        //Console.WriteLine("Resetting indexes");
                        activeEntryIndex = 0;
                        activeRowIndex = 0;
                    }
                    //Console.WriteLine("Polling for entry index " + activeEntryIndex + "; row index " + activeRowIndex);
                    if (entryRows[activeRowIndex].checkbox is CheckBox)
                    {
                        CheckBox chbx = entryRows[activeRowIndex].checkbox as CheckBox;
                        if (chbx.Checked)
                        {
                            //Console.WriteLine("Polling for " + device.entries[activeEntryIndex].name);
                            Entry entry = entries[activeEntryIndex];
                            PollForEntry(entry);
                            if (entries[activeEntryIndex].readOnce)
                                entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { chbx.Checked = false; }));
                        }
                    }

                    activeEntryIndex++;
                    activeRowIndex++;
                    //Console.WriteLine("Next entry index: " + activeEntryIndex);
                    Thread.Sleep(50);
                }


            }

        }

        public static void PollForEntry(Entry entry)
        {
            int retryAttempts = 0;
            isAwaitingResponse = true;
            Modbus.ReadRegisters(port, slaveID, (FunctionCode)entry.registerType, entry.address, entry.length);

            stopwatch.Restart();
            while (isAwaitingResponse)
            {
                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    if (retryAttempts > 3)
                        break;

                    //Console.WriteLine("Response timed out.");
                    retryAttempts++;
                    Modbus.ReadRegisters(port, slaveID, (FunctionCode)entry.registerType, entry.address, entry.length);
                    stopwatch.Restart();
                }
            }
            if (isAwaitingResponse)
            {
                MessageBox.Show("Устройство не отвечает на сообщения. Проверьте соединение с устройством.");
                isPolling = false;
                isAwaitingResponse = false;
            }
            try
            {
                if (latestMessage != null)
                {
                    dbc = latestMessage.Message[2]; // data byte count
                    if (entries[activeEntryIndex].labels is null || entries[activeEntryIndex].labels.Count == 0)
                        ParseSingle(); // assume that no labels = 1 entry
                    else ParseGroup();
                }
            }
            catch (Exception err) { MessageBox.Show(err.Message, "Publish response error"); }
        }
        public static void PollForEntry(EntryRow entryRow)
        {
            Entry entry = entryRow.entry;
            int retryAttempts = 0;
            isAwaitingResponse = true;
            Modbus.ReadRegisters(port, slaveID, (FunctionCode)entry.registerType, entry.address, entry.length);

            stopwatch.Restart();
            while (isAwaitingResponse)
            {
                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    if (retryAttempts > 3)
                        break;

                    //Console.WriteLine("Response timed out.");
                    retryAttempts++;
                    Modbus.ReadRegisters(port, slaveID, (FunctionCode)entry.registerType, entry.address, entry.length);
                    stopwatch.Restart();
                }
            }
            if (isAwaitingResponse)
            {
                MessageBox.Show("Устройство не отвечает на сообщения. Проверьте соединение с устройством.");
                isPolling = false;
            }
            try
            {
                //dbc = latestMessage.Message[2]; // data byte count
                entryRow.Invoke(new MethodInvoker(delegate { entryRow.SetValue(latestMessage.Data); }));
            }
            catch (Exception err) { MessageBox.Show(err.Message, "Publish error on toggle"); }
        }

        static void ParseSingle()
        {
            entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { entryRows[activeRowIndex].SetValue(latestMessage.Data); }));
        }
        static void ParseGroup()
        {
            switch (entries[activeEntryIndex].dataType)
            {
                case "bool":
                    List<byte> values = new List<byte>();
                    for (int i = 0; i < dbc; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            byte res = (byte)((latestMessage.Data[i] >> j) & 0x01);
                            values.Add(res);
                        }
                    }

                    for (int i = 0; i < entries[activeEntryIndex].labels.Count; i++)
                    {
                        entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { entryRows[activeRowIndex].SetValue(new byte[] { values[i] }); }));
                        activeRowIndex++;
                    }

                    break;
                case "uint16":
                    try
                    {
                        List<byte[]> bytes = new List<byte[]>();
                        for (int i = 0; i < dbc; i += 2)
                        {
                            byte[] pair = new byte[2];
                            pair[0] = latestMessage.Data[i];
                            pair[1] = latestMessage.Data[i + 1];
                            bytes.Add(pair);
                        }
                        //Console.WriteLine("ushort values count: " + values.Count);
                        //Console.WriteLine("entity labels count: " + entries[activeEntryIndex].labels.Count);
                        for (int i = 0; i < entries[activeEntryIndex].labels.Count; i++)
                        {
                            entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { entryRows[activeRowIndex].SetValue(bytes[i]); }));
                            activeRowIndex++;
                        }
                    }
                    catch (Exception err)
                    {
                        entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { entryRows[activeRowIndex].valueControl.Text = "Error"; }));
                        MessageBox.Show("Error parsing uint value at address: " + entries[activeEntryIndex].address + "; " + err.Message, "uint16 group req parse");
                    }
                    break;
                default:
                    MessageBox.Show("Wrong data type set for entry " + entries[activeEntryIndex].name);
                    //activeRowIndex++;
                    break;
            }
            //don't need to activeRowIndex++ here, because the last iteration of the `for` loop does that instead
        }

        void PublishResponse(object sender, ModbusResponseEventArgs e)
        {
            latestMessage = e;
            isAwaitingResponse = false;
        }

        private void Button_StartStop_Click(object sender, EventArgs e)
        {
            isPolling = !isPolling;
            Button_StartStop.Text = (isPolling ? "Стоп" : "Автоопрос");
        }

        public static void SetPolling(bool value)
        {
            isPolling = value;
        }

        private async void buttonSetId_Click(object sender, EventArgs e)
        {
            ushort value = (byte)udSlaveId.Value;
            await Task.Run(() =>
            {
                bool _isPolling = isPolling;
                if (isPolling)
                {
                    SetPolling(false);
                    Thread.Sleep(150); // wait for polling to finish
                }

                isAwaitingResponse = true;
                int retries = 0;
                stopwatch.Restart();
                Modbus.WriteSingleAsync(Modbus.port, FunctionCode.WriteRegister, slaveID, 128, value);

                while (isAwaitingResponse)
                {
                    if (stopwatch.ElapsedMilliseconds > 1000)
                    {
                        if (retries > 3)
                            break;
                        Console.WriteLine("Response for slave ID change timed out. isAwaitingResponse = " + isAwaitingResponse.ToString());
                        retries++;
                        stopwatch.Restart();
                        Modbus.WriteSingleAsync(Modbus.port, FunctionCode.WriteRegister, slaveID, 128, value);
                    }
                }

                if (isAwaitingResponse)
                {
                    MessageBox.Show("Устройство не отвечает на сообщения. Проверьте соединение с устройством.");
                    return;
                }

                slaveID = (byte)udSlaveId.Value;
                isPolling = _isPolling;
            });
        }

        private async void buttonSetSpeed_Click(object sender, EventArgs e)
        {
            ushort brate = (ushort)App.BaudRate[cbBaudrate.SelectedIndex];
            await Task.Run(() =>
            {
                bool _isPolling = isPolling;
                if (isPolling)
                {
                    SetPolling(false);
                    Thread.Sleep(150); // wait for polling to finish
                }
                isAwaitingResponse = true;
                int retries = 0;
                
                Modbus.WriteSingleAsync(Modbus.port, FunctionCode.WriteRegister, slaveID, 110, brate);
                stopwatch.Restart();

                while (isAwaitingResponse)
                {
                    if (stopwatch.ElapsedMilliseconds > 1000)
                    {
                        if (retries > 2)
                            break;
                        Console.WriteLine("Response for baudrate change timed out.");
                        retries++;
                        Modbus.WriteSingleAsync(Modbus.port, FunctionCode.WriteRegister, slaveID, 110, brate);
                        stopwatch.Restart();
                    }
                }

                if (isAwaitingResponse)
                {
                    MessageBox.Show("Устройство не отвечает на сообщения. Проверьте соединение с устройством.");
                    return;
                }

                port.Close();
                port.BaudRate = brate * 100;
                port.Open();

                isPolling = _isPolling;
            });
        }
    }
    public class EntryRow : FlowLayoutPanel
    {
        public byte[] data;
        public string dataType;
        public Entry entry;

        public Control checkbox;
        public Label numberLabel = new Label() { Height = 25, Width = 30, TextAlign = ContentAlignment.MiddleLeft, Margin = Padding.Empty };
        public Label addressLabel = new Label() { Height = 25, Width = 60, TextAlign = ContentAlignment.MiddleLeft, Margin = Padding.Empty };
        public Label nameLabel = new Label() { Height = 25, Width = 220, TextAlign = ContentAlignment.MiddleLeft, Margin = Padding.Empty };
        public Control valueControl = null;
        public Button valueButton = null;

        public void SetValue(byte[] value)
        {
            Array.Reverse(value); // for some reason the incoming array gets reversed *specifically* when it gets here
                                  //Console.WriteLine("byte array for entry: " + Modbus.ByteArrayToString(value, false));
            data = value;

            if (valueControl is null)
                return;
            switch (dataType)
            {
                case "uint16":
                    valueControl.Text = ((value[0] << 8) + value[1]).ToString();
                    break;
                case "uint32":
                    valueControl.Text = ((value[0] << 24) + (value[1] << 16) + (value[2] << 8) + value[3]).ToString();
                    break;
                case "string":
                    valueControl.Text = App.ByteArrayToUnicode(value);
                    break;
                case "bool":
                    valueControl.Text = value[0] > 0x00 ? "true" : "false";
                    break;
                default:
                    MessageBox.Show("Неизвестный или неправильный тип данных в конфигурации для адреса " + addressLabel.Text);
                    break;
            }
            this.Update();
        }

        public EntryRow(int number, Entry entry, string name, bool hasCheckbox = true)
        {
            this.entry = entry;

            dataType = entry.dataType;

            this.Padding = new Padding(5, 0, 0, 0);
            this.FlowDirection = FlowDirection.LeftToRight;
            if (number % 2 == 0)
                this.BackColor = Color.White;
            else this.BackColor = Color.LightGray;

            if (hasCheckbox)
                checkbox = new CheckBox() { Width = 25, Height = 20, Margin = Padding.Empty };
            else checkbox = new Label() { Width = 25, Height = 20, Margin = Padding.Empty };
            checkbox.Parent = this;


            numberLabel.Parent = this;
            addressLabel.Parent = this;
            nameLabel.Parent = this;

            numberLabel.Text = number.ToString();
            addressLabel.Text = entry.address.ToString();
            nameLabel.Text = name.ToString();

            if (entry.registerType == RegisterType.Holding)
            {
                valueControl = new TextBox() { Height = 25, Width = 120, Margin = Padding.Empty };
                valueButton = new Button() { Height = 25, Width = 120, Margin = Padding.Empty, Text = "Задать" };
                ushort address = entry.address;
                valueButton.Click += async delegate
                {
                    TextBox tbox = valueControl as TextBox;
                    string valueLower = tbox.Text.ToLower();
                    short value = 0;
                    bool canWrite = false;
                    if (App.IsDec(valueLower))
                    {
                        try { value = Convert.ToInt16(valueLower); canWrite = true; }
                        catch (Exception err) { MessageBox.Show(err.Message); }
                    }
                    else if (App.IsHex(valueLower))
                    {
                        Console.WriteLine("Got hex value");
                        for (int i = 0; i < valueLower.Length; i++)
                        {
                            if (valueLower[i] == 'x')
                            {
                                valueLower = valueLower.Remove(i, 1);
                                break;
                            }
                        }
                        try { value = Convert.ToInt16(valueLower, 16); canWrite = true; }
                        catch (Exception err) { MessageBox.Show(err.Message); }
                    }
                    else if (App.IsBin(valueLower))
                    {
                        Console.WriteLine("Got bin value");
                        for (int i = 0; i < valueLower.Length; i++)
                        {
                            if (valueLower[i] == 'b')
                            {
                                valueLower = valueLower.Remove(i, 1);
                                break;
                            }
                        }
                        try { value = Convert.ToInt16(valueLower, 2); canWrite = true; }
                        catch (Exception err) { MessageBox.Show(err.Message); }
                    }
                    if (canWrite)
                    {
                        Console.WriteLine("Setting a holding register to " + tbox.Text);

                        bool isPolling = Datasheet.isPolling;
                        if (isPolling)
                        {
                            Datasheet.SetPolling(false);
                            await Task.Delay(150); // wait for polling to finish
                        }

                        Modbus.WriteSingleAsync(Modbus.port, FunctionCode.WriteRegister, Modbus.slaveID, address, (ushort)value);

                        await Task.Delay(250).ContinueWith(_ =>
                        {
                            Datasheet.PollForEntry(this);
                            if (isPolling)
                                Datasheet.SetPolling(true);
                        });
                    }
                };
            }
            else if (entry.registerType == RegisterType.Coil)
            {
                valueControl = new Label() { Height = 25, Width = 120, Margin = Padding.Empty, Text = "n/a" };
                valueButton = new Button() { Height = 25, Width = 120, Margin = Padding.Empty, Text = "Переключить" };
                valueButton.Click += async delegate
                {
                    Console.WriteLine("Toggling a coil");
                    bool isPolling = Datasheet.isPolling;
                    if (isPolling)
                    {
                        Datasheet.SetPolling(false);
                        await Task.Delay(150); // wait for polling to finish
                    }


                    if (data is null) // entry wasn't polled, just set it to `true`
                        Modbus.WriteSingleAsync(Modbus.port, FunctionCode.WriteCoil, Modbus.slaveID, entry.address, 0xFF00);
                    else
                    {
                        if (data[0] > 0x00)
                            Modbus.WriteSingleAsync(Modbus.port, FunctionCode.WriteCoil, Modbus.slaveID, entry.address, 0x0000);
                        else Modbus.WriteSingleAsync(Modbus.port, FunctionCode.WriteCoil, Modbus.slaveID, entry.address, 0xFF00);
                    }

                    await Task.Delay(250).ContinueWith(_ =>
                    {
                        Datasheet.PollForEntry(this);
                        if (isPolling)
                            Datasheet.SetPolling(true);
                    });
                };
            }
            else valueControl = new Label() { Height = 25, Width = 120, TextAlign = ContentAlignment.MiddleLeft, Margin = Padding.Empty, Text = "n/a" };

            valueControl.Parent = this;

            if (valueButton != null)
                valueButton.Parent = this;
        }
    }
}
