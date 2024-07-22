using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Info;

namespace Client
{
    using DB = Database;

    public partial class FormHome : Form
    {
        public static FormHome? form { get; set; }

        public FormHome()
        {
            InitializeComponent();
            form = this;

            label_ID.Text = $" ID: {DB.Me.Id}";
            label_name.Text = $"Name: {DB.Me.Name}";
        }

        EventHandler<RoomCreate>? fc;
        EventHandler<MyJoinRoom>? fj;

        private void button_CreateRoom_Click(object sender, EventArgs e)
        {
            fc = (_, info) =>
            {
                Createroom_Callback();
                this.Hide();
                new FormChatRoom().Show();
            };
            Process.Roomcreate += fc;
            Functions.CreateRoom();
        }
        private void Createroom_Callback()
        {
            Process.Roomcreate -= fc;
        }

        private void button_JoinRoom_Click(object sender, EventArgs e)
        {
            if (textBox_roomid.Text == "")
            {
                errorProvider_join.SetError(button_JoinRoom, "房间号不可为空");
                label_tip.Text = "房间号不可为空";
                label_tip.Visible = true;
                return;
            }
            try { int room_id = int.Parse(textBox_roomid.Text); }
            catch
            {
                errorProvider_join.SetError(button_JoinRoom, "房间号格式错误");
                label_tip.Text = "房间号格式错误";
                label_tip.Visible = true;
                return;
            }

            Process.Myjoinroom += fj = (_, msg) =>
            {
                if (msg.ec != 0)
                {
                    errorProvider_join.SetError(button_JoinRoom, "房间号无效");
                    label_tip.Text = "房间号无效";
                    label_tip.Visible = true;
                    return;
                }
                else
                {
                    Joinroom_Callback();
                    label_tip.Visible = false;
                    errorProvider_join.Clear();
                    DB.Room = new(msg.room, msg.list); //无异常，加入房间
                    new FormChatRoom();
                    Hide();
                    FormChatRoom.form!.Show();
                }
            };
            Functions.JoinRoom(int.Parse(textBox_roomid.Text));
        }
        private void Joinroom_Callback()
        {
            Process.Myjoinroom -= fj;
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("真的要关闭吗？", "Home Menu", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            MessageBox.Show($"Your res: {result}");
            if (result == DialogResult.OK)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                e.Cancel = false;
                Application.Exit();
                Application.ExitThread();
                Environment.Exit(0);
            }
        }
    }
}
