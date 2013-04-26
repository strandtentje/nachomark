using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Spellie
{
    static class GLTools
    {
        public static Dictionary<string, int> texs = new Dictionary<string, int>();
        
        /// <summary>
        /// Load image from file and return texture ID.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static int loadTexture(string filename)
        {
            if (texs.ContainsKey(filename))
                return texs[filename];

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap(filename);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            texs.Add(filename, id);

            return id;
        }

        public static int loadBitmap(string tag, Bitmap bmp)
        {
            if (texs.ContainsKey(tag))
                return texs[tag];

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            texs.Add(tag, id);

            return id;
        }

        public static int getTexture(string tag)
        {
            return texs[tag];
        }

        /// <summary>
        /// Draws 3D vertex with a 2D vector and a seperate depth.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        public static void Vertex2p5(Vector2 v, float d)
        {
            GL.Vertex3(-v.X, -v.Y, d);
        }

        /// <summary>
        /// Draws 3D vertex with two added 2D vectors and a seperate depth.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="o"></param>
        /// <param name="d"></param>
        public static void Vertex2p5(Vector2 v, Vector2 o, float d)
        {
            Vertex2p5(Vector2.Add(v,o), d);
        }
    }
}
