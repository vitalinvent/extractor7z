using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace extractor7z
{
    class Program
    {
        static void Main(string[] args)
        {
            //to debug
            //args = new string[3] { "*.7z", "exclude", "japan,china" };
            SevenZip.SevenZipExtractor.SetLibraryPath(@"7z.dll");
            SevenZip.SevenZipExtractor zip;
            ulong size = 0;
            int idx = 0;
            string name = "";
            string[] arrExclude = null;
            if (!Directory.Exists("extracted")) Directory.CreateDirectory("extracted");
            if (args.Length > 0)
            {
                try
                {
                    string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), args[0]);
                    //string[] files = Directory.GetFiles("X:\\temp ", "1.7z ");
                    Console.WriteLine("Found " + files.Length + " files");
                    int countArchivesExtracted = 0;
                    if (args.Length > 1)
                    {
                        if (args[1].Equals("exclude"))
                        {
                            arrExclude = args[2].Split(',');
                        }
                    }
                    foreach (string file in files)
                    {
                        zip = new SevenZip.SevenZipExtractor(file);
                        try
                        {
                            foreach (string fileArc in zip.ArchiveFileNames)
                            {
                                size = 0;
                                try
                                {
                                    idx = 0;
                                    name = "";
                                    //ReadOnlyCollection<SevenZip.ArchiveFileInfo> infos = ;
                                    foreach (SevenZip.ArchiveFileInfo info in zip.ArchiveFileData)
                                    {
                                        try
                                        {
                                            string fileName = info.FileName;
                                            bool exist = false;
                                            foreach (string exclude in arrExclude)
                                            {
                                                if (fileName.ToLower().IndexOf(exclude.ToLower()) > 0) exist = true;
                                            }
                                            if ((size < info.Size) & (exist == false))
                                            {
                                                idx = info.Index;
                                                name = fileName;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                                //using (FileStream fs = File.OpenWrite(Directory.))
                            }
                            using (FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "\\extracted\\" + name, FileMode.Create))
                            {
                                Console.Write("\r {0} of {1} Archive: {2} File: {3}", (countArchivesExtracted + 1).ToString(), files.Length.ToString(), file, name);
                                zip.ExtractFile(idx, fs);
                                countArchivesExtracted++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    Console.WriteLine("");
                    Console.WriteLine("Extracted: " + countArchivesExtracted.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Usage: extractor [mask] [exclude string1,string2,string3] - extract one largest file from archive files by mask in dir , exclude string in name (get next one)");
                //Application.Run(new Form1());
            }
        }
    }
}
