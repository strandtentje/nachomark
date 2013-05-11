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
	public class CLON : Dictionary<string, ValueSet>
	{
        /// <summary>
        /// Load a list of valuesets from a specified file.
        /// </summary>
        /// <param name="file">File to read</param>
		public void Load(string file)
		{
			string entireFile = System.IO.File.ReadAllText (file);
			entireFile = Parsing.CleanUp (entireFile);

			StringReader srd = new StringReader (entireFile);

            ValueSet vsTemp;

            while (!Parsing.ConsumeChar(srd, '#'))
            {
                vsTemp = ValueSet.Parse(srd);
                this.Add(vsTemp.Name, vsTemp);
            }
		}

        /// <summary>
        /// Write the list of valuesets to a specified file.
        /// </summary>
        /// <param name="file">File to save to</param>
        public void Save(string file)
        {
            File.WriteAllText(file, "");

            foreach (ValueSet v in this.Values)
                File.AppendAllText(file, v.Compose());

            File.AppendAllText(file, "#");
        }
	}
}