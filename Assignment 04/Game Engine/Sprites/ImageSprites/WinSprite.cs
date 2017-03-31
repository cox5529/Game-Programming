using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	public class WinSprite:Sprite {

		private bool win;
		private string text;
		private int fontSize = 24;

		public bool Win {
			get { return win; }
			set { win = value; }
		}

		public string Text {
			get { return text; }
			set { text = value; }
		}

		public int FontSize {
			set { fontSize = value; }
		}

		public WinSprite(bool on, string text) {
			this.win = on;
			this.text = text;
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			if(win) {
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;
				g.DrawString(text, new Font("Arial", fontSize), Brushes.Red, 0, 0, sf);
			}
		}
	}
}
