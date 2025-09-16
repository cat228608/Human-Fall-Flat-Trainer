namespace HFF_Trainer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lblStatus = new System.Windows.Forms.Label();
            this.chkHighJump = new System.Windows.Forms.CheckBox();
            this.chkSpeedHack = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.chkMonitoring = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(12, 18);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(169, 16);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Статус: Ожидание игры...";
            // 
            // chkHighJump
            // 
            this.chkHighJump.AutoSize = true;
            this.chkHighJump.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkHighJump.Location = new System.Drawing.Point(12, 47);
            this.chkHighJump.Name = "chkHighJump";
            this.chkHighJump.Size = new System.Drawing.Size(217, 20);
            this.chkHighJump.TabIndex = 1;
            this.chkHighJump.Text = "Высокий прыжок (x3.0) [Num 1]";
            this.chkHighJump.UseVisualStyleBackColor = true;
            this.chkHighJump.CheckedChanged += new System.EventHandler(this.chkHighJump_CheckedChanged);
            // 
            // chkSpeedHack
            // 
            this.chkSpeedHack.AutoSize = true;
            this.chkSpeedHack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkSpeedHack.Location = new System.Drawing.Point(12, 73);
            this.chkSpeedHack.Name = "chkSpeedHack";
            this.chkSpeedHack.Size = new System.Drawing.Size(180, 20);
            this.chkSpeedHack.TabIndex = 2;
            this.chkSpeedHack.Text = "Ускорение (x2.5) [Num 2]";
            this.chkSpeedHack.UseVisualStyleBackColor = true;
            this.chkSpeedHack.CheckedChanged += new System.EventHandler(this.chkSpeedHack_CheckedChanged);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BW_DoWork);
            // 
            // chkMonitoring
            // 
            this.chkMonitoring.AutoSize = true;
            this.chkMonitoring.Checked = true;
            this.chkMonitoring.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMonitoring.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkMonitoring.Location = new System.Drawing.Point(12, 99);
            this.chkMonitoring.Name = "chkMonitoring";
            this.chkMonitoring.Size = new System.Drawing.Size(355, 20);
            this.chkMonitoring.TabIndex = 2;
            this.chkMonitoring.Text = "Постоянная проверка (отключите если слабый пк)";
            this.chkMonitoring.UseVisualStyleBackColor = true;
            this.chkMonitoring.CheckedChanged += new System.EventHandler(this.chkSpeedHack_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 130);
            this.Controls.Add(this.chkMonitoring);
            this.Controls.Add(this.chkSpeedHack);
            this.Controls.Add(this.chkHighJump);
            this.Controls.Add(this.lblStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "HFF Trainer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chkHighJump;
        private System.Windows.Forms.CheckBox chkSpeedHack;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox chkMonitoring;
    }
}

