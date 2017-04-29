using Engine.Utilities;
using System.Diagnostics;
using System.Drawing;

namespace Engine.Sprites {
	public class BulletSprite:PhysicsSprite {

		private float damage;

		public float Damage {
			get { return damage; }
			set { damage = value; }
		}

		public BulletSprite(Image img, float x, float y, float dmg, uint mask) : this(img, x, y, dmg, mask, new Vector2D(10.0f, 0f)) {
		}

		public BulletSprite(Image img, float x, float y, float dmg, uint mask, Vector2D velocity) : base(img, x, y, mask) {
			MotionModel = CONSTANT_VELOCITY;
			Velocity = velocity;
			damage = dmg;
		}

		public override void Act() {
			base.Act();
			Sprite center = Engine.engine.Center;
			if(center != null) {
				Vector2D vect = new Vector2D(center.X - X, center.Y - Y);
				if(vect.Magnitude > 2000)
					Engine.Remove(this);
			}
		}

		public override void OnCollide(int dir, CollisionSprite cs) {
			base.OnCollide(dir, cs);
			if(cs is CharacterSprite) {
				CharacterSprite c = (CharacterSprite)cs;
				c.DecrementHealth(damage);
			}
			if(!(cs is BulletSprite)) {
				Engine.Remove(this);
			} else if(cs.Mask != Mask) {
				Engine.Remove(this);
				Engine.Remove(cs);
			}
		}
	}
}
