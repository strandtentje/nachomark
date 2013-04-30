using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Color4 = OpenTK.Graphics.Color4;

namespace NachoMark
{
	public class Snake : List<Entity>
	{		
		static Random rnd = new Random();
		float rand ()
		{
			return (float)rnd.NextDouble();
		}

        Entity Bait = new Entity();
        int baitRunaway, baitCountdown;
        float offCenterBait, amplitudeBait;
        float nearBait, farBait;
                
        public Snake(C.ValueSet Settings, Vertex[] GraphicsBuffer, ref int GraphicsBufferPosition)
        {
            offCenterBait = Settings.TryGetFloat("center", 20f);
            amplitudeBait = Settings.TryGetFloat("ampli", 40f);
            nearBait = Settings.TryGetFloat("near", -1f);
            farBait = Settings.TryGetFloat("far", 17f);
            baitRunaway = Settings.TryGetInt("targetinterval", 120);

            this.Add(
                new Entity(GraphicsBuffer, ref GraphicsBufferPosition)
                {
                    PIDSetting = new PID()
                    {
                        ProportionalGain = 0.0001f,
                        IntegralGain = 0f,
                        DifferentialGain = 0.001f,
                        IntegralCap = 0.001f,
                    },
                    Friction = 0.989f,
                    Scale = 0.0f,
                }
                );

            int amountOfElements = Settings.TryGetInt("nelem", 300);
            float
                minimalSize = Settings.TryGetFloat("smallest", 0.5f),
                additionalSize = Settings.TryGetFloat("extra", 0.8f);

            int snakeColour = (int)(rand() * 16f);

            for (int i = 0; i < amountOfElements; i++)
                this.Add(new Entity(GraphicsBuffer, ref GraphicsBufferPosition)
                {
                    X = rand() * 8 - 4, Y = rand() * 8 - 4, Z = 4f,
                    Target = this[(int)(i * rand())],
                    PIDSetting = new PID()
                    {
                        ProportionalGain = 0.001f + rand() * 0.005f,
                        IntegralGain = 0.001f + rand() * 0.005f,
                        DifferentialGain = 0.001f + rand() * 0.005f,
                        IntegralCap = 0.001f,
                    },
                    Scale = minimalSize + rand() * additionalSize,
                    Colour = snakeColour, Friction = 0.99f - rand() * 0.1f,
                });
            
            this[0].Target = Bait;
        }
        		
        public void Update (float r, float g, float b)
		{
           baitCountdown++;

            if (baitCountdown == baitRunaway)
            {
                Bait.X = (offCenterBait + (float)rnd.NextDouble() * amplitudeBait * SpellieVenster.ratio) * (rnd.Next(2) == 1 ? -1 : 1);
                Bait.Y = (offCenterBait + (float)rnd.NextDouble() * amplitudeBait) * (rnd.Next(2) == 1 ? -1 : 1);
                Bait.Z = (nearBait + (float)rnd.NextDouble() * farBait);
                baitCountdown = 0;
            }	

            foreach (Entity I in this)
            {
                I.UpdateEntity();
                I.UpdateVertices();
                I.UpdateColour(r, g, b);
            }
		}
	}
}

