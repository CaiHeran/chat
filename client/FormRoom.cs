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

    public partial class FormRoom : Form
    {
        public FormRoom()
        {
            InitializeComponent();
            listView_members_Load();
        }

        private void listView_members_Load()
        {
            // Todo 申请访问房间成员列表
        }
        //
        // buttons
        // button_send
        private void button_send_Click(object sender, EventArgs e)
        {
            string msg = textBox_input.Text;
            Functions.SendMessage(msg);
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
        //
        // richTextBoxs
        // richTextBox_view
        // Todo：怎样实现文本实时更新
    }
}
