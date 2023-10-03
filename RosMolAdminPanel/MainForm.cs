using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RosMolAdminPanel
{
    public partial class MainForm : Form
    {
        public static MainForm instance;

        public MainForm()
        {
            InitializeComponent();
            instance = this;
        }

        public static void ShowForm()
        {
            instance.Show();
        }

        public void LoadForm(string key)
        {
            DBViewer form = new DBViewer(key);
            form.Show();
            Hide();
        }
        private void AnnouncesButton_Click(object sender, EventArgs e)
        {
            LoadForm("Announces");
        }

        private void OptionButton_Click(object sender, EventArgs e)
        {
            LoadForm("Options");
        }

        private void WishesButton_Click(object sender, EventArgs e)
        {
            LoadForm("Wishes");
        }
    }
}
