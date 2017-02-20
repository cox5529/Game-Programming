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
using WindowsFormsApplication1.Properties;

namespace Engine {
	public partial class Form1:Form {

		private static double seconds;
		private Sprite main;

		public static Image[] levi;
		public static Form1 form;
		public static int s = 100;
		public static double fps;
		public static DateTime start;
		public static Thread frameThread;
		public static Thread gameThread;

		const int FRAME_RATE = 60;
		const int UPDATE_RATE = 6;

		public double Seconds {
			get { return seconds; }
		}

		public Sprite Main {
			get { return main; }
		}

		public Form1() {
			start = DateTime.Now;
			InitializeComponent();
			DoubleBuffered = true;
			main = new Sprite();
			Image img = (Image)Resources.ResourceManager.GetObject("face1");
			Random rand = new Random();
			for(int i = 0; i < 10; i++) {
				main.Add(new ImageSprite(img, rand.Next(200000)));
			}
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
				now = temp;
				TimeSpan diff = now - last;
				if(diff.TotalMilliseconds < frameTime.TotalMilliseconds)
					Thread.Sleep((frameTime - diff).Milliseconds);
				last = DateTime.Now;
				form.Main.Act();
				TimeSpan span = DateTime.Now - start;
				seconds = span.Seconds;
			}
		}

		protected override void OnKeyPress(KeyPressEventArgs e) {
			base.OnKeyPress(e);
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
		}
	}
}
