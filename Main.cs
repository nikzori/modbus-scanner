using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Runtime;
using System.Web;
using System.Windows.Forms.Automation;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;

namespace Gidrolock_Modbus_Scanner
{
    public partial class App : Form
    {
        public static int[] BaudRate = new int[] 
        { 
            110, 300, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 57600, 76800, 115200, 230300, 460800, 921600 
        };
        public static int[] DataBits = new int[] { 7, 8 };
        Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        int offset = 0;
        byte[] message = new byte[255];
        public bool isAwaitingResponse = false;
        public bool isProcessingResponse = false;
        public short[] res = new short[12];
        public static SerialPort port = new SerialPort();
        public int expectedLength = 0;

        public static Device device;

        public App()
        {
            InitializeComponent();
            port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(PortDataReceived);
            this.UpDown_ModbusID.Value = 30;
            TextBox_Log.Text = "Приложение готово к работе.";

            CBox_Function.Items.Add("01 - Read Coil");
            CBox_Function.Items.Add("02 - Read Discrete Input");
            CBox_Function.Items.Add("03 - Read Holding Register");
            CBox_Function.Items.Add("04 - Read Input Register");
            CBox_Function.Items.Add("05 - Write Single Coil");
            CBox_Function.Items.Add("06 - Write Single Register");
            CBox_Function.Items.Add("0F - Write Multiple Coils");
            CBox_Function.Items.Add("10 - Write Multiple Registers");
            CBox_Function.SelectedItem = CBox_Function.Items[0];

            CBox_BaudRate.Items.Add("110");
            CBox_BaudRate.Items.Add("300");
            CBox_BaudRate.Items.Add("1200");
            CBox_BaudRate.Items.Add("2400");
            CBox_BaudRate.Items.Add("4800");
            CBox_BaudRate.Items.Add("9600");
            CBox_BaudRate.Items.Add("14400");
            CBox_BaudRate.Items.Add("19200");
            CBox_BaudRate.Items.Add("28800");
            CBox_BaudRate.Items.Add("38400");
            CBox_BaudRate.Items.Add("57600");
            CBox_BaudRate.Items.Add("76800");
            CBox_BaudRate.Items.Add("115200");
            CBox_BaudRate.Items.Add("230300");
            CBox_BaudRate.Items.Add("460800");
            CBox_BaudRate.Items.Add("921600");
            CBox_BaudRate.SelectedIndex = 5;

            CBox_DataBits.Items.Add("7");
            CBox_DataBits.Items.Add("8");
            CBox_DataBits.SelectedIndex = 1;

            CBox_StopBits.Items.Add("Нет");
            CBox_StopBits.Items.Add("1");
            CBox_StopBits.Items.Add("1.5");
            CBox_StopBits.Items.Add("2");
            CBox_StopBits.SelectedIndex = 1;

            CBox_Parity.Items.Add("Нет");
            CBox_Parity.Items.Add("Четн.");
            CBox_Parity.Items.Add("Нечетн.");
            CBox_Parity.SelectedIndex = 0;


            UpDown_RegAddress.Minimum = 0;
            UpDown_RegAddress.Maximum = 65535;

            UpDown_Value.Minimum = 0;
            UpDown_Value.Maximum = 65535; // 2^16 

            Radio_SerialPort.Checked = true;
            GBox_Ethernet.Enabled = false;
            TBox_IP.Text = "192.168.3.7";
            TBox_Port.Text = "8887";
            TBox_Timeout.Text = "3";
        }
        void App_FormClosed(object sender, FormClosedEventArgs e)
        {
            port.Close();
            if (!port.IsOpen)
                Application.Exit();
        }

        void Form1_Load(object sender, EventArgs e)
        {
            CBox_Ports.Items.AddRange(SerialPort.GetPortNames());
            if (CBox_Ports.Items.Count > 0)
                CBox_Ports.SelectedIndex = 0;
            Init();
        }

        void Init()
        {
            if (UpDown_ModbusID.Value == 0)
                Button_Connect.Text = "Найти адрес";
            else Button_Connect.Text = "Подключиться";
        }

        async Task SendMessageAsync(FunctionCode functionCode, ushort address, ushort length)
        {
            if (CBox_Ports.Text == "")
                MessageBox.Show("Необходимо выбрать COM порт.", "Ошибка", MessageBoxButtons.OK);
            if (UpDown_ModbusID.Value == 0)
                MessageBox.Show("Глобальное вещание пока не поддерживается");

            // Port Setup
            if (port.IsOpen)
                port.Close();

            port.Handshake = Handshake.None;
            port.PortName = CBox_Ports.Text;
            port.BaudRate = BaudRate[CBox_BaudRate.SelectedIndex];
            port.Parity = Parity.None;
            port.DataBits = DataBits[CBox_DataBits.SelectedIndex];
            port.StopBits = (StopBits)CBox_StopBits.SelectedIndex;

            port.ReadTimeout = 3000;
            port.WriteTimeout = 3000;


            offset = 0;
            message = new byte[255];
            port.Open();

            /* - Reading from Registers - */
            if (CBox_Function.SelectedIndex < 4)
            {
                try
                { 
                    byte[] request = new byte[8];
                    Modbus.BuildMessage((byte)UpDown_ModbusID.Value, (byte)(1 + functionCode), address, length, ref request);
                    string messageParsed = Modbus.ParseByteArray(request);

                    var send = await Modbus.ReadRegAsync(port, functionCode, (byte)UpDown_ModbusID.Value, address, length);
                    AddLog("Отправка сообщения: " + messageParsed);
                    isAwaitingResponse = true;
                    Task timer = Task.Delay(port.ReadTimeout);
                    await timer.ContinueWith(_ =>
                    {
                        if (isAwaitingResponse)
                        {
                            MessageBox.Show("Истекло время ожидания ответа.", "Ошибка");
                            port.Close();
                        }
                    });
                }
                catch (Exception err)
                {
                    port.Close();
                    MessageBox.Show(err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            /* - Writing to Registers - */
            else
            {
                try
                {
                    if (CBox_Function.SelectedIndex < 6) // Single Registers
                    {
                        byte[] request = new byte[8];
                        Modbus.BuildMessage((byte)UpDown_ModbusID.Value, (byte)(1 + functionCode), address, length, ref request);
                        string messageParsed = Modbus.ParseByteArray(request);

                        var send = await Modbus.WriteSingle(port, functionCode, (byte)UpDown_ModbusID.Value, address, (ushort)UpDown_Value.Value);
                    }
                    else // Multiple Registers
                    {
                        byte[] request = new byte[(int)UpDown_RegLength.Value * 2 + 6];
                    }
                }
                catch (Exception err)
                {
                    port.Close();
                    MessageBox.Show(err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private async void ButtonConnect_Click(object sender, EventArgs e)
        {
            if (device is null)
                MessageBox.Show("Выберите конфигурацию для подключения и опроса устройства.");
            else
            {
                AddLog("Попытка подключиться к устройству " + device.name);
                Datasheet datasheet = new Datasheet();
                datasheet.Show();
                /*
                if (Radio_SerialPort.Checked)
                    await SendMessageAsync(FunctionCode.InputRegister, 200, 6);
                //else EthernetParse();
                */
            }
        }

        async void EthernetParse()
        {
            string ipText = TBox_IP.Text;
            string portText = TBox_Port.Text;

            Regex ip = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            Regex port = new Regex(@"\d");

            if (!ip.IsMatch(ipText))
                MessageBox.Show("Неправильный формат IP-адреса.");
            else if (!port.IsMatch(portText))
                MessageBox.Show("Неправильный формат TCP-порта.");
            else
            {
                int portParsed = Int32.Parse(portText);
                await socket.ConnectAsync(ipText, portParsed);
                byte[] data = new byte[8];
                Modbus.BuildMessage(0x1E, 0x03, 128, 1, ref data);
                AddLog("Sending to " + ipText + ":" + portText + ":" + Modbus.ParseByteArray(data));

                // set up an event listener to receive the response
                await SocketDataTransfer(data);

            }
            
        }

        void CBox_Ports_Click(object sender, EventArgs e)
        {
            CBox_Ports.Items.Clear();
            CBox_Ports.Items.AddRange(SerialPort.GetPortNames());
        }

        void PortDataReceived(object sender, EventArgs e)
        {
            Console.WriteLine("Data receieved on Serial Port");
            isAwaitingResponse = false;

            if (!isProcessingResponse)
            {
                isProcessingResponse = true;
                try
                {
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
                    Console.WriteLine("Data: " + Modbus.ParseByteArray(data));
                    string dataCleaned = Modbus.ParseByteArray(message);

                    TextBox_Log.Invoke((MethodInvoker)delegate { AddLog("Получен ответ: " + dataCleaned); });
                    TextBox_Log.Invoke((MethodInvoker)delegate { AddLog("ASCII: " + "wip"); });
                    //MessageBox.Show("Получен ответ от устройства: " + dataCleaned, "Успех", MessageBoxButtons.OK);
                    port.DiscardInBuffer();
                    isProcessingResponse = false;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                    isProcessingResponse = false;
                }
            }
            else Console.WriteLine("Already processing incoming message!");

            //port.Close();
            
        }

        void AddLog(string message)
        {
            TextBox_Log.AppendText(Environment.NewLine + message);
        }

        private async void Button_SendCommand_Click(object sender, EventArgs e)
        {
            FunctionCode functionCode = (FunctionCode)CBox_Function.SelectedIndex;
            ushort address = (ushort)UpDown_RegAddress.Value;
            ushort length = (ushort)UpDown_RegLength.Value;

            await SendMessageAsync(functionCode, address, length);
        }

        private void OnSelectedFunctionChanged(object sender, EventArgs e)
        {
            if (CBox_Function.SelectedIndex < 4)
                UpDown_Value.Enabled = false;
            else
            {
                if (CBox_Function.SelectedIndex == 4 || CBox_Function.SelectedIndex == 6)
                    UpDown_Value.Maximum = 1;
                else UpDown_Value.Maximum = 65535;

                UpDown_Value.Enabled = true;
            }
        }

        private void Radio_SerialPort_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_SerialPort.Checked)
                GBox_Serial.Enabled = true;
            else GBox_Serial.Enabled = false;
        }

        private void Radio_Ethernet_CheckedChanged(object sender, EventArgs e)
        {
            if (Radio_Ethernet.Checked)
                GBox_Ethernet.Enabled = true;
            else GBox_Ethernet.Enabled = false;
        }

        private async Task<bool> SocketDataTransfer(byte[] data)
        {
            await Task.Run(() => { socket.Send(data); });
            byte[] res = new byte[64];
            await Task.Run(() =>
            {
                while (true)
                {
                    int bytesReceived = socket.Receive(res);

                    if (bytesReceived == 0)
                        break;

                    string resParsed = "";
                    Modbus.ParseResponse(res, ref resParsed);

                    Console.Out.WriteLine("Received data on TCP socket: " + resParsed);
                    AddLog("Response from TCP Server: " + resParsed);
                }                
            });
            return true;
        }

        private void LoadConfig(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Application.StartupPath;
                openFileDialog.Filter = "JSON files (*.json)|*.json";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }

                    try
                    {
                        device = JsonConvert.DeserializeObject<Device>(fileContent);
                        Label_Config.Text = device.name;
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }
            }
        }
    }
}   

public enum FunctionCode { Coil, DiscreteInput, HoldingRegister, InputRegister, WriteCoil, WriteRegister, WriteMultCoils, WriteMultRegisters };
