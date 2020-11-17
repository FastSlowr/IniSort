using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace WinIniSort
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Section> sections = ReadINI(@"C:\Users\vladi\Downloads\Пример_исходного_файла.ini");
            sections.Sort();
            WriteINI(@"C:\Users\vladi\Downloads\Пример_исходного_файла2.ini",sections);
        }
        static public void WriteINI(String path,List<Section> sections)
        {
            using(StreamWriter sw=new StreamWriter(path))
            {
                foreach (Section section in sections)
                {
                    sw.WriteLine();
                    sw.WriteLine(section.Name);
                    for (int i = 0; i < section.Count(); i++)
                    {
                        sw.WriteLine(section[i]);
                    }
                }
            }
        }
        static public List<Section> ReadINI(String path)
        {
            List<Section> sections = new List<Section>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                Regex Sregex = new Regex(@"^\[\w*\]$");
                while ((line = sr.ReadLine()) != null)
                {
                    if (Sregex.IsMatch(line))
                    {
                        sections.Add(new Section(line));
                    }
                    else
                    {
                        Regex Kregex = new Regex(@"\w*=\w*");
                        if (Kregex.IsMatch(line))
                        {
                            String[] vs = line.Split('=');
                            if (vs.Length>2)
                            {
                                Console.WriteLine("Нарушена структура файла. Нажмите любую клавишу, чтобы продолжить...");
                                Console.ReadKey();
                            }
                            try
                            {
                                sections[sections.Count - 1].AddValue(vs[0], vs[1]);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("Нарушена структура файла. Нажмите любую клавишу, чтобы продолжить...");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            if (line!=""&&line[0]!=';')
                            {
                                Console.WriteLine("Нарушена структура файла. Нажмите любую клавишу, чтобы продолжить...");
                                Console.ReadKey();
                            }
                        }
                    }
                }
            }
            return sections;
        }
    }
}
