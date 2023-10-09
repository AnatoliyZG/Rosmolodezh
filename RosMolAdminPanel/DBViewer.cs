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

using Rectangle = System.Drawing.Rectangle;

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

        private TextEditor textEditor;

        private Dictionary<string, Image> images = new Dictionary<string, Image>();

        public DBViewer()
        {
            InitializeComponent();

            LoadDB("Announces");

        }

        private void announcesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            UpdateTextEditor();
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
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < DataGridView.RowCount; j++)
                {
                    if (DataGridView[i, j].Value is DBNull)
                    {
                        DataGridView[i, j].Value = string.Empty;
                    }
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

            if (ServerConnected && photos != null)
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
        }

        private void LoadImages()
        {
            if (!ServerConnected) return;

            for (int i = 0; i < DataGridView.RowCount; i++)
            {
                string value = $"{DataGridView[0, i].Value}.jpg";
                if (images.ContainsKey(value))
                {
                    DataGridView[7, i].Value = images[value];
                }
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            changed = true;
        }

        private bool changed = false;

        private int index = 0;

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (index == toolStripComboBox1.SelectedIndex)
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
                    toolStripComboBox1.Text = key;
                    toolStripComboBox1.SelectedIndex = index;
                    return;
                }
            }

            changed = false;

            index = toolStripComboBox1.SelectedIndex;
            LoadDB(pages[index]);
        }

        private void DataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            changed = true;
        }
        private void DataGridView_Sorted(object sender, EventArgs e)
        {
            LoadImages();
        }

        private async void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine(e.ColumnIndex);
        }

        private void UpdateTextEditor()
        {
            if (textEditor == null) return;

            if (textEditor.GetText == null)
            {
                textEditor.Cell.Value = DBNull.Value;
            }
            else
            {
                textEditor.Cell.Value = textEditor.GetText;
            }
        }

        public static Bitmap resizeImage(Bitmap imgToResize, Size size)
        {
            int height = (int)(imgToResize.Width / 2.235f);

            if (height > imgToResize.Height)
            {
                return (new Bitmap(imgToResize, size));
            }

            int width = imgToResize.Width;

            int x = 0;
            int y = (imgToResize.Height - height) / 2;

            Rectangle rectangle = new Rectangle(x, y, width, height);

            Bitmap bmp = new Bitmap(imgToResize);
            bmp = bmp.Clone(rectangle, imgToResize.PixelFormat);

            imgToResize.Dispose();

            return (new Bitmap(bmp, size));
        }
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (textEditor != null) textEditor.Bold();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (textEditor != null) textEditor.Italic();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (textEditor != null) textEditor.Underline();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (textEditor != null) textEditor.UnorderedList();
        }

        private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int column = e.ColumnIndex;

            if (column != 1 && column != 2 && column != 3)
                return;

            if (textEditor != null)
            {
                if (DataGridView[column, e.RowIndex].Value is DBNull)
                {
                    textEditor.GetText = "";
                }
                else
                {
                    textEditor.GetText = (string)DataGridView[column, e.RowIndex].Value;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (textEditor != null) textEditor.Tab();
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            if (e.ColumnIndex == 7 && ServerConnected)
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

                        string val = $"{DataGridView[0, e.RowIndex].Value}.jpg";

                        if (images.ContainsKey(val))
                        {
                            images[val].Dispose();
                            images[val] = bmp;
                        }
                        else
                        {
                            images.Add(val, bmp);
                        }

                        if (bmp.Width > 380 || bmp.Height > 170)
                        {
                            try
                            {
                                bmp = resizeImage(bmp, new Size(380, 170));
                                UploadPhoto($"{key}/{val}", ImageToByte(bmp));

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        else
                        {
                            UploadPhoto($"{key}/{val}", buffer);
                        }

                        DataGridView[e.ColumnIndex, e.RowIndex].Value = bmp;
                    }
                }
            }
            else
            {
                int selected = e.ColumnIndex;
                int row = e.RowIndex;

                if (selected == 1 || selected == 2 || selected == 3)
                {


                    var cell = DataGridView[selected, row];

                    if (textEditor != null)
                    {
                        if (textEditor.Cell == cell)
                        {
                            return;
                        }
                        UpdateTextEditor();
                    }

                    object obj = cell.Value;
                    textEditor = new TextEditor(webBrowser1, obj is DBNull ? "" : (string)obj, cell);
                }
            }
        }
    }
}
