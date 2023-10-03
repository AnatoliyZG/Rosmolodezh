namespace RosMolAdminPanel
{
    partial class DBViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBViewer));
            this.label1 = new System.Windows.Forms.Label();
            this.PhotoViewer = new System.Windows.Forms.ListBox();
            this.PhotoView = new System.Windows.Forms.PictureBox();
            this.BindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.announcesBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.DataGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.summaryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wishesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rosDBDataSet = new RosMolAdminPanel.RosDBDataSet();
            this.announcesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.announcesTableAdapter = new RosMolAdminPanel.RosDBDataSetTableAdapters.AnnouncesTableAdapter();
            this.tableAdapterManager = new RosMolAdminPanel.RosDBDataSetTableAdapters.TableAdapterManager();
            this.optionsTableAdapter = new RosMolAdminPanel.RosDBDataSetTableAdapters.OptionsTableAdapter();
            this.wishesTableAdapter = new RosMolAdminPanel.RosDBDataSetTableAdapters.WishesTableAdapter();
            this.optionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PhotoView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingNavigator)).BeginInit();
            this.BindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wishesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rosDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.announcesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.optionsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(649, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Изображения:";
            // 
            // PhotoViewer
            // 
            this.PhotoViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PhotoViewer.FormattingEnabled = true;
            this.PhotoViewer.Location = new System.Drawing.Point(649, 44);
            this.PhotoViewer.Name = "PhotoViewer";
            this.PhotoViewer.Size = new System.Drawing.Size(155, 290);
            this.PhotoViewer.TabIndex = 3;
            this.PhotoViewer.SelectedIndexChanged += new System.EventHandler(this.PhotoViewer_SelectedIndexChanged);
            // 
            // PhotoView
            // 
            this.PhotoView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PhotoView.Location = new System.Drawing.Point(649, 348);
            this.PhotoView.Name = "PhotoView";
            this.PhotoView.Size = new System.Drawing.Size(155, 102);
            this.PhotoView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PhotoView.TabIndex = 4;
            this.PhotoView.TabStop = false;
            // 
            // BindingNavigator
            // 
            this.BindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.BindingNavigator.BindingSource = this.announcesBindingSource;
            this.BindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.BindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.BindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.announcesBindingNavigatorSaveItem});
            this.BindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.BindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.BindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.BindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.BindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.BindingNavigator.Name = "BindingNavigator";
            this.BindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.BindingNavigator.Size = new System.Drawing.Size(816, 25);
            this.BindingNavigator.TabIndex = 5;
            this.BindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Добавить";
            this.bindingNavigatorAddNewItem.Click += new System.EventHandler(this.bindingNavigatorAddNewItem_Click);
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(43, 22);
            this.bindingNavigatorCountItem.Text = "для {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Общее число элементов";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Удалить";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Переместить в начало";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Переместить назад";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Положение";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Текущее положение";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Переместить вперед";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Переместить в конец";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // announcesBindingNavigatorSaveItem
            // 
            this.announcesBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.announcesBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("announcesBindingNavigatorSaveItem.Image")));
            this.announcesBindingNavigatorSaveItem.Name = "announcesBindingNavigatorSaveItem";
            this.announcesBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.announcesBindingNavigatorSaveItem.Text = "Сохранить данные";
            this.announcesBindingNavigatorSaveItem.Click += new System.EventHandler(this.announcesBindingNavigatorSaveItem_Click);
            // 
            // DataGridView
            // 
            this.DataGridView.AllowUserToAddRows = false;
            this.DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridView.AutoGenerateColumns = false;
            this.DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.summaryDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn});
            this.DataGridView.DataSource = this.wishesBindingSource;
            this.DataGridView.Location = new System.Drawing.Point(0, 28);
            this.DataGridView.Name = "DataGridView";
            this.DataGridView.Size = new System.Drawing.Size(643, 422);
            this.DataGridView.TabIndex = 5;
            this.DataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.DataGridView_RowsAdded);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.MaxInputLength = 8;
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.Visible = false;
            this.idDataGridViewTextBoxColumn.Width = 60;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "name";
            this.nameDataGridViewTextBoxColumn.MaxInputLength = 128;
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 120;
            // 
            // summaryDataGridViewTextBoxColumn
            // 
            this.summaryDataGridViewTextBoxColumn.DataPropertyName = "summary";
            this.summaryDataGridViewTextBoxColumn.HeaderText = "summary";
            this.summaryDataGridViewTextBoxColumn.MaxInputLength = 512;
            this.summaryDataGridViewTextBoxColumn.Name = "summaryDataGridViewTextBoxColumn";
            this.summaryDataGridViewTextBoxColumn.Width = 150;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // wishesBindingSource
            // 
            this.wishesBindingSource.DataMember = "Wishes";
            this.wishesBindingSource.DataSource = this.rosDBDataSet;
            // 
            // rosDBDataSet
            // 
            this.rosDBDataSet.DataSetName = "RosDBDataSet";
            this.rosDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // announcesBindingSource
            // 
            this.announcesBindingSource.DataMember = "Announces";
            this.announcesBindingSource.DataSource = this.rosDBDataSet;
            // 
            // announcesTableAdapter
            // 
            this.announcesTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AnnouncesTableAdapter = this.announcesTableAdapter;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.OptionsTableAdapter = this.optionsTableAdapter;
            this.tableAdapterManager.UpdateOrder = RosMolAdminPanel.RosDBDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.UsersTableAdapter = null;
            this.tableAdapterManager.WishesTableAdapter = this.wishesTableAdapter;
            // 
            // optionsTableAdapter
            // 
            this.optionsTableAdapter.ClearBeforeFill = true;
            // 
            // wishesTableAdapter
            // 
            this.wishesTableAdapter.ClearBeforeFill = true;
            // 
            // optionsBindingSource
            // 
            this.optionsBindingSource.DataMember = "Options";
            this.optionsBindingSource.DataSource = this.rosDBDataSet;
            // 
            // DBViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 462);
            this.Controls.Add(this.DataGridView);
            this.Controls.Add(this.BindingNavigator);
            this.Controls.Add(this.PhotoView);
            this.Controls.Add(this.PhotoViewer);
            this.Controls.Add(this.label1);
            this.Name = "DBViewer";
            this.Text = "DBViewer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DBViewer_FormClosed);
            this.Load += new System.EventHandler(this.DBAnnounces_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PhotoView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingNavigator)).EndInit();
            this.BindingNavigator.ResumeLayout(false);
            this.BindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wishesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rosDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.announcesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optionsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox PhotoViewer;
        private System.Windows.Forms.PictureBox PhotoView;
        private RosDBDataSet rosDBDataSet;
        private System.Windows.Forms.BindingSource announcesBindingSource;
        private RosDBDataSetTableAdapters.AnnouncesTableAdapter announcesTableAdapter;
        private RosDBDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator BindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton announcesBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView DataGridView;
        private RosDBDataSetTableAdapters.OptionsTableAdapter optionsTableAdapter;
        private System.Windows.Forms.BindingSource optionsBindingSource;
        private RosDBDataSetTableAdapters.WishesTableAdapter wishesTableAdapter;
        private System.Windows.Forms.BindingSource wishesBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn summaryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
    }
}