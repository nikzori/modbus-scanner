using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows.Forms;

namespace Gidrolock_Modbus_Scanner
{
    public static class Modbus
    {
        public static SerialPort port = new SerialPort();
        public static byte slaveID = 0x1E;
        public static event EventHandler<ModbusResponseEventArgs> ResponseReceived = delegate { };

        public static void Init()
        {
            port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(PortDataReceived);
        }

        #region Build Message
        public static byte[] BuildReadMessage(byte modbusID, byte functionCode, ushort address, ushort length, ref byte[] message)
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
        public static byte[] BuildWriteSingleMessage(byte modbusID, byte functionCode, ushort address, byte[] data)
        {
            if (functionCode == 0x05 || functionCode == 0x06)
            {
                byte[] _message = new byte[8];
                byte[] CRC = new byte[2];

                _message[0] = modbusID;
                _message[1] = functionCode;
                _message[2] = (byte)(address >> 8);
                _message[3] = (byte)address;
                _message[4] = data[0];
                _message[5] = data[1];
                GetCRC(_message, ref CRC);
                _message[6] = CRC[0];
                _message[7] = CRC[1];
                return _message;
            }
            else return new byte[1] { 0xFF };
        }
        #endregion

        #region Read Functions
        public static bool ReadRegisters(SerialPort port, byte slaveID, FunctionCode functionCode, ushort address, ushort length)
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
                BuildReadMessage(slaveID, (byte)functionCode, address, length, ref message);
                Console.WriteLine("Outgoing message: " + ByteArrayToString(message));
                if (message.Length > 1)
                {
                    //Send modbus message to Serial Port:
                    try
                    {
                        port.Write(message, 0, message.Length);
                        return true;
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message, "aeiou");
                        port.Close();
                        return false;
                    }
                }
                else return false;
            }
            else
            {
                MessageBox.Show("Порт не открыт");
                return false;
            }

        }
        #endregion

        #region Write Single Coil/Register
        public static bool WriteSingleAsync(SerialPort port, FunctionCode functionCode, byte slaveID, ushort address, ushort value, ref byte[] msg)
        {
            //Ensure port is open:
            if (!port.IsOpen)
            {
                try
                {
                    port.Open();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                    return false;
                }
            }
            //Clear in/out buffers:
            port.DiscardOutBuffer();
            port.DiscardInBuffer();

            //Build outgoing modbus message:
            byte[] _value = BitConverter.GetBytes(value);
            Array.Reverse(_value);

            byte[] message = BuildWriteSingleMessage(slaveID, (byte)functionCode, address, _value);
            msg = message;
            Console.WriteLine("Write message: " + ByteArrayToString(message));
            //Send modbus message to Serial Port:
            try
            {
                port.Write(message, 0, message.Length);
                Console.WriteLine("Message sent successfully.");
                return true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                port.Close();
                return false;
            }

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

        public static string ByteArrayToString(byte[] bytes, bool cleanEmpty = true)
        {
            if (bytes is null || bytes.Length == 0)
                return "";

            byte[] res;

            if (cleanEmpty)
                res = CleanByteArray(bytes);
            else res = bytes;

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


        /// <summary>
        /// Рассчет CRC для 
        /// </summary>
        /// <param name="message">Сообщение с двумя байтами под CRC</param>
        /// <param name="CRC"></param>
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


        /// <summary>
        /// Подсчет фактического CRC сообщения и сравнение с CRC в самом сообщении.
        /// </summary>
        /// <param name="message">Сообщение с CRC.</param>
        /// <returns></returns>
        public static bool CheckCRC(byte[] message)                     // Проверка пакета на контрольную сумму
        {
            //Perform a basic CRC check:
            byte[] CRC = new byte[2];
            try
            {
                CRC[0] = message[message.Length - 2];
                CRC[1] = message[message.Length - 1];
            }
            catch (Exception err)
            {
                Console.WriteLine("CheckCRC() error: " + err.ToString());
                return false;
            }
            GetCRC(message, ref CRC);

            if (CRC[0] == message[message.Length - 2] && CRC[1] == message[message.Length - 1])
                return true;
            else
                return false;
        }

        static Stopwatch stopwatch = new Stopwatch();
        static byte[] buffer = new byte[255];
        static int offset = 0;
        static int count = 0;
        static void PortDataReceived(object sender, EventArgs e)
        {
            //reset values on every event call;
            buffer = new byte[255];
            offset = 0;
            try
            {
                stopwatch.Restart();
                while (stopwatch.ElapsedMilliseconds < 20)
                {
                    if (port.BytesToRead > 0)
                    {
                        stopwatch.Restart();
                        count = port.BytesToRead;
                        port.Read(buffer, offset, port.BytesToRead);
                        offset += count;
                    }
                }
                // assume that the message ended
                
                List<byte> message = new List<byte>();
                for (int i = 0; i < offset; i++)
                {
                    message.Add(buffer[i]);
                }
                if (message.Count == 0)
                    return;

                Console.WriteLine("Incoming message: " + ByteArrayToString(message.ToArray(), false));
                
                if (!CheckCRC(message.ToArray()))
                    Console.WriteLine("Bad CRC or not a modbus message!"); 
                
                if (message[1] <= 0x04) // read functions
                {
                    //Console.WriteLine("It's a read message");
                    ResponseReceived.Invoke(null, new ModbusResponseEventArgs(message.ToArray(), ModbusStatus.ReadSuccess));
                }
                else
                {
                    if (message[1] <= 0x10) // write functions
                    {
                        //Console.WriteLine("It's a write message");
                        ResponseReceived.Invoke(null, new ModbusResponseEventArgs(message.ToArray(), ModbusStatus.WriteSuccess));
                    }
                    else // error codes
                    {
                        //Console.WriteLine("It's an error");
                        ResponseReceived.Invoke(null, new ModbusResponseEventArgs(message.ToArray(), ModbusStatus.Error));
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Modbus message reception error");
            }

            port.DiscardInBuffer();
        }
    }

    public class ModbusResponseEventArgs : EventArgs
    {
        public byte[] Message { get; set; }
        public byte[] Data { get; set; }
        public ModbusStatus Status { get; set; }
        public ModbusResponseEventArgs(byte[] message, ModbusStatus status)
        {
            this.Message = message;
            this.Status = status;
            if (status == ModbusStatus.ReadSuccess)
            {
                int dataLength = message[2];
                Data = new byte[dataLength];
                for (int i = 0; i < dataLength; i++) { Data[i] = message[i + 3]; }
            }
        }
    }

    public enum ModbusStatus { ReadSuccess, WriteSuccess, Error };
}