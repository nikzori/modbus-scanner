﻿using System;
using System.IO.Ports;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
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
        public static bool WriteSingleAsync(SerialPort port, FunctionCode functionCode, byte slaveID, ushort address, ushort value)
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
                Thread.Sleep(50);
                byte[] message = new byte[port.BytesToRead];
                //Console.WriteLine("Bytes to read:" + port.BytesToRead);
                port.Read(message, 0, port.BytesToRead);
                //Console.WriteLine("Incoming message: " + ByteArrayToString(message, false));
                if (message[1] <= 0x04) // read functions
                {
                    //Console.WriteLine("It's a read message");
                    ResponseReceived.Invoke(null, new ModbusResponseEventArgs(message, ModbusStatus.ReadSuccess));
                }
                else
                {
                    if (message[1] <= 0x10) // write functions
                    {
                        //Console.WriteLine("It's a write message");
                        ResponseReceived.Invoke(null, new ModbusResponseEventArgs(message, ModbusStatus.WriteSuccess));
                    }
                    else // error codes
                    {
                        //Console.WriteLine("It's an error");
                        ResponseReceived.Invoke(null, new ModbusResponseEventArgs(message, ModbusStatus.Error));
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