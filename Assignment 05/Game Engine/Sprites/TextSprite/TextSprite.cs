using System.Drawing;

namespace Engine.Sprites {
	public class TextSprite:Sprite {

		private string text;
		private int fontSize;
		private StringAlignment horizontalAlignment;
		private StringAlignment verticalAlignment;

		public string Text {
			get { return text; }
			set { text = value; }
		}

		public StringAlignment HorizontalAlignment {
			get { return horizontalAlignment; }
			set { horizontalAlignment = value; }
		}

		public StringAlignment VerticalAlignment {
			get { return verticalAlignment; }
			set { verticalAlignment = value; }
		}

		public int FontSize {
			get { return fontSize; }
			set { fontSize = value; }
		}

		public TextSprite(float x, float y, string text) {
			X = x;
			Y = y;
			this.text = text;
			fontSize = 24;
			horizontalAlignment = StringAlignment.Center;
			verticalAlignment = StringAlignment.Center;
		}

		public void Append(string s) {
			text += s + "\n";
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			StringFormat sf = new StringFormat();
			sf.Alignment = horizontalAlignment;
			sf.LineAlignment = verticalAlignment;
			g.DrawString(text, new Font("Arial", fontSize), Brushes.White, 0, 0, sf);
		}
	}
}
