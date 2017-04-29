using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Engine.Sprites {
	public class VectorSprite:Sprite, IDisposable {

		private Vector2D vector;
		private Pen pen;

		public Vector2D Vector {
			get { return vector; }
			set { vector = value; }
		}

		public Pen Pen {
			get { return pen; }
		}

		public VectorSprite(Vector2D vector, float x, float y) {
			this.vector = vector;
			pen = new Pen(Brushes.Red);
			pen.EndCap = LineCap.ArrowAnchor;
			pen.StartCap = LineCap.RoundAnchor;
			X = x;
			Y = y;
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			g.DrawLine(pen, 0, 0, vector.X, vector.Y);
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool managed) {
			pen.Dispose();
		}
	}
}
