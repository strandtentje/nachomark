using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;

namespace NachoMark.IO
{
	/// <summary>
	/// C-like datafile parser and composer.
	/// </summary>
	public static class CLON
	{
		/// <summary>
		/// Read the specified file and execute action for every
		/// valueset found.
		/// </summary>
		/// <param name='file'>
		/// File to read.
		/// </param>
		/// <param name='action'>
		/// Action to execute.
		/// </param>
		public static void Read (string file, Action<ValueSet> action)
		{
			string entireFile = System.IO.File.ReadAllText (file);
			entireFile = Parsing.CleanUp (entireFile);

			StringReader srd = new StringReader (entireFile);
			
			while (!Parsing.ConsumeChar(srd, '#'))
				action(ValueSet.Parse(srd));			

		}

		/// <summary>
		/// Clear the specified file.
		/// </summary>
		/// <param name='file'>
		/// File.
		/// </param>
		public static void Clear (string file)
		{
			System.IO.File.WriteAllText(file,"");
		}

		/// <summary>
		/// Append the valueset to the file.
		/// </summary>
		/// <param name='file'>
		/// File to write to.
		/// </param>
		/// <param name='v'>
		/// Valueset to write.
		/// </param>
		public static void Append (string file, ValueSet v)
		{
			System.IO.File.AppendAllText(file, v.Compose());
		}
	}
}