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

			for (int i = 0; i < 250; i++) {
				this.Add(new Entity(
					rand() * 2 - 1, rand() * 2 - 1, 4f, 
					this[(int)(i * rand())], 0.001f + rand() * 0.005f, 0.001f + rand() * 0.005f, 0.001f + rand() * 0.005f, 0.001f,
					0.1f + rand () * 0.3f, 0f, cBase, 0f, 0.99f - rand() * 0.1f, "toet"));
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spellie.Level"/> class.
		/// </summary>
		/// <param name='fileName'>
		/// File name of the file to load the level out of.
		/// </param>
		public Level (string fileName)
		{
			C.Read(fileName, delegate(C.ValueSet v)
        	{	
				Entity e = new Entity(name: v.Name);
				e.X = v.TryGetFloat("x", e.X);
				e.Y = v.TryGetFloat("y", e.Y);
				e.Z = v.TryGetFloat("z", e.Z);

				try
				{
					e.Target = internalReference[v["target"]];
				}
				catch
				{

				}

				e.ProportionalGain = v.TryGetFloat("p", e.ProportionalGain);
				e.IntegralGain = v.TryGetFloat("i", e.IntegralGain);
				e.DifferentialGain = v.TryGetFloat("d", e.DifferentialGain);
				e.IntegralCap = v.TryGetFloat("c", e.IntegralCap);
				e.Scale = v.TryGetFloat("size", e.Scale);
				e.Rotate = v.TryGetFloat("rotate", e.Rotate);
				e.Color = v.TryGetInt("color", e.Color);
				e.Gentleness = v.TryGetFloat("gentle", e.Gentleness);
				e.Friction = v.TryGetFloat("frict", e.Friction);

				internalReference.Add(e.Name, e);
				this.Add(e);
			});
		}

		public void Save (string file)
		{			
			C.Clear (file);

			foreach (Entity e in this) {
				C.ValueSet vSet = new C.ValueSet();
				vSet.Name = e.Name;
				vSet.Set("x", e.X);
				vSet.Set("y", e.Y);
				vSet.Set("z", e.Z);
				if (e.Target != null) vSet.Set("target", e.Target.Name);
				vSet.Set("p", e.ProportionalGain);
				vSet.Set("i", e.IntegralGain);
				vSet.Set("d", e.DifferentialGain);
				vSet.Set("c", e.IntegralCap);
				vSet.Set("size", e.Scale);
				vSet.Set("rotate", e.Rotate);
				vSet.Set("color", e.Color);
				vSet.Set("gentle", e.Gentleness);
				vSet.Set("frict", e.Friction);

				C.Append(file,  vSet);
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

		public void Draw (float r, float g, float b)
		{			
			GL.Begin(BeginMode.Triangles);
			foreach (Entity ent in this) {
				if (ent.Points != null) 
				{	
					Color4 c = ent.GetRealColor() ;

					float m = (16f - ent.Z) / 16f ;

					GL.Color3(
						(c.R + r) * m,
						(c.G + g) * m,
						(c.B + b) * m);


					foreach (Vector3 v in ent.Points)
						GL.Vertex3 (v);

				}
			}
			GL.End();
		}
	}
}

