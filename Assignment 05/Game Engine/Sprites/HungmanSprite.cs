using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApplication1.Properties;

namespace Engine.Sprites
{
	public class HungmanSprite:Sprite {

		private static HungmanSprite hs;

		private float width;
		private float height;
		private bool isFinished;
		private Thread hungmanThread;
		private RectangleSprite rs;
		private TextSprite console;
		private TextFieldSprite ts;
		private Queue<string> input;

		private TextSprite Console {
			get { return console; }
		}

		private Queue<string> Queue {
			get { return input; }
		}

		public bool IsFinished { get => isFinished; set => isFinished = value; }

		public HungmanSprite(float width, float height) {
			this.width = width;
			this.height = height;
			rs = new RectangleSprite(0, 0, width, height);
			console = new TextSprite(10, 10, "");
			console.HorizontalAlignment = StringAlignment.Near;
			console.VerticalAlignment = StringAlignment.Near;
			ts = new TextFieldSprite(20, height * 0.8f, width - 40, 30)
			{
				HorizontalAlignment = StringAlignment.Near,
				VerticalAlignment = StringAlignment.Near
			};
			hs = this;
			input = new Queue<string>();
			hungmanThread = new Thread(new ThreadStart(PlayHungman));
			hungmanThread.Start();
		}

		public void Input(KeyEventArgs e) {
			if(e.KeyCode == Keys.Enter) {
				input.Enqueue(ts.Text);
				ts.Text = "";
			} else {
				ts.Input(e);
			}
		}

		public override void Paint(Graphics g) {
			base.Paint(g);
			rs.Render(g);
			console.Render(g);
			ts.Render(g);
		}

		public void KillThread() {
			if(hungmanThread.IsAlive) {
				hungmanThread.Abort();
			}
		}

		public static void PlayHungman() {
			hs.Console.Append("Welcome to HungMan C# 1.0!");
			hs.Console.Append("The goal is to guess the right word before you get 8 misses.");
			hs.Console.Append("If you choose more than one letter, I will only take the first one.");
			hs.Console.Append("\nPress enter to continue...");
			ReadLine();
			string word = GetRandomWord().ToLower();
			string usedLetters = "";
			int misses = 0;
			int err = 0;
			while(GetMaskedWord(word, usedLetters).Contains("_") && misses < 8) {
				hs.Console.Text = "";
				hs.Console.Append(GetMaskedWord(word, usedLetters));
				hs.Console.Append("\nUsed Letters: " + usedLetters);
				hs.Console.Append("Misses: " + misses + " / 8");
				if(err == 1)
					hs.Console.Append("You must guess a letter!");
				else if(err == 2)
					hs.Console.Append("You must guess a letter that you haven't guessed before!");
				else
					hs.Console.Append("Guess a letter!");
				string guess = ReadLine();
				if(guess.Length > 0) {
					guess = guess.Substring(0, 1).ToLower();
					if(usedLetters.Contains(guess)) {
						err = 2;
						continue;
					}
					err = 0;
					usedLetters += guess;
					if(!word.Contains(guess))
						misses++;
				} else {
					err = 1;
				}
			}
			hs.Console.Text = "";
			if(!GetMaskedWord(word, usedLetters).Contains("_")) {
				hs.Console.Append(GetMaskedWord(word, usedLetters));
				hs.Console.Append("Congratulations! You won!");
			} else {
				hs.Console.Append("You lose!");
				hs.Console.Append("The word was " + word + ".");
			}
			hs.Console.Append("\nPress enter to continue...");
			ReadLine();
			hs.IsFinished = true;
		}

		private static string ReadLine() {
			while(hs.Queue.Count == 0) {
				Thread.Sleep(1);
			}
			return hs.Queue.Dequeue();
		}

		private static string GetRandomWord() {
			string[] words = Resources.words.Replace("\"", "").Split(',');
			Random rnd = new Random();
			return words[rnd.Next(0, words.Length - 1)];
		}

		private static string GetMaskedWord(string word, string usedLetters) {
			string output = "";
			for(int i = 0; i < word.Length; i++) {
				string letter = word.Substring(i, 1);
				if(usedLetters.Contains(letter))
					output += letter + " ";
				else
					output += "_ ";
			}
			return output;
		}
	}
}
