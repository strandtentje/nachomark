using System;
using System.Collections.Generic;
using OpenTK;

namespace Spellie
{
	public class Entity
	{
		public Entity (
			Model Model, 
			float X, float Y, float Z,
			float Scale, float Rotate, bool Dark)
		{
			this.Model = Model;
			this.X = X; this.Y = Y; this.Z = Z;
			this.Scale = Scale; this.Rotate = Rotate;
			this.Dark = Dark;
		}

		public Vector3[] Points { get; private set; }

		public void UpdatePoints()
		{
			Points = Model.GetPoints (X, Y, Sin, Cos, Scale, Z);
		}

		public Model Model {  get; private set; }

		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
		public float Scale { get; set; }

		float myRotate;
		public float Rotate { 
			get {
				return myRotate;
			}
			set {
				myRotate = value;

				Sin = (float)Math.Sin(myRotate);
				Cos = (float)Math.Cos(myRotate);
			}
		}

		public float Sin { get; private set; }
		public float Cos { get; private set; }

		public bool Dark { get; set; }
		public bool Select { get; set; }
	}
}

