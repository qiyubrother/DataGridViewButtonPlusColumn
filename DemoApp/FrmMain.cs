using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoApp
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        DataTable dt = new DataTable();
        BindingSource bs = new BindingSource();
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            dgv.AllowUserToAddRows = false;
            dgv.RowTemplate.Height = 30;
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Age", typeof(string));
            dt.Rows.Add("Liuzhenhua", "30");
            bs.DataSource = dt;
            dgv.DataSource = bs;

            var dc = new DataGridViewButtonPlusColumn(); 
            dgv.Columns.Add(dc);
            var dc2 = new DataGridViewButtonPlusColumn();
            dgv.Columns.Add(dc2);
            var dc3 = new DataGridViewButtonPlusColumn();
            dgv.Columns.Add(dc3);

            dgv.CellPainting += Dgv_CellPainting;
            dgv.CellContentClick += Dgv_CellContentClick;
        }

        private void Dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var dv = sender as DataGridView;
            if (e.ColumnIndex == -1 || e.RowIndex == -1 || dv == null) return;

            if (dv.Columns[e.ColumnIndex] is DataGridViewButtonPlusColumn)
            {
                var cell = dv.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonPlusCell;
                if (e.ColumnIndex == 2)
                {
                    cell.Text = "按钮A";                                      // 设置按钮文字内容
                    cell.TextFont = new Font("微软雅黑", 12, FontStyle.Bold); // 设置按钮文字字体
                    cell.FontColor = Color.Yellow;                            // 设置按钮文字颜色
                    cell.HoverFontColor = Color.White;                        // 设置按钮文字进入时颜色
                    cell.HoverBackColor = Color.Red;                          // 设置按钮进入时背景颜色
                    cell.Radius = 20;                                         // 设置按钮圆角(1-25)
                    cell.BackgroundColor = Color.DimGray;                     // 设置画布背景颜色
                    cell.DefaultBackColor = Color.Gold;                       // 设置按钮背景颜色
                }
                else if (e.ColumnIndex == 3)
                {
                    cell.TextFont = new Font("华文行楷", 12);
                    cell.Text = "按钮B";
                    cell.Radius = 1;
                }
                else if (e.ColumnIndex == 4)
                {
                    cell.FontColor = Color.Blue;
                    cell.Text = "按钮C";
                }
            }
        }

        private void Dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dv = sender as DataGridView;
            if (e.ColumnIndex == -1 || e.RowIndex == -1 || dv == null) return;

            if (dv.Columns[e.ColumnIndex] is DataGridViewButtonPlusColumn)
            {
                var cell = dv.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonPlusCell;
                MessageBox.Show("已经点击过了");
            }
        }
    }
}
