using System.Diagnostics;
using System.Drawing;

namespace Engine {
	public class ImageSprite:Sprite {

		private Image img;

		public Image Image {
			get { return img; }
			set { img = value; }
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
