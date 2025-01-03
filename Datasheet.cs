﻿using System;
using System.Collections.Generic;
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
        byte[] message = new byte[255];

        int timeout = 3000;
        int pollDelay = 250; // delay between each entry poll, ms
        byte slaveID;
        Device device = App.device;
        List<Entry> entries;
        List<EntryRow> entryRows;
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



            int rowCount = 0;

            entryRows = new List<EntryRow>();
            foreach (Entry e in entries)
            {
                if (e.length > 1)
                {
                    // multi-register entry check
                    if ((e.length == 2 && e.dataType == "uint32") || e.dataType == "string")
                    {
                        entryRows.Add(new EntryRow(rowCount, e.address, e.name));
                        rowCount++;
                    }
                    else
                    {
                        for (int i = 0; i < e.length; i++)
                        {
                            if (i < e.labels.Count)
                            {
                                entryRows.Add(new EntryRow(rowCount, e.address + i, e.labels[i]));
                            }
                            else
                            {
                                entryRows.Add(new EntryRow(rowCount, e.address + i, (e.address + i).ToString()));
                            }
                            rowCount++;
                        }
                    }

                }
                else
                {
                    entryRows.Add(new EntryRow(rowCount, e.address, e.name));
                    rowCount++;
                }
            }
            foreach (EntryRow row in entryRows)
                Panel_Device.Controls.Add(row);
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
                        if (entryRows[activeDGVIndex].chboxPanel.chbox.Checked)
                        {
                            //Console.WriteLine("Polling for " + device.entries[activeEntryIndex].name);
                            await PollForEntry(entries[activeEntryIndex]);
                            Thread.Sleep(150);
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
                        if (activeDGVIndex >= entryRows.Count)
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
            var send = Modbus.ReadRegAsync(port, slaveID, (FunctionCode)entry.registerType, entry.address, entry.length);
            isAwaitingResponse = true;

            Task delay = Task.WhenAny(Task.Delay(timeout), Task.Run(() => { while (isAwaitingResponse) { } return true; })).ContinueWith((t) =>
            {
                if (isAwaitingResponse)
                {
                    Console.WriteLine("Response timed out.");
                    isAwaitingResponse = false;
                }
                return false;
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
                        entryRows[activeDGVIndex].chboxPanel.chbox.Checked = false;
                    }
                    int dbc = e.Message[2]; // data byte count
                    switch (entries[activeEntryIndex].dataType)
                    {
                        case ("bool"):
                            if (entries[activeEntryIndex].labels is null || entries[activeEntryIndex].labels.Count == 0) // assume that no labels = 1 entry
                            {
                                //no valueParse keys
                                if (entries[activeEntryIndex].valueParse is null || entries[activeEntryIndex].valueParse.Keys.Count == 0)
                                {
                                    // coil combobox
                                    if (entries[activeEntryIndex].registerType == RegisterType.Coil)
                                    {
                                        entryRows[activeDGVIndex].SetValue(e.Data[0] > 0x00 ? "True" : "False");
                                    }
                                    // discrete inputs
                                    else
                                    {
                                        entryRows[activeDGVIndex].SetValue(e.Data[0] > 0x00 ? "True" : "False");
                                    }
                                }
                                else
                                {
                                    try { entryRows[activeDGVIndex].valueLabel.Text = e.Data[0] > 0x00 ? entries[activeEntryIndex].valueParse["true"] : entries[activeEntryIndex].valueParse["false"]; }
                                    catch (Exception err)
                                    {
                                        // coil combobox
                                        if (entries[activeEntryIndex].registerType == RegisterType.Coil)
                                        {
                                            entryRows[activeDGVIndex].SetValue(e.Data[0] > 0x00 ? "True" : "False");
                                        }
                                        else // discrete inputs
                                        {
                                            entryRows[activeDGVIndex].SetValue(e.Data[0] > 0x00 ? "True" : "False");
                                        }
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
                                    if (entries[activeEntryIndex].registerType == RegisterType.Coil)
                                    {
                                        entryRows[activeDGVIndex].SetValue(e.Data[0] > 0x00 ? "True" : "False");
                                    }
                                    else
                                    {
                                        entryRows[activeDGVIndex].SetValue(e.Data[0] > 0x00 ? "True" : "False");
                                    }
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

                                    //Console.WriteLine("ushort parsed value: " + value);
                                    entryRows[activeDGVIndex].SetValue(value.ToString());
                                }
                                else
                                {
                                    try
                                    {
                                        entryRows[activeDGVIndex].SetValue(entries[activeEntryIndex].valueParse[value.ToString()]);
                                    }
                                    catch (Exception err)
                                    {
                                        entryRows[activeDGVIndex].SetValue(value.ToString());
                                        MessageBox.Show("Error parsing uint value at address: " + entries[activeEntryIndex].address + "; " + err.Message, "uint16 parse");
                                    }
                                }

                                activeDGVIndex++;
                            }
                            else // value group
                            {
                                try
                                {
                                    List<ushort> values = new List<ushort>();
                                    for (int i = 0; i < dbc; i += 2)
                                    {
                                        ushort s = BitConverter.ToUInt16(e.Data, i);
                                        values.Add(s);
                                    }
                                    //Console.WriteLine("ushort values count: " + values.Count);
                                    //Console.WriteLine("entity labels count: " + entries[activeEntryIndex].labels.Count);
                                    for (int i = 0; i < entries[activeEntryIndex].labels.Count; i++)
                                    {
                                        if (device.entries[activeEntryIndex].valueParse != null)
                                        {
                                            entryRows[activeDGVIndex].SetValue(values[i].ToString());
                                        }
                                        else
                                        {
                                            entryRows[activeDGVIndex].SetValue(values[i].ToString());
                                        }
                                        activeDGVIndex++;
                                    }
                                }
                                catch (Exception err)
                                {
                                    entryRows[activeDGVIndex].SetValue(value.ToString());
                                    MessageBox.Show("Error parsing uint value at address: " + entries[activeEntryIndex].address + "; " + err.Message, "uint16 group req parse");
                                }
                            }


                            break;
                        case ("uint32"):
                            Array.Reverse(e.Data);
                            entryRows[activeDGVIndex].SetValue(BitConverter.ToUInt32(e.Data, 0).ToString());

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
                            entryRows[activeDGVIndex].SetValue(System.Text.Encoding.UTF8.GetString(bytes.ToArray()));

                            activeDGVIndex++;

                            break;
                        default:
                            MessageBox.Show("Wrong data type set for entry " + entries[activeEntryIndex].name);
                            activeDGVIndex++;
                            break;
                    }
                    if (activeDGVIndex >= entryRows.Count)
                        activeDGVIndex = 0;

                    //MessageBox.Show("Получен ответ от устройства: " + dataCleaned, "Успех", MessageBoxButtons.OK);
                    port.DiscardInBuffer();
                }
                catch (Exception err) { MessageBox.Show(err.Message, "Publish response error"); }

            }
            if (activeDGVIndex >= entryRows.Count)
                activeDGVIndex = 0;
            isAwaitingResponse = false;
        }

        private void Button_StartStop_Click(object sender, EventArgs e)
        {
            isPolling = !isPolling;
            Button_StartStop.Text = (isPolling ? "Стоп" : "Старт");
        }


        public class EntryRow : Panel
        {
            public CheckboxPanel chboxPanel = new CheckboxPanel() { Width = 20, Height = 20 };
            public Label numberLabel = new Label() { Height = 20, Width = 30, TextAlign = ContentAlignment.MiddleLeft };
            public Label addressLabel = new Label() { Height = 20, Width = 60, TextAlign = ContentAlignment.MiddleLeft };
            public Label nameLabel = new Label() { Height = 20, Width = 220, TextAlign = ContentAlignment.MiddleLeft };
            public Label valueLabel = new Label() { Height = 20, Width = 220, TextAlign = ContentAlignment.MiddleLeft };

            public void SetValue(string value)
            {
                valueLabel.Text = value;
            }

            public EntryRow(int number, int address, string name)
            {
                chboxPanel.Parent = this;
                numberLabel.Parent = this;
                addressLabel.Parent = this;
                nameLabel.Parent = this;
                valueLabel.Parent = this;

                chboxPanel.Location = new Point(Left, Top);
                numberLabel.Location = new Point(20, Top);
                addressLabel.Location = new Point(50, Top);
                nameLabel.Location = new Point(110, Top);
                valueLabel.Location = new Point(330, Top);

                numberLabel.Text = number.ToString();
                addressLabel.Text = address.ToString();
                nameLabel.Text = name.ToString();
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
}