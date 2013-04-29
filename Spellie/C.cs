using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NachoMark
{
	/// <summary>
	/// C-like datafile parser and composer.
	/// </summary>
	public static class C
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
			bool stop = false;

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

		/// <summary>A map of values from a file.</summary>
		public class ValueSet : Dictionary<string, string>
		{
			/// <summary>
			/// Gets or sets the name of the ValueSet.
			/// </summary>
			/// <value>
			/// The name.
			/// </value>
			public string Name { get; set; }

			/// <summary>
			/// Tries to get a float at the specified name.
			/// </summary>
			/// <returns>
			/// The gotten float.
			/// </returns>
			/// <param name='name'>
			/// Name of the value
			/// </param>
			/// <param name='dflt'>
			/// Default value
			/// </param>
			public float TryGetFloat (string name, float dflt)
			{
				try {
					return float.Parse(this[name].Replace(".",","), System.Globalization.NumberStyles.Any);
				} catch {
					return dflt;
				}
			}

			/// <summary>
			/// Tries to get an int at the specified name.
			/// </summary>
			/// <returns>
			/// The gotten int.
			/// </returns>
			/// <param name='name'>
			/// Name of the value
			/// </param>
			/// <param name='dflt'>
			/// Default
			/// </param>
			public int TryGetInt (string name, int dflt)
			{
				try {
					return int.Parse (this [name]);
				} catch {
					return dflt;
				}
			}

			public void Set(string name, string value)
			{
				if (this.ContainsKey(name)) this.Remove(name);
				this.Add(name, value.ToString());
			}

			/// <summary>
			/// Set value at name.
			/// </summary>
			/// <param name='name'>
			/// Name.
			/// </param>
			/// <param name='value'>
			/// Value.
			/// </param>
			public void Set (string name, float value)
			{
				if (this.ContainsKey(name)) this.Remove(name);
				this.Add(name, value.ToString().Replace(",","."));
			}

			/// <summary>
			/// Set value at name
			/// </summary>
			/// <param name='name'>
			/// Name.
			/// </param>
			/// <param name='value'>
			/// Value.
			/// </param>
			public void Set (string name, int value)
			{
				if (this.ContainsKey(name)) this.Remove(name);
				this.Add(name, value.ToString());
			}

			/// <summary>
			/// Parse the specified stream into a valueset.
			/// </summary>
			/// <param name='stream'>
			/// Stream to parse.
			/// </param>
			public static ValueSet Parse (StringReader stream)
			{
				string k, v, name;
				ValueSet nVs = new ValueSet ();

				if (Parsing.ConsumeIdentifier (stream, out name)) {
					nVs.Name = name;
					if (Parsing.ConsumeChar (stream, '{')) {
						while (!Parsing.ConsumeChar(stream, '}')) {	
							if (!Parsing.ConsumeIdentifier (stream, out k))
								throw new Exception ("Expected Identifier");
							
							if (!Parsing.ConsumeChar (stream, '='))
								throw new Exception ("Expected Assignment");
							
							if (!Parsing.ConsumeValue (stream, out v))
								throw new Exception ("Expected Expression");
							
							if (!Parsing.ConsumeChar (stream, ';'))
								throw new Exception ("Expected semicolon");

							nVs.Add (k, v);
						}
					} else {
						throw new Exception ("Expected Block");
					}
				} else {
					throw new Exception ("Expected Identifier");
				}

				return nVs;
			}

			/// <summary>
			/// Compose this ValueSet into a human-readable data.
			/// </summary>
			public string Compose ()
			{
				StringBuilder sbl = new StringBuilder ();

				sbl.AppendLine (Name.ToLower ());
				sbl.AppendLine ("{");

				foreach (KeyValuePair<string, string> kvp in this) {
					sbl.Append(kvp.Key.ToLower());
					sbl.Append("=");
					sbl.Append(kvp.Value.ToLower());
					sbl.AppendLine(";");
				}

				sbl.AppendLine("}");

				return sbl.ToString();
			}
		}
	}
}