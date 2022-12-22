using MaterialSkin;
using MaterialSkin.Controls;
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
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            addcombobox();
            createPlayList();
        }
        int Shuffle_check = 0;
        int Repeat_check = 0;
        int currentIDSong = 0;
        List<int> IDSong = new List<int>();
        string pathfile = "";
        string list_name = "";
        WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
        DataProvider provider = new DataProvider();
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            panelSearch.Visible = false;
            panelPlayList.Visible = false;
            button2.Visible = true;
            button1.Visible = true;
        }


        private void buttonSearchTab_Click(object sender, EventArgs e)
        {
            panelSearch.Visible = true;
            panelPlayList.Visible = false;
            button2.Visible = true;
            button1.Visible = true;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string rbcheck ="";
            if (radioButtonName.Checked) rbcheck = "NAME_SONG";
            else if (radioButtonArtist.Checked) rbcheck = "ARTIST";
            else if (radioButtonTopic.Checked) rbcheck = "TOPIC";
            string sqlcmd = "SELECT * FROM dbo.INFO_SONG" + " WHERE " + rbcheck + " LIKE '%" + textBoxSearch.Text+"%'";
            DataTable data = provider.excuteQuery(sqlcmd);
            int tmp = 0;
            IDSong.Clear();
            flowLayoutPanelList.Controls.Clear();
            foreach (DataRow dataR in data.Rows)
            {
                IDSong.Add(Int32.Parse(dataR[0].ToString()));
                flowLayoutPanelList.Controls.Add(create_list(dataR, tmp));
                tmp += 1;
            }
        }
        private Panel create_list(DataRow data, int tmp)
        {
            //create panel
            Panel panel7 = new Panel();
            panel7.Location = new System.Drawing.Point(3, 3);
            panel7.Name = "pnl";
            panel7.Size = new System.Drawing.Size(840, 100);
            panel7.Cursor = System.Windows.Forms.Cursors.Hand;
            panel7.TabIndex = 0;
            panel7.BorderStyle = BorderStyle.FixedSingle;
            //picture
            PictureBox pictureBox6 = new PictureBox();
            string path = Application.StartupPath + "\\img\\" + data[2].ToString()+".jpg";
            Bitmap bit = new Bitmap(path);
            pictureBox6.BackgroundImage = bit;
            pictureBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new System.Drawing.Size(102, 76);
            pictureBox6.TabIndex = 10;
            pictureBox6.TabStop = false;
            //labelName
            Label label12 = new Label();
            label12.AutoSize = true;
            label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label12.ForeColor = System.Drawing.Color.White;
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(165, 29);
            label12.TabIndex = 10;
            label12.Text = data[1].ToString();
            //label Artist
            Label label11 = new Label();
            label11.AutoSize = true;
            label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label11.ForeColor = System.Drawing.Color.White;
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(107, 20);
            label11.TabIndex = 11;
            label11.Text = data[3].ToString();
            //labelAlbum
            Label label13 = new Label();
            label13.AutoSize = true;
            label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label13.ForeColor = System.Drawing.Color.White;
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(86, 29);
            label13.TabIndex = 12;
            label13.Text = "Album: " + data[5].ToString();
            //laberl time
            Label label14 = new Label();
            label14.AutoSize = true;
            label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label14.ForeColor = System.Drawing.Color.White;
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(73, 29);
            label14.TabIndex = 13;
            label14.Text = data[4].ToString();
            panel7.Controls.Add(pictureBox6);
            panel7.Controls.Add(label12);
            panel7.Controls.Add(label11);
            panel7.Controls.Add(label13);
            panel7.Controls.Add(label14);
            panel7.Click+= (sender, e) => Onclick(this, e, data, tmp);
            pictureBox6.Location = new System.Drawing.Point(10, 10);
            label12.Location = new System.Drawing.Point(141, 19);
            label11.Location = new System.Drawing.Point(142, 57);
            label13.Location = new System.Drawing.Point(350, 48);
            label14.Location = new System.Drawing.Point(760, 50);
            return panel7;
        }
        private void createEvent(DataRow data, int tmp)
        {
            string path_ = Application.StartupPath + "\\img\\" + data[2].ToString() + ".jpg";
            Bitmap bit = new Bitmap(path_);
            pictureBoxMusic.BackgroundImage = bit;
            player.URL = Application.StartupPath + "\\DataMusic\\" + data[2].ToString() + ".mp3";
            player.controls.play();
            labelNameSong.Text = data[1].ToString();
            labelTimeMax.Text = data[4].ToString();
            labelArtist.Text = data[3].ToString();
            pathfile = data[2].ToString();
            panel6.Visible = true;
            check_like();
            buttonPlay.BackgroundImage = Properties.Resources.stop;
            currentIDSong = tmp;
        }
        private void Onclick(object sender, EventArgs e, DataRow data, int tmp)
        {
            createEvent(data, tmp);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                p_bar.Maximum = (int)player.controls.currentItem.duration-1;
                p_bar.Value = (int)player.controls.currentPosition;
            }
            labelTimeCurrent.Text = player.controls.currentPositionString;
            if (trackBarVol.Value == 0)
            {
                buttonVol.BackgroundImage = Properties.Resources.Vol_Min;
            }
            else
            {
                buttonVol.BackgroundImage = Properties.Resources.Vol_Max;
            }
            if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                if (p_bar.Value == p_bar.Maximum)
                {
                    if (Repeat_check != 1)
                    {
                        int tmp = 0;
                        Random rnd = new Random();
                        if (Shuffle_check == 1) { tmp = rnd.Next(0, IDSong.Count); }
                        else { tmp = 1; }
                        string sqlcmd = "SELECT * FROM dbo.INFO_SONG WHERE ID = " + IDSong[(currentIDSong + tmp) % (IDSong.Count)].ToString();
                        DataTable dt = provider.excuteQuery(sqlcmd);
                        DataRow dtR = dt.Rows[0];
                        createEvent(dtR, currentIDSong + 1);
                        Repeat_check = 0;
                    }
                    else
                    {
                        string sqlcmd = "SELECT * FROM dbo.INFO_SONG WHERE ID = " + IDSong[currentIDSong].ToString();
                        DataTable dt = provider.excuteQuery(sqlcmd);
                        DataRow dtR = dt.Rows[0];
                        createEvent(dtR, currentIDSong);
                        ;
                    }
                }
            }
        }

        private void p_bar_MouseDown(object sender, MouseEventArgs e)
        {
            player.controls.currentPosition = player.currentMedia.duration * e.X / p_bar.Width;
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                player.controls.pause();
                buttonPlay.BackgroundImage = Properties.Resources.play;
            }
            else
            {
                player.controls.play();
                buttonPlay.BackgroundImage = Properties.Resources.stop;
            }
        }
        public void check_like()
        {
            DataTable dt = provider.excuteQuery("SELECT * FROM dbo.LIST_LIKED");
            if (check_name_tmp(pathfile, dt) == 1){
                buttonLiked.BackgroundImage = Properties.Resources.liked;
            }
            else { buttonLiked.BackgroundImage = Properties.Resources.unliked; }
        }
        private void trackBarVol_Scroll(object sender, EventArgs e)
        {
            player.settings.volume = trackBarVol.Value;
        }
        int tmp = 0;
        private void buttonVol_Click(object sender, EventArgs e)
        {
            if (trackBarVol.Value != 0)
            {
                tmp = trackBarVol.Value;
                trackBarVol.Value = 0;
                player.settings.volume = 0;
                buttonVol.BackgroundImage = Properties.Resources.Vol_Min;
            }
            else
            {
                trackBarVol.Value = tmp;
                player.settings.volume = tmp;
                buttonVol.BackgroundImage = Properties.Resources.Vol_Max;
            }
        }

        private void buttonLyrics_Click(object sender, EventArgs e)
        {
            FormLyrics f = new FormLyrics(pathfile);
            f.ShowDialog();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            FormAdd f = new FormAdd();
            f.ShowDialog();
            addcombobox();
            createPlayList();
        }
        int check_name_tmp(string s, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (s == dt.Rows[i].ItemArray[1].ToString()) return 1;
            }
            return 0;
        }
        private void addcombobox()
        {
            comboBoxList.Items.Clear();
            DataTable dt = provider.excuteQuery("SELECT * FROM dbo.DS_LIST");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxList.Items.Add(dt.Rows[i].ItemArray[1].ToString());
            }
            comboBoxList.SelectedItem = 0;
        }
        private void buttonLiked_Click(object sender, EventArgs e)
        {
            DataTable dt = provider.excuteQuery("SELECT * FROM dbo.LIST_LIKED");
            if (check_name_tmp(pathfile, dt) != 1)
            {
                buttonLiked.BackgroundImage = Properties.Resources.liked;
                string sqlcmd = "INSERT INTO LIST_LIKED(NAME_SONG) VALUES('" + pathfile + "')";
                provider.excuteNonquery(sqlcmd);
            }
            else
            {
                buttonLiked.BackgroundImage = Properties.Resources.unliked;
                string sqlcmd = "DELETE FROM LIST_LIKED WHERE NAME_SONG ='" + pathfile + "'";
                provider.excuteNonquery(sqlcmd);
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxList.Text == "")
            {
                MessageBox.Show("Please choose list you want to add");
            }
            else
            {
                DataTable dt = provider.excuteQuery("SELECT * FROM dbo.[" + comboBoxList.Text+"]");
                if (check_name_tmp(pathfile, dt) != 1)
                {
                    string sqlcmd = "INSERT INTO [" + comboBoxList.Text + "] (NAME_SONG) VALUES('" + pathfile + "')";
                    provider.excuteNonquery(sqlcmd);
                }
            }
        }
        private void PlayList(string list_name)
        {
            IDSong.Clear();
            string sqlcmd = "SELECT * FROM dbo.INFO_SONG, dbo.["+list_name+"] as liked WHERE  PATH_SONG = liked.NAME_SONG";
            DataTable dt = provider.excuteQuery(sqlcmd);
            int tmp = 0;
            flowLayoutPanelSong.Controls.Clear();
            foreach (DataRow dataR in dt.Rows)
            {
                flowLayoutPanelSong.Controls.Add(create_list(dataR,tmp));
                IDSong.Add(Int32.Parse(dataR[0].ToString()));
                tmp += 1;
            }
        }
        private void buttonLikeList_Click(object sender, EventArgs e)
        {
            flowLayoutPanelSong.Controls.Clear();
            panelSearch.Visible = false;
            panelPlayList.Visible = true;
            PlayList("LIST_LIKED");
            button2.Visible = false;
            button1.Visible = false;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            int tmp = 0;
            Random rnd = new Random();
            if (Shuffle_check == 1) { tmp = rnd.Next(0, IDSong.Count); }
            else { tmp = 1; }
            string sqlcmd = "SELECT * FROM dbo.INFO_SONG WHERE ID = " + IDSong[(currentIDSong + tmp) % (IDSong.Count)].ToString();
            DataTable dt = provider.excuteQuery(sqlcmd);
            DataRow dtR = dt.Rows[0];
            createEvent(dtR, currentIDSong + 1);
            buttonRepeat.BackColor = Color.MediumSeaGreen;
            Repeat_check = 0;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (0 == currentIDSong) { currentIDSong = IDSong.Count; }
            string sqlcmd = "SELECT * FROM dbo.INFO_SONG WHERE ID = " + IDSong[currentIDSong  - 1].ToString();
            DataTable dt = provider.excuteQuery(sqlcmd);
            DataRow dtR = dt.Rows[0];
            createEvent(dtR, currentIDSong - 1);
            buttonRepeat.BackColor = Color.MediumSeaGreen;
            Repeat_check = 0;
        }
        private void createPlayList()
        {
            flowLayoutPanelPlayList.Controls.Clear();
            DataTable dt = provider.excuteQuery("SELECT * FROM dbo.DS_LIST");
            foreach(DataRow dtR in dt.Rows)
            {
                flowLayoutPanelPlayList.Controls.Add(createLabel(dtR));
            }
        }
        private Label createLabel(DataRow data)
        {
            Label label11 = new Label();
            label11.AutoSize = true;
            label11.Cursor = System.Windows.Forms.Cursors.Hand;
            label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label11.Location = new System.Drawing.Point(3, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(300, 36);
            label11.TabIndex = 0;
            label11.Text = data[1].ToString();
            label11.AutoSize = false;
            label11.MouseLeave += (sender, e) => label11_MouseHover(this, e, ref label11);
            label11.MouseHover += (sender, e) => label11_MouseLeave(this, e, ref label11);
            label11.Click += (sender, e) => label11_Click(this, e, data);
            return label11;
        }
        private void label11_MouseHover(object sender, EventArgs e, ref Label label11)
        {
            label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void label11_MouseLeave(object sender, EventArgs e, ref Label label11)
        {
            label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
        private void label11_Click(object sender, EventArgs e, DataRow data)
        {
            flowLayoutPanelSong.Controls.Clear();
            panelSearch.Visible = false;
            panelPlayList.Visible = true;
            PlayList(data[1].ToString());
            list_name = data[1].ToString();
            button1.Visible = true;
            button2.Visible = true;
        }

        private void buttonShuffle_Click(object sender, EventArgs e)
        {
            if (Shuffle_check == 1)
            {
                buttonShuffle.BackColor = Color.MediumSeaGreen;
                Shuffle_check = 0;
            }
            buttonShuffle.BackColor = Color.DarkGreen;
            Shuffle_check = 1;
        }

        private void buttonRepeat_Click(object sender, EventArgs e)
        {
            if (Repeat_check != 1)
            {
                buttonRepeat.BackColor = Color.DarkGreen;
                Repeat_check = 1;
            }
            else
            {
                buttonRepeat.BackColor = Color.MediumSeaGreen;
                Repeat_check = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlcmd = "DELETE FROM [" + list_name + "] WHERE NAME_SONG ='" + pathfile + "'";
            provider.excuteNonquery(sqlcmd);
            PlayList(list_name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sqlcmd = "DROP TABLE ["+list_name+"]";
            provider.excuteNonquery(sqlcmd);
            sqlcmd = "DELETE FROM DS_LIST WHERE NAME_LIST ='" + list_name + "'";
            provider.excuteNonquery(sqlcmd);
            panelPlayList.Visible = false;
            createPlayList();
        }


        private void panel4_MouseClick(object sender, MouseEventArgs e)
        {
            flowLayoutPanelSong.Controls.Clear();
            panelSearch.Visible = false;
            panelPlayList.Visible = true;
            PlayList("RAP");
            button2.Visible = false;
            button1.Visible = false;
        }
        private void panel5_MouseClick(object sender, MouseEventArgs e)
        {
            flowLayoutPanelSong.Controls.Clear();
            panelSearch.Visible = false;
            panelPlayList.Visible = true;
            PlayList("CHILL");
            button2.Visible = false;
            button1.Visible = false;
        }
        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {
            flowLayoutPanelSong.Controls.Clear();
            panelSearch.Visible = false;
            panelPlayList.Visible = true;
            PlayList("HOT_HIT");
            button2.Visible = false;
            button1.Visible = false;
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
