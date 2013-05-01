using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace NachoMark.OpenGL
{
	public class Camera
	{
		public Vector3 camera = Vector3.Zero;
		public Vector3 subject = Vector3.UnitZ;
		public Vector3 above = Vector3.UnitY;

		public void MoveHorizontally (bool left, bool right, bool faster)
		{
			MoveHorizontally(
				((left ? 0.15f : 0f) + 
			 	 (right ? -0.15f : 0f)) * 
				(faster ? 3.0f : 0.5f));
		}

		public void MoveHorizontally(float delta)
		{
			camera.X += delta;
			subject.X += delta;
		}

		public void Update()
		{			
			Matrix4 modelview = Matrix4.LookAt (camera, subject, above);
			GL.MatrixMode (MatrixMode.Modelview);
			GL.LoadMatrix (ref modelview);
		}

        public void Initialize(int Width, int Height, float fov)
        {
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)System.Math.PI / fov, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }
    }
}

