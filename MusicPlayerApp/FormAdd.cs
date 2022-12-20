using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicPlayerApp
{
    public partial class FormAdd : Form
    {
        DataProvider provider = new DataProvider();
        public FormAdd()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        int check_name_tmp(string s, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (s == dt.Rows[i].ItemArray[1].ToString()) return 1;
            }
            return 0;
        }
        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text == "") { MessageBox.Show("Please enter  list name!"); }
            else
            {
                DataTable dt = provider.excuteQuery("SELECT * FROM dbo.DS_LIST");
                if(check_name_tmp(textBoxName.Text, dt) == 1) { MessageBox.Show("This name has existed"); }
                else
                {
                    string sqlcmd_1 = "CREATE TABLE [" + textBoxName.Text + "] (" + "\n" +
                                   "ID INT NOT NULL IDENTITY(1, 1)," + "\n" +
                                    "NAME_SONG NVARCHAR(255) NOT NULL," + "\n" +
                                    "PRIMARY KEY(ID)" + "\n" +
                                    "); ";
                    provider.excuteNonquery(sqlcmd_1);
                    string sql_2 = "INSERT INTO DS_LIST(NAME_LIST) VALUES(N'" + textBoxName.Text + "')";
                    provider.excuteNonquery(sql_2);
                    MessageBox.Show("Create list completely");
                }
            }
            this.Close();
        }
    }
}
