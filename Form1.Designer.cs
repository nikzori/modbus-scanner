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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ButtonConnect = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.UpDown_ModbusID = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CBox_Ports = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBox_Log = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
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
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_ModbusID)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_RegLength)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_RegAddress)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ButtonConnect);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 63);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Подключение";
            // 
            // ButtonConnect
            // 
            this.ButtonConnect.Location = new System.Drawing.Point(162, 34);
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Size = new System.Drawing.Size(176, 23);
            this.ButtonConnect.TabIndex = 4;
            this.ButtonConnect.Text = "Подключиться";
            this.ButtonConnect.UseVisualStyleBackColor = true;
            this.ButtonConnect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.UpDown_ModbusID);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(81, 19);
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
            this.CBox_Ports.Text = "COM 1";
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
            // TextBox_Log
            // 
            this.TextBox_Log.Location = new System.Drawing.Point(12, 82);
            this.TextBox_Log.Multiline = true;
            this.TextBox_Log.Name = "TextBox_Log";
            this.TextBox_Log.ReadOnly = true;
            this.TextBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox_Log.Size = new System.Drawing.Size(417, 87);
            this.TextBox_Log.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel5);
            this.groupBox2.Controls.Add(this.Button_SendCommand);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Location = new System.Drawing.Point(12, 175);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(417, 63);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Команды";
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
            this.Button_SendCommand.Location = new System.Drawing.Point(337, 35);
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
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 242);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.TextBox_Log);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "App";
            this.Text = "Gidrolock Modbus Scanner";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.App_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_ModbusID)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_RegLength)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDown_RegAddress)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown UpDown_ModbusID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox CBox_Ports;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButtonConnect;
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
    }
}

