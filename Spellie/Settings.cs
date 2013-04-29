using System;
using OpenTK;
using OpenTK.Graphics;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace NachoMark
{
	public class Settings
	{
		Dictionary<string, string> settings =
			new Dictionary<string, string>();

		string myFile;

		public Settings (string file)
		{
			myFile = file;

			if (!File.Exists(file)) return;

			loadFile();
		}

		public string this [string setting, int index = -1] {
			get {
				if (index == -1) return settings[setting];
				return settings[setting + '\t' + index.ToString()];
			}
			set {
				if (index == -1) settings[setting] = value;
				else settings[setting + '\t' + index.ToString()] = value;
				saveFile();
			}
		}

		Semaphore FileSemaphore = new Semaphore(0,1);

		void saveFile ()
		{
			FileSemaphore.WaitOne();

			StreamWriter fileWriter = new StreamWriter(myFile);

			foreach(KeyValuePair<string, string> setting in settings)
				fileWriter.WriteLine(setting.Key + '=' + setting.Value);

			fileWriter.Close();

			FileSemaphore.Release();
		}

		void loadFile ()
		{
			StreamReader fileReader = new StreamReader (myFile);

			while (!fileReader.EndOfStream) 
			{	// Bestandje lees 
				string[] line = fileReader.ReadLine ().Split ('=');  // Regeltje splijt

				if (line.Length > 0)
				{
					if ((!line[0].StartsWith("#")) && 	// Niet comment
					    (line.Length > 1))   // Wel instelling
						settings.Add (line [0], line [1]);  // Opsla.
				}
			}

			fileReader.Close();
		}

	}

}
