using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using GraphicsMode = OpenTK.Graphics.GraphicsMode;
using Color4 = OpenTK.Graphics.Color4;

namespace Spellie
{
	class SpellieVenster : OpenTK.GameWindow
	{
		Camera Camera = new Camera();

		public SpellieVenster (int w, int h, GraphicsMode g, string t, GameWindowFlags f)
			: base(1600, 900, GraphicsMode.Default, "Spellie", GameWindowFlags.Fullscreen)
		{

		}

		float gameSpeed = -0.4f;
		int cEnt;

		List<Snake> snakes = new List<Snake>();

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);

			for(int i = 1; i < 50; i++)
				snakes.Add(new Snake(i % 15));
		
			GL.ClearColor(0f, 0f, 0f, 0f);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.AlphaTest);

		}

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }
	
		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);

			Camera.MoveHorizontally(Keyboard[Key.Number9], Keyboard[Key.Number0], Keyboard[Key.LShift]);

			foreach(Snake s in snakes)
				s.Update();

			if (Keyboard[Key.Escape]) Exit();
		}

		float ar = 0.2f, ag = 0.5f, ab = 0.8f;
		float dr = 0.005f, dg = 0.005f, db = 0.005f;

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
					

			Camera.subject = new Vector3(
				snakes[0].Level[1].X,
				snakes[0].Level[1].Y,
				snakes[0].Level[1].Z);
			
			/* Camera.camera = new Vector3(
				snakes[1].Level[1].X,
				snakes[1].Level[1].Y,
				snakes[1].Level[1].Z);

			Camera.above = new Vector3(
				snakes[2].Level[1].X,
				snakes[2].Level[1].Y,
				snakes[2].Level[1].Z);*/

			Camera.Update();

			ar += dr; ag += dg; ab += db;

			if (ar > 1f) dr = -0.005f;
			if (ag > 1f) dg = -0.005f;
			if (ab > 1f) db = -0.005f;

			if (ar < 0f) dr = 0.005f;
			if (ag < 0f) dg = 0.005f;
			if (ab < 0f) db = 0.005f;

			foreach(Snake s in snakes)
				s.Render(ar, ag, ab);

			SwapBuffers();
		}

	}
}
