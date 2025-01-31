using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    using DB = Database;

    internal partial class FormChatRoom : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal static FormChatRoom? form { get; set; }

        private readonly Room room;
        //
        // Forms
        // FormChatRoom
        public FormChatRoom(Room room)
        {
            InitializeComponent();
            form = this;
            this.room = room;
        }
        public void FormChatRoom_Load(object sender, EventArgs e)
        {
            // Otherjoinroom
            Process.Register(21, (json) =>
            {
                // TODO check json["roomid"]
                JsonNode userinfo = json["info"]!;
                int userid = userinfo["id"]!.GetValue<int>();
                if (userid == DB.Me!.Id)
                    return;
                room.Join(new User(userinfo));
                Grid_AddData(new User(userid, userinfo["name"]!.GetValue<string>()));
            });
            // Roommessage
            Process.Register(22, (json) =>
            {
                // TODO check json["roomid"]
                int userid = json["userid"]!.GetValue<int>();
                string message = json["message"]!.GetValue<string>();
                Add_text($"{room.Parts[userid].Name}: {message}");
            });
            // Leaveroom
            Process.Register(29, (json) =>
            {
                int id = json["userid"]!.GetValue<int>();
                Grid_DelData(id);
                room.Leave(id);
            });
            Grid_Load();
            label_roomid.Text = $"房间号：{room.Id}";
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
            foreach ((int id, User userinfo) in room.Parts)
                Grid_AddData(userinfo);
        }
        internal void Grid_AddData(User userinfo)
        {
            // todo:首先判断数据是否异常
            int cnt = dataGridView_list.Rows.Count;                         // 得到总行数
            dataGridView_list.Rows.Insert(cnt);                          // 准备向下一行插入一行数据
            dataGridView_list.Rows[cnt].Cells[0].Value = userinfo.Id;
            dataGridView_list.Rows[cnt].Cells[1].Value = userinfo.Name;
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
            richTextBox_view.AppendText(text + '\n');
            return;
        }
        //
        // buttons
        // button_send
        private async void button_send_Click(object sender, EventArgs e)
        {
            if(richTextBox_input.Text.Length == 0)
            {
                errorProvider_send.SetError(button_send, "消息不可为空");
                return;
            }
            errorProvider_send.Clear();
            string msg = richTextBox_input.Text;
            richTextBox_input.Text = "";
            JsonNode res = await Requests.SendRoomMessageAsync(msg);
            if (res["errors"] == null)
                return;
            MessageBox.Show(res["errors"].GetValue<string>());
        }
        private async void button_exit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("真的要退出房间吗？", "标题", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;
            await Requests.LeaveRoomAsync(room.Id);
            Process.Clear(21);
            Process.Clear(22);
            Process.Clear(23);
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
                string name = room.Parts[id].Name;

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
