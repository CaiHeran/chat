using Info;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Client
{

    using DB = Database;

    internal partial class FormChatRoom : Form
    {
        internal static FormChatRoom formchatroom { get; set; }
        //
        // Forms
        // FormChatRoom
        public FormChatRoom()
        {
            InitializeComponent();
            formchatroom = this;
        }
        public void FormChatRoom_Load(object sender, EventArgs e)
        {
            Process.Otherjoinroom += (_, msg) => { Grid_AddData(msg); };
            Process.Roommessage += (_, msg) => {
                int sender_id = msg.sender;
                string message = msg.message;
                FormChatRoom.formchatroom.Add_text($"{sender_id} : {message}");
            };
            Grid_Load();
            label_roomid.Text = $"房间号：{DB.Room.Id}";
        }
        //
        // dataGrid_View
        // dataGrid_View_list
        internal void Grid_Init()
        {
            const int Listnum = 3;
            dataGridView_list.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;              // 自动调整行高
            dataGridView_list.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;// 居中
            dataGridView_list.RowHeadersWidth = 4;                                                  // 设置表格头列（无内容）宽度，最小为4
            dataGridView_list.AllowUserToAddRows = false;                                           // 不可以增加空行
            for (int i = 0; i < Listnum; i++)
            {
                dataGridView_list.Columns.Add(new DataGridViewTextBoxColumn());//添加表头
                dataGridView_list.Columns[0].Width = 180;                      // 列宽
            }
            dataGridView_list.Columns[0].HeaderText = "序号";
            dataGridView_list.Columns[1].HeaderText = "昵称";
        }
        internal void Grid_AddData(OtherJoinRoom msg)
        {
            // todo:首先判断数据是否异常
            int cnt = dataGridView_list.Rows.Count;                         //得到总行数 
            dataGridView_list.Rows.Insert(0, 1);                            //向第一行插入一行数据
            dataGridView_list.Rows[0].Cells[0].Value = $"{msg.num}";        //
            dataGridView_list.Rows[0].Cells[1].Value = $"{msg.info.name}";  //
        }
        internal void Grid_AddData(int num, User userinfo)
        {
            // todo:首先判断数据是否异常
            int cnt = dataGridView_list.Rows.Count;//得到总行数 
            dataGridView_list.Rows.Insert(0, 1);//向第一行插入一行数据
            dataGridView_list.Rows[0].Cells[0].Value = $"{num}";           //
            dataGridView_list.Rows[0].Cells[1].Value = $"{userinfo.Name}"; //
            dataGridView_list.ClearSelection();//去除选择
        }
        internal void Grid_Load()
        {
            Grid_Init();
            foreach ((int num, User userinfo) in DB.Room.Parts)
                Grid_AddData(num, userinfo);
        }
        //
        // richTextBoxs
        // richTextBox_view
        internal void Add_text(string text)
        {
            richTextBox_view.AppendText(text + "\n");
            return;
        }
        //
        // buttons
        // button_send
        private void button_send_Click(object sender, EventArgs e)
        {
            string msg = richTextBox_input.Text;
            richTextBox_input.Text = "";
            Functions.SendMessage(msg);
        }

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
