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

namespace Gidrolock_Modbus_Scanner
{
    public partial class App : Form
    {
        public static int[] BaudRate = new int[] 
        { 
            110, 300, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 57600, 76800, 115200, 230300, 460800, 921600 
        };
        public static int[] DataBits = new int[] { 7, 8 };

        int offset = 0;
        byte[] message = new byte[255];
        public bool isAwaitingResponse = false;
        public bool isProcessingResponse = false;
        public short[] res = new short[12];
        public SerialPort port = new SerialPort();
        public int expectedLength = 0;
        

        public App()
        {
            InitializeComponent();
            this.port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(PortDataReceived);
            this.UpDown_ModbusID.Value = 30;
            TextBox_Log.Text = "Приложение готово к работе.";

            CBox_Function.Items.Add("01 - Read Coil");
            CBox_Function.Items.Add("02 - Read Discrete Input");
            CBox_Function.Items.Add("03 - Read Holding Register");
            CBox_Function.Items.Add("04 - Read Input Register");
            CBox_Function.Items.Add("05 - Write Single Coil");
            CBox_Function.Items.Add("06 - Write Single Holding Register");
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

            UpDown_RegAddress.Minimum = 0;
            UpDown_RegAddress.Maximum = 65535;

            UpDown_Value.Minimum = 0;
            UpDown_Value.Maximum = 65535; // 2^16 
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
                ButtonConnect.Text = "Найти адрес";
            else ButtonConnect.Text = "Подключиться";
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

            port.ReadTimeout = 1000;
            port.WriteTimeout = 1000;


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
                    Task timer = Task.Delay(2000);
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
            AddLog("Попытка подключиться к устройству Gidrolock.");
            await SendMessageAsync(FunctionCode.DiscreteInput, 200, 5);
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
                }
            }

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
    }
}   

public enum FunctionCode { Coil, DiscreteInput, HoldingRegister, InputRegister, WriteCoil, WriteRegister, WriteMultCoils, WriteMultRegisters };
