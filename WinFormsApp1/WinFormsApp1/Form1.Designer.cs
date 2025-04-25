namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            buttonSendData = new Button();
            btnReadFromQueue = new Button();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = SystemColors.ActiveCaption;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(984, 347);
            dataGridView1.TabIndex = 0;
            // 
            // buttonSendData
            // 
            buttonSendData.BackColor = SystemColors.GradientInactiveCaption;
            buttonSendData.Location = new Point(0, 597);
            buttonSendData.Name = "buttonSendData";
            buttonSendData.Size = new Size(131, 85);
            buttonSendData.TabIndex = 1;
            buttonSendData.Text = "Выгрузить данные";
            buttonSendData.UseVisualStyleBackColor = false;
            buttonSendData.Click += RunPythonScriptButton_Click;
            // 
            // btnReadFromQueue
            // 
            btnReadFromQueue.Location = new Point(289, 597);
            btnReadFromQueue.Name = "btnReadFromQueue";
            btnReadFromQueue.Size = new Size(124, 85);
            btnReadFromQueue.TabIndex = 2;
            btnReadFromQueue.Text = "Чтение из очереди";
            btnReadFromQueue.UseVisualStyleBackColor = true;
            btnReadFromQueue.Click += btnReadFromQueue_Click;
            // 
            // button1
            // 
            button1.Location = new Point(137, 597);
            button1.Name = "button1";
            button1.Size = new Size(137, 85);
            button1.TabIndex = 3;
            button1.Text = "Чтение из сокетов";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(0, 506);
            button2.Name = "button2";
            button2.Size = new Size(122, 67);
            button2.TabIndex = 4;
            button2.Text = "Очистить содержимое таблиц";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(428, 597);
            button3.Name = "button3";
            button3.Size = new Size(151, 85);
            button3.TabIndex = 5;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(589, 597);
            button4.Name = "btn4";
            button4.Size = new Size(124, 85);
            button4.TabIndex = 2;
            button4.Text = "Отправка данных через сокеты";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(984, 681);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(button4);
            Controls.Add(button5);
            Controls.Add(button6);
            Controls.Add(btnReadFromQueue);
            Controls.Add(buttonSendData);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Button buttonSendData;
        private Button btnReadFromQueue;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
    }
}
