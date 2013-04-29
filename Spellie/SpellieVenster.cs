using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using GraphicsMode = OpenTK.Graphics.GraphicsMode;
using Color4 = OpenTK.Graphics.Color4;
using System.Threading;

namespace NachoMark
{
	class SpellieVenster : OpenTK.GameWindow
	{
		Camera Camera = new Camera();
		C.ValueSet config;

        public static bool getFPS;

		bool follow; float fov;
		int snakeCount, elemCount;

		float offCenter, amplitude;
		float near, far;
		float smallest, extra;

		int threads;
		int erval;

        int[] threadWork;

		public static float ratio;

        Semaphore[] smph;
        bool[] done;

		public SpellieVenster (C.ValueSet config)
			: base(1600, 900, GraphicsMode.Default, "Spellie", GameWindowFlags.Default)
		{
			this.config = config;

            VSync = (config.TryGetInt("vsync", 0) == 1 ? VSyncMode.On : VSyncMode.Off);

			if (true) {
				this.WindowBorder = OpenTK.WindowBorder.Hidden;
				this.Location = 
				new System.Drawing.Point (
					config.TryGetInt ("left", 0),
					config.TryGetInt ("top", 0));
				this.Size = 
				new System.Drawing.Size (
					config.TryGetInt ("width", 800), 
					config.TryGetInt ("height", 600));
			}

			ratio = (float)(this.Width) / (float)(this.Height);

			follow = config.TryGetInt ("follow", 1) == 1;
			snakeCount = config.TryGetInt ("nsnakes", 10);
			elemCount = config.TryGetInt ("nelem", 100);
			fov = config.TryGetFloat ("fov", 4.0f);

			offCenter = config.TryGetFloat ("center", 4.0f);
			amplitude = config.TryGetFloat ("ampli", 4.0f);
			near = config.TryGetFloat ("near", 1.0f);
			far = config.TryGetFloat ("far", 16f);
			smallest = config.TryGetFloat ("smallest", 0.1f);
			extra = config.TryGetFloat ("extra", 0.3f);

			erval = config.TryGetInt("targetinterval", 110);

			vti = new Vertex[snakeCount * (elemCount + 2) * 3];

			threads = config.TryGetInt ("threads", 2);

            done = new bool[threads];
            smph = new Semaphore[threads];

            for (int i = 0; i < threads; i++)
            {
                smph[i] = new Semaphore(0, 1);
                done[i] = true;
            }
                        
			//updateReady= new Semaphore(0, threads);
		}

        List<Snake> snakes = new List<Snake>();


		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);

			for(int i = 1; i <= snakeCount; i++)
				snakes.Add(new Snake(i % 15, elemCount, offCenter, amplitude, near, far, smallest, extra, erval));
		
			GL.ClearColor(0f, 0f, 0f, 0f);

			GL.Enable(EnableCap.AlphaTest);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.DepthTest);
            
            for (int i = 0; i < threads; i++)
                (new Thread(update)).Start(i);
		}

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / fov, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        bool stopped;


		void update (object offset)
		{
            int p = (int)offset;

            while (!stopped)
            {
                smph[p].WaitOne();
                for (int i = (int)offset; i < snakeCount; i += threads)
                {
                    snakes[i].Update();
                    snakes[i].Draw(ar, ag, ab, ref vti, i * (elemCount + 2) * 3);
                }
                done[p] = true ;
            }
		}

		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);

            for (int i = 0; i < threads; i++)
            {
                if (done[i])
                {
                    done[i] = false;
                    smph[i].Release();
                }
            }

			ColourFade ();

			if (Keyboard [Key.Escape]) {
                stopped = true;

				Exit ();
			}
		}

		void ColourFade()
		{
			
			ar += dr;
			ag += dg;
			ab += db;

			if (ar > 1f)
				dr = -0.005f;
			if (ag > 1f)
				dg = -0.005f;
			if (ab > 1f)
				db = -0.005f;

			if (ar < 0f)
				dr = 0.005f;
			if (ag < 0f)
				dg = 0.005f;
			if (ab < 0f)
				db = 0.005f;
		}

		float ar = 0.2f, ag = 0.5f, ab = 0.8f;
		float dr = 0.005f, dg = 0.005f, db = 0.005f;

		double pDate = DateTime.Now.TimeOfDay.TotalMilliseconds;
		double cDate ;

		double fps;

		double hFps, aFps;

		VertexBuffer vbo = new VertexBuffer();
		Vertex[] vti;

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
					
			if (follow) {
				Camera.subject.X = snakes [0] [1].X;
				Camera.subject.Y = snakes [0] [1].Y;
				Camera.subject.Z = snakes [0] [1].Z;
			}

			/* Camera.camera = new Vector3(
				snakes[1].Level[1].X,
				snakes[1].Level[1].Y,
				snakes[1].Level[1].Z);

			Camera.above = new Vector3(
				snakes[2].Level[1].X,
				snakes[2].Level[1].Y,
				snakes[2].Level[1].Z);*/

			Camera.Update ();

			vbo.SetData(vti);
			vbo.Render();

			SwapBuffers();
		}
	}
}
