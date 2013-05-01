using System;
using System.IO;
using System.Text;

namespace NachoMark
{
	public static class Parsing
	{
		public static string CleanUp(string dirty)
		{
			return dirty
				.ToLower().Replace("\r","").Replace("\n","").Replace(" ","").Replace("\t","");
		}

		public static bool ConsumeIdentifier (StringReader stream, out string target)
		{
			StringBuilder sbl = new StringBuilder ();

			while ((stream.Peek() >= (int)'a') && (stream.Peek() <= (int)'z'))
				sbl.Append ((char)stream.Read ());

			target = sbl.ToString();
			return target.Length > 0;
		}

		public static bool ConsumeValue (StringReader stream, out string target)
		{
			StringBuilder sbl = new StringBuilder ();

			while (
				((stream.Peek() >= (int)'a') && (stream.Peek() <= (int)'z')) ||
				((stream.Peek() >= (int)'0') && (stream.Peek () <= (int)'9')) ||
				(stream.Peek() == (int)'.') || (stream.Peek() == (int)'-'))
				sbl.Append ((char)stream.Read ());

			target = sbl.ToString();
			return target.Length > 0;
		}

		public static bool ConsumeChar (StringReader stream, char c)
		{
			if (stream.Peek() != (int)c) return false;
			stream.Read();
			return true;
		}
	}
}

