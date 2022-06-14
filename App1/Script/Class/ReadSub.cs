using System;
using System.Collections.Generic;
using System.IO;

namespace sub_Transleator_x
{
    internal class ProjectInfo 
    {
        public string pathSrtFile;
        public int form;
        public int to;
        public List<string> Transleat = new List<string>();
    }
    internal class myDictionary 
    {
        public string info;
        public string text;
    }
    internal class ReadSub
    {
        public myDictionary[] ReadFile(string Path)
        {
            List<string> fileLine = new List<string>();
            List<string> itemSub = new List<string>();
            //---------------------------------------------------
            List<string> info = new List<string>();
            List<string> Text = new List<string>();
            //---------------------------------------------------

            foreach (var element in File.ReadLines(Path))
            {
                fileLine.Add(element);
            }
            //---------------------------------------------------
            //Extrexat Sube Item in Liste File Line 
            //Read Sub File
            for (int i = 0; i < fileLine.Count; i++)
            {
                int outINT = 0;
                if (int.TryParse(fileLine[i], out outINT))
                {
                    int iv = i;
                    string item = "";
                    while (true)
                    {
                        item = item + fileLine[iv] + '\n';
                        iv += 1;
                        //if(iv < fileLine.Count)
                        try
                        {
                            if (int.TryParse(fileLine[iv], out outINT))
                            {
                                itemSub.Add(item);
                                i = iv - 1;
                                break;
                            }
                        }
                        //else
                        catch (Exception)
                        {
                            itemSub.Add(item);
                            break;
                        }
                    }
                }
            }
            //--------------------------------------------
            //Change Info as Text
            for (int i = 0; i < itemSub.Count; i++)
            {
                string tempInfo = "", tempText = "";
                for (int j = 0; j < itemSub[i].Split('\n').Length; j++)
                {
                    if (j == 0 || j == 1)
                    {
                        tempInfo += itemSub[i].Split('\n')[j] + '\n';
                    }
                    else
                    {
                        tempText += itemSub[i].Split('\n')[j] + '\n';
                    }
                }
                info.Add(tempInfo);
                Text.Add(tempText);
            }
            //-------------------------------------------
            fileLine.Clear();
            itemSub.Clear();
            //-------------------------------------------
            //Dictionary<string, string> TempDoc = new Dictionary<string, string>();
            myDictionary[] tempdoc = new myDictionary[info.Count];
            for (int i = 0; i < info.Count; i++)
            {
                tempdoc[i] = new myDictionary();
                tempdoc[i].info = info[i];
                tempdoc[i].text = Text[i];
            }
            return tempdoc;
        }
    }
}