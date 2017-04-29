using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprites {
	public class SoundSlideSprite:SlideSprite {

		private Stream str;

		public float TargetLeviX {
			get { return TargetX; }
			set {
				TargetX = value;
				PlaySound();
			}
		}

		public float TargetLeviY {
			get { return TargetY; }
			set {
				TargetY = value;
				PlaySound();
			}
		}

		public SoundSlideSprite(Stream str, Image img, float x, float y, bool slideFromOrigin) : base(img, x, y, slideFromOrigin) {
			this.str = str;
		}

		public void PlaySound() {
			new SoundPlayer(str).Play();
		}
	}
}
