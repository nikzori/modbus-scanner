using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gidrolock_Modbus_Scanner
{
    public partial class Datasheet : Form
    {
        bool isPolling = false;
        bool isAwaitingResponse = false;
        byte[] message = new byte[255];

        int timeout = 3000;
        int pollDelay = 250; // delay between each entry poll, ms
        byte slaveID;
        Device device = App.device;
        List<Entry> entries;
        int activeEntryIndex; // entry index for modbus responses
        int activeDGVIndex; // index for DGV rows
        SerialPort port = Modbus.port;

        bool closed = false;
        public Datasheet(byte slaveID)
        {
            Modbus.ResponseReceived += PublishResponse;
            this.slaveID = slaveID;
            entries = device.entries;

            InitializeComponent();

            Label_DeviceName.Text = device.name;
            Label_Description.Text = device.description;

            DGV_Device.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DGV_Device.MultiSelect = false;
            DGV_Device.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            DGV_Device.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DGV_Device.Columns.Add("#", "#");
            DGV_Device.Columns[0].FillWeight = 20;
            DGV_Device.Columns.Add("Name", "Имя");
            DGV_Device.Columns[1].FillWeight = 40;
            DGV_Device.Columns.Add("Value", "Значение");
            DGV_Device.Columns[2].FillWeight = 60;
            DGV_Device.Columns.Add("Address", "Адрес");
            DGV_Device.Columns[3].FillWeight = 30;
            DGV_Device.Columns.Add(new DataGridViewCheckBoxColumn());
            DGV_Device.Columns[4].Name = "Опрос";
            DGV_Device.Columns[4].FillWeight = 20;
            DGV_Device.Columns[4].ValueType = typeof(bool);

            int rowCount = 0;

            foreach (Entry e in entries)
            {
                if (e.length > 1)
                {
                    // multi-register entry check
                    if ((e.length == 2 && e.dataType == "uint32") || e.dataType == "string")
                    {
                        DGV_Device.Rows.Add(rowCount, e.name, "", e.address);
                        rowCount++;
                    }
                    else
                    {
                        for (int i = 0; i < e.length; i++)
                        {
                            if (i < e.labels.Count)
                                DGV_Device.Rows.Add(rowCount, e.name + ": " + e.labels[i], "", e.address + i);
                            else DGV_Device.Rows.Add(rowCount, e.address + i, "", e.address + i);
                            rowCount++;
                        }
                    }

                }
                else
                {
                    DGV_Device.Rows.Add(rowCount, e.name, "", e.address);
                    rowCount++;
                }
            }
            foreach (DataGridViewRow row in DGV_Device.Rows)
            {
                row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            /*
            for (int i = 0; i < entries.Count; i++)
            {
                DGV_Device.Rows.Add(i.ToString(), entries[i].name, "", entries[i].address);
                DGV_Device.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            */
            foreach (DataGridViewColumn column in DGV_Device.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable; // disabling sorting for now

            FormClosing += (s, e) => { closed = true; };
            Task.Run(() => AutoPollAsync());
        }

        public async void AutoPollAsync()
        {
            if (!port.IsOpen)
                port.Open();
            port.ReadTimeout = timeout;
            try
            {
                while (!closed)
                {
                    if (isPolling)
                    {
                        // holy fuck DGV is awful
                        DataGridViewCheckBoxCell chbox = DGV_Device.Rows[activeDGVIndex].Cells[4] as DataGridViewCheckBoxCell;
                        if (Convert.ToBoolean(chbox.Value))
                        {
                            Console.WriteLine("Polling for " + device.entries[activeEntryIndex].name);
                            await PollForEntry(entries[activeEntryIndex]).ContinueWith(_ => Task.Delay(150));
                        }
                        else //need to skip multiple dgv entries without accidentaly skipping entries
                        {
                            if (device.entries[activeEntryIndex].labels is null || device.entries[activeEntryIndex].labels.Count == 0)
                                activeDGVIndex++;
                            else
                            {
                                for (int i = 0; i < device.entries[activeEntryIndex].labels.Count; i++)
                                {
                                    activeDGVIndex++;
                                }
                            }
                        }

                        activeEntryIndex++;
                        if (activeEntryIndex >= device.entries.Count)
                            activeEntryIndex = 0;
                        if (activeDGVIndex >= DGV_Device.RowCount)
                            activeDGVIndex = 0;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "AutoPollAsync");
            }

        }

        public async Task PollForEntry(Entry entry)
        {
            byte[] message = new byte[8];
            var send = await Modbus.ReadRegAsync(port, slaveID, (FunctionCode)entry.registerType, entry.address, entry.length);
            isAwaitingResponse = true;

            Task delay = Task.WhenAny(Task.Delay(timeout), Task.Run(() => { while (isAwaitingResponse) { } return; })).ContinueWith((t) =>
            {
                if (isAwaitingResponse)
                {
                    Console.WriteLine("Response timed out.");
                    isAwaitingResponse = false;
                }
            });


            await delay;
        }

        void PublishResponse(object sender, ModbusResponseEventArgs e)
        {
            if (isAwaitingResponse)
            {
                try
                {
                    if (entries[activeEntryIndex].readOnce)
                    {
                        DataGridViewCheckBoxCell chbox = DGV_Device.Rows[activeDGVIndex].Cells[4] as DataGridViewCheckBoxCell;
                        chbox.Value = false;
                    }
                    int dbc = e.Message[2]; // data byte count
                    switch (entries[activeEntryIndex].dataType)
                    {
                        case ("bool"):
                            if (entries[activeEntryIndex].labels is null || entries[activeEntryIndex].labels.Count == 0) // assume that no labels = 1 entry
                            {
                                if (entries[activeEntryIndex].valueParse is null || entries[activeEntryIndex].valueParse.Keys.Count == 0)
                                    DGV_Device.Rows[activeEntryIndex].Cells[2].Value = e.Data[0] > 0x00 ? "true" : "false";
                                else
                                {
                                    try { DGV_Device.Rows[activeEntryIndex].Cells[2].Value = e.Data[0] > 0x00 ? entries[activeEntryIndex].valueParse["true"] : entries[activeEntryIndex].valueParse["false"]; }
                                    catch (Exception err)
                                    {
                                        MessageBox.Show("Value parsing error for bool entry: " + entries[activeEntryIndex].name + "; " + err.Message);
                                        DGV_Device.Rows[activeEntryIndex].Cells[2].Value = e.Data[0] > 0x00 ? "true" : "false";
                                    }
                                }
                                activeDGVIndex++;
                            }
                            else
                            {
                                List<bool> values = new List<bool>();
                                for (int i = 0; i < dbc; i++)
                                {
                                    for (int j = 0; j < 8; j++)
                                    {
                                        bool res = (((e.Data[i] >> j) & 0x01) >= 1) ? true : false;
                                        values.Add(res);
                                    }
                                }
                                for (int i = 0; i < entries[activeEntryIndex].labels.Count; i++)
                                {
                                    DGV_Device.Rows[activeDGVIndex].Cells[2].Value = values[i];
                                    activeDGVIndex++;
                                }
                            }
                            break;
                        case ("uint16"):
                            ushort value = BitConverter.ToUInt16(e.Data, 0);
                            if (entries[activeEntryIndex].labels is null || entries[activeEntryIndex].labels.Count == 0) // single value
                            {
                                if (entries[activeEntryIndex].valueParse is null || entries[activeEntryIndex].valueParse.Keys.Count == 0)
                                {
                                    //Array.Reverse(e.Data); // this was necessary, but something changed, idk

                                    Console.WriteLine("ushort parsed value: " + value);
                                    DGV_Device.Rows[activeDGVIndex].Cells[2].Value = value;
                                }
                                else
                                {
                                    try
                                    {
                                        DGV_Device.Rows[activeDGVIndex].Cells[2].Value = entries[activeEntryIndex].valueParse[value.ToString()];
                                    }
                                    catch (Exception err)
                                    {
                                        DGV_Device.Rows[activeDGVIndex].Cells[2].Value = value; MessageBox.Show("Error parsing uint value at address: " + entries[activeEntryIndex].address + "; " + err.Message, "uint16 parse");
                                    }
                                }

                                activeDGVIndex++;
                            }
                            else // value group
                            {
                                try
                                {
                                    List<ushort> values = new List<ushort>();
                                    for (int i = 0; i < dbc - 2; i += 2)
                                    {
                                        ushort s = BitConverter.ToUInt16(e.Data, i);
                                        Console.WriteLine("ushort value: " + s);
                                        values.Add(s);
                                    }
                                    for (int i = 0; i < entries[activeEntryIndex].labels.Count; i++)
                                    {
                                        DGV_Device.Rows[activeDGVIndex].Cells[2].Value = values[i];
                                        activeDGVIndex++;
                                    }
                                }
                                catch (Exception err)
                                {
                                    DGV_Device.Rows[activeDGVIndex].Cells[2].Value = value; MessageBox.Show("Error parsing uint value at address: " + entries[activeEntryIndex].address + "; " + err.Message, "uint16 group req parse");
                                }
                            }


                            break;
                        case ("uint32"):
                            Array.Reverse(e.Data);
                            DGV_Device.Rows[activeDGVIndex].Cells[2].Value = BitConverter.ToUInt32(e.Data, 0);

                            activeDGVIndex++;

                            break;
                        case ("string"):
                            List<byte> bytes = new List<byte>();
                            for (int i = 0; i < e.Data.Length; i++)
                            {
                                if (e.Data[i] != 0)
                                    bytes.Add(e.Data[i]);
                            }
                            bytes.Reverse();
                            DGV_Device.Rows[activeDGVIndex].Cells[2].Value = System.Text.Encoding.UTF8.GetString(bytes.ToArray());

                            activeDGVIndex++;

                            break;
                        default:
                            MessageBox.Show("Wrong data type set for entry " + entries[activeEntryIndex].name);
                            activeDGVIndex++;
                            break;
                    }
                    if (activeDGVIndex >= DGV_Device.RowCount)
                        activeDGVIndex = 0;

                    //MessageBox.Show("Получен ответ от устройства: " + dataCleaned, "Успех", MessageBoxButtons.OK);
                    port.DiscardInBuffer();
                }
                catch (Exception err) {
                    MessageBox.Show(err.Message, "Publish response error");
                }

            }
            if (activeDGVIndex >= DGV_Device.Rows.Count)
                activeDGVIndex = 0;
            isAwaitingResponse = false;
        }

        private void Button_StartStop_Click(object sender, EventArgs e)
        {
            isPolling = !isPolling;
            Button_StartStop.Text = (isPolling ? "Стоп" : "Старт");
        }
    }

}