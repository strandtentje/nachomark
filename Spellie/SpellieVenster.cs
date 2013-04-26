using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using GraphicsMode = OpenTK.Graphics.GraphicsMode;

namespace Spellie
{
	class SpellieVenster : OpenTK.GameWindow
	{
		public SpellieVenster (int w, int h, GraphicsMode g, string t, GameWindowFlags f)
			: base(1600, 900, GraphicsMode.Default, "Spellie", GameWindowFlags.Fullscreen)
		{

		}

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);

			Dictionary<string, Model> Models = new Dictionary<string, Model> ();

			CSV.Read("model/index", delegate(string[] parts)
	        {				
				Models.Add (parts [0], new Model (parts[0], parts [1]));
			});

			CSV.Read("level", delegate(string[] parts)
	        {				
				Entiteiten.Add (new Entity(
					Model: Models[parts[0]], 
					X: float.Parse(parts[1]), Y: float.Parse(parts[2]), Z: float.Parse(parts[3]),
					Scale: float.Parse(parts[4]), Rotate: float.Parse(parts[5]),
					Dark: parts[6] == "dark"));
			});

			GL.ClearColor(0f, 0f, 0f, 0f);
			GL.Enable(EnableCap.DepthTest);

			Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(toets);
			Mouse.Move += new EventHandler<MouseMoveEventArgs>(pijl);
		}

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

		int cEnt = 0;
		List<Entity> Entiteiten = new List<Entity>();

		bool shift;

		protected void toets (object s, KeyboardKeyEventArgs e)
		{
			base.OnKeyUp (e);

			switch (e.Key) {
				case Key.Tab:
					selEnt(cEnt + (shift ? 1 : -1)); break;
				case Key.F1:
					addEnt(); break;
				case Key.F2:
					rmEnt(); break;
				case Key.F3:
					saveAll(); break;
				case Key.I:
					invEnt(); break;
				default: 
					break;
			}
		}

		Vector2 muis;

		void pijl (object s, MouseMoveEventArgs e)
		{


			muis.X += (float)e.XDelta / -500f;
			muis.Y += (float)e.YDelta / -500f;
		}

		void saveAll ()
		{
			CSV.Clear("level");

			List<string> parts = new List<string>();

			foreach (Entity ent in Entiteiten) {
				parts.Clear();
				parts.Add(ent.Model.Name); 
				parts.Add(ent.X.ToString()); parts.Add(ent.Y.ToString()); parts.Add(ent.Z.ToString());
				parts.Add(ent.Scale.ToString()); parts.Add(ent.Rotate.ToString());
				parts.Add((ent.Dark ? "dark" : "light"));
				CSV.Append("level", parts.ToArray());
			}
		}

		#region entops
		void invEnt ()
		{
			if (Entiteiten.Count == 0) return;

			Entiteiten[cEnt].Dark = !Entiteiten[cEnt].Dark;
		}

		void moveEnt(float x, float y, float angle, float size, float depth)
		{
			if (Entiteiten.Count == 0) return;

			Entity e = Entiteiten[cEnt];

			e.X += x; e.Y += y; e.Z += depth;
			e.Scale += size; e.Rotate += angle;
		}

		void rmEnt()
		{
			if (Entiteiten.Count == 0) return;
			Entiteiten.RemoveAt(cEnt);
			selEnt(cEnt - 1);
		}

		void addEnt ()
		{
			Entity baseEntity = Entiteiten[cEnt];

			Entity ent = new Entity(
				baseEntity.Model, 
				baseEntity.X, baseEntity.Y, baseEntity.Z,
				baseEntity.Scale, baseEntity.Rotate, baseEntity.Dark);

			Entiteiten.Add(ent);

			selEnt(Entiteiten.Count - 1);
		}

		void selEnt (int entNum)
		{
			if (Entiteiten.Count == 0) return;

			if (cEnt < Entiteiten.Count) 
				Entiteiten[cEnt].Select = false;

			cEnt = entNum;

			if (entNum >= Entiteiten.Count) 	
				cEnt = 0;
			else 
				if (entNum < 0) 
					cEnt = Entiteiten.Count - 1;					

			Entiteiten[cEnt].Select = true;
		}
		#endregion

		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);
			
			shift = Keyboard[Key.ShiftLeft];

			moveEnt (
				(Keyboard [Key.A] ? 0.05f : 0f) + (Keyboard [Key.D] ? -0.05f : 0f),
				(Keyboard [Key.W] ? 0.05f : 0f) + (Keyboard [Key.S] ? -0.05f : 0f), 
				(Keyboard [Key.Left] ? 0.01f : 0f) + (Keyboard [Key.Right] ? -0.01f : 0f), 
				(Keyboard [Key.Q] ? -0.05f : 0f) + (Keyboard [Key.E] ? 0.05f : 0f), 
				(Keyboard [Key.Up] ? 0.05f : 0f) + (Keyboard [Key.Down] ? -0.05f : 0f));

			if (Keyboard [Key.Number9]) {
				camera.X -= (shift ? 0.15f : 0.05f);
				subject.X -= (shift ? 0.15f : 0.05f);
			}
			if (Keyboard [Key.Number0]) {
				camera.X += (shift ? 0.15f : 0.05f);
				subject.X += (shift ? 0.15f : 0.05f);
			}

			foreach(Entity ent in Entiteiten)
				ent.UpdatePoints();

			if (Keyboard[Key.Escape]) Exit();
		}

		Vector3 camera = Vector3.Zero;
		Vector3 subject = Vector3.UnitZ;

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);
			
			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Matrix4 modelview = Matrix4.LookAt (camera, subject, Vector3.UnitY);

			GL.MatrixMode (MatrixMode.Modelview);
			GL.LoadMatrix (ref modelview);

			foreach(Entity ent in Entiteiten) if (ent.Points != null)
			{
				if (ent.Dark)
					GL.Color3((ent.Select ? 1f : 0f), 0f, 0f);
				else 
					GL.Color3((ent.Select ? 0f : 1f), 1f, 1f);

				GL.Begin(BeginMode.Polygon);
				foreach(Vector3 v in ent.Points)
					GL.Vertex3(v);
				GL.End();
			}

			GL.Color3(0f, 1f, 0f);

			GL.Begin(BeginMode.Triangles);
			GL.Vertex3(muis.X, muis.Y, 2.0f);
			GL.Vertex3(muis.X + 0.1f, muis.Y, 2.0f);
			GL.Vertex3(muis.X, muis.Y + 0.1f, 2.0f);
			GL.End();

			SwapBuffers();
		}

	}
}
