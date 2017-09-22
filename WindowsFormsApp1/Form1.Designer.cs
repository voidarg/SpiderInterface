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
            this.currentLoad12 = new System.Windows.Forms.TextBox();
            this.currentLoad11 = new System.Windows.Forms.TextBox();
            this.currentLoad10 = new System.Windows.Forms.TextBox();
            this.currentLoad9 = new System.Windows.Forms.TextBox();
            this.currentLoad8 = new System.Windows.Forms.TextBox();
            this.currentLoad7 = new System.Windows.Forms.TextBox();
            this.currentLoad6 = new System.Windows.Forms.TextBox();
            this.currentLoad5 = new System.Windows.Forms.TextBox();
            this.currentLoad4 = new System.Windows.Forms.TextBox();
            this.currentLoad3 = new System.Windows.Forms.TextBox();
            this.currentLoad2 = new System.Windows.Forms.TextBox();
            this.currentLoad1 = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.maxPos = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.minPos = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.targetLoad = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.motorId2 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.motorId = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
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
            this.groupBox1.Size = new System.Drawing.Size(817, 736);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Arms";
            // 
            // StopButton
            // 
            this.StopButton.Enabled = false;
            this.StopButton.Location = new System.Drawing.Point(611, 707);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(75, 23);
            this.StopButton.TabIndex = 18;
            this.StopButton.Text = "S&top";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(530, 707);
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
            this.groupBox2.Controls.Add(this.currentLoad12);
            this.groupBox2.Controls.Add(this.currentLoad11);
            this.groupBox2.Controls.Add(this.currentLoad10);
            this.groupBox2.Controls.Add(this.currentLoad9);
            this.groupBox2.Controls.Add(this.currentLoad8);
            this.groupBox2.Controls.Add(this.currentLoad7);
            this.groupBox2.Controls.Add(this.currentLoad6);
            this.groupBox2.Controls.Add(this.currentLoad5);
            this.groupBox2.Controls.Add(this.currentLoad4);
            this.groupBox2.Controls.Add(this.currentLoad3);
            this.groupBox2.Controls.Add(this.currentLoad2);
            this.groupBox2.Controls.Add(this.currentLoad1);
            this.groupBox2.Controls.Add(this.label25);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.maxPos);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.minPos);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.targetLoad);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.motorId2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.motorId);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(17, 53);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(794, 648);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sensor Mapping";
            // 
            // currentTorque
            // 
            this.currentTorque.Location = new System.Drawing.Point(270, 599);
            this.currentTorque.Name = "currentTorque";
            this.currentTorque.ReadOnly = true;
            this.currentTorque.Size = new System.Drawing.Size(92, 20);
            this.currentTorque.TabIndex = 32;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(227, 603);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Torque";
            // 
            // currentPosition
            // 
            this.currentPosition.Location = new System.Drawing.Point(149, 599);
            this.currentPosition.Name = "currentPosition";
            this.currentPosition.ReadOnly = true;
            this.currentPosition.Size = new System.Drawing.Size(63, 20);
            this.currentPosition.TabIndex = 30;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(105, 603);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "Position";
            // 
            // currentLoad12
            // 
            this.currentLoad12.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoad12.Location = new System.Drawing.Point(566, 539);
            this.currentLoad12.Name = "currentLoad12";
            this.currentLoad12.ReadOnly = true;
            this.currentLoad12.Size = new System.Drawing.Size(199, 38);
            this.currentLoad12.TabIndex = 28;
            // 
            // currentLoad11
            // 
            this.currentLoad11.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoad11.Location = new System.Drawing.Point(566, 495);
            this.currentLoad11.Name = "currentLoad11";
            this.currentLoad11.ReadOnly = true;
            this.currentLoad11.Size = new System.Drawing.Size(199, 38);
            this.currentLoad11.TabIndex = 28;
            // 
            // currentLoad10
            // 
            this.currentLoad10.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoad10.Location = new System.Drawing.Point(566, 451);
            this.currentLoad10.Name = "currentLoad10";
            this.currentLoad10.ReadOnly = true;
            this.currentLoad10.Size = new System.Drawing.Size(199, 38);
            this.currentLoad10.TabIndex = 28;
            // 
            // currentLoad9
            // 
            this.currentLoad9.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoad9.Location = new System.Drawing.Point(566, 407);
            this.currentLoad9.Name = "currentLoad9";
            this.currentLoad9.ReadOnly = true;
            this.currentLoad9.Size = new System.Drawing.Size(199, 38);
            this.currentLoad9.TabIndex = 28;
            // 
            // currentLoad8
            // 
            this.currentLoad8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoad8.Location = new System.Drawing.Point(566, 363);
            this.currentLoad8.Name = "currentLoad8";
            this.currentLoad8.ReadOnly = true;
            this.currentLoad8.Size = new System.Drawing.Size(199, 38);
            this.currentLoad8.TabIndex = 28;
            // 
            // currentLoad7
            // 
            this.currentLoad7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoad7.Location = new System.Drawing.Point(566, 319);
            this.currentLoad7.Name = "currentLoad7";
            this.currentLoad7.ReadOnly = true;
            this.currentLoad7.Size = new System.Drawing.Size(199, 38);
            this.currentLoad7.TabIndex = 28;
            // 
            // currentLoad6
            // 
            this.currentLoad6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoad6.Location = new System.Drawing.Point(566, 275);
            this.currentLoad6.Name = "currentLoad6";
            this.currentLoad6.ReadOnly = true;
            this.currentLoad6.Size = new System.Drawing.Size(199, 38);
            this.currentLoad6.TabIndex = 28;
            // 
            // currentLoad5
            // 
            this.currentLoad5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoad5.Location = new System.Drawing.Point(566, 231);
            this.currentLoad5.Name = "currentLoad5";
            this.currentLoad5.ReadOnly = true;
            this.currentLoad5.Size = new System.Drawing.Size(199, 38);
            this.currentLoad5.TabIndex = 28;
            // 
            // currentLoad4
            // 
            this.currentLoad4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoad4.Location = new System.Drawing.Point(566, 187);
            this.currentLoad4.Name = "currentLoad4";
            this.currentLoad4.ReadOnly = true;
            this.currentLoad4.Size = new System.Drawing.Size(199, 38);
            this.currentLoad4.TabIndex = 28;
            // 
            // currentLoad3
            // 
            this.currentLoad3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoad3.Location = new System.Drawing.Point(566, 143);
            this.currentLoad3.Name = "currentLoad3";
            this.currentLoad3.ReadOnly = true;
            this.currentLoad3.Size = new System.Drawing.Size(199, 38);
            this.currentLoad3.TabIndex = 28;
            // 
            // currentLoad2
            // 
            this.currentLoad2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoad2.Location = new System.Drawing.Point(566, 99);
            this.currentLoad2.Name = "currentLoad2";
            this.currentLoad2.ReadOnly = true;
            this.currentLoad2.Size = new System.Drawing.Size(199, 38);
            this.currentLoad2.TabIndex = 28;
            // 
            // currentLoad1
            // 
            this.currentLoad1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoad1.Location = new System.Drawing.Point(566, 55);
            this.currentLoad1.Name = "currentLoad1";
            this.currentLoad1.ReadOnly = true;
            this.currentLoad1.Size = new System.Drawing.Size(199, 38);
            this.currentLoad1.TabIndex = 28;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(486, 64);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(77, 31);
            this.label25.TabIndex = 27;
            this.label25.Text = "LAL1";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(486, 106);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(77, 31);
            this.label24.TabIndex = 27;
            this.label24.Text = "LAL2";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(486, 150);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(77, 31);
            this.label23.TabIndex = 27;
            this.label23.Text = "LAL3";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(486, 194);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(82, 31);
            this.label22.TabIndex = 27;
            this.label22.Text = "LAR1";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(486, 238);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(74, 31);
            this.label21.TabIndex = 27;
            this.label21.Text = "Load";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(486, 278);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(74, 31);
            this.label20.TabIndex = 27;
            this.label20.Text = "Load";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(486, 322);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(74, 31);
            this.label19.TabIndex = 27;
            this.label19.Text = "Load";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(486, 319);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(74, 31);
            this.label18.TabIndex = 27;
            this.label18.Text = "Load";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(486, 370);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(74, 31);
            this.label17.TabIndex = 27;
            this.label17.Text = "Load";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(486, 414);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(82, 31);
            this.label16.TabIndex = 27;
            this.label16.Text = "LAR2";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(486, 407);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(74, 31);
            this.label15.TabIndex = 27;
            this.label15.Text = "Load";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(486, 458);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(82, 31);
            this.label14.TabIndex = 27;
            this.label14.Text = "LAR3";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(464, 502);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 31);
            this.label13.TabIndex = 27;
            this.label13.Text = "RLegF";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(463, 546);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(97, 31);
            this.label12.TabIndex = 27;
            this.label12.Text = "RLegB";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(615, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 31);
            this.label8.TabIndex = 27;
            this.label8.Text = "Load";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(395, 103);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(44, 20);
            this.textBox3.TabIndex = 26;
            this.textBox3.Text = "750";
            // 
            // maxPos
            // 
            this.maxPos.Location = new System.Drawing.Point(395, 64);
            this.maxPos.Name = "maxPos";
            this.maxPos.Size = new System.Drawing.Size(44, 20);
            this.maxPos.TabIndex = 26;
            this.maxPos.Text = "750";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(345, 103);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(44, 20);
            this.textBox2.TabIndex = 25;
            this.textBox2.Text = "500";
            // 
            // minPos
            // 
            this.minPos.Location = new System.Drawing.Point(345, 64);
            this.minPos.Name = "minPos";
            this.minPos.Size = new System.Drawing.Size(44, 20);
            this.minPos.TabIndex = 25;
            this.minPos.Text = "500";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(398, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Max Pos";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(347, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Min Pos";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(276, 103);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(62, 20);
            this.textBox1.TabIndex = 22;
            this.textBox1.Text = "0";
            // 
            // targetLoad
            // 
            this.targetLoad.Location = new System.Drawing.Point(276, 64);
            this.targetLoad.Name = "targetLoad";
            this.targetLoad.Size = new System.Drawing.Size(62, 20);
            this.targetLoad.TabIndex = 22;
            this.targetLoad.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(276, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Target Load";
            // 
            // motorId2
            // 
            this.motorId2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.motorId2.Enabled = false;
            this.motorId2.FormattingEnabled = true;
            this.motorId2.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.motorId2.Location = new System.Drawing.Point(55, 102);
            this.motorId2.MaxDropDownItems = 6;
            this.motorId2.MaxLength = 6;
            this.motorId2.Name = "motorId2";
            this.motorId2.Size = new System.Drawing.Size(32, 21);
            this.motorId2.Sorted = true;
            this.motorId2.TabIndex = 18;
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
            this.motorId.Location = new System.Drawing.Point(55, 63);
            this.motorId.MaxDropDownItems = 6;
            this.motorId.MaxLength = 6;
            this.motorId.Name = "motorId";
            this.motorId.Size = new System.Drawing.Size(32, 21);
            this.motorId.Sorted = true;
            this.motorId.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Motor:";
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
            this.label3.Location = new System.Drawing.Point(3, 67);
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
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 761);
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
        private System.Windows.Forms.TextBox currentLoad1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox currentTorque;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox motorId2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox currentLoad2;
        private System.Windows.Forms.TextBox currentLoad10;
        private System.Windows.Forms.TextBox currentLoad9;
        private System.Windows.Forms.TextBox currentLoad8;
        private System.Windows.Forms.TextBox currentLoad7;
        private System.Windows.Forms.TextBox currentLoad6;
        private System.Windows.Forms.TextBox currentLoad5;
        private System.Windows.Forms.TextBox currentLoad4;
        private System.Windows.Forms.TextBox currentLoad3;
        private System.Windows.Forms.TextBox currentLoad11;
        private System.Windows.Forms.TextBox currentLoad12;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
    }
}

