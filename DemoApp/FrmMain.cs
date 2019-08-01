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
            dt.Rows.Add("Zhaoshasha", "40");
            bs.DataSource = dt;
            dgv.DataSource = bs;
#if true
            // 针对不同列进行个性化设置
            var dc = new DataGridViewButtonPlusColumn();
            dgv.Columns.Add(dc);
            var dc2 = new DataGridViewButtonPlusColumn(new DataGridViewButtonPlusColumnParam
            {
                Text = "按钮2",                                // 设置按钮文字内容
                TextFont = new Font("微软雅黑", 12, FontStyle.Bold), // 设置按钮文字字体
                FontColor = Color.Yellow,                            // 设置按钮文字颜色
                HoverFontColor = Color.White,                        // 设置按钮文字进入时颜色
                HoverBackColor = Color.Red,                          // 设置按钮进入时背景颜色
                Radius = 20,                                         // 设置按钮圆角(1-25)
                BackgroundColor = Color.DimGray,                     // 设置画布背景颜色
                DefaultBackColor = Color.Gold                        // 设置按钮背景颜色
            });
            dgv.Columns.Add(dc2);
            var dc3 = new DataGridViewButtonPlusColumn(new DataGridViewButtonPlusColumnParam
            {
                Text = "按钮3",                                      // 设置按钮文字内容
                FontColor = Color.Red,                               // 设置按钮文字颜色
                Radius = 15,                                         // 设置按钮圆角(1-25)
            });
            dgv.Columns.Add(dc3);

            dgv.CellContentClick += Dgv_CellContentClick;

            new[] { dc, dc2, dc3 }.All(x => { x.Redraw(); return true; });
#else

            // 针对不同单元格进行个性化设置
            var dc = new DataGridViewButtonPlusColumn();
            var dc2 = new DataGridViewButtonPlusColumn();
            var dc3 = new DataGridViewButtonPlusColumn();
            dgv.Columns.AddRange(new[] { dc, dc2, dc3 });

            foreach (DataGridViewRow dgvr in dgv.Rows)
            {
                foreach (DataGridViewColumn dgvc in dgv.Columns)
                {
                    if (dgvc is DataGridViewButtonPlusColumn)
                    {
                        var cell = dgvr.Cells[dgvc.Index] as DataGridViewButtonPlusCell;
                        cell.Init(new DataGridViewButtonPlusCellParam
                        {
                            Text = $"{dgvr.Index},{dgvc.Index}",
                        });
                        //cell.ReDraw();
                    }
                }
            }
            dgv.CellContentClick += Dgv_CellContentClick;
#endif

        }

        private void Dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dv = sender as DataGridView;
            if (e.ColumnIndex == -1 || e.RowIndex == -1 || dv == null) return;

            if (dv.Columns[e.ColumnIndex] is DataGridViewButtonPlusColumn)
            {
                var cell = dv.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonPlusCell;
                cell.Text = "Yahoo！";
                MessageBox.Show($"Cell({e.RowIndex},{e.ColumnIndex}) Clicked.");
            }
        }
    }
}
