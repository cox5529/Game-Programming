using System.Diagnostics;
using System.Drawing;

namespace Engine.Sprites {
	public class ImageSprite:Sprite {

		private Image img;
		private bool flip;

		private float height;
		private float width;

		public float CenterX {
			get { return X + Width / 2; }
		}

		public float CenterY {
			get { return Y + Height / 2; }
		}

		public float Width {
			get { return width * Scale; }
		}

		public float Height {
			get { return height * Scale; }
		}

		public bool Flip {
			set { flip = value; }
		}

		public Image Image {
			get { return img; }
			set {
				img = value;
				width = img.Width;
				height = img.Height;
			}
		}

		public ImageSprite(Image img) : this(img, 0, 0) { }

		public ImageSprite(Image img, float x, float y) {
			X = x;
			Y = y;
			this.img = img;
			width = img.Width;
			height = img.Height;
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			g.DrawImage(img, 0, 0);
			if(flip) {
				Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
				flip = false;
			}
		}
	}
}
