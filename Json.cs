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
        public int address;
        public int length;
        public string dataType;
        public bool readOnce;

        public Entry(string name, RegisterType registerType, int address, int length, string dataType, bool readOnce)
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
