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
	public partial class Engine:Form {

		private static double seconds;
		private static Sprite main;
		private static Sprite gui;

		public static Engine form;
		public static double fps;
		public static DateTime start;
		public static Thread frameThread;
		public static Thread gameThread;

		const int FRAME_RATE = 60;
		const int UPDATE_RATE = 30;

		public double Seconds {
			get { return seconds; }
		}

		public static Sprite Canvas {
			get {
				if(main == null)
					main = new Sprite();
				return main;
			}
		}

		public static Sprite GUI {
			get {
				if(gui == null)
					gui = new Sprite();
				return gui;
			}
		}

		public Engine() {
			start = DateTime.Now;
			InitializeComponent();
			DoubleBuffered = true;
			form = this;
			if(main == null)
				main = new Sprite();
			if(gui == null)
				gui = new Sprite();
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
				fps = 0.9 * fps + 0.1 * (temp - now).TotalMilliseconds;
				Debug.WriteLine(fps);
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
				Canvas.Act();
				gui.Act();
				TimeSpan span = DateTime.Now - start;
				seconds = span.Seconds;
			}
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			main.Render(e.Graphics);
			gui.Render(e.Graphics);
		}

		protected override void OnClosed(EventArgs e) {
			base.OnClosed(e);
			frameThread.Abort();
			gameThread.Abort();
		}
	}
}
