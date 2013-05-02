using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using GraphicsMode = OpenTK.Graphics.GraphicsMode;
using Color4 = OpenTK.Graphics.Color4;
using System.Threading;
using NachoMark.OpenGL;
using NachoMark.IO;
using NachoMark.Math;

namespace NachoMark
{
	class SpellieVenster : OpenTK.GameWindow
    {
        ValueSet config;

        public SpellieVenster(ValueSet config)
            : base(1600, 900, GraphicsMode.Default, "Spellie", GameWindowFlags.Default)
        {
            this.config = config;

            SetGraphics();

            snakeCount = config.TryGetInt("nsnakes", 10);
            fov = config.TryGetFloat("fov", 1.1f);
            elemCount = config.TryGetInt("nelem", 300);

            SetGraphicsBuffer();

            InitThreads();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0f, 0f, 0f, 0f);

            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);

            LoadSnakes();
            StartThreads();
        }
        
        bool stopped;

        #region View
        Camera Camera = new Camera(); 
        float fov;
        public static float ratio;
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Camera.Initialize(Width, Height, fov);
        }
        #endregion

        #region Render
        Vertex[] GraphicsBuffer;
        int GraphicsBufferPosition;

        /// <summary>
        /// Enable/disable vsync, set window boundaries
        /// and calculate aspect ratio
        /// </summary>
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

        /// <summary>
        /// Construct a new Vertex array for all graphics.
        /// </summary>
        void SetGraphicsBuffer()
        {
            GraphicsBuffer = new Vertex[snakeCount * (elemCount + 2) * 3];
            GraphicsBufferPosition = 0;
        }

        VertexBuffer vbo = new VertexBuffer();
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Camera.Update();

            vbo.SetData(GraphicsBuffer);
            vbo.Render();

            SwapBuffers();
        }
        #endregion Render
                
        #region Snakes
        int snakeCount, elemCount;
        List<Snake> snakes = new List<Snake>();

        FloatBounce
            RAdd = new FloatBounce(0.0f, 1.0f, 0.005f),
            GAdd = new FloatBounce(0.0f, 1.0f, 0.005f),
            BAdd = new FloatBounce(0.0f, 1.0f, 0.005f);
        	
        void LoadSnakes()
        {
            for(int i = 0; i <= snakeCount; i++)
                snakes.Add(new Snake(config, GraphicsBuffer, ref GraphicsBufferPosition));
        }

        void UpdateSnakes(object offset)
        {
            int p = (int)offset;

            while (!stopped)
            {
                smph[p].WaitOne();
                for (int i = (int)offset; i < snakeCount; i += threads)
                    snakes[i].Update(RAdd.Current, GAdd.Current, BAdd.Current);

                done[p] = true;
            }
        }
        #endregion

        #region Threads
        int threads;
        Semaphore[] smph;
        bool[] done;        
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
        void StartThreads()
        {
            for (int i = 0; i < threads; i++)
                (new Thread(UpdateSnakes)).Start(i);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            RAdd.Update(); GAdd.Update(); BAdd.Update();

            for (int i = 0; i < threads; i++)
                if (done[i])
                {
                    done[i] = false;
                    smph[i].Release();
                }

            if (Keyboard[Key.Escape])
            {
                stopped = true;
                Exit();
            }
        }
        #endregion
	}
}
