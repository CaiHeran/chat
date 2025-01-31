using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    using DB = Database;

    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private async void button_login_Click(object sender, EventArgs e)
        {
            JsonNode res = await Requests.RegisterAsync(textBox_name.Text);
            if (res["errors"] != null)
            {
                MessageBox.Show("注册失败。", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DB.Me = new User(res["id"]!.GetValue<int>(), textBox_name.Text);

            Hide();
            new FormHome().Show();
        }
    }
}
