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
    public partial class FormTest : Form
    {
        public static FormTest formtest;

        public FormTest()
        {
            InitializeComponent();
            formtest = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Server.Send(richTextBox_send.Text);
        }

        public void AddMessage(string message)
        {
            richTextBox_recv.AppendText(message);
            richTextBox_recv.AppendText("\n");
        }
    }
}
