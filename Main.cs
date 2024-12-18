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
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Threading;


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

        byte[] message = new byte[255];
        public bool isAwaitingResponse = false;
        public short[] res = new short[12];
        SerialPort port = Modbus.port;
        public int expectedLength = 0;
        Datasheet datasheet;
        public SelectedPath selectedPath = SelectedPath.Folder;

        public static Device device; // Deserialized .json object
        string path = String.Empty; // Path to selected file/folder
        string defaultPath = Application.StartupPath + "\\Configs"; // Default folder path
        OpenFileDialog ofd = new OpenFileDialog();
        FolderBrowserDialog fbd = new FolderBrowserDialog();

        Dictionary<CheckEntry, List<Device>> juju = new Dictionary<CheckEntry, List<Device>>(); // dictionary for device identification
        string[] configs;

        public byte[] latestMessage;

        DateTime dateTime;
        #region Initialization
        public App()
        {
            InitializeComponent();
            Modbus.Init();
            Modbus.ResponseReceived += OnResponseReceived;
            ofd.InitialDirectory = Application.StartupPath;
            ofd.Filter = "JSON files (*.json)|*.json";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;

            this.UpDown_ModbusID.Value = 30;
            TextBox_Log.Text = "Приложение готово к работе.";

            CBox_Function.Items.Add("01 Read Coil");
            CBox_Function.Items.Add("02 Read Discrete Input");
            CBox_Function.Items.Add("03 Read Holding Register");
            CBox_Function.Items.Add("04 Read Input Register");
            CBox_Function.Items.Add("05 Write Single Coil");
            CBox_Function.Items.Add("06 Write Single Register");
            CBox_Function.Items.Add("0F Write Multiple Coils");
            CBox_Function.Items.Add("10 Write Multiple Registers");
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

            UpDown_RegLength.Value = 0;

            Radio_SerialPort.Checked = true;
            GBox_Ethernet.Enabled = false;
            TBox_IP.Text = "192.168.3.7";
            TBox_Port.Text = "8887";
            TBox_Timeout.Text = "3";
            if (Directory.GetDirectories(Application.StartupPath).Contains(Application.StartupPath + "\\Configs") == false)
            {
                Task.Delay(1500).ContinueWith(t =>
                {
                    MessageBox.Show("Приложение не нашло стандартную папку для конфигураций. Была создана папка 'Configs' в папке с приложением.");
                    Directory.CreateDirectory(Application.StartupPath + "\\Configs");
                    Console.WriteLine("New initial directory for OpenFile: " + ofd.InitialDirectory);
                });
            }
            ofd.InitialDirectory = Application.StartupPath + "\\Configs";
            path = defaultPath;
            UpdatePathLabel();
        }
        void UpdatePathLabel()
        {
            Label_ConfPath.Text = path;
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
        #endregion

        // Send a custom message
        async Task ReadRegisterAsync(FunctionCode functionCode, ushort address, ushort length)
        {
            if (CBox_Ports.Text == "")
                MessageBox.Show("Необходимо выбрать COM порт.", "Ошибка", MessageBoxButtons.OK);
            if (UpDown_ModbusID.Value == 0)
                MessageBox.Show("Глобальное вещание пока не поддерживается");

            /* - Port Setup - */
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


            message = new byte[255];
            port.Open();


            /* - Reading from Registers - */
            if (CBox_Function.SelectedIndex < 4)
            {
                try
                {
                    await Modbus.ReadRegAsync(port, (byte)UpDown_ModbusID.Value, functionCode, address, length);
                    isAwaitingResponse = true;
                    await Task.Delay(port.ReadTimeout).ContinueWith(_ =>
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
        }

        private async void ButtonConnect_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            if (path == String.Empty)
            {
                MessageBox.Show("Выберите конфигурацию для подключения и опроса устройства.");
                return;
            }
            if (CBox_Ports.SelectedItem.ToString() == "COM1")
            {
                DialogResult res = MessageBox.Show("Выбран серийный порт COM1, который обычно является портом PS/2 или RS-232, не подключенным к Modbus устройству. Продолжить?", "Внимание", MessageBoxButtons.OKCancel);
                if (res == DialogResult.Cancel)
                    return;
            }

            /* - Port Setup - */
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


            message = new byte[255];
            port.Open();

            /* - Checking - */
            if (selectedPath == SelectedPath.File)
            {
                var fileContent = string.Empty;
                var filePath = string.Empty;
                //Read the contents of the file into a stream
                var fileStream = ofd.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                }
                progressBar1.Value = 100;

                try
                {
                    device = JsonConvert.DeserializeObject<Device>(fileContent);
                    Label_ConfigTip.Text = device.name;
                }
                catch (Exception err) { MessageBox.Show(err.Message); }

                try
                {
                    AddLog("Попытка подключиться к устройству " + device.name);
                    datasheet = new Datasheet((byte)UpDown_ModbusID.Value);
                    datasheet.Show();
                }
                catch (Exception err) { MessageBox.Show(err.Message); }
            }
            else
            {
                string[] _configs = Directory.GetFiles(path, "*.json");
                if (configs != _configs)
                {
                    // something changed in the config folder, or we haven't gone through configs, 
                    // remake the dictionary
                    configs = _configs;
                    juju = new Dictionary<CheckEntry, List<Device>>();

                    var fileContent = string.Empty;
                    FileStream fileStream;
                    Device _device;

                    foreach (string path in configs)
                    {
                        fileStream = File.OpenRead(path);
                        using (StreamReader reader = new StreamReader(fileStream))
                            fileContent = reader.ReadToEnd();
                        // get device object from .json
                        _device = JsonConvert.DeserializeObject<Device>(fileContent);

                        // compare device object to key of each dictionary;
                        // add to that dictionary if device's check entry registers match the key
                        bool matched = false;
                        foreach (CheckEntry ce in juju.Keys)
                        {
                            if (_device.checkEntry.address == ce.address && _device.checkEntry.length == ce.length && _device.checkEntry.dataType == ce.dataType)
                            {
                                juju[ce].Add(_device);
                                matched = true;
                                break;
                            }
                        }
                        if (!matched)
                        {
                            juju.Add(_device.checkEntry, new List<Device>());
                            juju[_device.checkEntry].Add(_device);
                        }
                    } // all configs are sorted out, we can poll for each checkEntry 
                }
                // setup event listener
                byte[] message = null;
                Modbus.ResponseReceived += (sndr, msg) =>
                {
                    message = msg.Message;
                };

                foreach (CheckEntry ce in juju.Keys)
                {
                    await ReadRegisterAsync((FunctionCode)ce.registerType, ce.address, ce.length);   // send read request to device,

                    while (message is null) // wait for response to arrive
                        Thread.Sleep(10);

                    if (message[1] > 0x10) // checking for exception code
                        continue;
                    else
                    {
                        // get pure data
                        byte[] data = new byte[message[2]];
                        for (int i = 0; i < data.Length; i++)
                            data[i] = message[i + 3];

                        if (ce.dataType == "string")
                        {
                           
                            List<byte> bytes = new List<byte>();
                            for (int i = 0; i < data.Length; i++)
                            {
                                if (data[i] != 0)       // clean empty bytes from 16-bit registers
                                    bytes.Add(data[i]); 
                            }
                            string value = Encoding.UTF8.GetString(bytes.ToArray());
                            foreach (Device dev in juju[ce])
                            { 
                                if (dev.checkEntry.expectedValue == value)
                                {
                                    Console.WriteLine("It's a match!");
                                    device = dev;
                                    break;
                                }
                            }
                        }
                        else if (ce.dataType == "bool") 
                        {
                            // why would you even do that lmao
                        }
                        else if (ce.dataType == "uint16" || ce.dataType == "uint32")
                        {
                            byte[] _data = data;
                            Array.Reverse(_data);
                            if (ce.dataType == "uint16")
                            {
                                ushort value = BitConverter.ToUInt16(_data, 0);
                                foreach (Device dev in juju[ce])
                                {
                                    short expValue;
                                    if (Int16.TryParse(dev.checkEntry.expectedValue, out expValue) && expValue == value)
                                    {
                                        device = dev;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                int value = BitConverter.ToInt32(_data, 0);
                                foreach (Device dev in juju[ce])
                                {
                                    int expValue;
                                    if (Int32.TryParse(dev.checkEntry.expectedValue, out expValue) && expValue == value)
                                    {
                                        device = dev;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (!(device is null)) // found the correct device, abort loop
                        break;
                    message = null; // clear the array for the next item in case we haven't found the correct value
                }
                if (device is /* still */ null)
                {
                    // none of the configs match the device responses
                    MessageBox.Show("Ни один из файлов конфигурации не подходит для устройства.");
                    return;
                }
                else
                {
                    try
                    {
                        AddLog("Попытка подключиться к устройству " + device.name);
                        datasheet = new Datasheet((byte)UpDown_ModbusID.Value);
                        datasheet.Show();
                    }
                    catch (Exception err) { MessageBox.Show(err.Message); }
                }
                progressBar1.Value = 100;
            }

            /*
            if (Radio_SerialPort.Checked)
                await ReadRegisterAsync(FunctionCode.InputRegister, 200, 6);
            //else EthernetParse();
            */
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
                Modbus.BuildReadMessage(0x1E, 0x03, 128, 1, ref data);
                AddLog("Sending to " + ipText + ":" + portText + ":" + Modbus.ByteArrayToString(data));

                // set up an event listener to receive the response
                await SocketDataTransfer(data);
            }

        }

        void CBox_Ports_Click(object sender, EventArgs e)
        {
            CBox_Ports.Items.Clear();
            CBox_Ports.Items.AddRange(SerialPort.GetPortNames());
        }

        void OnResponseReceived(object sender, ModbusResponseEventArgs e)
        {
            isAwaitingResponse = false;
            AddLog("Получен ответ: " + Modbus.ByteArrayToString(e.Message));
            switch(e.Status)
            {
                case (ModbusStatus.ReadSuccess):
                    if (e.Message[1] == 0x03 || e.Message[1] == 0x04)
                    {
                        AddLog("Unicode: " + ByteArrayToUnicode(e.Data));

                    } 
                    break;
                case (ModbusStatus.WriteSuccess):
                    AddLog("Write success;");
                    break;
                case (ModbusStatus.Error):
                    string errorDesc;
                    switch (e.Message[2])
                    {
                        case (0x01):
                            errorDesc = "01 - Illegal Function";
                            break;
                        case (0x02):
                            errorDesc = "02 - Illegal Data Address";
                            break;
                        case (0x03):
                            errorDesc = "03 - Illegal Data Value";
                            break;
                        case (0x04):
                            errorDesc = "04 - Slave Device Failure";
                            break;
                        case (0x05):
                            errorDesc = "05 - Acknowledge";
                            break;
                        case (0x06):
                            errorDesc = "06 - Slave Device Busy";
                            break;
                        case (0x07):
                            errorDesc = "07 - Negative Acknowledge";
                            break;
                        case (0x08):
                            errorDesc = "08 - Memory Parity Error";
                            break;
                        case (0x0A):
                            errorDesc = "10 - Gateway Path Unavailable";
                            break;
                        case (0x0B):
                            errorDesc = "11 - Gateway Target Device Failed to Respond";
                            break;
                        default:
                            errorDesc = "Unknown error code";
                            break;
                    }
                    AddLog("Error code: " + errorDesc);
                    break;

            }
        }

        void AddLog(string message)
        {
            dateTime = DateTime.Now;
            TextBox_Log.Invoke((MethodInvoker)delegate { TextBox_Log.AppendText(Environment.NewLine + "[" + dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second + "] " + message); });
        }

        private async void Button_SendCommand_Click(object sender, EventArgs e)
        {
            /* - Port Setup - */
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


            message = new byte[255];
            port.Open();


            int functionCode = CBox_Function.SelectedIndex + 1;
            short address;
            ushort length = (ushort)UpDown_RegLength.Value;
            if (Int16.TryParse(TBox_RegAddress.Text, out address))
            {
                if (functionCode <= 4)
                    await ReadRegisterAsync((FunctionCode)functionCode, (ushort)address, length);
                else
                {
                    string valueLower = TBox_RegValue.Text.ToLower();
                    switch ((FunctionCode)functionCode)
                    {
                        case (FunctionCode.WriteCoil):
                            Console.WriteLine("Trying to force single coil");
                            if (valueLower == "true" || valueLower == "1")
                                await Modbus.WriteSingleAsync(port, (FunctionCode)functionCode, (byte)UpDown_ModbusID.Value, (ushort)address, 0xFF_00);
                            else if (valueLower == "false" || valueLower == "0")
                                await Modbus.WriteSingleAsync(port, (FunctionCode)functionCode, (byte)UpDown_ModbusID.Value, (ushort)address, 0x00_00);
                            else MessageBox.Show("Неподходящие значения для регистра типа Coil");
                            break;
                        case (FunctionCode.WriteRegister):
                            short value = 0x00_00;
                            bool canWrite = false;
                            if (IsHex(valueLower)) //assume this is hex
                            {
                                try { value = Convert.ToInt16(valueLower, 16); canWrite = true; }
                                catch (Exception err) { MessageBox.Show(err.Message); }
                                break;
                            }
                            else if (IsDec(valueLower))
                            {
                                try { value = Convert.ToInt16(valueLower); canWrite = true; }
                                catch (Exception err) { MessageBox.Show(err.Message); }
                                break;
                            }
                            else if (valueLower == "true" || valueLower == "1")
                            {
                                value = 0x00_01;
                                canWrite = true;
                            }
                            else if (valueLower == "false" || valueLower == "0")
                            {
                                canWrite = true;
                            }
                            if (canWrite)
                            {
                                _ = Modbus.WriteSingleAsync(port, (FunctionCode)functionCode, (byte)UpDown_ModbusID.Value, (ushort)address,
                                        (ushort)value);
                            }
                            else MessageBox.Show("Неподходящие значения для регистра типа Input Register");
                            break;
                        default:
                            break;

                    }
                }
            }
        }

        private void OnSelectedFunctionChanged(object sender, EventArgs e)
        {
            if (CBox_Function.SelectedIndex < 4)
                TBox_RegValue.Enabled = false;
            else TBox_RegValue.Enabled = true;
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
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                path = ofd.FileName;
                Label_ConfPath.Text = ofd.FileName;

                selectedPath = SelectedPath.File;
            }
            UpdatePathLabel();
        }

        private void LoadFolder(object sender, EventArgs e)
        {

            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                path = fbd.SelectedPath;
                Label_ConfPath.Text = fbd.SelectedPath;
                selectedPath = SelectedPath.Folder;
            }
            UpdatePathLabel();
        }

        private void CBox_Ports_Click(object sender, MouseEventArgs e)
        {

        }

        public static bool IsHex(string str)
        {
            str = str.ToLower();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] < '0' || str[i] > 'F')
                {
                    if ((i == 0 || i == 1) && str[i] == 'x') 
                        continue;
                    else return false;
                }
            }
            return true;
        }
        public static bool IsDec(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
        public static string ByteArrayToUnicode(byte[] input) 
        {
            // stupid fucking WinForm textbox breaks from null symbols
            // stupid fucking Encoding class does byte-by-byte conversion
            List<char> result = new List<char>(input.Length/2);
            byte[] flip = input;
            Array.Reverse(flip);
            for (int i = 0; i < flip.Length; i += 2)
            {
                result.Add(BitConverter.ToChar(flip, i));
            }
            result.Reverse();
            return new string(result.ToArray());
        }
    }
}

public enum FunctionCode { ReadCoil = 1, ReadDiscrete = 2, ReadHolding = 3, ReadInput = 4, WriteCoil = 5, WriteRegister = 6, WriteMultCoils = 15, WriteMultRegisters = 16 };
public enum SelectedPath { File, Folder };
