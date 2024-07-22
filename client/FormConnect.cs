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
                if (!Server.Connect(host, port))
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}",//对话框的显示内容
                  "连接失败", //对话框的标题 
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Information);
                return;
            }

            Close();
        }
    }
}
