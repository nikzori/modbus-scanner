using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gidrolock_Modbus_Scanner
{
    public partial class Datasheet : Form
    {
        int timeout = 3000;
        int pollDelay = 250; // delay between each entry poll, ms
        byte slaveID;
        Device device = App.device;
        List<Entry> entries;
        SerialPort port = App.port;
        public Datasheet(byte slaveID)
        {
            this.slaveID = slaveID;
            entries = device.entries;

            InitializeComponent();

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

            for (int i = 0; i < entries.Count; i++)
            {
                DGV_Device.Rows.Add(i.ToString(), entries[i].name, "", entries[i].address);
            }
            Task.Delay(1000).ContinueWith(_ => AutoPollAsync());
        }

        public async Task<bool> AutoPollAsync()
        {
            port.Open();
            while (true)
            {
                for (int i = 0; i < entries.Count; i++)
                {
                    if ((bool)DGV_Device.Rows[i].Cells[4].Value)
                    {
                        DGV_Device.Rows[i].Cells[2].Value = await PollForEntry(entries[i]);
                        await Task.Delay(pollDelay);
                    }
                }
            }
        }

        public async Task<string> PollForEntry(Entry entry)
        {
            byte[] result = new byte[] { 0xFF };
            try
            {
                await Modbus.ReadRegAsync(port, slaveID, (FunctionCode)entry.registerType, entry.address, entry.length);
                var task = Task.Delay(10).ContinueWith(_ =>
                {
                    result = new byte[port.BytesToRead];
                    port.Read(result, 0, port.BytesToRead);
                });

                if (await Task.WhenAny(Task.Delay(timeout + 10), task) == task)
                {
                    if (result.Length > 5)
                    {
                        return Modbus.ByteArrayToString(result);
                    }
                    else return "N/A";
                }
                else return "N/A";
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return "N/A";
            }
        }
    }
}