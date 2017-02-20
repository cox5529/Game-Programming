using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	public class Sprite {
		private float x;
		private float y;
		private float scale;
		protected List<Sprite> children;

		public float X {
			get { return x; }
			set { x = value; }
		}

		public float Y {
			get { return y; }
			set { y = value; }
		}

		public float Scale {
			get { return scale; }
			set { scale = value; }
		}

		public int ChildCount {
			get { return children.Count; }
		}

		public Sprite() {
			children = new List<Sprite>();
			this.x = 0;
			this.y = 0;
			this.scale = 1;
		}

		public void Render(Graphics g) {
			Matrix original = g.Transform.Clone();
			g.ScaleTransform(scale, scale);
			g.TranslateTransform(x, y);
			Paint(g);
			foreach(Sprite s in children) {
				s.Render(g);
			}
			g.Transform = original;
		}

		public void Add(Sprite s) {
			children.Add(s);
		}

		public void Remove(int n) {
			for(int i = 0; i < n; i++) {
				if(children.Count == 0)
					break;
				children.Remove(children.Last());
			}
		}

		public virtual void Paint(Graphics g) {

		}
	}
}
