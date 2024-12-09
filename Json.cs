using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Gidrolock_Modbus_Scanner
{
    public class Device
    {
        public string name;
        public string description;
        public List<Entry> entries;

        public Device(string name, string description, List<Entry> entries)
        {
            this.name = name;  
            this.description = description; 
            this.entries = entries;
        }
    }
    public struct Entry
    {
        public string name;
        public RegisterType registerType;
        public ushort address;
        public ushort length;
        public string dataType;
        public bool readOnce;

        public Entry(string name, RegisterType registerType, ushort address, ushort length = 1, string dataType = "uint16", bool readOnce = false)
        {
            this.name = name;
            this.registerType = registerType;   
            this.address = address;
            this.length = length;
            this.dataType = dataType;
            this.readOnce = readOnce;
        }
    }
    public enum RegisterType { Coil, Discrete, Holding, Input }
}
