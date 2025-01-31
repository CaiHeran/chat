using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    using static System.Windows.Forms.Design.AxImporter;
    using DB = Database;

    public partial class FormHome : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static FormHome? form { get; set; }

        public FormHome()
        {
            InitializeComponent();
            form = this;

            label_ID.Text = $" ID: {DB.Me.Id}";
            label_name.Text = $"Name: {DB.Me.Name}";
        }

        private async void button_CreateRoom_Click(object sender, EventArgs e)
        {
            JsonNode res = await Requests.CreateRoomAsync();
            if (res["errors"] != null)
            {
                MessageBox.Show("创建房间失败", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DB.Room = new Room(res["roomid"]!.GetValue<int>());
            this.Hide();
            new FormChatRoom(DB.Room).Show();
        }

        private async void button_JoinRoom_Click(object sender, EventArgs e)
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

            JsonNode res = await Requests.JoinRoomAsync(int.Parse(textBox_roomid.Text));

            if (res["errors"] != null)
            {
                errorProvider_join.SetError(button_JoinRoom, "房间号无效");
                label_tip.Text = "房间号无效 " + res["errors"].GetValue<string>();
                label_tip.Visible = true;
                return;
            }
            else
            {
                label_tip.Visible = false;
                errorProvider_join.Clear();
                DB.Room = new(res);
                Hide();
                new FormChatRoom(DB.Room).Show();
            }
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
