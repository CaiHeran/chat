﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Text.Json.Nodes;
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

        EventHandler<JsonNode?>? f;

        private void button_login_Click(object sender, EventArgs e)
        {
            f = (_, info) =>
            {
                Login_callback();
                new FormHome();
                Hide();
                FormHome.form!.Show();
            };
            //Process.Login += f;
            Process.Register(1, f);
            Functions.Login(textBox_name.Text);
        }

        public void Login_callback()
        {
            Process.Unregister(1, f);
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("真的要退出吗？", "Login", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
