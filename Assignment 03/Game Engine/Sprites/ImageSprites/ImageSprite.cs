using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	class ImageSprite:Sprite {

		private Image img;

		public Image Image {
			get { return img; }
		}

		public ImageSprite(Image img) {
			this.img = img;
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			g.DrawImage(img, 0, 0);
		}
	}
}
