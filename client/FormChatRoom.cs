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

using Info;

namespace Client
{
    using DB = Database;

    internal partial class FormChatRoom : Form
    {
        internal static FormChatRoom? form { get; set; }
        //
        // Forms
        // FormChatRoom
        public FormChatRoom()
        {
            InitializeComponent();
            form = this;
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
                DB.Room!.Leave(msg.id);
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
            dataGridView_list.Rows.Insert(cnt);                          // 准备向下一行插入一行数据
            dataGridView_list.Rows[cnt].Cells[0].Value = id;
            dataGridView_list.Rows[cnt].Cells[1].Value = $"{userinfo.Name}";
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
            var result = MessageBox.Show("真的要退出房间吗？", "标题", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;
            Functions.LeaveRoom(DB.Room!.Id);
            DB.Room = null;
            form = null;
            this.Close();
            this.Dispose();
            FormHome.form!.Show();
        }

        // 鼠标悬停显示信息
        private void dataGridView_list_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            int row = e.RowIndex;
            if (row < 0)
            {
                FormUserData.form?.Close();
                return;
            }
            else
            {
                if (FormUserData.form is not null)
                {
                    if (FormUserData.form.RowIndex == row)
                        return;
                    else
                        FormUserData.form.Close();
                }
                int id = (int)dataGridView_list[0, row].Value;        // 获取id
                string name = DB.Room.Parts[id].Name;

                var r = dataGridView_list.GetCellDisplayRectangle(0, row, false);
                Point p = this.Location + (Size)dataGridView_list.Location;
                p.X += r.X;
                p.Y += r.Y;
                new FormUserData(row, p, name, id).Show();
            }
        }
        // 鼠标离开关闭信息
        private void dataGridView_list_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            FormUserData.form?.Close();
        }
    }
}
