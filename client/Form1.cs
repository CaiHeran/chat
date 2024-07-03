using client;
using System.Net.Security;
using System.Security.Authentication;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

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
    }
}
