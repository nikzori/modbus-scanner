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

namespace Gidrolock_Modbus_Scanner
{
    public partial class App : Form
    {
        int offset = 0;
        byte[] data = new byte[255];
        public bool isAwaitingResponse = false;
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
            CBox_Function.SelectedItem = CBox_Function.Items[0];

            UpDown_RegAddress.Minimum = 0;
            UpDown_RegAddress.Maximum = 65536;
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

            try
            {
                if (port.IsOpen)
                    port.Close();

                port.Handshake = Handshake.None;
                port.PortName = CBox_Ports.Text;
                port.BaudRate = 9600;
                port.Parity = Parity.None;
                port.DataBits = 8;
                port.StopBits = StopBits.One;

                port.ReadTimeout = 1000;
                port.WriteTimeout = 1000;


                offset = 0;
                data = new byte[255];
                port.Open();

                byte[] message = new byte[8];
                Modbus.BuildMessage((byte)UpDown_ModbusID.Value, (byte)(1 + functionCode), address, length, ref message);
                string messageParsed = Modbus.ParseByteArray(message);

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

        private async void ButtonConnect_Click(object sender, EventArgs e)
        {
            await SendMessageAsync(FunctionCode.HoldingRegister, 128, 1);
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
            try
            {
                int len = port.BytesToRead;
                Console.WriteLine("Data length: " + len);
                port.Read(data, offset, len);
                offset += len;
                string dataCleaned = Modbus.ParseByteArray(data);

                TextBox_Log.Invoke((MethodInvoker)delegate { AddLog("Получен ответ: " + dataCleaned); });
                //MessageBox.Show("Получен ответ от устройства: " + dataCleaned, "Успех", MessageBoxButtons.OK);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
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
    }
}   

public enum FunctionCode { Coil, DiscreteInput, HoldingRegister, InputRegister}
