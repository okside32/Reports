namespace Report
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.OpenForTopsButton = new System.Windows.Forms.Button();
            this.salesDataGridView = new System.Windows.Forms.DataGridView();
            this.YesterdaySalesButton = new System.Windows.Forms.Button();
            this.TodaySalesButton = new System.Windows.Forms.Button();
            this.TopSalesPath = new System.Windows.Forms.TextBox();
            this.YesterdaySalesPath = new System.Windows.Forms.TextBox();
            this.TodaySalesPath = new System.Windows.Forms.TextBox();
            this.SaveTableButton = new System.Windows.Forms.Button();
            this.Tab = new System.Windows.Forms.TabControl();
            this.tabTop = new System.Windows.Forms.TabPage();
            this.tabRemains = new System.Windows.Forms.TabPage();
            this.tabAudit = new System.Windows.Forms.TabPage();
            this.RevisionTextBox = new System.Windows.Forms.TextBox();
            this.RemainTextBox = new System.Windows.Forms.TextBox();
            this.RevisionButton = new System.Windows.Forms.Button();
            this.RemainButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.salesDataGridView)).BeginInit();
            this.Tab.SuspendLayout();
            this.tabTop.SuspendLayout();
            this.tabRemains.SuspendLayout();
            this.tabAudit.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenForTopsButton
            // 
            this.OpenForTopsButton.Location = new System.Drawing.Point(3, 21);
            this.OpenForTopsButton.Name = "OpenForTopsButton";
            this.OpenForTopsButton.Size = new System.Drawing.Size(87, 23);
            this.OpenForTopsButton.TabIndex = 0;
            this.OpenForTopsButton.Text = "Top";
            this.OpenForTopsButton.UseVisualStyleBackColor = true;
            this.OpenForTopsButton.Click += new System.EventHandler(this.Open_Click);
            // 
            // salesDataGridView
            // 
            this.salesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.salesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.salesDataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.salesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.salesDataGridView.Location = new System.Drawing.Point(8, 106);
            this.salesDataGridView.Name = "salesDataGridView";
            this.salesDataGridView.Size = new System.Drawing.Size(585, 448);
            this.salesDataGridView.TabIndex = 1;
            this.salesDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.sales_DataBindingComplete);
            // 
            // YesterdaySalesButton
            // 
            this.YesterdaySalesButton.Location = new System.Drawing.Point(3, 6);
            this.YesterdaySalesButton.Name = "YesterdaySalesButton";
            this.YesterdaySalesButton.Size = new System.Drawing.Size(75, 23);
            this.YesterdaySalesButton.TabIndex = 2;
            this.YesterdaySalesButton.Text = "First";
            this.YesterdaySalesButton.UseVisualStyleBackColor = true;
            this.YesterdaySalesButton.Click += new System.EventHandler(this.YesterdaySales_Click);
            // 
            // TodaySalesButton
            // 
            this.TodaySalesButton.Location = new System.Drawing.Point(3, 36);
            this.TodaySalesButton.Name = "TodaySalesButton";
            this.TodaySalesButton.Size = new System.Drawing.Size(75, 23);
            this.TodaySalesButton.TabIndex = 3;
            this.TodaySalesButton.Text = "Second";
            this.TodaySalesButton.UseVisualStyleBackColor = true;
            this.TodaySalesButton.Click += new System.EventHandler(this.TodaySales_Click);
            // 
            // TopSalesPath
            // 
            this.TopSalesPath.Location = new System.Drawing.Point(93, 23);
            this.TopSalesPath.Name = "TopSalesPath";
            this.TopSalesPath.Size = new System.Drawing.Size(488, 20);
            this.TopSalesPath.TabIndex = 4;
            // 
            // YesterdaySalesPath
            // 
            this.YesterdaySalesPath.Location = new System.Drawing.Point(84, 8);
            this.YesterdaySalesPath.Name = "YesterdaySalesPath";
            this.YesterdaySalesPath.Size = new System.Drawing.Size(497, 20);
            this.YesterdaySalesPath.TabIndex = 5;
            // 
            // TodaySalesPath
            // 
            this.TodaySalesPath.Location = new System.Drawing.Point(84, 39);
            this.TodaySalesPath.Name = "TodaySalesPath";
            this.TodaySalesPath.Size = new System.Drawing.Size(497, 20);
            this.TodaySalesPath.TabIndex = 6;
            // 
            // SaveTableButton
            // 
            this.SaveTableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveTableButton.Location = new System.Drawing.Point(8, 560);
            this.SaveTableButton.Name = "SaveTableButton";
            this.SaveTableButton.Size = new System.Drawing.Size(75, 23);
            this.SaveTableButton.TabIndex = 7;
            this.SaveTableButton.Text = "Save";
            this.SaveTableButton.UseVisualStyleBackColor = true;
            this.SaveTableButton.Click += new System.EventHandler(this.SaveTable_Click);
            // 
            // Tab
            // 
            this.Tab.AllowDrop = true;
            this.Tab.Controls.Add(this.tabTop);
            this.Tab.Controls.Add(this.tabRemains);
            this.Tab.Controls.Add(this.tabAudit);
            this.Tab.Location = new System.Drawing.Point(1, 1);
            this.Tab.Multiline = true;
            this.Tab.Name = "Tab";
            this.Tab.SelectedIndex = 0;
            this.Tab.Size = new System.Drawing.Size(592, 99);
            this.Tab.TabIndex = 8;
            // 
            // tabTop
            // 
            this.tabTop.Controls.Add(this.OpenForTopsButton);
            this.tabTop.Controls.Add(this.TopSalesPath);
            this.tabTop.Location = new System.Drawing.Point(4, 22);
            this.tabTop.Name = "tabTop";
            this.tabTop.Padding = new System.Windows.Forms.Padding(3);
            this.tabTop.Size = new System.Drawing.Size(584, 73);
            this.tabTop.TabIndex = 0;
            this.tabTop.Text = "Топ";
            this.tabTop.UseVisualStyleBackColor = true;
            // 
            // tabRemains
            // 
            this.tabRemains.Controls.Add(this.YesterdaySalesButton);
            this.tabRemains.Controls.Add(this.TodaySalesButton);
            this.tabRemains.Controls.Add(this.TodaySalesPath);
            this.tabRemains.Controls.Add(this.YesterdaySalesPath);
            this.tabRemains.Location = new System.Drawing.Point(4, 22);
            this.tabRemains.Name = "tabRemains";
            this.tabRemains.Padding = new System.Windows.Forms.Padding(3);
            this.tabRemains.Size = new System.Drawing.Size(584, 73);
            this.tabRemains.TabIndex = 1;
            this.tabRemains.Text = "Остатки";
            this.tabRemains.UseVisualStyleBackColor = true;
            // 
            // tabAudit
            // 
            this.tabAudit.Controls.Add(this.RevisionTextBox);
            this.tabAudit.Controls.Add(this.RemainTextBox);
            this.tabAudit.Controls.Add(this.RevisionButton);
            this.tabAudit.Controls.Add(this.RemainButton);
            this.tabAudit.Location = new System.Drawing.Point(4, 22);
            this.tabAudit.Name = "tabAudit";
            this.tabAudit.Padding = new System.Windows.Forms.Padding(3);
            this.tabAudit.Size = new System.Drawing.Size(584, 73);
            this.tabAudit.TabIndex = 2;
            this.tabAudit.Text = "Переоценка";
            this.tabAudit.UseVisualStyleBackColor = true;
            // 
            // RevisionTextBox
            // 
            this.RevisionTextBox.Location = new System.Drawing.Point(100, 35);
            this.RevisionTextBox.Name = "RevisionTextBox";
            this.RevisionTextBox.Size = new System.Drawing.Size(478, 20);
            this.RevisionTextBox.TabIndex = 3;
            // 
            // RemainTextBox
            // 
            this.RemainTextBox.Location = new System.Drawing.Point(100, 9);
            this.RemainTextBox.Name = "RemainTextBox";
            this.RemainTextBox.Size = new System.Drawing.Size(478, 20);
            this.RemainTextBox.TabIndex = 2;
            // 
            // RevisionButton
            // 
            this.RevisionButton.Location = new System.Drawing.Point(7, 35);
            this.RevisionButton.Name = "RevisionButton";
            this.RevisionButton.Size = new System.Drawing.Size(87, 23);
            this.RevisionButton.TabIndex = 1;
            this.RevisionButton.Text = "Переоценка";
            this.RevisionButton.UseVisualStyleBackColor = true;
            this.RevisionButton.Click += new System.EventHandler(this.RevisionButton_Click);
            // 
            // RemainButton
            // 
            this.RemainButton.Location = new System.Drawing.Point(7, 6);
            this.RemainButton.Name = "RemainButton";
            this.RemainButton.Size = new System.Drawing.Size(87, 23);
            this.RemainButton.TabIndex = 0;
            this.RemainButton.Text = "Остатки";
            this.RemainButton.UseVisualStyleBackColor = true;
            this.RemainButton.Click += new System.EventHandler(this.RemainsButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 588);
            this.Controls.Add(this.Tab);
            this.Controls.Add(this.SaveTableButton);
            this.Controls.Add(this.salesDataGridView);
            this.Name = "Form1";
            this.Text = "Отчеты";
            ((System.ComponentModel.ISupportInitialize)(this.salesDataGridView)).EndInit();
            this.Tab.ResumeLayout(false);
            this.tabTop.ResumeLayout(false);
            this.tabTop.PerformLayout();
            this.tabRemains.ResumeLayout(false);
            this.tabRemains.PerformLayout();
            this.tabAudit.ResumeLayout(false);
            this.tabAudit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OpenForTopsButton;
        private System.Windows.Forms.DataGridView salesDataGridView;
        private System.Windows.Forms.Button YesterdaySalesButton;
        private System.Windows.Forms.Button TodaySalesButton;
        private System.Windows.Forms.TextBox TopSalesPath;
        private System.Windows.Forms.TextBox YesterdaySalesPath;
        private System.Windows.Forms.TextBox TodaySalesPath;
        private System.Windows.Forms.Button SaveTableButton;
        private System.Windows.Forms.TabControl Tab;
        private System.Windows.Forms.TabPage tabTop;
        private System.Windows.Forms.TabPage tabRemains;
        private System.Windows.Forms.TabPage tabAudit;
        private System.Windows.Forms.TextBox RevisionTextBox;
        private System.Windows.Forms.TextBox RemainTextBox;
        private System.Windows.Forms.Button RevisionButton;
        private System.Windows.Forms.Button RemainButton;
    }
}

