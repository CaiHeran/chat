using client;
using System.Net.Security;
using System.Security.Authentication;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        //
        // Form1
        //
        public Form1()
        {
            InitializeComponent();
            //
            //
            // Form1
            this.ControlBox = false;
            //
            //
            // pictureBox_background
            pictureBox_background.Location = new System.Drawing.Point(0, 0);
            pictureBox_background.Size = this.Size;
            pictureBox_background.SendToBack();
            Image imagefile = Image.FromFile("images/background2.jpg");
            pictureBox_background.Image = imagefile;
            pictureBox_background.SizeMode = PictureBoxSizeMode.AutoSize;//6����ĺ�����
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
                MessageBox.Show($"{ex.Message}",//�Ի������ʾ����
                  "����ʧ��", //�Ի���ı��� 
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Information);
                return;
            }

            FormTest.formtest = new FormTest();
            this.Hide();
            FormTest.formtest.Show();
        }
        //
        // button_exit_Click
        // 
        private void button_exit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("���Ҫ�˳���", "��Ϸ����", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            pictureBox_background.Size = this.Size;
        }
    }
}
