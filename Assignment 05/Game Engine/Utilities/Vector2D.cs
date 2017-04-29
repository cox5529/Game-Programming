using System;

namespace Engine.Utilities {
	public class Vector2D {

		private float x;
		private float y;

		public float X {
			get { return x; }
			set { x = value; }
		}

		public float Y {
			get { return y; }
			set { y = value; }
		}

		public float Direction {
			get {
				return (float)(Math.Atan2(y, x) * (180.0 / Math.PI));
			}
			set {
				double dir = value * Math.PI / 180.0;
				float mag = Magnitude;
				x = (float)(mag * Math.Cos(dir));
				y = (float)(mag * Math.Sin(dir));
			}
		}

		public float Magnitude {
			get { return (float)(Math.Sqrt(y * y + x * x)); }
			set { double dir = Math.Atan2(y, x); x = (float)(value * Math.Cos(dir)); y = (float)(value * Math.Sin(dir)); }
		}

		public Vector2D(float x, float y) {
			this.x = x;
			this.y = y;
		}

		public static bool operator ==(Vector2D v1, Vector2D v2) {
			return (v1.X == v2.X && v1.Y == v2.Y);
		}

		public static bool operator !=(Vector2D v1, Vector2D v2) {
			return !(v1 == v2);
		}

		public static Vector2D operator +(Vector2D v1, Vector2D v2) {
			return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
		}

		public static Vector2D Add(Vector2D v1, Vector2D v2) {
			return v1 + v2;
		}

		public static Vector2D operator -(Vector2D v1, Vector2D v2) {
			return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
		}

		public static Vector2D Subtract(Vector2D v1, Vector2D v2) {
			return v1 - v2;
		}

		public override bool Equals(object obj) {
			if (obj is Vector2D)
				return (Vector2D)obj == this;
			else
				return false;
		}

		public override int GetHashCode() {
			return (int)(x * 4096 + y);
		}
	}
}
