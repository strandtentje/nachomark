using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Runtime.InteropServices;

namespace NachoMark.OpenGL
{
    /// <summary>
    /// A vertex for the buffer consisting of 
    /// a colour structure and a vector3 position structure.
    /// </summary>
	public struct Vertex
	{
	    public OpenTK.Graphics.Color4 Color;
		public Vector3 Position;

	    public static readonly int Stride = Marshal.SizeOf(default(Vertex));
	}
	 
	/// <summary>
	/// Vertex Buffer of all vertices to be loaded to 
    /// the gpu in one easy gesture.
	/// </summary>
    sealed class VertexBuffer
	{
	    int id;
	 
        /// <summary>
        /// Get the buffer ID to use.
        /// </summary>
	    int Id
	    {
	        get 
	        {
	            // Create an id on first use.
	            if (id == 0)
	            {
	                OpenTK.Graphics.GraphicsContext.Assert();
	 
	                GL.GenBuffers(1, out id);
	                if (id == 0)
	                    throw new Exception("Could not create VBO.");
	            }
	 
	            return id;
	        }
	    }	 

		Vertex[] data;
	 
        /// <summary>
        /// Put data in opengl buffer.
        /// </summary>
        /// <param name="data">Data</param>
	    public void SetData(Vertex[] data)
	    {
	        if (data == null)
	            throw new ArgumentNullException("data");
	 
			this.data = data;

	        GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
	        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(data.Length * Vertex.Stride), data, BufferUsageHint.StaticDraw);
	    }
	 
        /// <summary>
        /// Draw data in buffer.
        /// </summary>
	    public void Render()
	    {
			GL.EnableClientState(ArrayCap.ColorArray);
			GL.EnableClientState(ArrayCap.VertexArray);

	        GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
			GL.ColorPointer(4, ColorPointerType.Float, Vertex.Stride, 0);
			GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, Marshal.SizeOf(default(OpenTK.Graphics.Color4)));
			GL.DrawArrays(BeginMode.Triangles, 0, data.Length);
	    }
	}
}

