using System;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Gidrolock_Modbus_Scanner
{
    public static class Modbus
    {
        public static SerialPort port = new SerialPort();


        public static event EventHandler<ModbusResponseEventArgs> ResponseReceived = delegate { };

        public static void Init()
        {
            port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(PortDataReceived);
        }

        #region Build Message
        public static byte[] BuildMessage(byte modbusID, byte functionCode, ushort address, ushort length, ref byte[] message)
        {
            //Array to receive CRC bytes:
            byte[] CRC = new byte[2];

            message[0] = modbusID;
            message[1] = functionCode;
            message[2] = (byte)(address >> 8);
            message[3] = (byte)address;
            message[4] = (byte)(length >> 8);
            message[5] = (byte)length;

            GetCRC(message, ref CRC);
            message[message.Length - 2] = CRC[0];
            message[message.Length - 1] = CRC[1];
            string msg = ByteArrayToString(message);
            //Console.WriteLine("Message: " + msg);
            return message;
        }
        #endregion

        #region Read Functions
        public static async Task<bool> ReadRegAsync(SerialPort port, byte slaveID, FunctionCode functionCode, ushort address, ushort length)
        {
            //Ensure port is open:
            if (port.IsOpen)
            {
                //Clear in/out buffers:
                port.DiscardOutBuffer();
                port.DiscardInBuffer();

                //Read functions are always 8 bytes long
                byte[] message = new byte[8];
                
                //Build outgoing modbus message:
                BuildMessage(slaveID, (byte)functionCode, address, length, ref message);

                //Send modbus message to Serial Port:
                try
                {
                    await Task.Run(() => { port.Write(message, 0, message.Length); });
                    return true;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                    port.Close();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Порт не открыт");
                return false;
            }

        }
        #endregion
        
        #region Write Single Coil/Register
        public static async Task<bool> WriteSingle(SerialPort port, FunctionCode functionCode, byte slaveID, ushort address, uint value)
        {

        }
        #endregion
        public static bool ParseResponse(byte[] res, ref string verbose)
        {
            try
            {
                int dataLength = (int)res[2];
                if (res.Length < dataLength + 4)
                {
                    verbose = "Сообщение устройства не соответствует ожидаемой длине!";
                    return false;
                }

                //TODO: Check CRC
                return true;
            }
            catch
            {
                verbose = "Сообщение устройства не соответствует ожидаемой длине!";
                return false;
            }
            
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            byte[] res = CleanByteArray(bytes);

            string dataString = BitConverter.ToString(res);
            string result = "";
            for (int i = 0; i < dataString.Length; i++)
            {
                if (dataString[i] == '-')
                    result += " ";
                else result += dataString[i];
            }

            return result;
        }
        public static byte[] CleanByteArray(byte[] bytes)
        {
            int length = bytes.Length - 1;
            // snip off the empty bytes at the end
            for (int i = length; i >= 0; i--)
            {
                if (bytes[i] != 0)
                {
                    length = i + 1;
                    break;
                }
            }

            byte[] res = new byte[length];
            for (int i = 0; i < length; i++) { res[i] = bytes[i]; }

            return res;
        }


        #region CRC Computation
        static void GetCRC(byte[] message, ref byte[] CRC)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }
        #endregion

        static void PortDataReceived(object sender, EventArgs e)
        {
            try
            {
                byte[] message = new byte[port.BytesToRead];
                port.Read(message, 0, 3);
                int length = (int)message[2];
                for (int i = 0; i < length + 2; i++)
                {
                    port.Read(message, i + 3, 1);
                }

                byte[] data = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    data[i] = message[i + 3];
                }
                string dataCleaned = ByteArrayToString(message);

                port.DiscardInBuffer();

                ResponseReceived.Invoke(null, new ModbusResponseEventArgs(message, data, Encoding.UTF8.GetString(data)));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

    }

    public class ModbusResponseEventArgs : EventArgs
    {
        public byte[] Message { get; set; }
        public byte[] Data { get; set; }
        public string Text { get; set; }
        public ModbusResponseEventArgs(byte[] message, byte[] data, string text)
        {
            this.Message = message;
            this.Data = data;
            this.Text = text;
        }
    }

}