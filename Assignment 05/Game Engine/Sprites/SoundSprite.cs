using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprites {
	class SoundSprite:Sprite {

		private string fname;

		public SoundSprite(string fname) : base() {
			this.fname = fname;
		}

		public void PlaySound() {
			new SoundPlayer(fname).Play();
		}
	}
}
