using System.ComponentModel;

namespace Client
{
    public partial class FormUserData : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal static FormUserData? form { get; set; }

        internal int RowIndex;       // 用于与FormChatRoom核对数据，以确定是否需要更新
        public FormUserData(int row, Point location, string name, int id)
        {
            InitializeComponent();
            form = this;
            location.X -= this.Width;
            this.Location = location;
            RowIndex = row;
            label_name.Text = name;
            label_id.Text = $"{id}";
        }
    }
}
