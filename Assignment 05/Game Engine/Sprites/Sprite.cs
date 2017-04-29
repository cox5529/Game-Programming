﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprites {
	public class Sprite {

		private Sprite parent = null;
		public List<Sprite> children = new List<Sprite>();

		//instance variable
		private float x = 0;

		public float X {
			get { return x; }
			set { x = value; }
		}

		private float y = 0;

		public float Y {
			get { return y; }
			set { y = value; }
		}

		private float scale = 1;

		public float Scale {
			get { return scale; }
			set { scale = value; }
		}

		private float rotation = 0;

		/// <summary>
		/// The rotation in degrees.
		/// </summary>
		public float Rotation {
			get { return rotation; }
			set { rotation = value; }
		}

		public Sprite Parent {
			get { return parent; }
			set { parent = value; }
		}

		public virtual void Kill() {
			if(parent!= null)
				parent.children.Remove(this);
		}

		public void ClearChildren() {
			children = new List<Sprite>();
		}

		//methods
		public virtual void Render(Graphics g) {
			Matrix original = g.Transform.Clone();
			g.TranslateTransform(x, y);
			g.ScaleTransform(scale, scale);
			g.RotateTransform(rotation);
			Paint(g);
			foreach(Sprite s in children) {
				s.Render(g);
			}
			g.Transform = original;
		}

		public virtual void Paint(Graphics g) {

		}

		public virtual void Act() {
			foreach(Sprite s in children) {
				s.Act();
			}
		}

		public void Add(Sprite s) {
			s.parent = this;
			children.Add(s);
		}
	}

}
