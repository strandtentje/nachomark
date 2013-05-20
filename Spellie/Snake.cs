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
    /// <summary>
    /// A snake consisting of entities that chase each other 
    /// in order to reach a goal.
    /// </summary>
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
		int[] snakeColours;
        float minimalSize, additionalSize;

        /// <summary>
        /// Load settings from valueset that modify 
        /// attributes of target spawning.
        /// </summary>
        /// <param name="Settings">Settings to load from
        /// </param>
        void LoadBaitSettings(ValueSet Settings)
        {
            offCenterBait = Settings.TryGetFloat("center", 20f);
            amplitudeBait = Settings.TryGetFloat("ampli", 40f);
            nearBait = Settings.TryGetFloat("near", -1f);
            farBait = Settings.TryGetFloat("far", 17f);
            baitRunaway = Settings.TryGetInt("targetinterval", 120);
        }

        /// <summary>
        /// Load settings from valueset that specify how one
        /// element follows another.
        /// </summary>
        /// <param name="Settings">Settings to load from
        /// </param>
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

        /// <summary>
        /// Load settings from valueset that specify
        /// how long a snake is and what sizes its elements
        /// can be.
        /// </summary>
        /// <param name="Settings">Settings to load from
        /// </param>
        void LoadProportions(ValueSet Settings)
        {
            amountOfElements = Settings.TryGetInt("length", 300);
            minimalSize = Settings.TryGetFloat("smallest", 0.5f);
            additionalSize = Settings.TryGetFloat("extra", 0.8f);
			snakeColours = Settings.TryGetInts("snakecolour");
            // snakeColour = Settings.TryGetInt("snakecolour", 3);
        }

        /// <summary>
        /// Construct a new snake from settings for a graphics buffer.
        /// </summary>
        /// <param name="Settings">Settings to load snake attributes from</param>
        /// <param name="GraphicsBuffer">Graphics buffer to write to</param>
        /// <param name="GraphicsBufferPosition">Position in graphics buffer
        /// at which to start.</param>
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

            for (int i = 0; i < amountOfElements - 1; i++)
                this.Add(GetNewRandomEntity(GraphicsBuffer, ref GraphicsBufferPosition, currentLength: i));
            
            this[0].Target = Bait;
        }

        /// <summary>
        /// Produces a new random entry that adheres to
        /// the set boundaries of this snake.
        /// </summary>
        /// <param name="GraphicsBuffer">Graphics buffer to
        /// write to.</param>
        /// <param name="GraphicsBufferPosition">Position
        /// in graphics buffer at which to start</param>
        /// <param name="snakeColour">Colour of this snake
        /// </param>
        /// <param name="currentLength">Current length of the
        /// snake (not the final length!)</param>
        /// <returns></returns>
        Entity GetNewRandomEntity(Vertex[] GraphicsBuffer, ref int GraphicsBufferPosition, int currentLength)
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
                    Colour = snakeColours[Rand.um(snakeColours.Length)], 
					Friction = Rand.om(FMin, FAmp)
                };
        }

        /// <summary>
        /// Update all elements in this snake and use the
        /// given colour modification.
        /// </summary>
        /// <param name="r">Red channel to add</param>
        /// <param name="g">Green channel to add</param>
        /// <param name="b">Blue channel to add</param>       		
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

