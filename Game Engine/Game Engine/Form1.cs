using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine {
	public partial class Form1:Form {

		private Sprite main;
		private Box b1;

		public Form1() {
			InitializeComponent();
			DoubleBuffered = true;
			main = new Sprite();
			b1 = new Box(100, 100);
			b1.X = 100;
			b1.Y = 100;
			main.Add(b1);
			Box b2 = new Box(100, 100);
			b2.Scale = 0.25f;
			b2.X = 5;
			b2.Y = 5;
			b1.Add(b2);
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			b1.X = e.X;
			b1.Y = e.Y;
			Refresh();
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			Refresh();
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			main.Render(e.Graphics);
		}
	}
}
