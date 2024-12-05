using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gidrolock_Modbus_Scanner
{
    public partial class Datasheet : Form
    {
        int pollDelay = 250; // delay between each entry poll, ms
        Device device = App.device;
        public Datasheet()
        {
            InitializeComponent();
            listView1.AllowColumnReorder = true;
            listView1.CheckBoxes = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            listView1.Columns.Add("#", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Name", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Value", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Address", -2, HorizontalAlignment.Left);


            for (int i = 0; i < device.entries.Count; i++)
            {
                ListViewItem item = new ListViewItem(i.ToString());
                item.SubItems.Add(device.entries[i].name);
                item.SubItems.Add(" ");
                item.SubItems.Add(device.entries[i].address.ToString());

                listView1.Items.Add(item);
            }
        }
    }
}
