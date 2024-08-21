using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

using Info;

namespace Client
{
    using static System.Windows.Forms.Design.AxImporter;
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

        EventHandler<JsonNode?>? fc;
        EventHandler<JsonNode?>? fj;

        private void button_CreateRoom_Click(object sender, EventArgs e)
        {
            //Process.Roomcreate += fc;
            Process.Register(20, fc = (_, info) =>
            {
                Createroom_Callback();
                this.Hide();
                new FormChatRoom().Show();
            });
            Functions.CreateRoom();
        }
        private void Createroom_Callback()
        {
            Process.Unregister(20, fc);
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

            Process.Register(21, fj = (_, json) =>
            {
                var msg = JsonSerializer.Deserialize<MyJoinRoom>(json, new JsonSerializerOptions{IncludeFields=true})!;
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
            });
            Functions.JoinRoom(int.Parse(textBox_roomid.Text));
        }
        private void Joinroom_Callback()
        {
            Process.Unregister(21, fj);
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("真的要关闭吗？", "Home Menu", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
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
