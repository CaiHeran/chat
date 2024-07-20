using Info;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    using DB = Database;

    public partial class FormHome : Form
    {
        public static FormHome? formhome { get; set; }

        public FormHome()
        {
            InitializeComponent();
            formhome = this;

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
                FormChatRoom.formchatroom = new FormChatRoom();
                FormChatRoom.formchatroom!.Show();
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
            int room_id = int.Parse(textBox_roomid.Text);
            fj = (_, msg) =>
            {
                if (msg.ec != 0)
                {
                    MessageBox.Show("房间号无效，请重新输入", "游戏标题");
                    return;
                }
                else
                {
                    Joinroom_Callback();
                    DB.Room = new(msg.room, msg.list); //无异常，加入房间
                    this.Hide();
                    FormChatRoom.formchatroom = new FormChatRoom();
                    FormChatRoom.formchatroom!.Show();
                }
            };
            Process.Myjoinroom += fj;
            Functions.JoinRoom(int.Parse(textBox_roomid.Text));
        }
        private void Joinroom_Callback()
        {
            Process.Myjoinroom -= fj;
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("真的要退出吗？", "游戏标题", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;
            this.Close();
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }
    }
}
