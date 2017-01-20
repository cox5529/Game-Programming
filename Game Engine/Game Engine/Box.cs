using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {

	class Box : Sprite{
		private float xDim;
		private float yDim;

		public float XDim {
			get { return xDim; }
			set { xDim = value; }
		}

		public float YDim {
			get { return yDim; }
			set { yDim = value; }
		}

		public Box(float xDim, float yDim) {
			this.xDim = xDim;
			this.yDim = yDim;
		}

		public override void Paint(Graphics g) {
			foreach(Sprite s in children) {
				s.X = X;
				s.Y = Y;
			}
			g.DrawRectangle(Pens.Black, X, Y, xDim, yDim);
		}
	}
}
