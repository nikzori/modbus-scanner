using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace Gidrolock_Modbus_Scanner
{
    public partial class Datasheet : Form
    {
        bool isPolling = false;
        bool isAwaitingResponse = false;
        ModbusResponseEventArgs latestMessage = null;

        int timeout = 3000;
        int dbc; //data byte count
        byte slaveID;
        Device device = App.device;
        List<Entry> entries;
        List<EntryRow> entryRows;
        int activeEntryIndex;   // entry index for modbus requests/responses
        int activeRowIndex;     // index for actual rows on the panel; separate indexes needed for group requests
        SerialPort port = Modbus.port;
        Thread poll;
        Thread timer;
        Stopwatch stopwatch = new Stopwatch();
        bool closed = false;
        public Datasheet(byte slaveID)
        {
            Modbus.ResponseReceived += PublishResponse;
            this.slaveID = slaveID;
            entries = device.entries;
            Console.WriteLine("Initializing datasheet");
            InitializeComponent();

            Label_DeviceName.Text = device.name;
            Label_Description.Text = device.description;



            int rowCount = 0;

            entryRows = new List<EntryRow>();
            foreach (Entry e in entries)
            {
                if (e.length > 1)
                {
                    if ((e.length == 2 && e.dataType == "uint32") || e.dataType == "string") // this is a single multi-register entry
                    {
                        entryRows.Add(new EntryRow(rowCount, e, e.name) { Width = flowLayoutPanel1.Width - 10, Height = 20 });
                        rowCount++;
                    }
                    else // this is a collection of registers
                    {
                        for (int i = 0; i < e.length; i++)
                        {
                            if (i < e.labels.Count)
                                entryRows.Add(new EntryRow(rowCount, e, e.labels[i], (i == 0 ? true : false)) { Width = flowLayoutPanel1.Width - 10, Height = 20 });
                            else entryRows.Add(new EntryRow(rowCount, e, (e.address + i).ToString(), (i == 0 ? true : false)) { Width = flowLayoutPanel1.Width - 10, Height = 20 });

                            rowCount++;
                        }
                    }

                }
                else
                {
                    entryRows.Add(new EntryRow(rowCount, e, e.name) { Width = flowLayoutPanel1.Width - 10, Height = 20 });
                    rowCount++;
                }
            }


            for (int i = 0; i < entryRows.Count; i++)
            { flowLayoutPanel1.Controls.Add(entryRows[i]); }

            this.Update();
            FormClosing += (s, e) => { closed = true; };
            poll = new Thread(new ThreadStart(delegate { AutoPollAsync(); }));
            Task.Run(() => AutoPollAsync());
        }

        public void AutoPollAsync()
        {
            if (!port.IsOpen)
                port.Open();
            port.ReadTimeout = timeout;
            int retryAttempts = 0;
            while (!closed)
            {
                if (isPolling)
                {
                    if (entryRows[activeRowIndex].chboxPanel is null)
                    {
                        activeRowIndex++;
                        continue;
                    }
                    if (entryRows[activeRowIndex].chboxPanel.chbox.Checked)
                    {
                        //Console.WriteLine("Polling for " + device.entries[activeEntryIndex].name);
                        Entry entry = entries[activeEntryIndex];
                        isAwaitingResponse = true;
                        Modbus.ReadRegisters(port, slaveID, (FunctionCode)entry.registerType, entry.address, entry.length);

                        stopwatch.Restart();
                        while (isAwaitingResponse)
                        {
                            if (stopwatch.ElapsedMilliseconds > 1000)
                            {
                                if (retryAttempts > 3)
                                {
                                    break;
                                }
                                Console.WriteLine("Response timed out.");
                                retryAttempts++;
                                Modbus.ReadRegisters(port, slaveID, (FunctionCode)entry.registerType, entry.address, entry.length);
                                stopwatch.Restart();
                            }
                        }
                        if (isAwaitingResponse)
                        {
                            MessageBox.Show("Устройство не отвечает на сообщения. Проверьте соединение с устройством.");
                            isPolling = false;
                            continue;
                        }
                        try
                        {
                            if (entries[activeEntryIndex].readOnce)
                            {
                                entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { entryRows[activeRowIndex].chboxPanel.chbox.Checked = false; }));
                            }
                            dbc = latestMessage.Message[2]; // data byte count
                            if (entries[activeEntryIndex].labels is null || entries[activeEntryIndex].labels.Count == 0)
                                ParseSingle(); // assume that no labels = 1 entry
                            else ParseGroup();

                            if (activeRowIndex >= entryRows.Count)
                                activeRowIndex = 0;

                            //MessageBox.Show("Получен ответ от устройства: " + dataCleaned, "Успех", MessageBoxButtons.OK);
                            port.DiscardInBuffer();
                        }
                        catch (Exception err) { MessageBox.Show(err.Message, "Publish response error"); }
                    }
                    if (activeRowIndex >= entryRows.Count)
                        activeRowIndex = 0;

                    else //need to skip multiple dgv entries without accidentaly skipping entries
                    {
                        if (device.entries[activeEntryIndex].labels is null || device.entries[activeEntryIndex].labels.Count == 0)
                            activeRowIndex++;
                        else
                        {
                            for (int i = 0; i < device.entries[activeEntryIndex].labels.Count; i++)
                            {
                                activeRowIndex++;
                            }
                        }
                    }
                    activeEntryIndex++;
                    if (activeEntryIndex >= device.entries.Count)
                        activeEntryIndex = 0;
                    if (activeRowIndex >= entryRows.Count)
                        activeRowIndex = 0;

                    Thread.Sleep(50);
                }
                else
                {
                    retryAttempts = 0; // reset retry counter
                }
            }

        }

        void ParseSingle()
        {
            switch (entries[activeEntryIndex].dataType)
            {
                case "bool":
                    //no valueParse keys
                    if (entries[activeEntryIndex].valueParse is null || entries[activeEntryIndex].valueParse.Keys.Count == 0)
                        entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { entryRows[activeRowIndex].SetValue(latestMessage.Data[0] > 0x00 ? "True" : "False"); }));

                    else try
                        {
                            entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { entryRows[activeRowIndex].SetValue(latestMessage.Data[0] > 0x00 ? entries[activeEntryIndex].valueParse["true"] : entries[activeEntryIndex].valueParse["false"]); }));
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                            entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { entryRows[activeRowIndex].SetValue(latestMessage.Data[0] > 0x00 ? "True" : "False"); }));
                        }
                break;
                case ("uint16"):
                    ushort value = BitConverter.ToUInt16(latestMessage.Data, 0);
                    if (entries[activeEntryIndex].labels is null || entries[activeEntryIndex].labels.Count == 0) // single value
                    {
                        if (entries[activeEntryIndex].valueParse is null || entries[activeEntryIndex].valueParse.Keys.Count == 0)
                        {
                            //Array.Reverse(e.Data); // this was necessary, but something changed, idk
                            //Console.WriteLine("ushort parsed value: " + value);
                            entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { entryRows[activeRowIndex].SetValue(value.ToString()); }));
                        }
                        else
                        {
                            try
                            {
                                entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { entryRows[activeRowIndex].SetValue(entries[activeEntryIndex].valueParse[value.ToString()]); }));
                            }
                            catch (Exception err)
                            {
                                entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { entryRows[activeRowIndex].SetValue(value.ToString()); }));
                                MessageBox.Show("Error parsing uint value at address: " + entries[activeEntryIndex].address + "; " + err.Message, "uint16 parse");
                            }
                        }
                    }
                break;

                case ("uint32"):
                    Array.Reverse(latestMessage.Data);
                    entryRows[activeRowIndex].SetValue(BitConverter.ToUInt32(latestMessage.Data, 0).ToString());
                    activeRowIndex++;
                break;

                case ("string"):
                    List<byte> bytes = new List<byte>();
                    for (int i = 0; i < latestMessage.Data.Length; i++)
                    {
                        if (latestMessage.Data[i] != 0)
                            bytes.Add(latestMessage.Data[i]);
                    }
                    bytes.Reverse();
                    string str = System.Text.Encoding.UTF8.GetString(bytes.ToArray());
                    entryRows[activeRowIndex].Invoke(new MethodInvoker(delegate { entryRows[activeRowIndex].SetValue(str); }));
                    activeRowIndex++;
                break;
                default:
                    MessageBox.Show("Wrong data type set for entry " + entries[activeEntryIndex].name);
                    activeRowIndex++;
                break;
            }
            activeRowIndex++;
        }
        void ParseGroup()
        {
            switch (entries[activeEntryIndex].dataType)
            {
                case "bool":
                    List<bool> values = new List<bool>();
                    for (int i = 0; i < dbc; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            bool res = (((latestMessage.Data[i] >> j) & 0x01) >= 1) ? true : false;
                            values.Add(res);
                        }
                    }
                    for (int i = 0; i < entries[activeEntryIndex].labels.Count; i++)
                    {
                        if (entries[activeEntryIndex].registerType == RegisterType.Coil)
                            entryRows[activeRowIndex].BeginInvoke(new MethodInvoker(delegate { entryRows[activeRowIndex].SetValue(latestMessage.Data[0] > 0x00 ? "True" : "False"); }));
                        else entryRows[activeRowIndex].BeginInvoke(new MethodInvoker(delegate
                        {
                            entryRows[activeRowIndex].SetValue(latestMessage.Data[0] > 0x00 ? "True" : "False");
                        }));
                        activeRowIndex++;
                    }
                break;
                case "uint16":
                    try
                    {
                        List<ushort> _values = new List<ushort>();
                        for (int i = 0; i < dbc; i += 2)
                        {
                            ushort s = BitConverter.ToUInt16(latestMessage.Data, i);
                            _values.Add(s);
                        }
                        //Console.WriteLine("ushort values count: " + values.Count);
                        //Console.WriteLine("entity labels count: " + entries[activeEntryIndex].labels.Count);
                        for (int i = 0; i < entries[activeEntryIndex].labels.Count; i++)
                        {
                            if (device.entries[activeEntryIndex].valueParse != null)
                            {
                                entryRows[activeRowIndex].BeginInvoke(new MethodInvoker(delegate
                                {
                                    entryRows[activeRowIndex].SetValue(_values[i].ToString());
                                }));
                            }
                            else
                            {
                                entryRows[activeRowIndex].BeginInvoke(new MethodInvoker(delegate
                                {
                                    entryRows[activeRowIndex].SetValue(_values[i].ToString());
                                }));
                            }
                            activeRowIndex++;
                        }
                    }
                    catch (Exception err)
                    {
                        entryRows[activeRowIndex].BeginInvoke(new MethodInvoker(delegate { entryRows[activeRowIndex].SetValue("Error"); }));
                        MessageBox.Show("Error parsing uint value at address: " + entries[activeEntryIndex].address + "; " + err.Message, "uint16 group req parse");
                    }
                break;
                default:
                    MessageBox.Show("Wrong data type set for entry " + entries[activeEntryIndex].name);
                    activeRowIndex++;
                break;
            }

        }

        void PublishResponse(object sender, ModbusResponseEventArgs e)
        {
            latestMessage = e;
            isAwaitingResponse = false;
        }

        private void Button_StartStop_Click(object sender, EventArgs e)
        {
            isPolling = !isPolling;
            Button_StartStop.Text = (isPolling ? "Стоп" : "Старт");
        }


        public class EntryRow : Panel
        {
            public CheckboxPanel chboxPanel;
            public Label numberLabel = new Label() { Height = 20, Width = 30, TextAlign = ContentAlignment.MiddleLeft };
            public Label addressLabel = new Label() { Height = 20, Width = 60, TextAlign = ContentAlignment.MiddleLeft };
            public Label nameLabel = new Label() { Height = 20, Width = 220, TextAlign = ContentAlignment.MiddleLeft };
            public Control valueControl = null;

            public void SetValue(string value)
            {
                if (valueControl is null)
                    return;

                if (valueControl is ComboBox)
                {
                    Console.WriteLine("Trying to change cbox value;");
                    ComboBox cb = valueControl as ComboBox;
                    if (value == "True")
                        cb.SelectedIndex = 1;
                    else cb.SelectedIndex = 0;
                }
                else valueControl.Text = value;

                this.Update();
            }

            public EntryRow(int number, Entry entry, string name, bool hasCheckbox = true)
            {
                if (number % 2 == 0)
                    this.BackColor = Color.White;
                else this.BackColor = Color.LightGray;

                numberLabel.Parent = this;
                addressLabel.Parent = this;
                nameLabel.Parent = this;

                numberLabel.Location = new Point(20, Top);
                addressLabel.Location = new Point(50, Top);
                nameLabel.Location = new Point(110, Top);

                numberLabel.Text = number.ToString();
                addressLabel.Text = entry.address.ToString();
                nameLabel.Text = name.ToString();
                if (hasCheckbox)
                {
                    chboxPanel = new CheckboxPanel() { Width = 20, Height = 20, Margin = Padding.Empty };
                    chboxPanel.Parent = this;
                    chboxPanel.Location = new Point(Left, Top);
                }


                if (entry.registerType == RegisterType.Holding)
                {
                    valueControl = new TextBox() { Height = 20, Width = 220 };
                }
                else if (entry.registerType == RegisterType.Coil)
                {
                    valueControl = new ComboBox() { Height = 20, Width = 220 };
                    ComboBox cb = valueControl as ComboBox;

                    cb.Items.Add("False");
                    cb.Items.Add("True");
                    //cb.SelectedIndex = 0; // empty value looks better
                    cb.SelectedIndexChanged += delegate { Console.WriteLine("Selected index changed;"); };
                }
                else
                {
                    valueControl = new Label() { Height = 20, Width = 220, TextAlign = ContentAlignment.MiddleLeft };
                }
                valueControl.Parent = this;
                valueControl.Location = new Point(330, Top);


            }
        }
    }

    public class CheckboxPanel : Panel
    {
        public CheckBox chbox;
        public CheckboxPanel() : base()
        {
            chbox = new CheckBox() { Parent = this, Location = new Point(Left, Top) };
        }
        public void SetVisibility(bool value)
        {
            chbox.Visible = value;
        }
    }
}
