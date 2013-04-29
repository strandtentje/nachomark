using System;
using System.IO;

namespace NachoMark
{
	public static class CSV
	{
		public static void Read (string file, Action<string[]> action)
		{
			StreamReader srd = new StreamReader (file);
			string[] parts;

			while (!srd.EndOfStream) {
				parts = srd.ReadLine().Split(';');

				if (!parts[0].StartsWith("//"))
					action(parts);
			}

			srd.Close();
		}

		public static void Clear (string file)
		{
			File.WriteAllText(file, "");
		}

		public static void Append (string file, string[] parts)
		{
			File.AppendAllText(
				file, 
				string.Join(";", parts) + '\n');
		}

	}
}

