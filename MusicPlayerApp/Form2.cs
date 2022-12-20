using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicPlayerApp
{
    public partial class FormLyrics : Form
    {
        public FormLyrics(string path)
        {
            InitializeComponent();
            string file = Application.StartupPath + "\\Lyrics\\" + path + ".txt";
            textBoxMain.Text = "";
            showLyrics(file);
        }
        private void showLyrics(string path)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    textBoxMain.Text += s;
                    textBoxMain.Text += "\r\n";
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
