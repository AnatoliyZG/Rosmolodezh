namespace RosMolAdminPanel
{
    partial class MainForm
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
            this.AnnouncesButton = new System.Windows.Forms.Button();
            this.OptionButton = new System.Windows.Forms.Button();
            this.WishesButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AnnouncesButton
            // 
            this.AnnouncesButton.Location = new System.Drawing.Point(12, 12);
            this.AnnouncesButton.Name = "AnnouncesButton";
            this.AnnouncesButton.Size = new System.Drawing.Size(75, 23);
            this.AnnouncesButton.TabIndex = 0;
            this.AnnouncesButton.Text = "Announces";
            this.AnnouncesButton.UseVisualStyleBackColor = true;
            this.AnnouncesButton.Click += new System.EventHandler(this.AnnouncesButton_Click);
            // 
            // OptionButton
            // 
            this.OptionButton.Location = new System.Drawing.Point(12, 41);
            this.OptionButton.Name = "OptionButton";
            this.OptionButton.Size = new System.Drawing.Size(75, 23);
            this.OptionButton.TabIndex = 1;
            this.OptionButton.Text = "Options";
            this.OptionButton.UseVisualStyleBackColor = true;
            this.OptionButton.Click += new System.EventHandler(this.OptionButton_Click);
            // 
            // WishesButton
            // 
            this.WishesButton.Location = new System.Drawing.Point(12, 70);
            this.WishesButton.Name = "WishesButton";
            this.WishesButton.Size = new System.Drawing.Size(75, 23);
            this.WishesButton.TabIndex = 2;
            this.WishesButton.Text = "Wishes";
            this.WishesButton.UseVisualStyleBackColor = true;
            this.WishesButton.Click += new System.EventHandler(this.WishesButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.WishesButton);
            this.Controls.Add(this.OptionButton);
            this.Controls.Add(this.AnnouncesButton);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AnnouncesButton;
        private System.Windows.Forms.Button OptionButton;
        private System.Windows.Forms.Button WishesButton;
    }
}

