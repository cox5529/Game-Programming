using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1.Properties;

namespace Engine.Sprites {
	class JasonSprite:RangedEnemy {

		private static float[] xVals = new float[] { 35, 25, 14, 10, 12, 32, 51, 74, 113, 117, 107, 103, 95, 95, 87 };
		private static float[] yVals = new float[] { 126, 92, 82, 59, 33, 9, 0, 0, 30, 62, 92, 92, 110, 121, 127 };

		public JasonSprite(float x, float y, uint mask, float health) : base(Resources.json, x, y, mask, health, xVals, yVals) {
			Mass = 20;
		}
	}
}
