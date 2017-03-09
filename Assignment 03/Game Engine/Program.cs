using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.Properties;

namespace Engine {
	public class Program:Engine {

		private static LeviSprite leviSprite;


		protected override void OnKeyDown(KeyEventArgs e) {
			//base.OnKeyDown(e);
			//Console.WriteLine("asdffasdf");
			if(e.KeyCode == Keys.Left)
				leviSprite.TargetLeviX -= 30;
			if(e.KeyCode == Keys.Right)
				leviSprite.TargetLeviX += 30;
			if(e.KeyCode == Keys.Up)
				leviSprite.TargetLeviY -= 30;
			if(e.KeyCode == Keys.Down)
				leviSprite.TargetLeviY += 30;
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			leviSprite = new LeviSprite(Resources.vroom, Resources.face1);
			leviSprite.TargetX = 100;
			leviSprite.TargetY = 100;
			leviSprite.Velocity = 50;
			Program.Canvas.Add(leviSprite);
			Application.Run(new Program());
		}
	}
}
