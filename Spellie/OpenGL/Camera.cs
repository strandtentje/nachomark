using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace NachoMark.OpenGL
{
    /// <summary>
    /// Camera openGL
    /// </summary>
	public class Camera
	{
		public Vector3 camera = Vector3.Zero;
		public Vector3 subject = Vector3.UnitZ;
		public Vector3 above = Vector3.UnitY;

        /// <summary>
        /// Move the camera
        /// </summary>
        /// <param name="left">Move the camera to the left</param>
        /// <param name="right">Move the camera to the right</param>
        /// <param name="faster">Move it faster</param>
		public void MoveHorizontally (bool left, bool right, bool faster)
		{
			MoveHorizontally(
				((left ? 0.15f : 0f) + 
			 	 (right ? -0.15f : 0f)) * 
				(faster ? 3.0f : 0.5f));
		}

        /// <summary>
        /// Move the camera horizontally
        /// </summary>
        /// <param name="delta">Rate at which to move</param>
		public void MoveHorizontally(float delta)
		{
			camera.X += delta;
			subject.X += delta;
		}

        /// <summary>
        /// Update the camera in openGL
        /// </summary>
		public void Update()
		{			
			Matrix4 modelview = Matrix4.LookAt (camera, subject, above);
			GL.MatrixMode (MatrixMode.Modelview);
			GL.LoadMatrix (ref modelview);
		}

        /// <summary>
        /// Initialize a camera.
        /// </summary>
        /// <param name="Width">Width of viewport</param>
        /// <param name="Height">Height of viewport</param>
        /// <param name="fov">Field of view</param>
        public void Initialize(int Width, int Height, float fov)
        {
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)System.Math.PI / fov, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }
    }
}

