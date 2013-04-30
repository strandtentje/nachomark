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
        
		bool follow; float fov;
		public static float ratio;

		int snakeCount, elemCount;
        
        int threads;
		
        Semaphore[] smph;
        bool[] done;

        Vertex[] GraphicsBuffer;
        int GraphicsBufferPosition;

		public SpellieVenster (C.ValueSet config)
			: base(1600, 900, GraphicsMode.Default, "Spellie", GameWindowFlags.Default)
		{
			this.config = config;

            SetGraphics();
            
			follow = config.TryGetInt ("follow", 1) == 1;
			snakeCount = config.TryGetInt ("nsnakes", 10);			
            fov = config.TryGetFloat ("fov", 1.1f);
            elemCount = config.TryGetInt("nelem", 300);

            SetGraphicsBuffer();

            InitThreads();
		}

        void SetGraphics()
        {
            VSync = (config.TryGetInt("vsync", 0) == 1 ? VSyncMode.On : VSyncMode.Off);
            this.WindowBorder = OpenTK.WindowBorder.Hidden;
            this.Location =
                new System.Drawing.Point(
                    config.TryGetInt("left", 0),
                    config.TryGetInt("top", 0));

            this.Size =
                new System.Drawing.Size(
                    config.TryGetInt("width", 800),
                    config.TryGetInt("height", 600));

            ratio = (float)(this.Width) / (float)(this.Height);
        }

        void SetGraphicsBuffer()
        {
            GraphicsBuffer = new Vertex[snakeCount * (elemCount + 2) * 3];
            GraphicsBufferPosition = 0;
        }

        void InitThreads()
        {
            threads = config.TryGetInt("threads", 2);

            done = new bool[threads];
            smph = new Semaphore[threads];

            for (int i = 0; i < threads; i++)
            {
                smph[i] = new Semaphore(0, 1);
                done[i] = true;
            }
        }

        List<Snake> snakes = new List<Snake>();
        
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);

			GL.ClearColor(0f, 0f, 0f, 0f);

			GL.Enable(EnableCap.AlphaTest);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.DepthTest);

            LoadSnakes();
            StartThreads();
		}

        void LoadSnakes()
        {
            for(int i = 0; i <= snakeCount; i++)
                snakes.Add(new Snake(config, GraphicsBuffer, ref GraphicsBufferPosition));
        }

        void StartThreads()
        {
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
                    snakes[i].Update(ar, ag, ab);
                
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

		VertexBuffer vbo = new VertexBuffer();
		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
					
			if (false) {
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

            vbo.SetData(GraphicsBuffer);
			vbo.Render();

			SwapBuffers();
		}
	}
}
