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
        EventHandler<TryJoinRoom>? fj;

        private void button_CreateRoom_Click(object sender, EventArgs e)
        {
            fc = (_, info) =>
            {
                Createroom_Callback();
                this.Hide();
                new FormChatRoom();
                FormChatRoom.formchatroom!.Show();
                //
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
            int room_id = int.Parse(textBox_id.Text);
            fj = (_, info) =>
            {
                Joinroom_Callback();
                this.Hide();
                new FormChatRoom();
                FormChatRoom.formchatroom!.Show();
            };
            Process.Tryjoinroom += fj;
            Functions.JoinRoom(DB.Me.Id);
        }
        private void Joinroom_Callback()
        {
            Process.Tryjoinroom -= fj;
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
