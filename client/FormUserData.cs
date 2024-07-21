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
    public partial class FormUserData : Form
    {
        internal static FormUserData formuserdata { get; set; }

        internal int RowIndex;       // 用于与FormChatRoom核对数据，以确定是否需要更新
        public FormUserData(int RowIndex, string name, int id, string ip)
        {
            InitializeComponent();
            this.Location = Control.MousePosition;
            label_name.Text = name;
            label_id.Text = $"{id}";
            label_ip.Text = ip;
        }
    }
}
