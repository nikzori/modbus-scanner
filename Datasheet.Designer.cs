namespace Gidrolock_Modbus_Scanner
{
    partial class Datasheet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Datasheet));
            this.Label_DeviceNameLabel = new System.Windows.Forms.Label();
            this.Label_DescriptionLabel = new System.Windows.Forms.Label();
            this.Label_DeviceName = new System.Windows.Forms.Label();
            this.Label_Description = new System.Windows.Forms.Label();
            this.Button_StartStop = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbBaudrate = new System.Windows.Forms.ComboBox();
            this.udSlaveId = new System.Windows.Forms.NumericUpDown();
            this.buttonSetSpeed = new System.Windows.Forms.Button();
            this.buttonSetId = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.udSlaveId)).BeginInit();
            this.SuspendLayout();
            // 
            // Label_DeviceNameLabel
            // 
            this.Label_DeviceNameLabel.AutoSize = true;
            this.Label_DeviceNameLabel.Location = new System.Drawing.Point(13, 18);
            this.Label_DeviceNameLabel.Name = "Label_DeviceNameLabel";
            this.Label_DeviceNameLabel.Size = new System.Drawing.Size(92, 13);
            this.Label_DeviceNameLabel.TabIndex = 1;
            this.Label_DeviceNameLabel.Text = "Имя устройства:";
            // 
            // Label_DescriptionLabel
            // 
            this.Label_DescriptionLabel.AutoSize = true;
            this.Label_DescriptionLabel.Location = new System.Drawing.Point(15, 42);
            this.Label_DescriptionLabel.Name = "Label_DescriptionLabel";
            this.Label_DescriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.Label_DescriptionLabel.TabIndex = 2;
            this.Label_DescriptionLabel.Text = "Описание:";
            // 
            // Label_DeviceName
            // 
            this.Label_DeviceName.AutoSize = true;
            this.Label_DeviceName.Location = new System.Drawing.Point(104, 18);
            this.Label_DeviceName.Name = "Label_DeviceName";
            this.Label_DeviceName.Size = new System.Drawing.Size(35, 13);
            this.Label_DeviceName.TabIndex = 3;
            this.Label_DeviceName.Text = "label3";
            // 
            // Label_Description
            // 
            this.Label_Description.AutoSize = true;
            this.Label_Description.Location = new System.Drawing.Point(74, 42);
            this.Label_Description.Name = "Label_Description";
            this.Label_Description.Size = new System.Drawing.Size(35, 13);
            this.Label_Description.TabIndex = 4;
            this.Label_Description.Text = "label4";
            // 
            // Button_StartStop
            // 
            this.Button_StartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_StartStop.Location = new System.Drawing.Point(577, 13);
            this.Button_StartStop.Name = "Button_StartStop";
            this.Button_StartStop.Size = new System.Drawing.Size(75, 23);
            this.Button_StartStop.TabIndex = 5;
            this.Button_StartStop.Text = "Автоопрос";
            this.Button_StartStop.UseVisualStyleBackColor = true;
            this.Button_StartStop.Click += new System.EventHandler(this.Button_StartStop_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.LightGray;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 146);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(640, 439);
            this.flowLayoutPanel1.TabIndex = 8;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Скорость, бит/с:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(577, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Опрос";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Modbus ID:";
            // 
            // cbBaudrate
            // 
            this.cbBaudrate.FormattingEnabled = true;
            this.cbBaudrate.Location = new System.Drawing.Point(112, 82);
            this.cbBaudrate.Name = "cbBaudrate";
            this.cbBaudrate.Size = new System.Drawing.Size(66, 21);
            this.cbBaudrate.TabIndex = 12;
            // 
            // udSlaveId
            // 
            this.udSlaveId.Location = new System.Drawing.Point(112, 109);
            this.udSlaveId.Name = "udSlaveId";
            this.udSlaveId.Size = new System.Drawing.Size(66, 20);
            this.udSlaveId.TabIndex = 13;
            // 
            // buttonSetSpeed
            // 
            this.buttonSetSpeed.Location = new System.Drawing.Point(184, 80);
            this.buttonSetSpeed.Name = "buttonSetSpeed";
            this.buttonSetSpeed.Size = new System.Drawing.Size(75, 23);
            this.buttonSetSpeed.TabIndex = 14;
            this.buttonSetSpeed.Text = "Задать";
            this.buttonSetSpeed.UseVisualStyleBackColor = true;
            this.buttonSetSpeed.Click += new System.EventHandler(this.buttonSetSpeed_Click);
            // 
            // buttonSetId
            // 
            this.buttonSetId.Location = new System.Drawing.Point(184, 106);
            this.buttonSetId.Name = "buttonSetId";
            this.buttonSetId.Size = new System.Drawing.Size(75, 23);
            this.buttonSetId.TabIndex = 15;
            this.buttonSetId.Text = "Задать";
            this.buttonSetId.UseVisualStyleBackColor = true;
            this.buttonSetId.Click += new System.EventHandler(this.buttonSetId_Click);
            // 
            // Datasheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 597);
            this.Controls.Add(this.buttonSetId);
            this.Controls.Add(this.buttonSetSpeed);
            this.Controls.Add(this.udSlaveId);
            this.Controls.Add(this.cbBaudrate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.Button_StartStop);
            this.Controls.Add(this.Label_Description);
            this.Controls.Add(this.Label_DeviceName);
            this.Controls.Add(this.Label_DescriptionLabel);
            this.Controls.Add(this.Label_DeviceNameLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Datasheet";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Datasheet";
            ((System.ComponentModel.ISupportInitialize)(this.udSlaveId)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Label_DeviceNameLabel;
        private System.Windows.Forms.Label Label_DescriptionLabel;
        private System.Windows.Forms.Label Label_DeviceName;
        private System.Windows.Forms.Label Label_Description;
        private System.Windows.Forms.Button Button_StartStop;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbBaudrate;
        private System.Windows.Forms.NumericUpDown udSlaveId;
        private System.Windows.Forms.Button buttonSetSpeed;
        private System.Windows.Forms.Button buttonSetId;
    }
}