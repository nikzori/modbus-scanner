﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Modbus_Tools
{
    public partial class App : Form
    {
        public static int[] BaudRate = new int[] { 12, 24, 48, 96, 144, 192, 384, 576, 1152 };
        public static int[] DataBits = new int[] { 7, 8 };
        //Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

        byte[] message = new byte[255];
        public bool isAwaitingResponse = false;
        public static bool isPolling = false;
        public ModbusResponseEventArgs latestMessage;


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

        DateTime dateTime;
        Stopwatch stopwatch = new Stopwatch();
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
            //CBox_Function.Items.Add("0F Write Multiple Coils");
            //CBox_Function.Items.Add("10 Write Multiple Registers");
            CBox_Function.SelectedItem = CBox_Function.Items[0];


            CBox_BaudRate.Items.Add("1200");
            CBox_BaudRate.Items.Add("2400");
            CBox_BaudRate.Items.Add("4800");
            CBox_BaudRate.Items.Add("9600");
            CBox_BaudRate.Items.Add("14400");
            CBox_BaudRate.Items.Add("19200");
            CBox_BaudRate.Items.Add("38400");
            CBox_BaudRate.Items.Add("57600");
            CBox_BaudRate.Items.Add("115200");
            CBox_BaudRate.SelectedIndex = 3;

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

            UpDown_RegLength.Value = 1;

            if (Directory.GetDirectories(Application.StartupPath).Contains(Application.StartupPath + "\\Configs") == false)
            {

                MessageBox.Show("Приложение не нашло стандартную папку для конфигураций. Была создана папка 'Configs' в папке с приложением.");
                Directory.CreateDirectory(Application.StartupPath + "\\Configs");
                Console.WriteLine("New initial directory for OpenFile: " + ofd.InitialDirectory);

            }
            ofd.InitialDirectory = Application.StartupPath + "\\Configs";
            path = defaultPath;
            UpdatePathLabel();

            /* - Version Check - */
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            Console.WriteLine("Version: " + version);
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

        /// <summary>
        /// Update port parameters according to values in UI.
        /// </summary>
        public void PortSetup()
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
            port.BaudRate = BaudRate[CBox_BaudRate.SelectedIndex] * 100;
            port.Parity = Parity.None;
            port.DataBits = DataBits[CBox_DataBits.SelectedIndex];
            port.StopBits = (StopBits)CBox_StopBits.SelectedIndex;

            port.ReadTimeout = 3000;
            port.WriteTimeout = 3000;
            port.ReadBufferSize = 8192;

            message = new byte[255];
            port.Open();

            Modbus.slaveID = (byte)UpDown_ModbusID.Value;
        }

        private async void ButtonConnect_Click(object sender, EventArgs e)
        {
            if (isPolling)
                return;
            Modbus.slaveID = (byte)UpDown_ModbusID.Value;
            device = null;
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
            if (UpDown_ModbusID.Value == 0)
            {
                DialogResult res = MessageBox.Show("Указан Modbus ID 0 — глобальное вещание. Если в линии находится больше одного ведомого устройства, возникнут проблемы с коллизией. Продолжить?", "Внимание", MessageBoxButtons.OKCancel);
                if (res == DialogResult.Cancel)
                    return;
            }

            PortSetup();
            await Task.Run(() =>
            {
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

                    try
                    {
                        Console.WriteLine("Deserializing `.json`;");
                        device = JsonConvert.DeserializeObject<Device>(fileContent);
                        Label_ConfigTip.Invoke(new MethodInvoker(delegate { Label_ConfigTip.Text = device.name; }));
                    }
                    catch (Exception err) { MessageBox.Show(err.Message); }
                    progressBar1.Invoke(new MethodInvoker(delegate { progressBar1.Value = 100; }));
                    try
                    {
                        Console.WriteLine("Connecting to the device;");
                        AddLog("Попытка подключиться к устройству " + device.name);
                        bool isCorrect = false;
                        if (Read((FunctionCode)device.checkEntry.registerType, device.checkEntry.address, device.checkEntry.length))
                        {
                            switch (device.checkEntry.dataType)
                            {
                                case "string":
                                    if (ByteArrayToUnicode(latestMessage.Data) == device.checkEntry.expectedValue)
                                        isCorrect = true;
                                    break;
                                case "uint16":
                                    if (ByteArrayToUint(latestMessage.Data) == Int32.Parse(device.checkEntry.expectedValue))
                                        isCorrect = true;
                                    break;
                                case "uint32":
                                    if (ByteArrayToUint(latestMessage.Data) == Int32.Parse(device.checkEntry.expectedValue))
                                        isCorrect = true;
                                    break;
                            }
                            if (!isCorrect)
                            {
                                if (MessageBox.Show("Устройство ответило на запрос проверочного значения, но ответ не совпадает с ожидаемым значением. Продолжить?", "Неподходящий ответ устройства", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                    return;
                            }
                        }
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


                    foreach (CheckEntry ce in juju.Keys)
                    {
                        if (Read((FunctionCode)ce.registerType, ce.address, ce.length))  // send read request to device; error check
                        {
                            byte[] data = latestMessage.Data;

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
                                return;
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
                    }
                    if (device is /* still */ null)
                    {
                        MessageBox.Show("Ни один из файлов конфигурации не подходит для устройства.");
                        return;
                    }

                    progressBar1.Invoke(new MethodInvoker(delegate { progressBar1.Value = 100; }));
                }
            });
            if (device != null)
            {
                try
                {
                    AddLog("Попытка подключиться к устройству " + device.name);
                    datasheet = new Datasheet(CBox_BaudRate.SelectedIndex);
                    datasheet.Show();
                }
                catch (Exception err) { MessageBox.Show(err.Message); }
            }
        }

        void CBox_Ports_Click(object sender, EventArgs e)
        {
            CBox_Ports.Items.Clear();
            CBox_Ports.Items.AddRange(SerialPort.GetPortNames());
        }

        private async void Button_SendCommand_Click(object sender, EventArgs e)
        {
            if (isPolling)
                return;

            PortSetup();
            try
            {
                int functionCode = CBox_Function.SelectedIndex + 1;
                
                short address;
                ushort length = (ushort)UpDown_RegLength.Value;
                byte[] _msg = new byte[8];

                if (Int16.TryParse(TBox_RegAddress.Text, out address))
                {
                    if (functionCode <= 4)
                    {
                        await Task.Run(() => Read((FunctionCode)functionCode, (ushort)address, length));
                    }
                    else
                    {
                        string valueLower = TBox_RegValue.Text.ToLower();
                        Console.WriteLine("valueLower: " + valueLower);
                        ushort value = 0x00_00;
                        await Task.Run(() =>
                        {
                            switch ((FunctionCode)functionCode)
                            {
                                case (FunctionCode.WriteCoil):
                                    if (valueLower == "true" || valueLower == "1")
                                        value = 0xFF_00;
                                    else if (valueLower == "false" || valueLower == "0")
                                        value = 0x00_00;
                                    else 
                                    { 
                                        MessageBox.Show("Неподходящие значения для регистра типа Coil");
                                        return;
                                    }
                                    Write((FunctionCode)functionCode, (ushort)address, value);
                                    break;
                                case (FunctionCode.WriteRegister):
                                    bool canWrite = false;
                                    if (IsDec(valueLower))
                                    {
                                        try { value = (ushort)Convert.ToInt16(valueLower); canWrite = true; }
                                        catch (Exception err) { MessageBox.Show(err.Message); }
                                    }
                                    else if (IsHex(valueLower))
                                    {
                                        Console.WriteLine("Got hex value");
                                        for (int i = 0; i < valueLower.Length; i++)
                                        {
                                            if (valueLower[i] == 'x')
                                            {
                                                valueLower = valueLower.Remove(i, 1);
                                                break;
                                            }
                                        }
                                        try { value = (ushort)Convert.ToInt16(valueLower, 16); canWrite = true; }
                                        catch (Exception err) { MessageBox.Show(err.Message); }
                                    }
                                    else if (IsBin(valueLower))
                                    {
                                        Console.WriteLine("Got bin value");
                                        for (int i = 0; i < valueLower.Length; i++)
                                        {
                                            if (valueLower[i] == 'b')
                                            {
                                                valueLower = valueLower.Remove(i, 1);
                                                break;
                                            }
                                        }
                                        try { value = (ushort)Convert.ToInt16(valueLower, 2); canWrite = true; }
                                        catch (Exception err) { MessageBox.Show(err.Message); }
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
                                    else
                                    {
                                        MessageBox.Show("Неподходящие значения для регистра типа Input Register");
                                        break;
                                    }

                                    if (canWrite)
                                        Write((FunctionCode)functionCode, (ushort)address, (ushort)value);
                                    break;
                                default:
                                    MessageBox.Show("WIP");
                                    break;
                            }
                        });
                    }

                }
                string msg = Modbus.ByteArrayToString(_msg);
            }
            catch (Exception err) { MessageBox.Show(err.Message, "Setting Register Error"); }
        }

        private void OnSelectedFunctionChanged(object sender, EventArgs e)
        {
            if (CBox_Function.SelectedIndex < 4)
                TBox_RegValue.Enabled = false;
            else TBox_RegValue.Enabled = true;
            if (CBox_Function.SelectedIndex == 4 || CBox_Function.SelectedIndex == 5)
                UpDown_RegLength.Enabled = false;
            else UpDown_RegLength.Enabled = true;
        }

        private void LoadConfig(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                path = ofd.FileName;
                Label_ConfPath.Invoke(new MethodInvoker(delegate { Label_ConfPath.Text = ofd.FileName; }));

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

        bool Read(FunctionCode functionCode, ushort address, ushort length)
        {
            isPolling = true;
            bool res = false;

            try
            {
                latestMessage = null;
                isAwaitingResponse = true;
                int attempts = 0;
                byte[] _msg;

                Modbus.Read(Modbus.slaveID, functionCode, address, length, out _msg);
                AddLog("Отправка сообщения: " + Modbus.ByteArrayToString(_msg));
                stopwatch.Restart();

                while (isAwaitingResponse && latestMessage is null)
                {
                    if (stopwatch.ElapsedMilliseconds > port.ReadTimeout)
                    {
                        attempts++;
                        if (attempts > 2)
                        {
                            AddLog("Устройство не отвечает. Проверьте соединение с устройством.");
                            break;
                        }
                        AddLog("Истекло время ожидания ответа от устройства. Повторный запрос.");
                        Modbus.Read(Modbus.slaveID, functionCode, address, length);
                        stopwatch.Restart();
                    }
                }

                if (latestMessage != null && latestMessage.Status != ModbusStatus.Error)
                    res = true;
            }
            catch (Exception err)
            {
                port.Close();
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Console.WriteLine("Poll attempt finished");
            isPolling = false;
            return res;
        }

        bool Write(FunctionCode functionCode, ushort address, ushort value)
        {
            isPolling = true;
            bool res = false;

            try
            {
                latestMessage = null;
                isAwaitingResponse = true;
                int attempts = 0;
                byte[] _msg;
                Modbus.WriteSingle(functionCode, address, value, out _msg);
                AddLog("Отправка сообщения: " + Modbus.ByteArrayToString(_msg));
                stopwatch.Restart();

                while (isAwaitingResponse && latestMessage is null)
                {
                    if (stopwatch.ElapsedMilliseconds > port.ReadTimeout)
                    {
                        attempts++;
                        if (attempts > 2)
                        {
                            AddLog("Устройство не отвечает. Проверьте соединение с устройством.");
                            break;
                        }
                        AddLog("Истекло время ожидания ответа от устройства. Повторный запрос.");
                        Modbus.WriteSingle(functionCode, address, value);
                        stopwatch.Restart();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Register Write Error");
            }
            isPolling = false;
            return res;
        }

        void OnResponseReceived(object sender, ModbusResponseEventArgs e)
        {
            isAwaitingResponse = false;
            latestMessage = e;
            AddLog("Получен ответ: " + Modbus.ByteArrayToString(e.Message));
            switch (e.Status)
            {
                case (ModbusStatus.ReadSuccess):
                    string values = "";
                    if (e.Message[1] <= 0x02)
                    {
                        for (int i = 0; i < e.Data.Length; i++)
                        {
                            values += Convert.ToString(e.Data[i], 2).PadLeft(8, '0') + " ";
                        }
                        AddLog("Bin: " + values);
                    }
                    if (e.Message[1] == 0x03 || e.Message[1] == 0x04)
                    {
                        for (int i = 0; i < e.Data.Length; i += 2)
                        {
                            values += ((e.Data[i] << 8) + e.Data[i + 1]).ToString() + "; ";
                        }
                        AddLog("Dec: " + values);
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
            TextBox_Log.Invoke((MethodInvoker)delegate { TextBox_Log.AppendText(Environment.NewLine + "[" + dateTime.Hour.ToString().PadLeft(2, '0') + ":" + dateTime.Minute.ToString().PadLeft(2, '0') + ":" + dateTime.Second.ToString().PadLeft(2, '0') + "] " + message); });
        }

        #region Parsers
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
        public static bool IsBin(string str)
        {
            str = str.ToLower();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != '0' && str[i] != '1')
                {
                    if ((i == 0 || i == 1) && str[i] == 'b')
                        continue;
                    else return false;
                }
            }
            return true;
        }

        //this assumes that every byte in the array is a single ASCII character
        public static string ByteArrayToUnicode(byte[] input)
        {
            List<char> result = new List<char>();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == 0x00)
                    continue;
                else result.Add(Convert.ToChar(input[i]));
            }
            return new string(result.ToArray());
        }

        public static int ByteArrayToUint(byte[] input)
        {
            if (input.Length > 4)
                MessageBox.Show("Числовые значения больше 4 байт не поддерживаются");
            int result = 0;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                result += input[i] << (i * 8);
            }
            return result;
        }
        #endregion

    }
}

public class LogEventArgs : EventArgs
{
    public string Message { get; set; }
    public LogEventArgs(string message)
    {
        Message = message;
    }
}

public enum FunctionCode
{ ReadCoil = 1, ReadDiscrete = 2, ReadHolding = 3, ReadInput = 4, WriteCoil = 5, WriteRegister = 6, WriteMultCoils = 15, WriteMultRegisters = 16 };
public enum SelectedPath { File, Folder };
