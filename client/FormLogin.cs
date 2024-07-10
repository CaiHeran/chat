using Client;
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
    public partial class FormLogin : Form
    {
        public static FormLogin? formlogin;

        public FormLogin()
        {
            InitializeComponent();
            formlogin = this;
        }

        EventHandler<Login>? f;

        // buttons
        // button_login
        //
        private void button_login_Click(object sender, EventArgs e)
        {
            f = (_, info) => {
                //Requiring fixing
            };
            Functions.Login(textBox_name.Text, f);
            this.Hide();
            Login_callback();
            FormHome.formhome = new FormHome();
            FormHome.formhome.ShowDialog();
        }

        public void Login_callback()
        {
            Process.Login -= f;//???
        }
        
        //
        // button_exit
        //
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
