using System;
using Color4 = OpenTK.Graphics.Color4;
using System.Collections.Generic;
using OpenTK.Input;

namespace Spellie
{
	public class Editor
	{
		public Editor (Level Level, Dictionary<Key, Action> Keymap)
		{
			this.Level = Level;

			Keymap.Add(Key.Tab, delegate() { Select(cEnt + 1); });
			Keymap.Add(Key.Tilde, delegate() { Select(cEnt - 1); });
			Keymap.Add(Key.F1, Duplicate);
			Keymap.Add(Key.F2, Delete);
			Keymap.Add(Key.F3, delegate() { Level.Save("level"); });
			Keymap.Add(Key.B, delegate() { Toggle(4); });
			Keymap.Add(Key.N, delegate() { Toggle(2); });
			Keymap.Add(Key.M, delegate() { Toggle(1); });
			Keymap.Add(Key.Comma, Darker);
			Keymap.Add(Key.Period, Brighter);
		}

		void Toggle(int bit)
		{
			if ((Level[cEnt].Color & bit) != 0) Level[cEnt].Color &= 255 - bit;
			else Level[cEnt].Color |= bit;
		}

		void Darker()
		{
			if (Level[cEnt].Color > 15)
				Level[cEnt].Color -= 16;
		}

		void Brighter()
		{
			if (Level[cEnt].Color < 240)
				Level[cEnt].Color += 16;
		}

		int cEnt;
		Level Level;
		Random r = new Random();

		public void Move(
			bool Left, bool Right, bool Up, bool Down,
			bool RotateLeft, bool RotateRight,
			bool Bigger, bool Smaller,
			bool Farther, bool Nearer,
			bool Faster)
		{
			float act = (Faster? 0.10f : 0.03f);

			Move (
				(Left ? act : 0) + (Right ? -act : 0),
				(Up ? act : 0) + (Down ? -act : 0),
				(RotateLeft ? act : 0) + (RotateRight ? -act : 0),
				(Bigger ? act : 0) + (Smaller ? -act : 0), 
				(Farther ? act : 0) + (Nearer ? -act : 0));
		}

		public void Move(float x, float y, float angle, float size, float depth)
		{
			if (Level.Count == 0) return;

			Entity e = Level[cEnt];

			e.X += x; e.Y += y; e.Z += depth;
			e.Scale += size; e.Rotate += angle;
		}

		public void Delete()
		{
			if (Level.Count < 2) return;
			Level.RemoveAt(cEnt);
			Select(cEnt - 1);
		}

		public void Duplicate()
		{
			Entity baseEntity = Level[cEnt];

			Level.Add(baseEntity.Clone());

			Select(Level.Count - 1);
		}

		Entity selected;

		public void Select (int entNum)
		{
			if (Level.Count == 0) return;

			if (cEnt < Level.Count) 
				Level[cEnt].Select = false;

			cEnt = entNum;

			if (entNum >= Level.Count) 	
				cEnt = 0;
			else 
				if (entNum < 0) 
					cEnt = Level.Count - 1;					

			Level[cEnt].Select = true;

			selected = Level[cEnt];
		}
	}
}

