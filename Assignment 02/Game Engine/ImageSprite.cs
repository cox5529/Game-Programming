using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	class ImageSprite:Sprite {

		private Image img;
		private int p;

		public Image Image {
			get { return img; }
		}

		public ImageSprite(Image img, int p) {
			this.p = p;
			this.img = img;
		}

		public override void Act() {
			base.Act();
			X = (int)(Math.Tan(p / 100.0) * 100);
			Y = (int)(Math.Cos(p / 100.0) * 100) + 100;
			p++;
			Rotation++;
			Rotation %= 360;
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			g.DrawImage(img, 0, 0);
		}
	}
}
