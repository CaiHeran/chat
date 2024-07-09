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
    using DB = Database;

    public partial class FormHome : Form
    {
        public static FormHome? formhome { get; set; }

        public FormHome()
        {
            InitializeComponent();
            formhome = this;

            label_ID.Text = $" ID: {DB.Me.Id}";
            label_name.Text = $"Name: {DB.Me.Name}";
        }

        private void button_CreateRoom_Click(object sender, EventArgs e)
        {

        }

        private void button_JoinRoom_Click(object sender, EventArgs e)
        {

        }

        private void button_Exist_Click(object sender, EventArgs e)
        {

        }
    }
}
