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
        public bool isAwaitingResponse = false;
        public short[] res = new short[12];
        public SerialPort port = new SerialPort();

        public App()
        {
            InitializeComponent();
            this.port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(PortDataReceived);
            this.UpDown_ModbusID.Value = 30;
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

        private async void ButtonConnect_Click(object sender, EventArgs e)
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

                port.ReadTimeout = 10;
                port.WriteTimeout = 10;

        

                port.Open();
                var send = await Modbus.ReadRegAsync(port, (byte)FunctionCode.HoldingRegister, (byte)UpDown_ModbusID.Value, 128, 1);
                isAwaitingResponse = true;
                Task timer = Task.Delay(2000);
                await timer.ContinueWith( _ => 
                {
                    if (isAwaitingResponse)
                        MessageBox.Show("Истекло время ожидания ответа.", "Ошибка");
                });

            }
            catch (Exception err)
            {
                port.Close();
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            int len;
            byte rdxLen = 0;
            
            try
            {
                len = port.BytesToRead;
                byte[] data = new byte[len];
                port.Read(data, rdxLen, len);
                rdxLen += (byte)len;
                int lastByte = len - 1;
                for (int i = len - 1; i >= 0; i--)
                {
                    if (data[i] != 0)
                    {
                        lastByte = i;
                        break;
                    }
                }
                string dataString = BitConverter.ToString(data);
                string dataCleaned = "";
                for (int i = 0; i < dataString.Length; i++)
                {
                    if (dataString[i] == '-')
                        dataCleaned += " ";
                    else dataCleaned += dataString[i];
                }
                MessageBox.Show("Получен ответ от устройства: " + dataCleaned, "Успех", MessageBoxButtons.OK);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            port.Close();
        }
    }
}   

public enum FunctionCode { Coil, DiscreteInput, HoldingRegister, InputRegister}
