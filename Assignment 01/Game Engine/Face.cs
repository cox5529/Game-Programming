using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	class Face:Sprite {

		private Image img;
		private double[] phaseShift;
		private double[] amp;
		private double[] speed;

		public Face(Image img, double[] phaseShift, double[] amp, double[] speed) {
			this.img = img;
			this.phaseShift = phaseShift;
			this.amp = amp;
			this.speed = speed;
		}

		public override void Paint(Graphics g) {
			X = (int)(amp[0] * Math.Cos(Form1.s / speed[0] + phaseShift[0]) + amp[0]) / 4;
			Y = (int)(amp[1] * Math.Sin(Form1.s / speed[1] + phaseShift[1]) + amp[1]) / 4;
			g.DrawImage(img, X, Y);
		}
	}
}
