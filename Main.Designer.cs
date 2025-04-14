namespace Gidrolock_Modbus_Scanner
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
            this.Button_Connect = new System.Windows.Forms.Button();
            this.TextBox_Log = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.TBox_RegValue = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.UpDown_RegLength = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.Button_SendCommand = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.TBox_RegAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.CBox_Function = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Label_ConfigTip = new System.Windows.Forms.Label();
            this.Button_LoadConfig = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Label_ConfPath = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
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
            this.groupBox2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_RegLength)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.GBox_Serial.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_ModbusID)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button_Connect
            // 
            this.Button_Connect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Connect.Location = new System.Drawing.Point(388, 69);
            this.Button_Connect.Name = "Button_Connect";
            this.Button_Connect.Size = new System.Drawing.Size(104, 25);
            this.Button_Connect.TabIndex = 4;
            this.Button_Connect.Text = "Подключиться";
            this.Button_Connect.UseVisualStyleBackColor = true;
            this.Button_Connect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // TextBox_Log
            // 
            this.TextBox_Log.Location = new System.Drawing.Point(12, 194);
            this.TextBox_Log.Multiline = true;
            this.TextBox_Log.Name = "TextBox_Log";
            this.TextBox_Log.ReadOnly = true;
            this.TextBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox_Log.Size = new System.Drawing.Size(498, 133);
            this.TextBox_Log.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel6);
            this.groupBox2.Controls.Add(this.panel5);
            this.groupBox2.Controls.Add(this.Button_SendCommand);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Location = new System.Drawing.Point(12, 333);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(498, 63);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Команды";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.TBox_RegValue);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Location = new System.Drawing.Point(303, 20);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(104, 43);
            this.panel6.TabIndex = 4;
            // 
            // TBox_RegValue
            // 
            this.TBox_RegValue.Location = new System.Drawing.Point(3, 17);
            this.TBox_RegValue.Name = "TBox_RegValue";
            this.TBox_RegValue.Size = new System.Drawing.Size(98, 20);
            this.TBox_RegValue.TabIndex = 1;
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
            this.panel5.Location = new System.Drawing.Point(225, 20);
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
            this.Button_SendCommand.Size = new System.Drawing.Size(77, 23);
            this.Button_SendCommand.TabIndex = 4;
            this.Button_SendCommand.Text = "Отправить";
            this.Button_SendCommand.UseVisualStyleBackColor = true;
            this.Button_SendCommand.Click += new System.EventHandler(this.Button_SendCommand_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.TBox_RegAddress);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(159, 20);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(63, 43);
            this.panel3.TabIndex = 2;
            // 
            // TBox_RegAddress
            // 
            this.TBox_RegAddress.Location = new System.Drawing.Point(3, 18);
            this.TBox_RegAddress.Name = "TBox_RegAddress";
            this.TBox_RegAddress.Size = new System.Drawing.Size(56, 20);
            this.TBox_RegAddress.TabIndex = 1;
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
            // Label_ConfigTip
            // 
            this.Label_ConfigTip.AutoSize = true;
            this.Label_ConfigTip.Location = new System.Drawing.Point(5, 16);
            this.Label_ConfigTip.Name = "Label_ConfigTip";
            this.Label_ConfigTip.Size = new System.Drawing.Size(94, 13);
            this.Label_ConfigTip.TabIndex = 10;
            this.Label_ConfigTip.Text = "Выбранный путь:";
            // 
            // Button_LoadConfig
            // 
            this.Button_LoadConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_LoadConfig.Location = new System.Drawing.Point(89, 69);
            this.Button_LoadConfig.Name = "Button_LoadConfig";
            this.Button_LoadConfig.Size = new System.Drawing.Size(77, 25);
            this.Button_LoadConfig.TabIndex = 11;
            this.Button_LoadConfig.Text = "Файл";
            this.Button_LoadConfig.UseVisualStyleBackColor = true;
            this.Button_LoadConfig.Click += new System.EventHandler(this.LoadConfig);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Label_ConfPath);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.Button_LoadConfig);
            this.groupBox1.Controls.Add(this.Label_ConfigTip);
            this.groupBox1.Controls.Add(this.Button_Connect);
            this.groupBox1.Location = new System.Drawing.Point(12, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(498, 103);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Конфигурация";
            // 
            // Label_ConfPath
            // 
            this.Label_ConfPath.Location = new System.Drawing.Point(6, 29);
            this.Label_ConfPath.Name = "Label_ConfPath";
            this.Label_ConfPath.Size = new System.Drawing.Size(480, 37);
            this.Label_ConfPath.TabIndex = 14;
            this.Label_ConfPath.Text = "Файл не выбран.";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(172, 69);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(210, 25);
            this.progressBar1.TabIndex = 14;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(6, 69);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 25);
            this.button1.TabIndex = 13;
            this.button1.Text = "Папка";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.LoadFolder);
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
            this.GBox_Serial.Size = new System.Drawing.Size(498, 67);
            this.GBox_Serial.TabIndex = 15;
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
            this.panel2.Location = new System.Drawing.Point(417, 19);
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
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 404);
            this.Controls.Add(this.GBox_Serial);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.TextBox_Log);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "App";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Zori\'s Modbus Scanner";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.App_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_RegLength)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Button_Connect;
        private System.Windows.Forms.TextBox TextBox_Log;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.NumericUpDown UpDown_RegLength;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button Button_SendCommand;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox CBox_Function;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label Label_ConfigTip;
        private System.Windows.Forms.Button Button_LoadConfig;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label Label_ConfPath;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox GBox_Serial;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.ComboBox CBox_Parity;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.ComboBox CBox_StopBits;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox CBox_DataBits;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.ComboBox CBox_BaudRate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown UpDown_ModbusID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox CBox_Ports;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBox_RegValue;
        private System.Windows.Forms.TextBox TBox_RegAddress;
    }
}

