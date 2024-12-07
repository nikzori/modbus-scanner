﻿namespace Gidrolock_Modbus_Scanner
{
    partial class App
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App));
            this.GBox_Serial = new System.Windows.Forms.GroupBox();
            this.panel11 = new System.Windows.Forms.Panel();
            this.CBox_Parity = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.CBox_StopBits = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.CBox_DataBits = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.CBox_BaudRate = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.UpDown_ModbusID = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CBox_Ports = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Button_Connect = new System.Windows.Forms.Button();
            this.TextBox_Log = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.UpDown_Value = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.UpDown_RegLength = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.Button_SendCommand = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.UpDown_RegAddress = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.CBox_Function = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.GBox_Ethernet = new System.Windows.Forms.GroupBox();
            this.panel16 = new System.Windows.Forms.Panel();
            this.TBox_Port = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.panel17 = new System.Windows.Forms.Panel();
            this.TBox_Timeout = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.panel18 = new System.Windows.Forms.Panel();
            this.TBox_IP = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Radio_Ethernet = new System.Windows.Forms.RadioButton();
            this.Radio_SerialPort = new System.Windows.Forms.RadioButton();
            this.Button_LoadConfig = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.Label_Config = new System.Windows.Forms.Label();
            this.GBox_Serial.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_ModbusID)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_Value)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_RegLength)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_RegAddress)).BeginInit();
            this.panel4.SuspendLayout();
            this.GBox_Ethernet.SuspendLayout();
            this.panel16.SuspendLayout();
            this.panel17.SuspendLayout();
            this.panel18.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBox_Serial
            // 
            this.GBox_Serial.Controls.Add(this.panel11);
            this.GBox_Serial.Controls.Add(this.panel10);
            this.GBox_Serial.Controls.Add(this.panel8);
            this.GBox_Serial.Controls.Add(this.panel7);
            this.GBox_Serial.Controls.Add(this.panel2);
            this.GBox_Serial.Controls.Add(this.panel1);
            this.GBox_Serial.Location = new System.Drawing.Point(12, 12);
            this.GBox_Serial.Name = "GBox_Serial";
            this.GBox_Serial.Size = new System.Drawing.Size(490, 65);
            this.GBox_Serial.TabIndex = 0;
            this.GBox_Serial.TabStop = false;
            this.GBox_Serial.Text = "Серийный порт";
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.CBox_Parity);
            this.panel11.Controls.Add(this.label11);
            this.panel11.Location = new System.Drawing.Point(330, 19);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(79, 43);
            this.panel11.TabIndex = 6;
            // 
            // CBox_Parity
            // 
            this.CBox_Parity.FormattingEnabled = true;
            this.CBox_Parity.Location = new System.Drawing.Point(6, 17);
            this.CBox_Parity.Name = "CBox_Parity";
            this.CBox_Parity.Size = new System.Drawing.Size(70, 21);
            this.CBox_Parity.TabIndex = 2;
            this.CBox_Parity.Text = "8";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(103, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Контроль четности";
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.CBox_StopBits);
            this.panel10.Controls.Add(this.label10);
            this.panel10.Location = new System.Drawing.Point(247, 19);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(79, 43);
            this.panel10.TabIndex = 5;
            // 
            // CBox_StopBits
            // 
            this.CBox_StopBits.FormattingEnabled = true;
            this.CBox_StopBits.Location = new System.Drawing.Point(6, 17);
            this.CBox_StopBits.Name = "CBox_StopBits";
            this.CBox_StopBits.Size = new System.Drawing.Size(70, 21);
            this.CBox_StopBits.TabIndex = 2;
            this.CBox_StopBits.Text = "8";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Стоп-биты";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.panel9);
            this.panel8.Controls.Add(this.CBox_DataBits);
            this.panel8.Controls.Add(this.label8);
            this.panel8.Location = new System.Drawing.Point(162, 19);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(79, 43);
            this.panel8.TabIndex = 4;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.comboBox1);
            this.panel9.Controls.Add(this.label9);
            this.panel9.Location = new System.Drawing.Point(82, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(79, 43);
            this.panel9.TabIndex = 5;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(70, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.Text = "8";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Биты данных";
            // 
            // CBox_DataBits
            // 
            this.CBox_DataBits.FormattingEnabled = true;
            this.CBox_DataBits.Location = new System.Drawing.Point(6, 17);
            this.CBox_DataBits.Name = "CBox_DataBits";
            this.CBox_DataBits.Size = new System.Drawing.Size(70, 21);
            this.CBox_DataBits.TabIndex = 2;
            this.CBox_DataBits.Text = "8";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Биты данных";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.CBox_BaudRate);
            this.panel7.Controls.Add(this.label7);
            this.panel7.Location = new System.Drawing.Point(81, 19);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(75, 43);
            this.panel7.TabIndex = 3;
            // 
            // CBox_BaudRate
            // 
            this.CBox_BaudRate.FormattingEnabled = true;
            this.CBox_BaudRate.Location = new System.Drawing.Point(6, 17);
            this.CBox_BaudRate.Name = "CBox_BaudRate";
            this.CBox_BaudRate.Size = new System.Drawing.Size(65, 21);
            this.CBox_BaudRate.TabIndex = 2;
            this.CBox_BaudRate.Text = "9600";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Боды";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.UpDown_ModbusID);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(415, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(75, 43);
            this.panel2.TabIndex = 2;
            // 
            // UpDown_ModbusID
            // 
            this.UpDown_ModbusID.Location = new System.Drawing.Point(6, 17);
            this.UpDown_ModbusID.Name = "UpDown_ModbusID";
            this.UpDown_ModbusID.Size = new System.Drawing.Size(66, 20);
            this.UpDown_ModbusID.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Modbus ID";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CBox_Ports);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(75, 43);
            this.panel1.TabIndex = 0;
            // 
            // CBox_Ports
            // 
            this.CBox_Ports.FormattingEnabled = true;
            this.CBox_Ports.Location = new System.Drawing.Point(6, 17);
            this.CBox_Ports.Name = "CBox_Ports";
            this.CBox_Ports.Size = new System.Drawing.Size(65, 21);
            this.CBox_Ports.TabIndex = 1;
            this.CBox_Ports.Text = "COM1";
            this.CBox_Ports.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CBox_Ports_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Порт";
            // 
            // Button_Connect
            // 
            this.Button_Connect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Connect.Location = new System.Drawing.Point(402, 155);
            this.Button_Connect.Name = "Button_Connect";
            this.Button_Connect.Size = new System.Drawing.Size(100, 25);
            this.Button_Connect.TabIndex = 4;
            this.Button_Connect.Text = "Подключиться";
            this.Button_Connect.UseVisualStyleBackColor = true;
            this.Button_Connect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // TextBox_Log
            // 
            this.TextBox_Log.Location = new System.Drawing.Point(12, 192);
            this.TextBox_Log.Multiline = true;
            this.TextBox_Log.Name = "TextBox_Log";
            this.TextBox_Log.ReadOnly = true;
            this.TextBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox_Log.Size = new System.Drawing.Size(490, 118);
            this.TextBox_Log.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel6);
            this.groupBox2.Controls.Add(this.panel5);
            this.groupBox2.Controls.Add(this.Button_SendCommand);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Location = new System.Drawing.Point(12, 316);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(490, 63);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Команды";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.UpDown_Value);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Location = new System.Drawing.Point(337, 20);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(75, 43);
            this.panel6.TabIndex = 4;
            // 
            // UpDown_Value
            // 
            this.UpDown_Value.Location = new System.Drawing.Point(6, 17);
            this.UpDown_Value.Name = "UpDown_Value";
            this.UpDown_Value.Size = new System.Drawing.Size(66, 20);
            this.UpDown_Value.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Значение";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.UpDown_RegLength);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Location = new System.Drawing.Point(259, 20);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(75, 43);
            this.panel5.TabIndex = 3;
            // 
            // UpDown_RegLength
            // 
            this.UpDown_RegLength.Location = new System.Drawing.Point(6, 17);
            this.UpDown_RegLength.Name = "UpDown_RegLength";
            this.UpDown_RegLength.Size = new System.Drawing.Size(66, 20);
            this.UpDown_RegLength.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Длина";
            // 
            // Button_SendCommand
            // 
            this.Button_SendCommand.Location = new System.Drawing.Point(415, 35);
            this.Button_SendCommand.Name = "Button_SendCommand";
            this.Button_SendCommand.Size = new System.Drawing.Size(71, 23);
            this.Button_SendCommand.TabIndex = 4;
            this.Button_SendCommand.Text = "Отправить";
            this.Button_SendCommand.UseVisualStyleBackColor = true;
            this.Button_SendCommand.Click += new System.EventHandler(this.Button_SendCommand_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.UpDown_RegAddress);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(162, 20);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(91, 43);
            this.panel3.TabIndex = 2;
            // 
            // UpDown_RegAddress
            // 
            this.UpDown_RegAddress.Location = new System.Drawing.Point(6, 17);
            this.UpDown_RegAddress.Name = "UpDown_RegAddress";
            this.UpDown_RegAddress.Size = new System.Drawing.Size(82, 20);
            this.UpDown_RegAddress.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Адрес";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.CBox_Function);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Location = new System.Drawing.Point(0, 20);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(156, 43);
            this.panel4.TabIndex = 0;
            // 
            // CBox_Function
            // 
            this.CBox_Function.FormattingEnabled = true;
            this.CBox_Function.Location = new System.Drawing.Point(6, 17);
            this.CBox_Function.Name = "CBox_Function";
            this.CBox_Function.Size = new System.Drawing.Size(147, 21);
            this.CBox_Function.TabIndex = 1;
            this.CBox_Function.Text = "01 - Read Coil";
            this.CBox_Function.SelectedIndexChanged += new System.EventHandler(this.OnSelectedFunctionChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Функция";
            // 
            // GBox_Ethernet
            // 
            this.GBox_Ethernet.Controls.Add(this.panel16);
            this.GBox_Ethernet.Controls.Add(this.panel17);
            this.GBox_Ethernet.Controls.Add(this.panel18);
            this.GBox_Ethernet.Location = new System.Drawing.Point(12, 82);
            this.GBox_Ethernet.Name = "GBox_Ethernet";
            this.GBox_Ethernet.Size = new System.Drawing.Size(334, 65);
            this.GBox_Ethernet.TabIndex = 6;
            this.GBox_Ethernet.TabStop = false;
            this.GBox_Ethernet.Text = "Ethernet";
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.TBox_Port);
            this.panel16.Controls.Add(this.label16);
            this.panel16.Location = new System.Drawing.Point(175, 19);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(75, 43);
            this.panel16.TabIndex = 3;
            // 
            // TBox_Port
            // 
            this.TBox_Port.Location = new System.Drawing.Point(6, 17);
            this.TBox_Port.Name = "TBox_Port";
            this.TBox_Port.Size = new System.Drawing.Size(66, 20);
            this.TBox_Port.TabIndex = 1;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(32, 13);
            this.label16.TabIndex = 0;
            this.label16.Text = "Порт";
            // 
            // panel17
            // 
            this.panel17.Controls.Add(this.TBox_Timeout);
            this.panel17.Controls.Add(this.label17);
            this.panel17.Location = new System.Drawing.Point(253, 19);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(75, 43);
            this.panel17.TabIndex = 2;
            // 
            // TBox_Timeout
            // 
            this.TBox_Timeout.Location = new System.Drawing.Point(6, 17);
            this.TBox_Timeout.Name = "TBox_Timeout";
            this.TBox_Timeout.Size = new System.Drawing.Size(66, 20);
            this.TBox_Timeout.TabIndex = 2;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(74, 13);
            this.label17.TabIndex = 0;
            this.label17.Text = "Таймаут, сек";
            // 
            // panel18
            // 
            this.panel18.Controls.Add(this.TBox_IP);
            this.panel18.Controls.Add(this.label18);
            this.panel18.Location = new System.Drawing.Point(0, 19);
            this.panel18.Name = "panel18";
            this.panel18.Size = new System.Drawing.Size(172, 43);
            this.panel18.TabIndex = 0;
            // 
            // TBox_IP
            // 
            this.TBox_IP.Location = new System.Drawing.Point(6, 17);
            this.TBox_IP.Name = "TBox_IP";
            this.TBox_IP.Size = new System.Drawing.Size(163, 20);
            this.TBox_IP.TabIndex = 1;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(3, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(50, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "IP адрес";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.Radio_Ethernet);
            this.groupBox4.Controls.Add(this.Radio_SerialPort);
            this.groupBox4.Location = new System.Drawing.Point(348, 82);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(154, 65);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Режим";
            // 
            // Radio_Ethernet
            // 
            this.Radio_Ethernet.AutoSize = true;
            this.Radio_Ethernet.Location = new System.Drawing.Point(7, 37);
            this.Radio_Ethernet.Name = "Radio_Ethernet";
            this.Radio_Ethernet.Size = new System.Drawing.Size(65, 17);
            this.Radio_Ethernet.TabIndex = 1;
            this.Radio_Ethernet.TabStop = true;
            this.Radio_Ethernet.Text = "Ethernet";
            this.Radio_Ethernet.UseVisualStyleBackColor = true;
            this.Radio_Ethernet.CheckedChanged += new System.EventHandler(this.Radio_Ethernet_CheckedChanged);
            // 
            // Radio_SerialPort
            // 
            this.Radio_SerialPort.AutoSize = true;
            this.Radio_SerialPort.Location = new System.Drawing.Point(7, 20);
            this.Radio_SerialPort.Name = "Radio_SerialPort";
            this.Radio_SerialPort.Size = new System.Drawing.Size(102, 17);
            this.Radio_SerialPort.TabIndex = 0;
            this.Radio_SerialPort.TabStop = true;
            this.Radio_SerialPort.Text = "Серийный порт";
            this.Radio_SerialPort.UseVisualStyleBackColor = true;
            this.Radio_SerialPort.CheckedChanged += new System.EventHandler(this.Radio_SerialPort_CheckedChanged);
            // 
            // Button_LoadConfig
            // 
            this.Button_LoadConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_LoadConfig.Location = new System.Drawing.Point(292, 155);
            this.Button_LoadConfig.Name = "Button_LoadConfig";
            this.Button_LoadConfig.Size = new System.Drawing.Size(100, 25);
            this.Button_LoadConfig.TabIndex = 8;
            this.Button_LoadConfig.Text = "Выбрать";
            this.Button_LoadConfig.UseVisualStyleBackColor = true;
            this.Button_LoadConfig.Click += new System.EventHandler(this.LoadConfig);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 161);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Конфигурация:";
            // 
            // Label_Config
            // 
            this.Label_Config.AutoSize = true;
            this.Label_Config.Location = new System.Drawing.Point(90, 161);
            this.Label_Config.Name = "Label_Config";
            this.Label_Config.Size = new System.Drawing.Size(66, 13);
            this.Label_Config.TabIndex = 1;
            this.Label_Config.Text = "не выбрана";
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 391);
            this.Controls.Add(this.Label_Config);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.Button_LoadConfig);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.GBox_Ethernet);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.TextBox_Log);
            this.Controls.Add(this.GBox_Serial);
            this.Controls.Add(this.Button_Connect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "App";
            this.Text = "Gidrolock Modbus Scanner";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.App_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GBox_Serial.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_ModbusID)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_Value)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_RegLength)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_RegAddress)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.GBox_Ethernet.ResumeLayout(false);
            this.panel16.ResumeLayout(false);
            this.panel16.PerformLayout();
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.panel18.ResumeLayout(false);
            this.panel18.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox GBox_Serial;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown UpDown_ModbusID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox CBox_Ports;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Button_Connect;
        private System.Windows.Forms.TextBox TextBox_Log;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.NumericUpDown UpDown_RegLength;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button Button_SendCommand;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.NumericUpDown UpDown_RegAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox CBox_Function;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.NumericUpDown UpDown_Value;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.ComboBox CBox_BaudRate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.ComboBox CBox_DataBits;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.ComboBox CBox_StopBits;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.ComboBox CBox_Parity;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox GBox_Ethernet;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Panel panel18;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox TBox_Port;
        private System.Windows.Forms.TextBox TBox_Timeout;
        private System.Windows.Forms.TextBox TBox_IP;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton Radio_Ethernet;
        private System.Windows.Forms.RadioButton Radio_SerialPort;
        private System.Windows.Forms.Button Button_LoadConfig;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label Label_Config;
    }
}

