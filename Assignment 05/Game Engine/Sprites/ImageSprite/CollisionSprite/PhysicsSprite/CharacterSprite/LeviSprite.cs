using System;
using System.Diagnostics;
using System.Media;
using WindowsFormsApplication1.Properties;
using System.Collections;
using Engine.Utilities;

namespace Engine.Sprites {
	public class LeviSprite:CharacterSprite {

		private static float[] xVals = new float[] { 28, 39, 27, 20, 19, 45, 62, 81, 92, 102, 106, 108, 102, 94, 86, 85, 50 };
		private static float[] yVals = new float[] { 123, 94, 78, 50, 29, 7, 0, 12, 12, 22, 44, 52, 73, 100, 104, 127, 127 };


		public LeviSprite() : this(0, 0) { }

		public LeviSprite(float x, float y) : base(Resources.levi, x, y, 9, xVals, yVals) {
			Mass = 20;
		}

		public void ShootAt(float x, float y) {
			base.ShootAt(x, y, 20, 6, 10, Resources.blue_circle);
			Random rnd = new Random();
			if(rnd.Next(1, 3) == 1)
				new SoundPlayer(Resources.pew1).Play();
			else
				new SoundPlayer(Resources.pew2).Play();
		}
	}
}
