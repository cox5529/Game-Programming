using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine {
	public partial class Form1:Form {

		private Sprite main;
		private Face top;
		private string text = "";
		private string input = "";

		public static Image[] levi;
		public static Form form;
		public static int s = 100;
		public static double fps;
		public static double tps;
		public static Thread frameThread;
		public static Thread gameThread;

		const int FRAME_RATE = 60;
		const int UPDATE_RATE = 60;

		public Form1() {
			levi = new Image[5];
			levi[0] = Image.FromFile("face1.png");
			levi[1] = ScaleImage(Image.FromFile("face2.png"), levi[0].Width, levi[0].Height);
			//levi[1] = Image.FromFile("face2.png");
			levi[2] = ScaleImage(Image.FromFile("face3.png"), levi[0].Width, levi[0].Height);
			levi[3] = ScaleImage(Image.FromFile("face4.png"), levi[0].Width, levi[0].Height);
			levi[4] = ScaleImage(Image.FromFile("face5.png"), levi[0].Width, levi[0].Height);

			Image brando = ScaleImage(Image.FromFile("face.png"), levi[0].Width, levi[0].Height);
			InitializeComponent();
			DoubleBuffered = true;
			main = new Sprite();
			top = new Face(brando, new double[] { 50, 50 }, new double[] { 100, 100 }, new double[] { 10, 10 });

			form = this;
			frameThread = new Thread(new ThreadStart(pushFrame));
			frameThread.Start();
			gameThread = new Thread(new ThreadStart(updateState));
			gameThread.Start();
		}

		public static Image ScaleImage(Image image, int maxWidth, int maxHeight) {
			double ratioX = (double)maxWidth / image.Width;
			double ratioY = (double)maxHeight / image.Height;
			double ratio = Math.Min(ratioX, ratioY);

			int newWidth = (int)(image.Width * ratio);
			int newHeight = (int)(image.Height * ratio);

			Bitmap newImage = new Bitmap(newWidth, newHeight);

			using(var graphics = Graphics.FromImage(newImage))
				graphics.DrawImage(image, 0, 0, newWidth, newHeight);

			return newImage;
		}

		public static void pushFrame() {
			DateTime last = DateTime.Now;
			DateTime now = last;
			TimeSpan frameTime = new TimeSpan(10000000 / FRAME_RATE);
			while(true) {
				DateTime temp = DateTime.Now;
				fps = .9 * fps + .1 * 1000.0 / (temp - now).TotalMilliseconds;
				now = temp;
				TimeSpan diff = now - last;
				if(diff.TotalMilliseconds < frameTime.TotalMilliseconds)
					Thread.Sleep((frameTime - diff).Milliseconds);
				last = DateTime.Now;
				form.Invoke(new MethodInvoker(form.Refresh));
			}
		}

		public static void updateState() {
			DateTime last = DateTime.Now;
			DateTime now = last;
			TimeSpan frameTime = new TimeSpan(10000000 / UPDATE_RATE);
			while(true) {
				DateTime temp = DateTime.Now;
				tps = .9 * tps + .1 * 1000.0 / (temp - now).TotalMilliseconds;
				now = temp;
				TimeSpan diff = now - last;
				if(diff.TotalMilliseconds < frameTime.TotalMilliseconds)
					Thread.Sleep((frameTime - diff).Milliseconds);
				last = DateTime.Now;
				s++;
			}
		}

		protected override void OnKeyPress(KeyPressEventArgs e) {
			base.OnKeyPress(e);
			Console.WriteLine((int)e.KeyChar);
			if(text == "") {
				if(e.KeyChar == 'a' || e.KeyChar == 'A') {
					text = "How many pictures would you like to add? ";
				} else if(e.KeyChar == 'd' || e.KeyChar == 'D' || e.KeyChar == 'r' || e.KeyChar == 'R') {
					text = "How many pictures would you like to remove? ";
				}
			} else if(text == "How many pictures would you like to add? " || text == "How many pictures would you like to remove? ") {
				int val = 0;
				if(int.TryParse(e.KeyChar + "", out val)) {
					input += "" + e.KeyChar;
				} else if((int)e.KeyChar == 13) {
					int.TryParse(input, out val);
					if(text == "How many pictures would you like to add? ") {
						Random r = new Random();
						for(int i = 0; i < val; i++) {
							main.Add(new Face(levi[r.Next() % levi.Length], new double[] { r.Next(), r.Next() }, new double[] { ClientSize.Width, ClientSize.Height }, new double[] { r.Next() % 200, r.Next() % 200 }));
						}
					} else {
						main.Remove(val);
					}
					text = "";
					input = "";
				} else if((int)e.KeyChar == 8 && input.Length > 0) {
					input = input.Substring(0, input.Length - 1);
				}
			} else if((int)e.KeyChar == 32) {
				if(frameThread.IsAlive) {
					frameThread.Abort();
				} else {
					frameThread.Start();
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			Refresh();
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			Refresh();
		}

		protected override void OnClosed(EventArgs e) {
			base.OnClosed(e);
			frameThread.Abort();
			gameThread.Abort();
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			main.Render(e.Graphics);
			top.Render(e.Graphics);
			e.Graphics.FillRectangle(Brushes.Black, 0, 0, ClientSize.Width, 20);
			Font f = new Font("Arial", 12);
			e.Graphics.DrawString("FPS: " + fps + "\tTPS: " + tps + "\tLevis: " + main.ChildCount + "\tBrandons: " + 1, f, Brushes.White, 0f, 0f);
			if(text != "") {
				e.Graphics.FillRectangle(Brushes.Black, 0, (ClientSize.Height - 50) / 2, ClientSize.Width, 50);
				e.Graphics.DrawString(text + input, f, Brushes.White, 0, (ClientSize.Height - 25) / 2);
			}
			e.Graphics.FillRectangle(Brushes.Black, 0, ClientSize.Height - 20, ClientSize.Width, 20);
			e.Graphics.DrawString("Press 'a' to open add Levi dialog. Press 'd' to open remove Levi dialog.", f, Brushes.White, 0, ClientSize.Height - 20);
		}
	}
}
