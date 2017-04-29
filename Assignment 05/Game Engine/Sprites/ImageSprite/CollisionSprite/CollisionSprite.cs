using Engine.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Engine.Sprites {
	public class CollisionSprite : ImageSprite {

		public static List<CollisionSprite> Sprites;

		public const int RIGHT = 0;
		public const int LEFT = 1;
		public const int UP = 2;
		public const int DOWN = 3;

		private uint mask;
		private Polygon box;
		private List<CollisionSprite> collisions;
		private bool showBox = false;
		private Hashtable table;
		private bool showVectors = false;

		public uint Mask {
			get { return mask; }
			set { this.mask = value; }
		}

		public Polygon Box {
			get {
				box.Transform(X, Y, Rotation, Scale);
				return box;
			}
		}

		public bool ShowVectors {
			get { return showVectors; }
			set { showVectors = value; }
		}

		public List<CollisionSprite> Collisions {
			get { return collisions; }
		}

		public bool ShowBox {
			set { showBox = value; }
		}

		public CollisionSprite(Image img) : this(img, 0, 0, 1, null) {
			box = new Polygon(Width / 2, Height / 2, new PointF(0, 0), new PointF(Width, 0), new PointF(Width, Height), new PointF(0, Height));
		}

		public CollisionSprite(Image img, float x, float y, uint mask) : this(img, x, y, mask, null) { }

		public CollisionSprite(Image img, Polygon box) : this(img, 0, 0, 1, box) { }

		public CollisionSprite(Image img, float x, float y, Polygon box) : this(img, x, y, 1, box) { }

		public CollisionSprite(Image img, float x, float y, uint mask, float[] xVals, float[] yVals) : base(img, x, y) {
			this.mask = mask;
			box = new Polygon(Width / 2, Height / 2, xVals, yVals);
			if(Sprites == null)
				Sprites = new List<CollisionSprite>();
			Sprites.Add(this);
			collisions = new List<CollisionSprite>();
			table = new Hashtable();
		}

		public CollisionSprite(Image img, float x, float y, uint mask, Polygon box) : base(img) {
			this.box = box;
			if(box == null) {
				this.box = new Polygon(Width / 2, Height / 2, new PointF(0, 0), new PointF(Width, 0), new PointF(Width, Height), new PointF(0, Height));
			}
			X = x;
			Y = y;
			this.mask = mask;
			if(Sprites == null)
				Sprites = new List<CollisionSprite>();
			Sprites.Add(this);
			collisions = new List<CollisionSprite>();
			table = new Hashtable();
		}

		public bool CanCollide(CollisionSprite s) {
			return s != null && (s.Mask & mask) != 0;
		}

		private int GetDirection(CollisionSprite cs) {
			Vector2D vect = new Vector2D(cs.CenterX - CenterX, cs.CenterY - CenterY);
			float deg = vect.Direction;
			float t1 = (float) Math.Atan((Width + cs.Width) / (Height + cs.Height)); // arctan((a+c)/(b+d))
			float t2 = (float) Math.Atan((Height + cs.Height) / (Width + cs.Width)); // arctan((b+d)/(a+c))
			t1 *= (float) (180.0 / Math.PI);
			t2 *= (float) (180.0 / Math.PI);
			if(Polygon.IsBetween(deg, 180 - t2, t1))
				return DOWN;
			else if(Polygon.IsBetween(deg, t1, -t2))
				return RIGHT;
			else if(Polygon.IsBetween(deg, -180, t1 - 180) || Polygon.IsBetween(deg, 180, 180 - t2))
				return LEFT;
			else
				return UP;
		}

		public virtual void OnCollide(int dir, CollisionSprite cs) {
			if(showVectors) {
				if(!table.ContainsKey(cs)) {
					Vector2D r = new Vector2D(cs.CenterX - CenterX, cs.CenterY - CenterY);
					VectorSprite vs = new VectorSprite(r, CenterX, CenterY);
					vs.Pen.Width = 5;
					table.Add(cs, vs);
					Engine.AddToCanvas(vs);
				} else {
					VectorSprite vs = (VectorSprite) table[cs];
					Vector2D r = vs.Vector;
					r.X = cs.CenterX - CenterX;
					r.Y = cs.CenterY - CenterY;
					vs.X = CenterX;
					vs.Y = CenterY;
				}
			}
		}

		public virtual void OnStopCollide(CollisionSprite cs) {
			if(showVectors) {
				Engine.Remove((VectorSprite) table[cs]);
				table.Remove(cs);
			}
		}

		public override void Act() {
			base.Act();
			Polygon myBox = Box;
			List<CollisionSprite> newCollisions = new List<CollisionSprite>();
			for(int i = 0; i < Sprites.Count; i++) {
				CollisionSprite cs = Sprites[i];
				if(cs != this && cs != null && CanCollide(cs) && BoundingBoxTest(cs) && myBox.Intersects(cs.Box)) {
					OnCollide(GetDirection(cs), cs);
					newCollisions.Add(cs);
					collisions.Remove(cs);
				} else if(collisions.Contains(cs)) {
					OnStopCollide(cs);
					collisions.Remove(cs);
				}
			}
			foreach(CollisionSprite cs in collisions) {
				OnStopCollide(cs);
			}
			collisions = newCollisions;
		}

		private bool BoundingBoxTest(CollisionSprite cs) {
			return (Math.Abs(CenterX - cs.CenterX) < Width / 2 + cs.Width / 2 && Math.Abs(CenterY - cs.CenterY) < Height / 2 + cs.Height / 2);
		}

		public override void Kill() {
			Sprites.Remove(this);
			base.Kill();
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			if(showBox) {
				List<PointF> myBox = Box.Points;
				for(int i = 0; i < myBox.Count; i++) {
					PointF start;
					if(i == 0)
						start = myBox[myBox.Count - 1];
					else
						start = myBox[i - 1];
					PointF end = myBox[i];
					g.DrawLine(Pens.Red, start.X - X, start.Y - Y, end.X - X, end.Y - Y);
				}
			}
		}
	}
}
