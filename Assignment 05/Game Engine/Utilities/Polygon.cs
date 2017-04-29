using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Engine.Utilities {
	public class Polygon {

		private List<PointF> points;
		private List<PointF> transformed;

		private float centerX;
		private float centerY;

		public List<PointF> Points {
			get { return transformed; }
		}

		public Polygon(float centerX, float centerY, float[] xVals, float[] yVals) {
			points = new List<PointF>();
			transformed = new List<PointF>();
			this.centerX = centerX;
			this.centerY = centerY;
			for(int i = 0; i < xVals.Length; i++) {
				points.Add(new PointF(xVals[i], yVals[i]));
				transformed.Add(new PointF(xVals[i], yVals[i]));
			}
		}

		public Polygon(float centerX, float centerY, params PointF[] points) {
			this.points = new List<PointF>();
			this.centerX = centerX;
			this.centerY = centerY;
			this.transformed = new List<PointF>();
			foreach(PointF p in points) {
				this.points.Add(p);
				this.transformed.Add(p);
			}
		}

		public void Transform(float x, float y, float deg, float s) {
			float[,] mat = new float[2, 3];
			deg *= (float)(Math.PI / 180.0);
			mat[0, 0] = (float)(s * Math.Cos(deg));
			mat[0, 1] = (float)(-s * Math.Sin(deg));
			mat[0, 2] = (float)(-centerX * s * Math.Cos(deg) + centerY * s * Math.Sin(deg) + centerX + x);
			mat[1, 0] = (float)(s * Math.Sin(deg));
			mat[1, 1] = (float)(s * Math.Cos(deg));
			mat[1, 2] = (float)(-centerY * s * Math.Sin(deg) - centerY * s * Math.Cos(deg) + centerY + y);

			transformed = new List<PointF>();
			foreach(PointF p in points) {
				PointF n = new PointF();
				n.X = mat[0, 0] * p.X + mat[0, 1] * p.Y + mat[0, 2];
				n.Y = mat[1, 0] * p.X + mat[1, 1] * p.Y + mat[1, 2];
				transformed.Add(n);
			}
		}

		public bool Intersects(Polygon other) {
			List<PointF> otherPoints = other.Points;
			foreach(PointF p in otherPoints) {
				if(ContainsPoint(p))
					return true;
			}
			foreach(PointF p in Points) {
				if(other.ContainsPoint(p))
					return true;
			}
			return false;
		}

		public Polygon Clone() {
			PointF[] nPoints = new PointF[points.Count];
			for(int i = 0; i < points.Count; i++) {
				PointF p = points[i];
				nPoints[i] = new PointF(p.X, p.Y);
			}
			return new Polygon(centerX, centerX, nPoints);
		}

		public override string ToString() {
			string re = "Polygon {\n";
			foreach(PointF p in transformed) {
				re += "\t" + p.ToString() + "\n";
			}
			re += "}";
			return re;
		}

		public bool ContainsPoint(PointF point) {
			int count = 0;
			double a1 = -point.Y;
			double b1 = point.X;
			double c1 = 0;
			List<PointF> pList = Points;
			for(int i = 0; i < pList.Count; i++) {
				PointF p1 = pList[i];
				PointF p2;
				if(i == pList.Count - 1)
					p2 = pList[0];
				else
					p2 = pList[i + 1];
				double a2 = p2.Y - p1.Y;
				double b2 = p1.X - p2.X;
				double c2 = a2 * p1.X + b2 * p1.Y;
				double det = a1 * b2 - a2 * b1;
				if(det != 0) {
					double x = (b2 * c1 - b1 * c2) / det;
					double y = (a1 * c2 - a2 * c1) / det;

					if(IsBetween(x, p1.X, p2.X) && IsBetween(y, p1.Y, p2.Y) && x <= point.X && y <= point.Y) {
						count++;
					}
				}
			}
			return count % 2 == 1;
		}

		public void Flip() {
			for(int i = 0; i < points.Count; i++) {
				points[i] = new PointF(-points[i].X + 2 * centerX, points[i].Y);
			}
		}

		public static bool IsBetween(double test, double x1, double x2) {
			if(x1 < x2)
				return test >= x1 && test <= x2;
			else
				return test >= x2 && test <= x1;
		}
	}
}
