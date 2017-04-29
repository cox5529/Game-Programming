using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprites {
	public class RectangleSprite:Sprite {

		private float width;
		private float height;
		private Brush fill;

		public float Width {
			get {
				return width;
			}

			set {
				width = value;
			}
		}

		public float Height {
			get {
				return height;
			}

			set {
				height = value;
			}
		}

		public Brush Fill {
			get {
				return fill;
			}

			set {
				fill = value;
			}
		}

		public RectangleSprite(float x, float y, float width, float height) {
			X = x;
			Y = y;
			this.width = width;
			this.height = height;
			fill = Brushes.Black;
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			g.FillRectangle(fill, 0, 0, width, height);
		}
	}
}
