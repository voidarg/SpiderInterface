namespace WindowsFormsApp1
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.StopButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.currentTorque = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.currentPosition = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.currentLoad = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.maxPos = new System.Windows.Forms.TextBox();
            this.minPos = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.targetLoad = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Inputs = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.motorId = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.arduinoDevices = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.StopButton);
            this.groupBox1.Controls.Add(this.StartButton);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.arduinoDevices);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(940, 314);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Arms";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // StopButton
            // 
            this.StopButton.Enabled = false;
            this.StopButton.Location = new System.Drawing.Point(843, 271);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(75, 23);
            this.StopButton.TabIndex = 18;
            this.StopButton.Text = "S&top";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(762, 271);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 17;
            this.StartButton.Text = "&Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.currentTorque);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.currentPosition);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.currentLoad);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.maxPos);
            this.groupBox2.Controls.Add(this.minPos);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.targetLoad);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.Inputs);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.motorId);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(17, 53);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(901, 212);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sensor Mapping";
            // 
            // currentTorque
            // 
            this.currentTorque.Location = new System.Drawing.Point(346, 110);
            this.currentTorque.Name = "currentTorque";
            this.currentTorque.ReadOnly = true;
            this.currentTorque.Size = new System.Drawing.Size(92, 20);
            this.currentTorque.TabIndex = 32;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(303, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Torque";
            // 
            // currentPosition
            // 
            this.currentPosition.Location = new System.Drawing.Point(225, 110);
            this.currentPosition.Name = "currentPosition";
            this.currentPosition.ReadOnly = true;
            this.currentPosition.Size = new System.Drawing.Size(63, 20);
            this.currentPosition.TabIndex = 30;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(181, 114);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "Position";
            // 
            // currentLoad
            // 
            this.currentLoad.Location = new System.Drawing.Point(89, 109);
            this.currentLoad.Name = "currentLoad";
            this.currentLoad.ReadOnly = true;
            this.currentLoad.Size = new System.Drawing.Size(77, 20);
            this.currentLoad.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(57, 113);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Load";
            // 
            // maxPos
            // 
            this.maxPos.Location = new System.Drawing.Point(519, 48);
            this.maxPos.Name = "maxPos";
            this.maxPos.Size = new System.Drawing.Size(44, 20);
            this.maxPos.TabIndex = 26;
            // 
            // minPos
            // 
            this.minPos.Location = new System.Drawing.Point(469, 48);
            this.minPos.Name = "minPos";
            this.minPos.Size = new System.Drawing.Size(44, 20);
            this.minPos.TabIndex = 25;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(519, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Max Pos";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(468, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Min Pos";
            // 
            // targetLoad
            // 
            this.targetLoad.Location = new System.Drawing.Point(400, 48);
            this.targetLoad.Name = "targetLoad";
            this.targetLoad.Size = new System.Drawing.Size(62, 20);
            this.targetLoad.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(397, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Target Load";
            // 
            // Inputs
            // 
            this.Inputs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Inputs.Enabled = false;
            this.Inputs.FormattingEnabled = true;
            this.Inputs.Location = new System.Drawing.Point(96, 47);
            this.Inputs.MaxLength = 8;
            this.Inputs.Name = "Inputs";
            this.Inputs.Size = new System.Drawing.Size(298, 21);
            this.Inputs.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(93, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Input";
            // 
            // motorId
            // 
            this.motorId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.motorId.Enabled = false;
            this.motorId.FormattingEnabled = true;
            this.motorId.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.motorId.Location = new System.Drawing.Point(58, 47);
            this.motorId.MaxDropDownItems = 6;
            this.motorId.MaxLength = 6;
            this.motorId.Name = "motorId";
            this.motorId.Size = new System.Drawing.Size(32, 21);
            this.motorId.Sorted = true;
            this.motorId.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(55, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Motor:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Arduino";
            // 
            // arduinoDevices
            // 
            this.arduinoDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.arduinoDevices.Enabled = false;
            this.arduinoDevices.FormattingEnabled = true;
            this.arduinoDevices.Location = new System.Drawing.Point(63, 20);
            this.arduinoDevices.Name = "arduinoDevices";
            this.arduinoDevices.Size = new System.Drawing.Size(297, 21);
            this.arduinoDevices.TabIndex = 0;
            this.arduinoDevices.SelectedIndexChanged += new System.EventHandler(this.arduinoDevices_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 347);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox arduinoDevices;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Inputs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox motorId;
        private System.Windows.Forms.TextBox targetLoad;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox maxPos;
        private System.Windows.Forms.TextBox minPos;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox currentPosition;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox currentLoad;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox currentTorque;
        private System.Windows.Forms.Label label11;
    }
}

