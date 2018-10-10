using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using static Pixel_IRC.Settings;

namespace Pixel_IRC
{
    class File_Tools
    {
        public static bool FileExists(string filename)
        {
            if (File.Exists(filename))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool FileExists(string filename, string directory)
        {
            directory = directory.Trim('\\');
            string fullPath = System.AppDomain.CurrentDomain.BaseDirectory + directory + "\\" + filename;

            if (File.Exists(fullPath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void file_CreateSettings()
        {
            Settings.createSettingsFile();
        }

        /// <summary>
        /// Reads from our custom settings file for specific values.
        /// </summary>
        /// <param name="value">The value to look for in the file.</param>
        /// <returns>Returns the content of the value searched for.</returns>
        public static string readSettings(string value)
        {
            string workingDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string configDir = "configs\\";
            string settingsFile = "sysconf.esf";

            string file = workingDir + configDir + settingsFile;

            string[] getValue = File.ReadAllLines(file);

            var settingsDirectory = new Dictionary<string, string>();
            foreach (string line in getValue)
            {
                string[] parser = line.Split(new string[] { " " }, StringSplitOptions.None);
                string dictKey = parser[0];
                dictKey = dictKey.Substring(1, dictKey.Length - 2);
                string dictValue = "";
                for (int i = 1; i < parser.Length; i++)
                {
                    dictValue = dictValue + " " + parser[i];
                }
                settingsDirectory[dictKey] = dictValue;
            }
            if (settingsDirectory.ContainsKey(value))
            {
                return settingsDirectory[value];
            }

            return "[ERROR] - Key does not exist!";
        }

        public static void writeSetting(string key, dynamic data)
        {
            string workingDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string configDir = "configs\\";
            string settingsFile = "sysconf.esf";

            string file = workingDir + configDir + settingsFile;

            int writeLine = 0;

            string[] line = File.ReadAllLines(file);
            List<string> lines = line.Cast<string>().ToList();
            if (lines.Any(x => x.Contains("[" + key + "]")))
            {
                writeLine = lines.FindIndex(x => x.Contains("[" + key + "]"));
            }

            lines[writeLine] = "[" + key + "] " + data;

            try
            {
                File.WriteAllLines(file, lines);
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error writing to the file.");
            }

        }

        // Colours File Management

        public static Dictionary<string, byte[]> getColours()
        {
            Dictionary<string, byte[]> colorIndex = new Dictionary<string, byte[]>();

            string file = System.AppDomain.CurrentDomain.BaseDirectory + "configs\\" + "colours.esf";

            if (File.Exists(file)) {
                string[] collector = new string[File.ReadAllLines(file).Count()];

                collector = File.ReadAllLines(file);
                foreach (string lineReader in collector)
                {
                    string colorName = "";
                    byte[] rgb = new byte[3];

                    string[] getData = lineReader.Split(new string[] { "," }, StringSplitOptions.None);
                    rgb[0] = Convert.ToByte(Convert.ToInt32(getData[0]));
                    rgb[1] = Convert.ToByte(Convert.ToInt32(getData[1]));
                    rgb[2] = Convert.ToByte(Convert.ToInt32(getData[2]));
                    colorName = getData[3].ToString();

                    colorIndex.Add(colorName, rgb);
                }

                return colorIndex;
            }
            else
            {
                FileStream createColourFile = null;
                createColourFile = File.Create(file);
                if (createColourFile != null)
                {
                    createColourFile.Close();
                }
            }
            return null;
        }

        public static void removeColour(string name)
        {
            string file = System.AppDomain.CurrentDomain.BaseDirectory + "configs\\" + "colours.esf";

            Dictionary<string, byte[]> reader = new Dictionary<string, byte[]>();

            if (File.Exists(file))
            {
                string[] scanFile = File.ReadAllLines(file);
                foreach (string line in scanFile)
                {
                    string[] parser = line.Split(new string[] { "," }, StringSplitOptions.None);
                    byte[] colors = { Convert.ToByte(parser[0]), Convert.ToByte(parser[1]), Convert.ToByte(parser[2]) };
                    reader.Add(parser[3], colors);
                }

                reader.Remove(name); // Deletes the colour specified by name [important! color file must not contain colors that share the same name!]

                //Wipe file and write dictionary contents that have been updated.
                File.Delete(file);
                FileStream newFile = null;
                newFile = File.Create(file);
                if (newFile != null)
                {
                    newFile.Close();
                }
                string[] lines = new string[reader.Keys.Count];
                int i = 0;
                foreach (KeyValuePair<string, byte[]> colorFile in reader)
                {
                    byte[] values = colorFile.Value;
                    lines[i] = values[0].ToString() + "," + values[1].ToString() + "," + values[2].ToString() + "," + colorFile.Key;
                    i++;
                }
                File.WriteAllLines(file, lines);
            }
            else { MessageBox.Show("Colours file doesn't exist, or no colors defined!"); }
        }

        public static void renameColour(string oldName, string newName)
        {
            string file = System.AppDomain.CurrentDomain.BaseDirectory + "configs\\" + "colours.esf";

            Dictionary<string, byte[]> reader = new Dictionary<string, byte[]>();

            if (File.Exists(file))
            {
                string[] scanFile = File.ReadAllLines(file);
                foreach (string line in scanFile)
                {
                    string[] parser = line.Split(new string[] { "," }, StringSplitOptions.None);
                    byte[] colors = { Convert.ToByte(parser[0]), Convert.ToByte(parser[1]), Convert.ToByte(parser[2]) };
                    reader.Add(parser[3], colors);
                }

                if (reader.ContainsKey(oldName))
                {
                    KeyValuePair<string, byte[]> newData = new KeyValuePair<string, byte[]>(newName, reader[oldName]);
                    reader.Remove(oldName);
                    reader.Add(newData.Key, newData.Value);
                }
                else { MessageBox.Show("Color we are trying to change doesn't exist."); }

                File.Delete(file);
                FileStream newFile = null;
                newFile = File.Create(file);
                if (newFile != null)
                {
                    newFile.Close();
                }
                string[] lines = new string[reader.Keys.Count];
                int i = 0;
                foreach (KeyValuePair<string, byte[]> colorFile in reader)
                {
                    byte[] values = colorFile.Value;
                    lines[i] = values[0].ToString() + "," + values[1].ToString() + "," + values[2].ToString() + "," + colorFile.Key;
                    i++;
                }
                File.WriteAllLines(file, lines);
            }
            else { MessageBox.Show("Colours file doesn't exist, or no colors defined!"); }
        }

        public static void writeColour(string red, string green, string blue, string name)
        {
            string file = System.AppDomain.CurrentDomain.BaseDirectory + "configs\\" + "colours.esf";

            Dictionary<string, byte[]> reader = new Dictionary<string, byte[]>();

            if (File.Exists(file))
            {
                string[] scanFile = File.ReadAllLines(file);
                foreach (string line in scanFile)
                {
                    string[] parser = line.Split(new string[] { "," }, StringSplitOptions.None);
                    byte[] colors = { Convert.ToByte(parser[0]), Convert.ToByte(parser[1]), Convert.ToByte(parser[2]) };
                    reader.Add(parser[3], colors);
                }
            }

            byte[] rgb = new byte[3];
            rgb[0] = Convert.ToByte(Convert.ToInt32(red));
            rgb[1] = Convert.ToByte(Convert.ToInt32(green));
            rgb[2] = Convert.ToByte(Convert.ToInt32(blue));

            reader.Add(name, rgb);

            File.Delete(file);

            FileStream colourFile = null;
            colourFile = File.Create(file);
            if (colourFile != null)
            {
                colourFile.Close();
            }

            string[] lines = new string[reader.Keys.Count()];
            int i = 0;
            foreach (KeyValuePair<string, byte[]> line in reader)
            {
                byte[] colors = line.Value;
                lines[i] = colors[0].ToString() + "," + colors[1].ToString() + "," + colors[2].ToString() + "," + line.Key;
                i++;
            }

            File.WriteAllLines(file, lines);
        }

        public static bool colorExists(string name)
        {
            string file = System.AppDomain.CurrentDomain.BaseDirectory + "configs\\" + "colours.esf";

            Dictionary<string, byte[]> reader = new Dictionary<string, byte[]>();

            if (File.Exists(file))
            {
                string[] scanFile = File.ReadAllLines(file);
                foreach (string line in scanFile)
                {
                    string[] parser = line.Split(new string[] { "," }, StringSplitOptions.None);
                    byte[] colors = { Convert.ToByte(parser[0]), Convert.ToByte(parser[1]), Convert.ToByte(parser[2]) };
                    reader.Add(parser[3], colors);
                }

                if (reader.ContainsKey(name))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the byte values for the color name specified.
        /// </summary>
        /// <param name="colorName">The setting name to grab</param>
        /// <returns></returns>
        public static byte[] getColor(string colorName)
        {
            string[] fetch = readSettings(colorName).Trim().Split(',');
            byte[] color = new byte[fetch.Length];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Convert.ToByte(fetch[i]);
            }
            return color;
        }
    }
}
