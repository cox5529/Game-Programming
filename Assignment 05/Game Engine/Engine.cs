using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Engine.Sprites;
using System.Collections.Generic;

namespace Engine {
	public partial class Engine:Form {

		private static double seconds;

		public static Engine engine;
		public static DateTime Start;
		public static Thread FrameThread;
		public static Thread GameThread;

		private static List<Sprite> toAddCanvas;
		private static List<Sprite> toAddGUI;
		private static List<Sprite> toRemove;
		private static List<Sprite> nextParents;
		private static bool frameActive;
		private static bool updateActive;
		private static bool removeActive;

		private const int FRAME_RATE = 60;
		private const int UPDATE_RATE = 60;
		private Sprite main;
		private Sprite gui;
		private Sprite center;
		private float offsetX;
		private float offsetY;

		public double Seconds {
			get { return seconds; }
		}

		public Sprite Canvas {
			get {
				if(main == null)
					main = new Sprite();
				return main;
			}
		}

		public Sprite GUI {
			get {
				if(gui == null)
					gui = new Sprite();
				return gui;
			}
		}

		public Sprite Center {
			get {
				return center;
			}
		}

		public Engine() {
			Start = DateTime.Now;
			InitializeComponent();
			DoubleBuffered = true;
			engine = this;
			if(main == null)
				main = new Sprite();
			if(gui == null)
				gui = new Sprite();
			FrameThread = new Thread(new ThreadStart(PushFrame));
			GameThread = new Thread(new ThreadStart(UpdateState));
		}

		public void StartThreads() {
			GameThread.Start();
			FrameThread.Start();
		}

		public static void PushFrame() {
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
				frameActive = true;
				engine.Invoke(new MethodInvoker(engine.Refresh));
				frameActive = false;
				RemoveChecks();
			}
		}

		public static void UpdateState() {
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
				updateActive = true;
				engine.Canvas.Act();
				engine.gui.Act();
				engine.OnFrame();
				if(engine.Center != null) {
					engine.Canvas.X = engine.ClientSize.Width / 2 - ((engine.Center.X + engine.offsetX)) * engine.Canvas.Scale;
					engine.Canvas.Y = engine.ClientSize.Height / 2 - ((engine.Center.Y + engine.offsetY)) * engine.Canvas.Scale;
				}
				updateActive = false;
				TimeSpan span = DateTime.Now - Start;
				seconds = span.Seconds;
				RemoveChecks();
			}
		}

		private static void RemoveChecks() {
			if(!updateActive && !frameActive && !removeActive) {
				removeActive = true;
				if(toRemove != null) {
					for(int i = 0; i < toRemove.Count; i++) {
						Sprite s = toRemove[i];
						s.Kill();
						if(nextParents[i] != null) {
							s.Parent = nextParents[i];
							nextParents[i].Add(s);
						}
						if(s is CharacterSprite)
							engine.OnSpriteKill((CharacterSprite)(s));
					}
					toRemove.Clear();
					nextParents.Clear();
				}
				if(toAddCanvas != null) {
					foreach(Sprite s in toAddCanvas) {
						engine.Canvas.Add(s);
					}
					toAddCanvas.Clear();
				}
				if(toAddGUI != null) {
					foreach(Sprite s in toAddGUI) {
						engine.GUI.Add(s);
					}
					toAddGUI.Clear();
				}
				removeActive = false;
			}
		}

		public void ClearGUI() {
			for(int i = 0; i < gui.children.Count; i++) {
				Remove(gui.children[i]);
			}
		}

		public void ClearCanvas() {
			for(int i = 0; i < main.children.Count; i++) {
				Remove(main.children[i]);
			}
		}

		public void CenterOn(Sprite s, float offsetX, float offsetY) {
			center = s;
			this.offsetX = offsetX;
			this.offsetY = offsetY;
		}

		public static void Remove(Sprite s) {
			SwapParent(s, null);
		}

		public static void SwapParent(Sprite s, Sprite parent) {
			if(toRemove == null) {
				toRemove = new List<Sprite>();
			}
			if(nextParents == null) {
				nextParents = new List<Sprite>();
			}
			if(!toRemove.Contains(s)) {
				toRemove.Add(s);
				nextParents.Add(parent);
			}
		}

		public static void AddToCanvas(Sprite s) {
			if(toAddCanvas == null) {
				toAddCanvas = new List<Sprite>();
			}
			toAddCanvas.Add(s);
		}

		public static void AddToGUI(Sprite s) {
			if(toAddGUI == null) {
				toAddGUI = new List<Sprite>();
			}
			toAddGUI.Add(s);
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			main.Render(e.Graphics);
			gui.Render(e.Graphics);
		}

		protected override void OnClosed(EventArgs e) {
			base.OnClosed(e);
			FrameThread.Abort();
			GameThread.Abort();
		}

		public virtual void OnSpriteKill(CharacterSprite cs) {

		}

		public virtual void OnFrame() { }
	}
}
