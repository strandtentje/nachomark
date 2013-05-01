using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using NachoMark.OpenGL;
using NachoMark.Math;

namespace NachoMark
{
    /// <summary>
    /// An entity chasing a target by means of PID control,
    /// which is visualised as a triangle with a hue.
    /// </summary>
	public class Entity
	{
        public float 
            X = 0.0f, Y = 0.0f, Z = 4.0f,
            VX = 0.0f, VY = 0.0f, VZ = 0.0f,
			Scale = 0.3f, Rotate = 0.0f,
		    Friction = 0.998f;

        PID myPID;
        public PID PIDSetting
        {
            get
            {
                return myPID;
            }
            set
            {
                myPID = value;

                XApproach = myPID.Clone();
                YApproach = myPID.Clone();
                ZApproach = myPID.Clone();
            }
        }

        float Cos, Sin;

        public int Colour = 255;

        /// <summary>
        /// Target entity.
        /// </summary>
        public Entity Target = null;

        PID XApproach, YApproach, ZApproach;

        Triangle MyGraphic;
        Colourization MyHue;

        public Entity()
        {

        }

        /// <summary>
        /// Construct a new entity.
        /// </summary>
        /// <param name="GraphicsBuffer">The graphics buffer where 
        /// the entity is displayed.</param>
        /// <param name="GraphicsBufferPosition">The position in
        /// the buffer where the entity is displayed.</param>
		public Entity (Vertex[] GraphicsBuffer, ref int GraphicsBufferPosition)
		{
            PIDSetting = new PID();
            
            MyGraphic = new Triangle(GraphicsBuffer, GraphicsBufferPosition);
            MyHue = new Colourization(GraphicsBuffer, GraphicsBufferPosition);
            GraphicsBufferPosition += 3;
		}

        
        /// <summary>
        /// Have the entity approach the target.
        /// </summary>
		public void UpdateEntity()
		{
            if (Target != null) UpdatePID();

            VX *= Friction;
            VY *= Friction;
            VZ *= Friction;

            Rotate = (float)System.Math.Atan2(VY, VX);
            X += VX; Y += VY; Z += VZ;
		}

        void UpdatePID()
        {
            VX += XApproach.GetAcceleration(X, Target.X);
            VY += YApproach.GetAcceleration(Y, Target.Y);
            VZ += ZApproach.GetAcceleration(Z, Target.Z);
        }

        /// <summary>
        /// Display the new position of the entity.
        /// </summary>
        public void UpdateVertices()
        {
            Sin = (float)System.Math.Sin(Rotate);
            Cos = (float)System.Math.Cos(Rotate);

            MyGraphic.Update(X, Y, Sin, Cos, Scale, Z);
        }

        /// <summary>
        /// Display the new colour of the entity
        /// </summary>
        public void UpdateColour(float RedMod, float GreenMod, float BlueMod)
        {
            MyHue.Update(
                Colour, (20f - Z) / 20f, 
                RedMod, GreenMod, BlueMod);
        }

        public void UpdateColour()
        {
            UpdateColour(
                Target.MyHue.RedAdd,
                Target.MyHue.GreenAdd,
                Target.MyHue.BlueAdd);
        }

	}
}

