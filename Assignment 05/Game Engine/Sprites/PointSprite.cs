using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprites {
	public class PointSprite:Sprite {

		public PointSprite(float x, float y) {
			X = x;
			Y = y;
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			g.FillEllipse(Brushes.Red, (float)(X - 0.5), (float)(Y - 0.5), 1, 1);
		}
	}
}
