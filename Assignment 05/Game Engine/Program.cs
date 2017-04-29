using Engine.Sprites;
using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApplication1.Properties;

namespace Engine {
	public class Program : Engine {

		private LeviSprite levi;
		private GoalSprite gs;
		private HungmanSprite hs;
		private static int difficulty = 0;
		private int levelIndex;
		private List<Level> levels;
		private Level level;

		private bool left = false;
		private bool right = false;
		private int state;

		public const int IMAGE_SIZE = 128;
		public const int HUNGMAN = 3;
		public const int WIN = 2;
		public const int NORMAL = 0;
		public const int LOSE = 1;
		public const int DIFFICULTY = 4;

		public Program() : base() {
			state = NORMAL;
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			if(state == NORMAL) {
				if(e.KeyCode == Keys.A) {
					left = true;
				}
				if(e.KeyCode == Keys.D) {
					right = true;
				}
				if(e.KeyCode == Keys.Space && levi.Grounded && !levi.Jumping) {
					levi.Velocity.Y -= IMAGE_SIZE / 6;
					levi.Jumping = true;
				}
				KeyCheck();
			} else if(state < NORMAL) {
				IncrementState();
			} else if(state == HUNGMAN) {
				hs.Input(e);
			} else if(state == LOSE || state == WIN) {
				levelIndex = 0;
				level = levels[levelIndex];
				state = -1 * level.Lore.Count - 1;
			} else if(state == DIFFICULTY) {
				if(e.KeyCode == Keys.D0) {
					difficulty = 0;
				} else if(e.KeyCode == Keys.D1) {
					difficulty = 1;
				} else if(e.KeyCode == Keys.D2) {
					difficulty = 2;
				} else if(e.KeyCode == Keys.D3) {
					difficulty = 3;
				} else if(e.KeyCode == Keys.D4) {
					difficulty = 4;
				} else if(e.KeyCode == Keys.D5) {
					difficulty = 5;
				} else if(e.KeyCode == Keys.D6) {
					difficulty = 6;
				} else if(e.KeyCode == Keys.D7) {
					difficulty = 7;
				} else if(e.KeyCode == Keys.D8) {
					difficulty = 8;
				} else if(e.KeyCode == Keys.D9) {
					difficulty = 9;
				} else if(e.KeyCode == Keys.Enter) {
					levelIndex = -1;
					OnWin();
				}
				if(e.KeyCode != Keys.Enter) {
					OnDifficultyChanged();
				}
			}
		}

		private void OnDifficultyChanged() {
			ShowText("Current difficulty = " + difficulty + ".\n\nPress enter to play or press a number to change difficulty.", StringAlignment.Near, StringAlignment.Near);
		}

		protected override void OnMouseClick(MouseEventArgs e) {
			base.OnMouseClick(e);
			if(state == NORMAL)
				levi.ShootAt(levi.X + levi.Width / 2 - (ClientSize.Width / 2 - e.X) / Canvas.Scale, levi.Y + levi.Height / 2 - (ClientSize.Height / 2 - e.Y) / Canvas.Scale);
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			if(state == NORMAL) {
				if(e.KeyCode == Keys.A) {
					left = false;
				}
				if(e.KeyCode == Keys.D) {
					right = false;
				}
				KeyCheck();
			}
		}

		public override void OnSpriteKill(CharacterSprite cs) {
			base.OnSpriteKill(cs);
			if(cs == levi) {
				state = LOSE;
				levi = null;
				gs = null;
				ClearCanvas();
				ClearGUI();
				ShowText("You lose! Press any key to reset.", StringAlignment.Center, StringAlignment.Center);
			}
		}

		private void KeyCheck() {
			if(right && !left)
				levi.Velocity.X = 10;
			else if(!right && left)
				levi.Velocity.X = -10;
			else
				levi.Velocity.X = 0;
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
			FixScale();
		}

		public override void OnFrame() {
			base.OnFrame();
			if(gs != null && gs.FlagCount == level.FlagCount) {
				gs.Inventory.Clear();
				OnWin();
			} else if(hs != null && hs.IsFinished) {
				ClearGUI();
				hs = null;
				OnWin();
			}
		}

		private void IncrementState() {
			state++;
			ClearGUI();
			right = false;
			left = false;
			if(state == NORMAL) {
				ClearCanvas();
				if(level.LevelText == "HUNGMAN\n") {
					state = HUNGMAN;
					hs = new HungmanSprite(ClientSize.Width / Canvas.Scale, ClientSize.Height / Canvas.Scale);
					AddToGUI(hs);
				} else {
					if(level.Flags.ContainsKey("DifficultyMultiplier")) {
						int mult = Int32.Parse((string) level.Flags["DifficultyMultiplier"]);
						if(difficulty == 0)
							difficulty = 1;
						else
							difficulty *= mult;
					}
					BuildLevel(level.LevelText);
				}
			} else {
				ShowText(level.Lore[state + level.Lore.Count], StringAlignment.Near, StringAlignment.Near);
			}
		}

		private void OnWin() {
			ClearCanvas();
			levi = null;
			gs = null;
			if(levelIndex == levels.Count - 1) {
				ShowText("Levi has successfully captured all known copies of Hungman.\nLevi may now seek the other treasures of the world.\nPress any key to reset.", StringAlignment.Center, StringAlignment.Center);
				state = WIN;
			} else {
				if(level != null && level.Flags.ContainsKey("DifficultyMultiplier")) {
					int mult = Int32.Parse((string) level.Flags["DifficultyMultiplier"]);
					if(difficulty == 1)
						difficulty = 0;
					else
						difficulty /= mult;
				}
				levelIndex++;
				level = levels[levelIndex];
				state = -level.Lore.Count - 1;
				IncrementState();
			}
		}

		private void ShowText(string text, StringAlignment horizontal, StringAlignment vertical) {
			ClearGUI();
			float w = ClientSize.Width / Canvas.Scale;
			float h = ClientSize.Height / Canvas.Scale;
			float x = 0;
			float y = 0;
			if(horizontal == StringAlignment.Center)
				x = w / 2;
			else if(horizontal == StringAlignment.Far)
				x = w;
			if(vertical == StringAlignment.Center)
				y = h / 2;
			else if(vertical == StringAlignment.Far)
				y = h;
			TextSprite ts = new TextSprite(x, y, text) {
				HorizontalAlignment = horizontal,
				VerticalAlignment = vertical
			};
			RectangleSprite rs = new RectangleSprite(0, 0, w, h);
			AddToGUI(rs);
			AddToGUI(ts);
		}

		protected override void OnClosed(EventArgs e) {
			base.OnClosed(e);
			if(hs != null)
				hs.KillThread();
		}

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			FixScale();
		}

		private void FixScale() {
			float s = (float) (ClientSize.Width / (IMAGE_SIZE * 10.0));
			Canvas.Scale = s;
			GUI.Scale = s;
		}

		private void BuildLevel(string s) {
			// L = levi
			// j = stationary enemy (jason)
			// J = moving enemy (jason)
			// B = block
			// b = bed
			// F = goal
			// H = base
			// \n => y += 1
			int yLevel = 0;
			int xLevel = 0;
			foreach(char c in s) {
				if(c == 'L') {
					levi = new LeviSprite(xLevel * IMAGE_SIZE, yLevel * IMAGE_SIZE) {
						Health = 100,
						ShowHealth = true
					};
					CenterOn(levi, IMAGE_SIZE / 2, IMAGE_SIZE / 2);
					AddToCanvas(levi);
				} else if(c == 'j') {
					JasonSprite enemy = new JasonSprite(xLevel * IMAGE_SIZE, yLevel * IMAGE_SIZE - 20, 10, 100 + difficulty * 50) {
						ShowHealth = true,
						Facing = CollisionSprite.LEFT,
						Image = Resources.json_station
					};
					AddToCanvas(enemy);
				} else if(c == 'J') {
					JasonSprite enemy = new JasonSprite(xLevel * IMAGE_SIZE, yLevel * IMAGE_SIZE - 20, 10, 100 + difficulty * 25) {
						ShowHealth = true,
						Start = new PointF((xLevel - 2) * IMAGE_SIZE, yLevel * IMAGE_SIZE),
						Stop = new PointF((xLevel + 2) * IMAGE_SIZE, yLevel * IMAGE_SIZE)
					};
					enemy.Velocity.X = 10;
					AddToCanvas(enemy);
				} else if(c == 'B') {
					BlockSprite bs = new BlockSprite(Resources.black, xLevel * IMAGE_SIZE, yLevel * IMAGE_SIZE);
					AddToCanvas(bs);
				} else if(c == 'F') {
					CollectableSprite cs = new CollectableSprite(Resources.hungman_java, xLevel * IMAGE_SIZE, (yLevel + 0.25f) * IMAGE_SIZE, 8, null);
					cs.LowerY = (yLevel + 1) * IMAGE_SIZE - cs.Height;
					cs.ID = "flag";
					AddToCanvas(cs);
				} else if(c == 'H') {
					gs = new GoalSprite(Resources.computer, xLevel * IMAGE_SIZE, yLevel * IMAGE_SIZE, 1);
					AddToCanvas(gs);
				} else if(c == '\n') {
					xLevel = -1;
					yLevel++;
				} else if(c == 'b') {
					float[] xs = new float[] { 0, 2, 95, 102, 120, 125, 125, 128, 128 };
					float[] ys = new float[] { 128, 90, 90, 83, 81, 85, 66, 66, 128 };
					BlockSprite bs = new BlockSprite(Resources.bed, xLevel * IMAGE_SIZE, yLevel * IMAGE_SIZE, new Polygon(IMAGE_SIZE / 2, 100, xs, ys));
					AddToCanvas(bs);
				}
				xLevel++;
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Debug.WriteLine("Start");
			Program p = new Program();
			p.BuildLevel("B");
			p.levels = new List<Level> {
				new Level(Resources._0),
				new Level(Resources._1),
				new Level(Resources._2),
				new Level(Resources._3)
			};
			p.state = DIFFICULTY;
			p.OnDifficultyChanged();
			p.StartThreads();
			Application.Run(p);
			p.FixScale();
		}
	}
}

// TODO victory/goals
// TODO bosses
// TODO multiple levels
