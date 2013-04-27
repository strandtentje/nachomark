using System;
using OpenTK.Graphics;
using OpenTK;
using System.Runtime.InteropServices;

namespace Spellie
{
	// Change this struct to add e.g. color data or anything else you need.
	struct Vertex
	{
	    public Vector3 Position;
		public Color4 Color;

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
	                GraphicsContext.Assert();
	 
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
	        /* GL.EnableClientState(EnableCap.VertexArray);
	        GL.EnableClientState(EnableCap.NormalArray);
	        GL.EnableClientState(EnableCap.TextureCoordArray);
	 
	        GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
	        GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, new IntPtr(0));
	        GL.NormalPointer(3, NormalPointerType.Float, Vertex.Stride, new IntPtr(Vector3.SizeInBytes));
	        GL.TexCoordPointer(2, VertexPointerType.Float, Vertex.Stride, new IntPtr(2 * Vector3.SizeInBytes));
	        GL.DrawArrays(BeginMode.Triangles, 0, data.Length);
	   */ }
	}
}

