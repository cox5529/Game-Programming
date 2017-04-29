using Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprites {
	public class SlideSprite:ImageSprite {

		private float targetX;
		private float targetY;
		private float velocity;
		
		public float TargetX {
			get { return targetX; }
			set {
				targetX = value;
			}
		}

		public float TargetY {
			get { return targetY; }
			set {
				targetY = value;
			}
		}

		public float Velocity {
			get { return velocity; }
			set {
				velocity = value;
			}
		}

		public SlideSprite(Image img) : base(img) {

		}

		public SlideSprite(Image img, float x, float y, bool slideFromOrigin) : base(img) {
			if(slideFromOrigin) {
				X = 0;
				Y = 0;
			} else {
				X = x;
				Y = y;
			}
			targetX = x;
			targetY = y;
			velocity = 50;
		}

		public override void Act() {
			base.Act();
			if(Math.Abs(targetX - X) > 0) {
				if(Math.Abs(targetX - X) > velocity) {
					X += Math.Sign(targetX - X) * velocity;
				} else {
					X += Math.Sign(targetX - X) * Math.Abs(targetX - X);
				}
			}

			if(Math.Abs(targetY - Y) > 0) {
				if(Math.Abs(targetY - Y) > velocity) {
					Y += Math.Sign(targetY - Y) * velocity;
				} else {
					Y += Math.Sign(targetY - Y) * Math.Abs(targetY - Y);
				}
			}

		}
	}
}
