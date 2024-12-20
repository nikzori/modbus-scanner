﻿using System;
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
            
            for (int i = 0; i < entries.Count; i++)
            {
                DGV_Device.Rows.Add(i.ToString(), entries[i].name, "", entries[i].address);
                DGV_Device.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

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
                        for (int i = 0; i < entries.Count; i++)
                        {
                            // holy fuck DGV is awful
                            DataGridViewCheckBoxCell chbox = DGV_Device.Rows[i].Cells[4] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(chbox.Value))
                            {
                                Console.WriteLine("Polling for " + device.entries[i].name);
                                activeEntryIndex = i;
                                await PollForEntry(entries[i]);
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public async Task PollForEntry(Entry entry)
        {
            byte[] message = new byte[9];
            var send = await Modbus.ReadRegAsync(port, slaveID, (FunctionCode)entry.registerType, entry.address, entry.length);
            isAwaitingResponse = true;

            Task delay = Task.Delay(timeout).ContinueWith((t) =>
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
                isAwaitingResponse = false;
                try
                {
                    switch (entries[activeEntryIndex].dataType)
                    {
                        case ("bool"):
                            DGV_Device.Rows[activeEntryIndex].Cells[2].Value = e.Data[0] > 0x00 ? "true" : "false";
                            break;
                        case ("uint16"):
                            //Array.Reverse(e.Data); // BitConverter.ToUInt is is little endian, so we need to flip the array
                            ushort test = BitConverter.ToUInt16(e.Data, 0);
                            Console.WriteLine("ushort parsed value: " + test);
                            DGV_Device.Rows[activeEntryIndex].Cells[2].Value = test;
                            break;
                        case ("uint32"):
                            Array.Reverse(e.Data);
                            DGV_Device.Rows[activeEntryIndex].Cells[2].Value = BitConverter.ToUInt32(e.Data, 0);
                            break;
                        case ("string"):
                            List<byte> bytes = new List<byte>();
                            for (int i = 0; i < e.Data.Length; i++)
                            {
                                if (e.Data[i] != 0)
                                    bytes.Add(e.Data[i]);
                            }
                            bytes.Reverse();
                            DGV_Device.Rows[activeEntryIndex].Cells[2].Value = System.Text.Encoding.UTF8.GetString(bytes.ToArray());
                            break;
                        default:
                            MessageBox.Show("Wrong data type set for entry " + entries[activeEntryIndex].name);
                            break;
                    }


                    //MessageBox.Show("Получен ответ от устройства: " + dataCleaned, "Успех", MessageBoxButtons.OK);
                    port.DiscardInBuffer();
                }
                catch { return; }

            }
        }

        private void Button_StartStop_Click(object sender, EventArgs e)
        {
            isPolling = !isPolling;
            Button_StartStop.Text = (isPolling ? "Стоп" : "Старт");
        }
    }

}