using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	class LeviSprite:SlideSprite {
		private SoundPlayer player;
		private bool played;

		public float TargetLeviX {
			get { return TargetX; }
			set {
				played = false;
				TargetX = value;
			}
		}

		public float TargetLeviY {
			get { return TargetY; }
			set {
				played = false;
				TargetY = value;
			}
		}

		public LeviSprite(Stream str, Image img) : base(img) {
			player = new SoundPlayer(str);
		}

		public void PlaySound() {
			player.Play();
		}

		public override void Act() {
			base.Act();
			if(Math.Abs(X - TargetX) > 5 || Math.Abs(Y - TargetY) > 5) {
				if(!played) {
					PlaySound();
					played = true;
				}
			} else
				played = false;
		}
	}
}
