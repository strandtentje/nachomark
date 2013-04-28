using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;

namespace Spellie
{
	public class Entity
	{
		public Entity (
			float X = 0.0f, float Y = 0.0f, float Z = 4.0f,
			Entity Target = null,
			float P = 0.1f, float I = 0.1f, float D = 0.1f, float C = 0.01f,
			float Scale = 0.3f, float Rotate = 0.0f, int Color = 255,
			float Gentleness = 20f, float Friction = 0.998f, string name = "lorem")
		{
			this.X = X; this.Y = Y; this.Z = Z;
			this.Scale = Scale; this.Rotate = Rotate;
			this.Color = Color;

			this.Target = Target;
			this.ProportionalGain = P;
			this.IntegralGain = I;
			this.DifferentialGain = D;
			this.IntegralCap = C;

			this.Gentleness = Gentleness;
			this.Friction = Friction;

			this.Name = name;

			UpdatePoints();
		}

		public Entity Clone ()
		{
			return new Entity(
				X, Y, Z, 
				Target,
				ProportionalGain, IntegralGain, DifferentialGain, IntegralCap, 
				Scale, Rotate, Color, Gentleness, Friction, Name + "a");
		}

		public Vector3[] Points { get; private set; }

		public string Name { get; private set; }

		public float ProportionalGain = 0.1f;
		public float IntegralGain = 0.1f;
		public float DifferentialGain = 0.1f;
		public float IntegralCap = 0.1f;		

		float pxError, pyError, pzError;
		float ixError, iyError, izError;
		float dxError, dyError, dzError;

		public float Gentleness = 20f;
		public float Friction = 0.95f;

		public static float PID (float current, float target,
		                       float pGain, float iGain, float dGain, float iCap,
		                       ref float propError, ref float inteError, ref float diffError)
		{
			propError = target - current;
			inteError += propError;
			inteError *= iCap;

			float ret =
				propError * pGain + inteError * iGain + (propError - diffError) * dGain;

			diffError = propError;

			return ret;
		}


		public void ApproachTarget ()
		{
			if (Target != null) {
				/*
				pxError = Target.X - X;
				pyError = Target.Y - Y;
				ixError += pxError;
				iyError += pyError;
				ixError *= IntegralCap;
				iyError *= IntegralCap;

				AX = 
				pxError * ProportionalGain + 
					ixError * IntegralGain +
					(pxError - dxError) * DifferentialGain;
				AY = 
				pyError * ProportionalGain + 
					iyError * IntegralGain + 
					(pyError - dyError) * DifferentialGain;

				dxError = pxError;
				dyError = pyError;

				VX += AX;
				VY += AY; */

				VX += PID (X, Target.X, ProportionalGain, IntegralGain, DifferentialGain, IntegralCap, ref pxError, ref ixError, ref dxError);
				VY += PID (Y, Target.Y, ProportionalGain, IntegralGain, DifferentialGain, IntegralCap, ref pyError, ref iyError, ref dyError);
				VZ += PID (Z, Target.Z, ProportionalGain, IntegralGain, DifferentialGain, IntegralCap, ref pzError, ref izError, ref dzError);

				VX *= Friction;
				VY *= Friction;
				VZ *= Friction;

				Rotate = /*(Rotate * (Gentleness -1)) +*/ (float)Math.Atan2 (VY, VX);
				//Rotate = nr / Gentleness;
			}

			X += VX; Y += VY; Z += VZ;
		}

		Triangle mine = new Triangle();

		public void UpdatePoints()
		{
			Points = mine.Get (X, Y, Sin, Cos, Scale, Z);
		}

		public float X, Y, Z, Scale;
		public float VX, VY, VZ;
		public float AX, AY;
		public Entity Target;

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

		public int Color { get; set; }
		public bool Select { get; set; }

		int pCol; Color4 npCol = new Color4(0f, 0f, 0f, 1f);

		public Color4 GetRealColor ()
		{
			if (pCol != Color) {
				pCol = Color;

				float red = ((Color & 4) != 0 ? 1.0f : 0f);
				float green = ((Color & 2) != 0 ? 1.0f : 0f);
				float blue = ((Color & 1) != 0 ? 1.0f : 0f);

				npCol.R = red;
				npCol.G = green;
				npCol.B = blue;
				npCol.A = 1.0f;
			}

			return npCol;
		}
	}
}

