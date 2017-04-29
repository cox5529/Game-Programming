using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprites {
	public class ToggleSprite:TextSprite {

		private bool on;

		public bool On {
			get { return on; }
			set { on = value; }
		}

		public ToggleSprite(bool on, string text, float x, float y) : base(x, y, text) {
			this.on = on;
		}

		public override void Paint(Graphics g) {
			if(on) {
				base.Paint(g);
			}
		}
	}
}
