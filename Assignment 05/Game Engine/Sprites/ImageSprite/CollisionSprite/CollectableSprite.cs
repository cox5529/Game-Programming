using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprites {
	public class CollectableSprite:PhysicsSprite{

		private string id;
		private float upperY;
		private float lowerY;

		public string ID {
			get { return id; }
			set { id = value; }
		}

		public float UpperY {
			set { upperY = value; }
		}

		public float LowerY {
			set { lowerY = value; }
		}

		public CollectableSprite(Image img, float x, float y, uint mask, float[] xVals, float[] yVals) : base(img, x, y, mask, xVals, yVals) {
			Initialize();
		}

		public CollectableSprite(Image img, float x, float y, uint mask, Polygon Box) : base(img, x, y, mask, Box) {
			Initialize();
		}

		private void Initialize() {
			Velocity.Y = 3;
			MotionModel = CONSTANT_VELOCITY;
			upperY = Y;
			lowerY = Y;
		}

		public override void Act() {
			base.Act();
			if(!Polygon.IsBetween(Y, lowerY, upperY)) {
				Velocity.Y *= -1;
			}
		}
	}
}
