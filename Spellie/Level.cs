using System;
using System.Collections.Generic;
using Color4 = OpenTK.Graphics.Color4;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Spellie
{
	/// <summary>
	/// Level with entities (triangles)
	/// </summary>
	public class Level : List<Entity>
	{
		Dictionary<string, Entity> internalReference = new Dictionary<string, Entity>();

		Random rnd = new Random();
		float rand ()
		{
			return (float)rnd.NextDouble();
		}

		public Level (int cBase)
		{
			Entity e = new Entity (0f, 0f, 0f, null, 0.0001f, 0f, 0.001f, 0.001f, 0.3f, 0f, 0, 1f, 0.989f, "main");
			this.Add (e);

			for (int i = 0; i < 80; i++) {
				this.Add(new Entity(
					rand() * 2 - 1, rand() * 2 - 1, 4f, 
					this[(int)(i * rand())], 0.001f + rand() * 0.005f, 0.001f + rand() * 0.005f, 0.001f + rand() * 0.005f, 0.001f,
					0.1f + rand () * 0.3f, 0f, cBase, 0f, 0.99f - rand() * 0.1f, "toet"));
			}
		}

		public void DoPathfinding ()
		{
			foreach(Entity ent in this)
				ent.ApproachTarget();
		}

		public void UpdatePoints()
		{
			foreach(Entity ent in this)
				ent.UpdatePoints();
		}

		Vector3 nearer = new Vector3(0,0,-0.05f);

		public Vertex[] Draw (float r, float g, float b)
		{			
			Entity ent;

			Vertex[] vti = new Vertex[this.Count * 3];

			for(int i = 0; i < this.Count; i++)
			{
				ent = this[i];

				if (ent.Points != null) 
				{	
					float m = (20f - ent.Z) / 20f ;

					Color4 c = ent.GetRealColor() ;

					for(int j = 0; j < 3; j++)
					{
						vti[i * 3 + j].Color.A = 1.0f;
						vti[i * 3 + j].Color.R = (c.R + r) * m;
						vti[i * 3 + j].Color.G = (c.G + g) * m;
						vti[i * 3 + j].Color.B = (c.B + b) * m;

						vti[i * 3 + j].Position = ent.Points[j];
					}
				}
			}

			return vti;
		}
	}
}

