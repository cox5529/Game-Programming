using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	class SoundSprite:Sprite {

		private SoundPlayer player;

		public SoundSprite(String fname) : base() {
			player = new SoundPlayer(fname);
		}

		public void PlaySound() {
			player.Play();
		}
	}
}
