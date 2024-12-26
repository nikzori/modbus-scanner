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
            this.Panel_Device = new System.Windows.Forms.Panel();
            this.PD_Scrollbar = new System.Windows.Forms.VScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.Panel_Device.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label_DeviceNameLabel
            // 
            this.Label_DeviceNameLabel.AutoSize = true;
            this.Label_DeviceNameLabel.Location = new System.Drawing.Point(13, 13);
            this.Label_DeviceNameLabel.Name = "Label_DeviceNameLabel";
            this.Label_DeviceNameLabel.Size = new System.Drawing.Size(92, 13);
            this.Label_DeviceNameLabel.TabIndex = 1;
            this.Label_DeviceNameLabel.Text = "Имя устройства:";
            // 
            // Label_DescriptionLabel
            // 
            this.Label_DescriptionLabel.AutoSize = true;
            this.Label_DescriptionLabel.Location = new System.Drawing.Point(13, 31);
            this.Label_DescriptionLabel.Name = "Label_DescriptionLabel";
            this.Label_DescriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.Label_DescriptionLabel.TabIndex = 2;
            this.Label_DescriptionLabel.Text = "Описание:";
            // 
            // Label_DeviceName
            // 
            this.Label_DeviceName.AutoSize = true;
            this.Label_DeviceName.Location = new System.Drawing.Point(104, 13);
            this.Label_DeviceName.Name = "Label_DeviceName";
            this.Label_DeviceName.Size = new System.Drawing.Size(35, 13);
            this.Label_DeviceName.TabIndex = 3;
            this.Label_DeviceName.Text = "label3";
            // 
            // Label_Description
            // 
            this.Label_Description.AutoSize = true;
            this.Label_Description.Location = new System.Drawing.Point(72, 31);
            this.Label_Description.Name = "Label_Description";
            this.Label_Description.Size = new System.Drawing.Size(35, 13);
            this.Label_Description.TabIndex = 4;
            this.Label_Description.Text = "label4";
            // 
            // Button_StartStop
            // 
            this.Button_StartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_StartStop.Location = new System.Drawing.Point(505, 13);
            this.Button_StartStop.Name = "Button_StartStop";
            this.Button_StartStop.Size = new System.Drawing.Size(75, 23);
            this.Button_StartStop.TabIndex = 5;
            this.Button_StartStop.Text = "Старт";
            this.Button_StartStop.UseVisualStyleBackColor = true;
            this.Button_StartStop.Click += new System.EventHandler(this.Button_StartStop_Click);
            // 
            // Panel_Device
            // 
            this.Panel_Device.Controls.Add(this.PD_Scrollbar);
            this.Panel_Device.Location = new System.Drawing.Point(13, 77);
            this.Panel_Device.Name = "Panel_Device";
            this.Panel_Device.Size = new System.Drawing.Size(567, 411);
            this.Panel_Device.TabIndex = 6;
            // 
            // PD_Scrollbar
            // 
            this.PD_Scrollbar.Location = new System.Drawing.Point(549, 0);
            this.PD_Scrollbar.Name = "PD_Scrollbar";
            this.PD_Scrollbar.Size = new System.Drawing.Size(17, 440);
            this.PD_Scrollbar.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "#";
            // 
            // Datasheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 500);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Panel_Device);
            this.Controls.Add(this.Button_StartStop);
            this.Controls.Add(this.Label_Description);
            this.Controls.Add(this.Label_DeviceName);
            this.Controls.Add(this.Label_DescriptionLabel);
            this.Controls.Add(this.Label_DeviceNameLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Datasheet";
            this.Text = "Datasheet";
            this.Panel_Device.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Label_DeviceNameLabel;
        private System.Windows.Forms.Label Label_DescriptionLabel;
        private System.Windows.Forms.Label Label_DeviceName;
        private System.Windows.Forms.Label Label_Description;
        private System.Windows.Forms.Button Button_StartStop;
        private System.Windows.Forms.Panel Panel_Device;
        private System.Windows.Forms.VScrollBar PD_Scrollbar;
        private System.Windows.Forms.Label label1;
    }
}