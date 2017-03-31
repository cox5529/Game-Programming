using Engine;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using WindowsFormsApplication1.Properties;

namespace Engine {
	public class Program:Engine {

		private bool win = false;

		public static SoundSlideSprite levi;
		public static SlideSprite[,] goals;
		public static SlideSprite[,] walls;
		public static SlideSprite[,] blocks;
		public static WinSprite winSprite;
		public static int x;
		public static int y;
		private static int width;
		private static int height;
		private static string stage;

		private static int startStage = -1;
		private static int playedStages = 0;
		private static string[] stages;

		public const int IMAGE_SIZE = 512;

		public Program() : base() {
			win = false;
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			bool changed = false;
			if(!win) {
				if(e.KeyCode == Keys.Right) {
					if(canMoveTo(x + 1, y, 1, 0)) {
						changed = true;
						x++;
					}
					if(blocks[x, y] != null)
						moveBlock(x, y, 1, 0);
				}
				if(e.KeyCode == Keys.Left) {
					if(canMoveTo(x - 1, y, -1, 0)) {
						changed = true;
						x--;
					}
					if(blocks[x, y] != null)
						moveBlock(x, y, -1, 0);
				}
				if(e.KeyCode == Keys.Up) {
					if(canMoveTo(x, y - 1, 0, -1)) {
						changed = true;
						y--;
					}
					if(blocks[x, y] != null)
						moveBlock(x, y, 0, -1);
				}
				if(e.KeyCode == Keys.Down) {
					if(canMoveTo(x, y + 1, 0, 1)) {
						changed = true;
						y++;
					}
					if(blocks[x, y] != null)
						moveBlock(x, y, 0, 1);
				}
				if(e.KeyCode == Keys.R) {
					Reset(false);
					return;
				}
				if(changed) {
					levi.TargetLeviX = x * IMAGE_SIZE;
					levi.TargetLeviY = y * IMAGE_SIZE;
					CheckWin();
				}
			} else {
				Reset(true);
			}
		}

		public void Reset(bool inc) {
			if(inc)
				playedStages++;
			int index = startStage + playedStages;
			while(index >= stages.Length)
				index -= stages.Length;
			stage = stages[index];
			Init();
			win = false;
			fixScale();
		}

		public void CheckWin() {
			for(int i = 0; i < blocks.Length; i++) {
				int x = i % width;
				int y = i / width;
				if(blocks[x, y] != null) {
					if(goals[x, y] == null) {
						return;
					}
				}
			}
			win = true;
			winSprite.Win = true;
		}

		public void moveBlock(int i, int j, int dx, int dy) {
			blocks[i + dx, j + dy] = blocks[i, j];
			blocks[i, j] = null;

			blocks[i + dx, j + dy].TargetX = (i + dx) * IMAGE_SIZE;
			blocks[i + dx, j + dy].TargetY = (j + dy) * IMAGE_SIZE;
			if(goals[i + dx, j + dy] != null)
				blocks[i + dx, j + dy].Image = Resources.green_circle;
			else
				blocks[i + dx, j + dy].Image = Resources.blue_circle;

		}

		public Boolean canMoveTo(int i, int j, int dx, int dy) {

			if(walls[i, j] == null && blocks[i, j] == null)
				return true;
			if(walls[i, j] != null)
				return false;
			if(blocks[i, j] != null && blocks[i + dx, j + dy] == null && walls[i + dx, j + dy] == null)
				return true;
			return false;

		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
			fixScale();
		}

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			fixScale();
		}

		private void fixScale() {
			float s = (float)(Math.Min(ClientSize.Width, ClientSize.Height) / (Math.Max(goals.GetLength(0), goals.GetLength(1)) * IMAGE_SIZE * 1.0));
			Canvas.Scale = s;
			winSprite.X = width * IMAGE_SIZE * s / 2;
			winSprite.Y = height * IMAGE_SIZE * s / 2;
			winSprite.FontSize = (int)(s * 384);
		}

		private static void Init() {
			Canvas.ClearChildren();
			winSprite.Win = false;
			String[] lines = stage.Split('\n');
			width = lines[0].Length;
			height = lines.Length;
			Debug.WriteLine(width);
			Debug.WriteLine(height);
			goals = new SlideSprite[width, height];
			walls = new SlideSprite[width, height];
			blocks = new SlideSprite[width, height];
			for(int j = 0; j < height; j++) {
				Debug.WriteLine(j);
				for(int i = 0; i < lines[j].Length; i++) {
					if(lines[j][i] == 'g' || lines[j][i] == 'B') {
						goals[i, j] = new SlideSprite(Resources.grey, i * IMAGE_SIZE, j * IMAGE_SIZE, true);
						Program.Canvas.Add(goals[i, j]);
					}
					if(lines[j][i] == 'w') {
						walls[i, j] = new SlideSprite(Resources.black, i * IMAGE_SIZE, j * IMAGE_SIZE, true);
						Program.Canvas.Add(walls[i, j]);
					}
					if(lines[j][i] == 'b' || lines[j][i] == 'B') {
						blocks[i, j] = new SlideSprite(Resources.blue_circle, i * IMAGE_SIZE, j * IMAGE_SIZE, true);
						if(lines[j][i] == 'B')
							blocks[i, j].Image = Resources.green_circle;

					}
					if(lines[j][i] == 'c') {
						levi = new SoundSlideSprite(Resources.vroom, Resources.face1, i * IMAGE_SIZE, j * IMAGE_SIZE, true);
						x = i;
						y = j;
					}
				}
			}
			for(int j = 0; j < height; j++)
				for(int i = 0; i < width; i++)
					if(blocks[i, j] != null)
						Program.Canvas.Add(blocks[i, j]);
			Program.Canvas.Add(levi);
		}

		private static void ChooseStage() {
			Random rnd = new Random();
			int index = rnd.Next(stages.Length);
			stage = stages[index];
			startStage = index;
		}


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			stages = new string[3];
			stages[0] = Resources.level1;
			stages[1] = Resources.level2;
			stages[2] = Resources.level3;
			ChooseStage();
			winSprite = new WinSprite(false, "You win!\nPress any key\nto continue.");
			Program.GUI.Add(winSprite);
			Init();
			Program p = new Program();
			Application.Run(p);
			p.fixScale();
		}
	}
}