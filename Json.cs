using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Modbus_Tools
{
    public class Device
    {
        public string name;
        public string description;
        public List<Entry> entries;
        public CheckEntry checkEntry;
        public Entry idEntry;
        public Entry speedEntry;

        public Device(string name, string description, CheckEntry checkEntry, Entry idEntry, Entry speedEntry, List<Entry> entries)
        {
            this.name = name;  
            this.description = description; 
            this.checkEntry = checkEntry;
            this.idEntry = idEntry;
            this.speedEntry = speedEntry;
            this.entries = entries;
        }
    }
    public class Entry
    {
        public string name;
        public RegisterType registerType;
        public ushort address;
        public ushort length = 1;
        public string dataType = "uint16";
        public List<string> labels;
        public Dictionary<string, string> valueParse;
        public bool readOnce;

        public Entry(string name, RegisterType registerType, ushort address, ushort length = 1, string dataType = "uint16", List<string> labels = null, Dictionary<string, string> valueParse = null, bool readOnce = false)
        {
            
            this.name = name;
            this.registerType = registerType;   
            this.address = address;
            this.length = length;
            this.dataType = dataType;
            this.labels = labels;
            this.valueParse = valueParse;

            this.readOnce = readOnce;
        }
    }
    public struct CheckEntry
    {
        public RegisterType registerType;
        public ushort address;
        public ushort length;
        public string dataType;
        public string expectedValue;
        public CheckEntry(RegisterType registerType, ushort address, ushort length, string dataType, string expectedValue)
        {
            this.registerType = registerType;
            this.address = address;
            this.length = length;
            this.dataType = dataType;
            this.expectedValue = expectedValue;
        }
    }
    public enum RegisterType { Coil = 1, Discrete = 2, Holding = 3, Input = 4}
}
