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

            //Requiring fixing
            //label_ID.Text = $" ID: {DB.Me.Id}";
            //label_name.Text = $"Name: {DB.Me.Name}";
        }

        EventHandler<RoomCreate>? fc;
        EventHandler<TryJoinRoom>? fj;
        private void button_CreateRoom_Click(object sender, EventArgs e)
        {
            fc = (_, info) =>
            {
            };
            Functions.CreateRoom(fc);
            this.Hide();
            FormRoom formroom = new FormRoom(); 
            formroom.ShowDialog();
        }

        private void button_JoinRoom_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox_id.Text);
            fj = (_, info) =>
            {
            };
            Functions.JoinRoom(DB.Me.Id, fj);
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
