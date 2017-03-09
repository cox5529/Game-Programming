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
		private static Sprite canvas;

		public static Image[] levi;
		public static Engine form;
		public static int s = 100;
		public static double fps;
		public static DateTime start;
		public static Thread frameThread;
		public static Thread gameThread;

		const int FRAME_RATE = 30;
		const int UPDATE_RATE = 30;

		public double Seconds {
			get { return seconds; }
		}

		public static Sprite Canvas {
			get {
				if(canvas == null)
					canvas = new Sprite();
				return canvas;
			}
		}

		public Engine() {
			start = DateTime.Now;
			if(canvas == null)
				canvas = new Sprite();
			DoubleBuffered = true;
			form = this;
			frameThread = new Thread(new ThreadStart(pushFrame));
			frameThread.Start();
			gameThread = new Thread(new ThreadStart(updateState));
			gameThread.Start();
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
				Canvas.Act();
				TimeSpan span = DateTime.Now - start;
				seconds = span.Seconds;
			}
		}

		protected override void OnClosed(EventArgs e) {
			base.OnClosed(e);
			frameThread.Abort();
			gameThread.Abort();
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			canvas.Render(e.Graphics);
		}
	}
}
