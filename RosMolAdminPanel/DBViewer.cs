using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static RosMolAdminPanel.General;

namespace RosMolAdminPanel
{
    public partial class DBViewer : Form
    {
        private string key;
        private bool serverConnect = true;

        public DBViewer(string key)
        {
            this.key = key;
            InitializeComponent();

            switch (key)
            {
                case "Options":
                    DataGridView.DataSource = optionsBindingSource;
                    BindingNavigator.BindingSource = optionsBindingSource;
                    break;
                case "Announces":
                    DataGridView.DataSource = announcesBindingSource;
                    BindingNavigator.BindingSource = announcesBindingSource;
                    break;
                case "Wishes":
                    DataGridView.DataSource = wishesBindingSource;
                    BindingNavigator.BindingSource = wishesBindingSource;
                    break;
            }

            PhotoViewerLoad();
        }

        private void announcesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();

            BindingNavigator.BindingSource.EndEdit();

            int max = 0;
            for (int i = 0; i < DataGridView.RowCount; i++)
            {
                max = Math.Max(max, (int)(DataGridView[0, i].Value ?? -1));
            }

            for (int i = 0; i < DataGridView.RowCount; i++)
            {
                if ((int)(DataGridView[0, i].Value ?? -1) == -1)
                {
                    DataGridView[0, i].Value = ++max;
                }
            }

            switch (key)
            {
                case "Options":
                    optionsTableAdapter.Update(rosDBDataSet.Options);
                    break;
                case "Announces":
                    announcesTableAdapter.Update(rosDBDataSet.Announces);
                    break;
                case "Wishes":
                    wishesTableAdapter.Update(rosDBDataSet.Wishes);
                    break;
            }

            UploadDB(key);
        }

        private void DBAnnounces_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "rosDBDataSet.Wishes". При необходимости она может быть перемещена или удалена.
            this.wishesTableAdapter.Fill(this.rosDBDataSet.Wishes);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "rosDBDataSet.Options". При необходимости она может быть перемещена или удалена.
            this.optionsTableAdapter.Fill(this.rosDBDataSet.Options);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "rosDBDataSet.Announces". При необходимости она может быть перемещена или удалена.
            this.announcesTableAdapter.Fill(this.rosDBDataSet.Announces);
            switch (key)
            {
                case "Options":
                    this.optionsTableAdapter.Fill(this.rosDBDataSet.Options);
                    break;
                case "Announces":
                    this.announcesTableAdapter.Fill(this.rosDBDataSet.Announces);
                    break;
                case "Wishes":
                    this.wishesTableAdapter.Fill(this.rosDBDataSet.Wishes);
                    break;
            }
        }

        private async void PhotoViewerLoad()
        {
            string[] photos = await GetServerPhotos(key);

            if (photos != null && photos.Length > 0)
            {
                PhotoViewer.Items.AddRange(photos);
            }
        }

        private void DBViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.ShowForm();
        }

        private async void PhotoViewer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!serverConnect) return;

            string fileName = (string)PhotoViewer.SelectedItem;
            try
            {
                var photo = await GetServerPhoto($"{key}/{fileName}");

                Console.WriteLine(photo);

                Bitmap bmp;

                using (var ms = new MemoryStream(photo))
                {
                    bmp = new Bitmap(ms);
                }

                PhotoView.Image = bmp;
                // bmp.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }

        private void DataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }
    }
}
