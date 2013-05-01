using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;

namespace NachoMark.IO
{
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

        NumberFormatInfo FloatParsing = CultureInfo.InvariantCulture.NumberFormat;

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
        public float TryGetFloat(string name, float dflt)
        {
            try
            {
                return float.Parse(this[name], FloatParsing);
            }
            catch
            {
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
        public int TryGetInt(string name, int dflt)
        {
            try
            {
                return int.Parse(this[name]);
            }
            catch
            {
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
        public void Set(string name, float value)
        {
            if (this.ContainsKey(name)) this.Remove(name);
            this.Add(name, value.ToString().Replace(",", "."));
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
        public void Set(string name, int value)
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
        public static ValueSet Parse(StringReader stream)
        {
            string k, v, name;
            ValueSet nVs = new ValueSet();

            if (Parsing.ConsumeIdentifier(stream, out name))
            {
                nVs.Name = name;
                if (Parsing.ConsumeChar(stream, '{'))
                {
                    while (!Parsing.ConsumeChar(stream, '}'))
                    {
                        if (!Parsing.ConsumeIdentifier(stream, out k))
                            throw new Exception("Expected Identifier");

                        if (!Parsing.ConsumeChar(stream, '='))
                            throw new Exception("Expected Assignment");

                        if (!Parsing.ConsumeValue(stream, out v))
                            throw new Exception("Expected Expression");

                        if (!Parsing.ConsumeChar(stream, ';'))
                            throw new Exception("Expected semicolon");

                        nVs.Add(k, v);
                    }
                }
                else
                {
                    throw new Exception("Expected Block");
                }
            }
            else
            {
                throw new Exception("Expected Identifier");
            }

            return nVs;
        }

        /// <summary>
        /// Compose this ValueSet into a human-readable data.
        /// </summary>
        public string Compose()
        {
            StringBuilder sbl = new StringBuilder();

            sbl.AppendLine(Name.ToLower());
            sbl.AppendLine("{");

            foreach (KeyValuePair<string, string> kvp in this)
            {
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
