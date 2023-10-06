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
        private string[] pages =
        {
            "Announces",
            "Wishes",
            "Options",
            "News",
            "Events",
        };

        private string key;

        private BindingSource _bindingSource;
        private dynamic _tableAdapter;
        private dynamic _tableData;

        private bool serverConnect = true;

        private Dictionary<string, Image> images = new Dictionary<string, Image>();

        public DBViewer()
        {
            InitializeComponent();

            LoadDB("Announces");
        }

        private void announcesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            SaveDB();
            changed = false;
        }


        public void SaveDB()
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

            _tableAdapter.Update(_tableData);

            UploadDB(key);
        }

        public async void LoadDB(string key)
        {
            this.key = key;

            foreach (var img in images.Values)
            {
                img.Dispose();
            }
            images.Clear();
            if (key == pages[4])
            {
                DataGridView.Columns[4].Visible = true;
                DataGridView.Columns[5].Visible = true;
                DataGridView.Columns[6].Visible = true;

                _bindingSource = eventsBindingSource;
                _tableAdapter = eventsTableAdapter;
                _tableData = rosDBDataSet.Events;
            }
            else if (key == pages[3])
            {
                DataGridView.Columns[4].Visible = false;
                DataGridView.Columns[5].Visible = true;
                DataGridView.Columns[6].Visible = true;

                _bindingSource = newsBindingSource;
                _tableAdapter = newsTableAdapter;
                _tableData = rosDBDataSet.News;
            }
            else
            {
                DataGridView.Columns[4].Visible = false;
                DataGridView.Columns[5].Visible = false;
                DataGridView.Columns[6].Visible = false;

                if (key == pages[0])
                {
                    _bindingSource = announcesBindingSource;
                    _tableAdapter = announcesTableAdapter;
                    _tableData = rosDBDataSet.Announces;

                }
                else if (key == pages[1])
                {
                    _bindingSource = wishesBindingSource;
                    _tableAdapter = wishesTableAdapter;
                    _tableData = rosDBDataSet.Wishes;

                }
                else if (key == pages[2])
                {
                    _bindingSource = optionsBindingSource;
                    _tableAdapter = optionsTableAdapter;
                    _tableData = rosDBDataSet.Options;
                }
            }
            DataGridView.DataSource = _bindingSource;
            BindingNavigator.BindingSource = _bindingSource;
            _tableAdapter.Fill(_tableData);

            string[] photos = await GetServerPhotos(key);

            serverConnect = photos != null && photos.Length > 0;

            if (serverConnect)
            {
                foreach (string val in photos)
                {
                    var photo = await GetServerPhoto($"{key}/{val}");

                    Bitmap bmp;

                    using (var ms = new MemoryStream(photo))
                    {
                        bmp = new Bitmap(ms);
                    }

                    images.Add(val, bmp);
                }
            }

            LoadImages();
            DataGridView.Columns[0].Visible = false;

        }

        private void LoadImages()
        {
            if (!serverConnect) return;

            for (int i = 0; i < DataGridView.RowCount; i++)
            {
                string value = $"{DataGridView.Rows[i].Cells[0].Value}.jpg";
                if (images.ContainsKey(value))
                {
                    DataGridView.Rows[i].Cells[7].Value = images[value];
                }
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            changed = true;
        }

        private bool changed = false;

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (key == pages[toolStripComboBox1.SelectedIndex])
                return;

            if (changed)
            {
                var result = MessageBox.Show("Сохранить изменения?", "Внимание", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    SaveDB();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }
            changed = false;

            LoadDB(pages[toolStripComboBox1.SelectedIndex]);
        }

        private void DataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            changed = true;
        }
        private void DataGridView_Sorted(object sender, EventArgs e)
        {
            LoadImages();
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine(e.ColumnIndex);

            if (e.ColumnIndex == 7 && serverConnect)
            {
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Filter = "Image file (*.jpg,*.jpeg,*.png)|*.jpg;*.jpeg;*.png";

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        byte[] buffer = File.ReadAllBytes(dialog.FileName);

                        Bitmap bmp;

                        using (var ms = new MemoryStream(buffer))
                        {
                            bmp = new Bitmap(ms);
                        }

                        string val = $"{DataGridView.Rows[e.RowIndex].Cells[0].Value}.jpg";

                        if (images.ContainsKey(val))
                        {
                            images[val].Dispose();
                            images[val] = bmp;
                        }
                        else
                        {
                            images.Add(val, bmp);
                        }

                        UploadPhoto($"{key}/{val}", buffer);

                        DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = bmp;
                    }
                }
            }
        }
    }
}
 