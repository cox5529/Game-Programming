using Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	class SlideSprite:ImageSprite {

		const float stepSize = 1000.0F;
		private float targetX;
		private float targetY;
		private float velocity;
		private int counter;
		private Image img1;

		public float TargetX {
			get { return targetX; }
			set {
				targetX = value;
				counter = 0;
			}
		}

		public float TargetY {
			get { return targetY; }
			set {
				targetY = value;
				counter = 0;
			}
		}

		public float Velocity {
			get { return velocity; }
			set {
				velocity = value;
			}
		}

		public SlideSprite(Image img) : base(img) {
			counter = 0;
		}

		public SlideSprite(Image img, Image img1) : this(img) {
			this.img1 = img1;
		}

		public override void Act() {
			base.Act();
			counter++;
			if(X != targetX) {
				X += (float)((targetX - X) / stepSize * velocity);
			}
			if(Y != targetY) {
				Y += (float)((targetY - Y) / stepSize * velocity);
			}
		}
	}
}
