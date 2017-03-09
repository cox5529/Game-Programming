using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	class ImageSprite:Sprite {

		public static SoundPlayer soundPlayer;

		private Boolean sound;
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
			if(X < 300)
				sound = false;
			else if(X > 300 && !sound) {
				sound = true;
				if(soundPlayer == null) {
					soundPlayer = new SoundPlayer(@"c:\vroom.wav");
				}
				soundPlayer.Play();
			}
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			g.DrawImage(img, 0, 0);
		}
	}
}
