using Info;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
        bool _is_showing = false;
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
            Process.Otherjoinroom += (_, msg) =>
            {
                // TODO check msg.room
                Grid_AddData(msg.info);
            };
            Process.Roommessage += (_, msg) =>
            {
                Add_text($"{DB.Room.Parts[msg.id].Name}: {msg.message}");
            };
            Process.Leaveroom += (_, msg) =>
            {
                Grid_DelData(msg.id);
            };
            Grid_Load();
            label_roomid.Text = $"房间号：{DB.Room.Id}";
        }
        //
        // dataGrid_View
        // dataGrid_View_list
        private void Grid_Init()
        {
            const int Listnum = 2;
            dataGridView_list.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;        // 自动调整行高
            dataGridView_list.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView_list.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;// 居中
            //dataGridView_list.RowHeadersWidth = 4;                                           // 设置表格头列（无内容）宽度，最小为4
            dataGridView_list.AllowUserToAddRows = false;                                      // 不可以增加空行
            for (int i = 0; i < Listnum; i++)
                dataGridView_list.Columns.Add(new DataGridViewTextBoxColumn());    // 添加表头
            dataGridView_list.RowHeadersVisible = false;
            dataGridView_list.Columns[0].Width = 50;
            dataGridView_list.Columns[0].HeaderText = "ID";
            dataGridView_list.Columns[1].HeaderText = "昵称";
            dataGridView_list.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        private void Grid_Load()
        {
            Grid_Init();
            foreach ((int num, User userinfo) in DB.Room.Parts)
                Grid_AddData(num, userinfo);
        }
        internal void Grid_AddData(UserBriefInfo info)
        {
            Grid_AddData(info.id, new User(info));
        }
        internal void Grid_AddData(int id, User userinfo)
        {
            // todo:首先判断数据是否异常
            int cnt = dataGridView_list.Rows.Count;                         // 得到总行数 
            dataGridView_list.Rows.Insert(cnt, 1);                          // 准备向下一行插入一行数据
            dataGridView_list.Rows[cnt].Cells[0].Value = id;                //
            dataGridView_list.Rows[cnt].Cells[1].Value = $"{userinfo.Name}";//
            dataGridView_list.ClearSelection();                             // 去除选择
        }
        internal void Grid_DelData(int id)
        {
            for (int i = 0; i < dataGridView_list.Rows.Count; i++)
                if ((int)dataGridView_list.Rows[i].Cells[0].Value == id)
                {
                    dataGridView_list.Rows.RemoveAt(i);
                    break;
                }
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
            if(richTextBox_input.Text.Length == 0)
            {
                errorProvider_send.SetError(button_send, "消息不可为空");
                return;
            }
            errorProvider_send.Clear();
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

        // 鼠标悬停显示信息
        private void dataGridView_list_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            int eColumnIndex = e.ColumnIndex, eRowIndex = e.RowIndex;
            if (eColumnIndex == 1 && eRowIndex >= 0)
            {
                int target = (int)dataGridView_list[0, eRowIndex].Value;        // 获取id
                int id = target;
                string name = DB.Room.Parts[target].Name;
                string IP = "IP";
                if (_is_showing) return;
                FormUserData.formuserdata = new FormUserData(eRowIndex, name, id, IP);
                _is_showing = true;
                FormUserData.formuserdata.Show();
            }
        }
        // 鼠标离开关闭信息
        private void dataGridView_list_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            _is_showing = false;
            FormUserData.formuserdata?.Close();
        }
    }
}
