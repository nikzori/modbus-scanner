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
        List<EntryUI> entries = new List<EntryUI>();

        public Datasheet()
        {
            InitializeComponent(); 
        }
    }

    public class EntryUI : GroupBox
    {
        public Label Label_Name;
        public Label Label_Value;
        public ToolTip ToolTip;
        public EntryUI(string name, string description, int address, string registerType, string dataType)
        {
            Label_Name.Text = name;
            ToolTip.SetToolTip(this, description);
        }
        
    }
}
