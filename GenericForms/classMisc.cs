using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GenericForms
{
    class Misc
    {
        public static string[] ReadLines(string path)
        {
            StreamReader file = new StreamReader(path);
            List<string> lines = new List<string>();
            string line;

            while (!file.EndOfStream)
            {
                line = file.ReadLine();

                if (!string.IsNullOrWhiteSpace(line) && line.Substring(0, 2) != "//")
                    lines.Add(line);
            }

            file.Close();

            return lines.ToArray();
        }
        
        public static string takePrefix(ref string line)
        {
            string[] delimiters = { "=", ":", "->", "-[" };

            int delimit = line.Length, ind;

            foreach (string del in delimiters)
            {
                ind = line.IndexOf(del);

                if (ind != -1 && ind < delimit)
                    delimit = ind;
            }

            if (line[0] == '"')
            {
                line = line.Substring(1);
                delimit = line.IndexOf('"');
            }

            string prefix = line.Substring(0, delimit);

            if (delimit != line.Length)
                line = line.Substring(delimit + 1);

            if (line.Length > 1 && delimiters.Contains(line[0].ToString()))
                line = line.Substring(1);

            return prefix;
        }
    }
}