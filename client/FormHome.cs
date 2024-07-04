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

namespace Client
{
    public partial class FormHome : Form
    {
        public static FormHome formhome = new FormHome();

        public FormHome()
        {
            InitializeComponent();
            formhome = this;
        }

        // buttons
        // button_login
        //
        private void button_login_Click(object sender, EventArgs e)
        {
            Functions.SetMyInfo(textBox_name.Text, (_, info) =>
            {

            });

        }
        //
        // button_back
        //
        private void button_back_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormConnect.formconnect.Show();
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
