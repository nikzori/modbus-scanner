using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gidrolock_Modbus_Scanner
{
    public struct Entry
    {
        public string label;
        public RegisterType registerType;
        public int offset;
        public int length; 

        public Entry(string label, RegisterType registerType, int offset, int length)
        {
            this.label = label;
            this.registerType = registerType;   
            this.offset = offset;
            this.length = length;
        }
    }
    public enum RegisterType { Coil, DiscreteInput, HoldingRegister, InputRegister }
}
