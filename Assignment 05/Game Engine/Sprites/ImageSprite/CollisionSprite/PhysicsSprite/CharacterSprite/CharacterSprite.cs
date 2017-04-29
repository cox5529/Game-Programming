using Engine.Utilities;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace Engine.Sprites {
	public class CharacterSprite : PhysicsSprite {

		private bool jumping;
		private bool facingRight;
		private bool showHealth;
		private float health;
		private float maxHealth;
		private Random rnd;
		private List<CollectableSprite> inventory;

		public bool Jumping {
			get { return jumping; }
			set { jumping = value; }
		}

		public float Health {
			get { return health; }
			set {
				health = value;
				if(maxHealth < value)
					maxHealth = value;
			}
		}

		public bool ShowHealth {
			get { return showHealth; }
			set { showHealth = value; }
		}

		public int Facing {
			get { return (facingRight ? RIGHT : LEFT); }
			set {
				if(value != Facing) {
					Flip = true;
					facingRight = !facingRight;
				}
			}
		}

		public List<CollectableSprite> Inventory {
			get { return inventory; }
		}

		public CharacterSprite(Image img, float x, float y, uint mask, float[] xVals, float[] yVals) : base(img, x, y, mask, xVals, yVals) {
			Initialize();
		}

		public CharacterSprite(Image img, float x, float y, uint mask, Polygon box) : base(img, x, y, mask, box) {
			Initialize();
		}

		private void Initialize() {
			MotionModel = ACCELERATING;
			jumping = false;
			facingRight = true;
			rnd = new Random();
			inventory = new List<CollectableSprite>();
		}

		public override void OnCollide(int dir, CollisionSprite cs) {
			base.OnCollide(dir, cs);
			if(cs is BlockSprite) {
				if(dir == DOWN) { //above
					if(!Grounded) {
						DecrementHealth((int) (Velocity.Y / 25.0f));
						Velocity.Y = 0;
						while(cs.Box.Intersects(Box)) {
							Y -= 1;
						}
						Y += 1;
						Grounded = true;
						jumping = false;
					}
				} else if(dir == RIGHT) { //left/right
					LeftBlocked = true;
					while(cs.Box.Intersects(Box))
						X -= 1;
				} else if(dir == LEFT) {
					RightBlocked = true;
					while(cs.Box.Intersects(Box))
						X += 1;
				} else if(dir == UP) { //below
					Grounded = false;
					Velocity.Y = 0;
					while(cs.Box.Intersects(Box))
						Y += 1;
				}
			} else if(cs is CollectableSprite) {
				Engine.SwapParent(cs, this);
				inventory.Add((CollectableSprite) cs);
				UpdateInventoryPositions();
			}
		}

		public void UpdateInventoryPositions() {
			float o = 0;
			int yLevel = 1;
			for(int i = 0; i < inventory.Count; i++) {
				CollectableSprite c = inventory[i];
				c.X = Width - c.Width - o;
				c.Y = -c.Height * yLevel - 17;
				while(c.X < 0) {
					c.X += Width;
					c.Y -= c.Height;
					yLevel++;
					o = 0;
				}
				c.Velocity.Y = 0;
				o += c.Width;
			}
		}

		public virtual void ShootAt(float x, float y, float mag, uint mask, float dmg, Image img) {
			Vector2D vect = new Vector2D(x - CenterX, y - CenterY);
			vect.Magnitude = mag;
			BulletSprite bs = new BulletSprite(img, CenterX, CenterY, dmg, mask, vect);
			bs.MotionModel = CONSTANT_VELOCITY;
			Engine.AddToCanvas(bs);
		}

		public virtual void OnDeath() {
			foreach(CollectableSprite cs in inventory) {
				cs.X = X + rnd.Next(-10, 10);
				cs.Y = Y + Height - cs.Height - rnd.Next(0, 50);
				cs.UpperY = Y + Height - cs.Height;
				cs.LowerY = Y + Height - cs.Height - 50;
				cs.Velocity.Y = 3;
				Sprites.Add(cs);
				Engine.AddToCanvas(cs);
			}
		}

		public void DecrementHealth(float h) {
			health -= h;
			if(health <= 0) {
				OnDeath();
				Engine.Remove(this);
				Engine.engine.OnSpriteKill(this);
			}
		}

		public override void OnStopCollide(CollisionSprite cs) {
			base.OnStopCollide(cs);
			if(cs is BlockSprite) {
				Grounded = false;
				LeftBlocked = false;
				RightBlocked = false;
			}
		}

		public override void Act() {
			base.Act();
			if(Velocity.X > 0 && !facingRight) {
				facingRight = true;
				Flip = true;
				Box.Flip();
			} else if(Velocity.X < 0 && facingRight) {
				facingRight = false;
				Flip = true;
				Box.Flip();
			}
			if(Y >= 2000)
				Engine.Remove(this);
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			if(showHealth) {
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;
				float pct = health / maxHealth;
				g.FillRectangle(Brushes.Red, 0, -15, Width, 15);
				g.FillRectangle(Brushes.Green, 0, -15, Width * pct, 15);
				g.DrawString((int) health + "", new Font("Arial", 10), Brushes.Black, Width * pct / 2, -15, sf);
			}
		}
	}
}
