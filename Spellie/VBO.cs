using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Runtime.InteropServices;

namespace NachoMark
{
	// Change this struct to add e.g. color data or anything else you need.
	public struct Vertex
	{
	    public OpenTK.Graphics.Color4 Color;
		public Vector3 Position;

	    public static readonly int Stride = Marshal.SizeOf(default(Vertex));
	}
	 
	// As simple as it gets. Usage:
	// VertexBuffer vbo  = new VertexBuffer();
	// vbo.SetData(new Vertex[] { ... });
	// vbo.Render();
	// You can make it as fancy as you want (for example, I have an implementation using generics).
	sealed class VertexBuffer
	{
	    int id;
	 
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
	 
	    public VertexBuffer()
	    { }

		Vertex[] data;
	 
	    public void SetData(Vertex[] data)
	    {
	        if (data == null)
	            throw new ArgumentNullException("data");
	 
			this.data = data;

	        GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
	        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(data.Length * Vertex.Stride), data, BufferUsageHint.StaticDraw);
	    }
	 
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

