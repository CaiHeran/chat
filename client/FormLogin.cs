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

    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        EventHandler<Login>? f;

        private void button_login_Click(object sender, EventArgs e)
        {
            f = (_, info) => {
                Login_callback();
                new FormHome();
                Hide();
                FormHome.form!.Show();
            };
            Process.Login += f;
            Functions.Login(textBox_name.Text);
        }

        public void Login_callback()
        {
            Process.Login -= f;
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("真的要退出吗？", "标题", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;
            this.Close();
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }
    }
}
