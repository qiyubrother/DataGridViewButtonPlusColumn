using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace DemoApp
{
    public class DataGridViewButtonPlusColumn : DataGridViewColumn
    {
        public DataGridViewButtonPlusColumn()
        {
            CellTemplate = new DataGridViewButtonPlusCell();
        }
    }

    public class DataGridViewButtonPlusCell : DataGridViewButtonCell
    {
        public DataGridViewButtonPlusCell()
        {

        }

        private bool mouseHover = false;    // 鼠标是否进入
        private static int nowColIndex = 0; // 当前列序号
        private static int nowRowIndex = 0; // 当前行序号

        // 默认按钮颜色
        public Color DefaultBackColor { get; set; } = Color.FromArgb(255, 79, 196, 123);
        // 进入时按钮颜色
        public Color HoverBackColor { get; set; } = Color.FromArgb(255, 90, 247, 149);
        // 圆角
        public int Radius { get; set; } = 12;
        // 文字字体
        public Font TextFont { get; set; } = new Font("宋体", 9);
        // 按钮文字
        public string Text { get; set; } = "按钮";
        // 按钮文字颜色
        public Color FontColor { get; set; } = Color.Black;
        // 进入时按钮文字颜色
        public Color HoverFontColor { get; set; } = Color.Black;
        // 画布背景颜色
        public Color BackgroundColor { get; set; } = Color.White;

        /// <summary>
        /// 对单元格的重绘事件进行的方法重写。
        /// </summary>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState,
            object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            cellBounds = PrivatePaint(graphics, cellBounds, rowIndex, cellStyle, true);
            base.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
            nowColIndex = this.DataGridView.Columns.Count - 1;
        }

        /// <summary>
        /// 私有的单元格重绘方法，根据鼠标是否移动到按钮上，对按钮的不同背景和边框进行绘制。
        /// </summary>
        private Rectangle PrivatePaint(Graphics graphics, Rectangle cellBounds, int rowIndex, DataGridViewCellStyle cellStyle, bool clearBackground)
        {
            //抗锯齿
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (clearBackground) // 是否需要重绘单元格的背景颜色
            {
                //cellStyle.SelectionBackColor
                Brush brushCellBack = new SolidBrush(BackgroundColor);
                graphics.FillRectangle(brushCellBack, cellBounds.X, cellBounds.Y, cellBounds.Width, cellBounds.Height);
            }

            Rectangle recDetail = new Rectangle(cellBounds.Location.X, cellBounds.Location.Y, cellBounds.Width, cellBounds.Height);
            var btnBackgroundColor = DefaultBackColor; // 默认颜色
            var btnFontColor = FontColor;
            if (mouseHover)
            {
                btnBackgroundColor = HoverBackColor; // 鼠标进入时颜色
                btnFontColor = HoverFontColor;
            }

            var _rect = new Rectangle { X = recDetail.X + 2, Y = recDetail.Y + 2, Width = recDetail.X + recDetail.Width - 4, Height = recDetail.Y + recDetail.Height - 4 };
            Draw(_rect, graphics, false, btnBackgroundColor);
            DrawText(Text, TextFont, recDetail, graphics, btnFontColor);

            return cellBounds;
        }
        private void DrawText(string text, Font f, Rectangle rectangle, Graphics g, Color color)
        {
            SolidBrush sbr = new SolidBrush(color);
            var rect = new RectangleF();
            rect = getTextRec(text, f, rectangle, g);
            g.DrawString(text, f, sbr, rect);
        }
        private RectangleF getTextRec(string text, Font f, Rectangle rectangle, Graphics g)
        {
            var rect = new RectangleF();
            var size = g.MeasureString(text, f);
            if (size.Width > rectangle.Width || size.Height > rectangle.Height)
            {
                rect = rectangle;
            }
            else
            {
                rect.Size = size;
                rect.Location = new PointF(rectangle.X + (rectangle.Width - size.Width) / 2, rectangle.Y + (rectangle.Height - size.Height) / 2);
            }
            return rect;
        }
        private void Draw(Rectangle rectangle, Graphics g, bool cusp, Color begin_color, Color? end_colorex = null)
        {
            Color end_color = end_colorex == null ? begin_color : (Color)end_colorex;
            int span = 2;
            //抗锯齿
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //渐变填充
            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush(rectangle, begin_color, end_color, LinearGradientMode.Vertical);
            //画尖角
            if (cusp)
            {
                span = 10;
                PointF p1 = new PointF(rectangle.Width - 12, rectangle.Y + 10);
                PointF p2 = new PointF(rectangle.Width - 12, rectangle.Y + 30);
                PointF p3 = new PointF(rectangle.Width, rectangle.Y + 20);
                PointF[] ptsArray = { p1, p2, p3 };
                g.FillPolygon(myLinearGradientBrush, ptsArray);
            }
            //填充
            g.FillPath(myLinearGradientBrush, DrawRoundRect(rectangle.X, rectangle.Y, rectangle.Width - span, rectangle.Height - 2, Radius));

        }
        private GraphicsPath DrawRoundRect(int x, int y, int width, int height, int radius)
        {
            //四边圆角
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(x, y, radius, radius, 180, 90);
            gp.AddArc(width - radius, y, radius, radius, 270, 90);
            gp.AddArc(width - radius, height - radius, radius, radius, 0, 90);
            gp.AddArc(x, height - radius, radius, radius, 90, 90);
            gp.CloseAllFigures();
            return gp;
        }

        /// <summary>
        /// 鼠标移动到单元格内时的事件处理，通过坐标判断鼠标是否移动到了修改或删除按钮上，并调用私有的重绘方法进行重绘。
        /// </summary>
        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            if (base.DataGridView == null) return;

            nowColIndex = e.ColumnIndex;
            nowRowIndex = e.RowIndex;

            Rectangle cellBounds = DataGridView[e.ColumnIndex, e.RowIndex].ContentBounds;
            Rectangle recDetail = new Rectangle(cellBounds.Location.X, cellBounds.Location.Y, cellBounds.Width, cellBounds.Height);
            Rectangle paintCellBounds = DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

            paintCellBounds.Width = DataGridView.Columns[nowColIndex].Width;
            paintCellBounds.Height = DataGridView.Rows[nowRowIndex].Height;

            if (IsInRect(e.X, e.Y, recDetail))
            {
                if (!mouseHover)
                {
                    mouseHover = true;
                    PrivatePaint(this.DataGridView.CreateGraphics(), paintCellBounds, e.RowIndex, this.DataGridView.RowTemplate.DefaultCellStyle, false);
                    DataGridView.Cursor = Cursors.Hand;
                }
            }
            else
            {
                if (mouseHover)
                {
                    mouseHover = false;
                    PrivatePaint(this.DataGridView.CreateGraphics(), paintCellBounds, e.RowIndex, this.DataGridView.RowTemplate.DefaultCellStyle, false);
                    DataGridView.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 鼠标从单元格内移出时的事件处理，调用私有的重绘方法进行重绘。
        /// </summary>
        protected override void OnMouseLeave(int rowIndex)
        {
            if (mouseHover != false)
            {
                mouseHover = false;

                Rectangle paintCellBounds = DataGridView.GetCellDisplayRectangle(nowColIndex, nowRowIndex, true);

                paintCellBounds.Width = DataGridView.Columns[nowColIndex].Width;
                paintCellBounds.Height = DataGridView.Rows[nowRowIndex].Height;

                PrivatePaint(this.DataGridView.CreateGraphics(), paintCellBounds, nowRowIndex, this.DataGridView.RowTemplate.DefaultCellStyle, false);
                DataGridView.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 判断鼠标坐标是否在指定的区域内。
        /// </summary>
        private static bool IsInRect(int x, int y, Rectangle area)
        {
            if (x > area.Left && x < area.Right && y > area.Top && y < area.Bottom)
                return true;
            return false;
        }
    }
}
