using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprites {
	public class GoalSprite:CollisionSprite {

		private List<CollectableSprite> inventory;

		public List<CollectableSprite> Inventory {
			get { return inventory; }
		}

		public int FlagCount {
			get { return inventory.Count; }
		}

		public GoalSprite(Image img, float x, float y, uint mask) : base(img, x, y, mask) {
			inventory = new List<CollectableSprite>();
		}

		public override void OnCollide(int dir, CollisionSprite cs) {
			base.OnCollide(dir, cs);
			if(cs is CharacterSprite) {
				CharacterSprite c = (CharacterSprite)cs;
				List<CollectableSprite> cinv = c.Inventory;
				for(int i = 0; i<cinv.Count; i++) {
					CollectableSprite s = cinv[i];
					if(s.ID == "flag") {
						c.children.Remove(s);
						Add(s);
						inventory.Add(s);
						cinv.Remove(s);
						UpdateInventoryPositions();
						i--;
					}
				}
				c.UpdateInventoryPositions();
			}
		}

		private void UpdateInventoryPositions() {
			float o = 0;
			int yLevel = 1;
			for(int i = 0; i < inventory.Count; i++) {
				CollectableSprite c = inventory[i];
				c.X = Width - c.Width - o;
				c.Y = -c.Height * yLevel - 17;
				while(c.X < 0) {
					c.X += Width;
					c.Y -= c.Height;
					yLevel++;
					o = 0;
				}
				c.Velocity.Y = 0;
				o += c.Width;
			}
		}
	}
}
