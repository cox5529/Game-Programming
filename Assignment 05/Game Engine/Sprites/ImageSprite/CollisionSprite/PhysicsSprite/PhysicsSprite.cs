using Engine.Utilities;
using System.Drawing;

namespace Engine.Sprites {
	public class PhysicsSprite : CollisionSprite {

		public const int STATIC = 0;
		public const int CONSTANT_VELOCITY = 1;
		public const int ACCELERATING = 2;
		public static Vector2D GRAVITY = new Vector2D(0f, 1.0f);

		private int motionModel;
		private int mass;
		private bool grounded;
		private bool leftBlocked;
		private bool rightBlocked;
		private Vector2D vel;
		private Vector2D accel;

		public int MotionModel {
			get { return motionModel; }
			set { motionModel = value; }
		}

		public bool Grounded {
			get { return grounded; }
			set { grounded = value; }
		}

		public Vector2D Velocity {
			get { return vel; }
			set { vel = value; }
		}

		public Vector2D Acceleration {
			get { return accel; }
		}

		public int Mass {
			get { return mass; }
			set { mass = value; }
		}

		public bool LeftBlocked { get => leftBlocked; set => leftBlocked = value; }
		public bool RightBlocked { get => rightBlocked; set => rightBlocked = value; }

		public PhysicsSprite(Image img, Polygon box) : this(img, 0, 0, 1, box, 2) { }

		public PhysicsSprite(Image img, float x, float y, uint mask) : base(img, x, y, mask) {
			motionModel = ACCELERATING;
			grounded = false;
			vel = new Vector2D(0, 0);
			accel = new Vector2D(0, 0);
		}

		public PhysicsSprite(Image img, float x, float y, Polygon box) : this(img, x, y, 1, box, 2) { }

		public PhysicsSprite(Image img, float x, float y, uint mask, Polygon box) : this(img, x, y, mask, box, 2) { }

		public PhysicsSprite(Image img, float x, float y, uint mask, float[] xVals, float[] yVals) : base(img, x, y, mask, xVals, yVals) {
			motionModel = ACCELERATING;
			grounded = false;
			vel = new Vector2D(0, 0);
			accel = new Vector2D(0, 0);
		}

		public PhysicsSprite(Image img, float x, float y, uint mask, Polygon box, int motionModel) : base(img, x, y, mask, box) {
			this.motionModel = motionModel;
			grounded = false;
			vel = new Vector2D(0, 0);
			accel = new Vector2D(0, 0);
		}

		public override void Act() {
			base.Act();
			if(motionModel == STATIC)
				return;
			if(!(leftBlocked && vel.X > 0) && !(rightBlocked && vel.X < 0))
				X += vel.X;
			Y += vel.Y;
			if(motionModel == CONSTANT_VELOCITY)
				return;
			if(!grounded)
				vel += accel + GRAVITY;
			else
				vel.X += accel.X + GRAVITY.X;
		}
	}
}
