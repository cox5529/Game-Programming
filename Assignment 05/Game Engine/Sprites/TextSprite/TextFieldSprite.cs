using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Sprites {
	public class TextFieldSprite:TextSprite {

		private float width;
		private float height;

		public TextFieldSprite(float x, float y, float width, float height) : base(x, y, "") {
			this.width = width;
			this.height = height;
		}

		public void Input(KeyEventArgs e){
			if(e.KeyCode == Keys.Back) {
				if(Text.Length >= 1)Text = Text.Substring(0, Text.Length - 1);
			} else {
				Text += e.KeyCode;
			}
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			g.DrawRectangle(Pens.White, -10, -10, width + 10, height + 20);
		}
	}
}
