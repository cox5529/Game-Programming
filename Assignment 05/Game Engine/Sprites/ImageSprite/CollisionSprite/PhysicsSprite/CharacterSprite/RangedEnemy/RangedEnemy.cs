using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1.Properties;

namespace Engine.Sprites {
	public class RangedEnemy:CharacterSprite {

		private PointF start;
		private PointF stop;
		private double shootChance = 0.98;
		private Random rnd;

		public PointF Start {
			get { return start; }
			set { start = value; }
		}

		public PointF Stop {
			get { return stop; }
			set { stop = value; }
		}

		public double ShootChance {
			set { shootChance = 0.95; }
		}

		public RangedEnemy(Image img, float x, float y, uint mask, float health, float[] xVals, float[] yVals) : base(img, x, y, mask, xVals, yVals) {
			Health = health;
			start = new PointF(x, y);
			stop = new PointF(x, y);
			MotionModel = ACCELERATING;
			Grounded = false;
			rnd = new Random();
		}

		public RangedEnemy(Image img, float x, float y, uint mask, float health, Polygon box) : base(img, x, y, mask, box) {
			Health = health;
			start = new PointF(x, y);
			stop = new PointF(x, y);
			MotionModel = ACCELERATING;
			Grounded = false;
			rnd = new Random();
		}

		public override void OnCollide(int dir, CollisionSprite cs) {
			base.OnCollide(dir, cs);
			if(cs is BlockSprite) {
				if(dir == LEFT || dir == RIGHT)
					Velocity.X *= -1;
			}
		}

		public override void Act() {
			base.Act();
			if(start != stop) {
				if(!Polygon.IsBetween(X, start.X, stop.X)) {
					Velocity.X *= -1;
				}
			}
			if(Facing == RIGHT) {
				if(rnd.NextDouble() > shootChance)
					ShootAt(CenterX + 50, CenterY, 20, 5, 10, Resources.green_circle);
			} else {
				if(rnd.NextDouble() > shootChance)
					ShootAt(CenterX - 50, CenterY, 20, 5, 10, Resources.green_circle);
			}
		}
	}
}
