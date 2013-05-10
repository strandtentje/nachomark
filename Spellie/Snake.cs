using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Color4 = OpenTK.Graphics.Color4;
using NachoMark.OpenGL;
using NachoMark.Math;
using NachoMark.IO;

namespace NachoMark
{
	public class Snake : List<Entity>
	{		
        Entity Bait = new Entity();
        int baitRunaway, baitCountdown;
        float offCenterBait, amplitudeBait;
        float nearBait, farBait;

        float PMin, PAmp;
        float IMin, IAmp;
        float DMin, DAmp;
        float ICap;
        float FMin, FAmp;
        float MinTarget, AmpTarget;

        int amountOfElements;
        float minimalSize, additionalSize;

        void LoadBaitSettings(ValueSet Settings)
        {
            offCenterBait = Settings.TryGetFloat("center", 20f);
            amplitudeBait = Settings.TryGetFloat("ampli", 40f);
            nearBait = Settings.TryGetFloat("near", -1f);
            farBait = Settings.TryGetFloat("far", 17f);
            baitRunaway = Settings.TryGetInt("targetinterval", 120);
        }

        void LoadPIDSettings(ValueSet Settings)
        {
            PMin = Settings.TryGetFloat("pmin", 0.003f);
            PAmp = Settings.TryGetFloat("pmax", 0.012f) - PMin;

            IMin = Settings.TryGetFloat("imin", 0.001f);
            IAmp = Settings.TryGetFloat("imax", 0.005f) - IMin;

            DMin = Settings.TryGetFloat("dmin", 0.012f);
            DAmp = Settings.TryGetFloat("dmax", 0.052f) - DMin;

            ICap = Settings.TryGetFloat("icap", 0.001f);

            FMin = Settings.TryGetFloat("maxslippery", 0.99f);
            FAmp = Settings.TryGetFloat("minslippery", 0.84f) - FMin;

            MinTarget = Settings.TryGetFloat("mintarget", 0.0f);
            AmpTarget = Settings.TryGetFloat("maxtarget", 1.0f) - MinTarget;
        }

        void LoadProportions(ValueSet Settings)
        {
            amountOfElements = Settings.TryGetInt("nelem", 300);
            minimalSize = Settings.TryGetFloat("smallest", 0.5f);
            additionalSize = Settings.TryGetFloat("extra", 0.8f);
        }

        public Snake(ValueSet Settings, Vertex[] GraphicsBuffer, ref int GraphicsBufferPosition)
        {
            LoadBaitSettings(Settings);
            LoadPIDSettings(Settings);
            LoadProportions(Settings);

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


            int snakeColour = Rand.um(16);

            for (int i = 0; i < amountOfElements; i++)
                this.Add(GetNewRandomEntity(GraphicsBuffer, ref GraphicsBufferPosition, snakeColour, currentLength: i));
            
            this[0].Target = Bait;
        }

        Entity GetNewRandomEntity(Vertex[] GraphicsBuffer, ref int GraphicsBufferPosition, int snakeColour, int currentLength)
        {
            return new Entity(GraphicsBuffer, ref GraphicsBufferPosition)
                {
                    X = Rand.om(-4f, 8f), Y = Rand.om(-4f, 8f), Z = 4f,
                    Target = this[(int)(Rand.om(MinTarget, AmpTarget) * (float)currentLength)],
                    PIDSetting = new PID()
                    {
                        ProportionalGain = Rand.om(PMin, PAmp),
                        IntegralGain = Rand.om(IMin, IAmp),
                        DifferentialGain = Rand.om(DMin, DAmp),
                        IntegralCap = ICap,
                    },
                    Scale = Rand.om(minimalSize, additionalSize),
                    Colour = snakeColour, Friction = Rand.om(FMin, FAmp)
                };
        }
        		
        public void Update (float r, float g, float b)
		{
           baitCountdown++;

            if (baitCountdown == baitRunaway)
            {
                Bait.X = Rand.om(offCenterBait, amplitudeBait * SpellieVenster.ratio) * (Rand.um(2) * 2 - 1);
                Bait.Y = Rand.om(offCenterBait, amplitudeBait * SpellieVenster.ratio) * (Rand.um(2) * 2 - 1);
                Bait.Z = Rand.om(nearBait, farBait);
                baitCountdown = 0;
            }

            foreach (Entity i in this)
            {
                i.UpdateEntity();
                i.UpdateVertices();
                i.UpdateColour(r, g, b);
            }
		}
	}
}

