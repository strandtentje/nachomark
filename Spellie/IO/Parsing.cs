using System;
using System.IO;
using System.Text;

namespace NachoMark
{
    /// <summary>
    /// Parsing utilities
    /// </summary>
	public static class Parsing
	{
        /// <summary>
        /// Removes any whitespaces from the string
        /// </summary>
        /// <param name="dirty">Dirty string</param>
        /// <returns>Clean string</returns>
		public static string CleanUp (string dirty)
		{
			int start, end;

			while (dirty.Contains("/*")) {
				start = dirty.IndexOf("/*");
				end = dirty.IndexOf("*/");

				dirty = dirty.Remove(start, (end - start) + 2);
			}

			return dirty
				.ToLower().Replace("\r","").Replace("\n","").Replace(" ","").Replace("\t","");
		}

        /// <summary>
        /// Consume a string of lower case characters
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <param name="target">Target to read to</param>
        /// <returns>True when succesful (target contains something)
        /// </returns>
		public static bool ConsumeIdentifier (StringReader stream, out string target)
		{
			StringBuilder sbl = new StringBuilder ();

			while ((stream.Peek() >= (int)'a') && (stream.Peek() <= (int)'z'))
				sbl.Append ((char)stream.Read ());

			target = sbl.ToString();
			return target.Length > 0;
		}

        /// <summary>
        /// Consume an expression (letters, numbers, dashes and dots)
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <param name="target">Target string to read to</param>
        /// <returns>True when succesful (target contains something)</returns>
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

        /// <summary>
        /// Consume a single character.
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <param name="c">Character to look for</param>
        /// <returns>True when character was found</returns>
		public static bool ConsumeChar (StringReader stream, char c)
		{
			if (stream.Peek() != (int)c) return false;
			stream.Read();
			return true;
		}

	}
}

