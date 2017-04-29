using Engine.Utilities;
using System.Drawing;

namespace Engine.Sprites {
	public class BlockSprite:PhysicsSprite {

		public BlockSprite(Image img, float x, float y) : base(img, x, y, 4294967295) {
			MotionModel = STATIC;
		}

		public BlockSprite(Image img, Polygon box) : this(img, 0, 0, box) { }

		public BlockSprite(Image img, float x, float y, Polygon box) : this(img, x, y, 4294967295, box, STATIC) { }

		public BlockSprite(Image img, float x, float y, uint mask, Polygon box, int motionModel) : base(img, x, y, mask, box, motionModel) { }
	}
}
