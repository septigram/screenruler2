using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Screenruler2
{
    public partial class Form1 : Form
    {
        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void SetCursorPos(int X, int Y);

        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        List<Point> points = new List<Point>();

        public Form1()
        {
            InitializeComponent();
        }

        internal void UpdateTitle()
        {
            var p = PointToClient(Cursor.Position);
            this.Text = $"{p.X},{p.Y} ({Size.Width},{Size.Height})" +
                $"@{Location.X},{Location.Y}";
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            UpdateTitle();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Point pt1 = new Point();
            Point pt2 = new Point();
            pt1.Y = 0;
            pt2.Y = Size.Height;
            for (int x = 0; x < Size.Width; x += 50)
            {
                pt1.X = pt2.X = x;
                g.DrawLine(x % 100 == 0 ? Pens.Black : Pens.Gray, pt1, pt2);
                g.DrawString($"{x}", DefaultFont, Brushes.Black, pt1);
            }
            pt1.X = 0;
            pt2.X = Size.Width;
            for (int y = 0; y < Size.Height; y += 50)
            {
                pt1.Y = pt2.Y = y;
                g.DrawLine(y % 100 == 0 ? Pens.Black : Pens.Gray, pt1, pt2);
                g.DrawString($"{y}", DefaultFont, Brushes.Black, pt1);
            }
            for (int i = 0; i < points.Count; i++)
            {
                var p = points[i];
                var str = $"{p.X},{p.Y}";
                var tsize = g.MeasureString(str, DefaultFont, 100);
                g.FillRectangle(Brushes.White, p.X, p.Y, tsize.Width, tsize.Height);
                g.DrawString(str, DefaultFont, Brushes.Black, p);
                g.DrawLine(Pens.Black, p.X - 4, p.Y - 4, p.X + 4, p.Y + 4);
                g.DrawLine(Pens.Black, p.X - 4, p.Y + 4, p.X + 4, p.Y - 4);
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var p = PointToClient(Cursor.Position);
                points.Add(p);
                Invalidate();
            }
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show();
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateTitle();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void Form1_Move(object sender, EventArgs e)
        {
            UpdateTitle();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateTitle();
        }

        private void topMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopMost = !TopMost;
            topMostToolStripMenuItem.Checked = TopMost;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            points.Clear();
            Invalidate();
        }

        /* 触れなくする
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020;
                return cp;
            }
        }
        */

        /* コントロールキーで一次的に透明にできないか検討。上手く動かない。

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control) {
                Opacity = 0;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control)
            {
                Opacity = 0.5;
            }
        }
        */
    }
}
