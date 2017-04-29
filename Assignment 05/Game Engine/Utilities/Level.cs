using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utilities {
	public class Level {

		private List<string> lore;
		private string level;
		private int flagCount;
		private Hashtable flags;

		public List<string> Lore {
			get {
				return lore;
			}
		}

		public string LevelText {
			get {
				return level;
			}

			set {
				level = value;
			}
		}

		public int FlagCount {
			get {
				return flagCount;
			}

			set {
				flagCount = value;
			}
		}

		public Hashtable Flags { get => flags; set => flags = value; }

		public Level(string s) {
			flags = new Hashtable();
			lore = new List<string>();
			level = "";
			string curLore = "";
			flagCount = 0;
			string[] lines = s.Split('\n');
			int state = 0;
			foreach(string line in lines) {
				if(line == "JASON_LY") {
					if(state == 2) {
						lore.Add(curLore);
						curLore = "";
					} else
						state++;
				} else if(state == 0) {
					level += line + "\n";
					foreach(char c in line) {
						if(c == 'F') {
							flagCount++;
						}
					}
				} else if(state == 2) {
					curLore += line + "\n";
				} else if(state == 1) {
					AddToFlags(line);
				}
			}
			if(!string.IsNullOrEmpty(curLore))
				lore.Add(curLore);
		}

		private void AddToFlags(string s) {
			string[] sep = s.Split(' ');
			for(int i = 0; i < sep.Length; i++) {
				if(sep[i] == "=" && i > 0 && i < sep.Length - 1) {
					flags.Add(sep[i - 1], sep[i + 1]);
				}
			}
		}
	}
}
