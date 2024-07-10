using client;
using System.Net.Security;
using System.Security.Authentication;
using System.Windows.Forms;

namespace Client
{
    public partial class FormConnect : Form
    {

        public static FormConnect formconnect = new FormConnect();
        //
        // FormConnect
        //
        public FormConnect()
        {
            InitializeComponent();
        }
        // buttons
        // button_connect
        //
        private void button_connect_Click(object sender, EventArgs ea)
        {
            string host = textBox_host.Text;
            int port = int.Parse(textBox_port.Text);

            try
            {
                Server.Connect(host, port);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}",//对话框的显示内容
                  "连接失败", //对话框的标题 
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Information);
                return;
            }

            Process.Start();
            this.Hide();
            FormLogin.formlogin = new FormLogin();
            FormLogin.formlogin.ShowDialog();
        }
        //
        // button_exit_Click
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
        //
        // button_mini_Click
        //
        private void button_mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
