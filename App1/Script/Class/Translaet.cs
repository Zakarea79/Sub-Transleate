//-------------------------------
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
//-------------------------------
namespace sub_Transleator_x
{
    internal class SubTransletor_Class : Loger
    {
        static public string PathFolder = "";
        static public string PathFolderExport;
        //static private List<string> Calection = new List<string>();
        //--------------------------------------------------------
        static private List<string> info = new List<string>();
        static private List<string> Text = new List<string>();
        //---------------------------------------------------------
        static private List<string> androidListView = new List<string>();

        //\Transleat
        private static List<string> Transleat(List<string> Input)
        {
            List<string> temp = new List<string>();
            //foreach (var element in Input)
            int prograss = 0;
            for (int i = 0; i < Input.Count; i++)
            {
                if (App1.publicClassAndroid.StButtonStatuse == true)
                {
                    return new List<string>();
                }
                //int prograss = 0;
                temp.Add(TransleatClass.Translate(Input[i]) + "\n\n");

                PrograssBar("#", i, Input.Count, ref prograss);
                App1.auto_transleat.data.Persent = prograss;
            }
            App1.auto_transleat.data.Persent = 100;
            Thread.Sleep(1000);
            //if(prograss >= 97)
            //{
            //    printZ("#" , 100 - prograss);
            //}
            return temp;
        }

        //CrateFinalList
        private static string CrateFinalList(List<string> INFO, List<string> TEXT)
        {
            string temp = "";
            if (TEXT.Count == INFO.Count)
            {
                for (int i = 0; i < TEXT.Count; i++)
                {
                    temp += (INFO[i] + TEXT[i]);
                }
                return temp;
            }
            else
            {
                return "";
            }
        }

        public static async Task SubTransleatAsync(string input, List<string> file)
        {
            await Task.Run(() =>
            {
                SubTransleat(input, file);
            });
        }
        //MINA METODE
        private static void SubTransleat(string input, List<string> file)
        {
            //FolderBrowserDialog ob = new FolderBrowserDialog();
            //ob.ShowDialog();
            //if(ob.SelectedPath != "")
            //{
            PathFolder = input;//ob.SelectedPath;
            //}
            if (PathFolder != "")
            {
                List<string> mylestFile = new List<string>();
                List<string> fileLine = new List<string>();
                List<string> itemSub = new List<string>();
                //Get List File Sub as Directory
                if (PathFolder != "selected file " + '\u2713')
                {
                    foreach (var element in Directory.GetFiles(PathFolder))
                    {
                        if (Path.GetExtension(element).ToUpper() == ".srt".ToUpper())
                        {
                            mylestFile.Add(element);
                        }
                    }
                }
                else
                {
                    PathFolder = "";
                    for (int i = 0; i < file[0].Split('/').Length - 1; i++)
                    {
                        PathFolder += file[0].Split('/')[i] + '/';
                    }
                    mylestFile.AddRange(file);
                }
                if (mylestFile.Count == 0)
                {
                    //println("Folder Slectde Is Empty !");
                    SubTransleat(input, file);
                }
                else
                {
                    //TransleatClass objectTransleatClass = new TransleatClass();
                    //objectTransleatClass.ShowMenueItem(out from, out to);
                    PathFolderExport = PathFolder + @"Convert/";
                    //-----------------------------------------------------
                    //print('\n');
                    //println("Selectde Directory : " + PathFolder + "\n");
                    //println("Export Directory : " + PathFolderExport + "\n\n");
                    //-----------------------------------------------------

                    try
                    {
                        Directory.CreateDirectory(PathFolderExport);
                    }
                    catch (Exception)
                    {
                        PathFolderExport = "/storage/emulated/0/Download/";
                    }

                    //---------------------------------------------------
                    //Read Line File Seave TO List file Line
                    for (int v = 0; v < mylestFile.Count; v++)
                    {
                        foreach (var element in File.ReadLines(mylestFile[v]))
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
                        var obj = new SubTransletor_Class();
                        //print("\n");
                        //println($"start Transleate File : {Path.GetFileName(mylestFile[v])}");
                        List<string> Transleatv = Transleat(Text);
                        if (App1.publicClassAndroid.StButtonStatuse == true)
                        {
                            App1.publicClassAndroid.StButtonStatuse = false;
                            return;
                        }
                        string temp = CrateFinalList(info, Transleatv);
                        //-------------------------------------------
                        try
                        {
                            File.WriteAllLines(PathFolderExport + Path.GetFileName(mylestFile[v]), temp.Split('\n'), Encoding.UTF8);
                        }
                        catch (Exception)
                        {
                            if (File.Exists("/storage/emulated/0/Download/" + Path.GetFileName(mylestFile[v])))
                            {
                                File.Delete("/storage/emulated/0/Download/" + Path.GetFileName(mylestFile[v]));
                            }
                            File.WriteAllLines("/storage/emulated/0/Download/" + Path.GetFileName(mylestFile[v]), temp.Split('\n'), Encoding.UTF8);
                        }

                        App1.auto_transleat.data.Name = mylestFile[v];
                        //Log Prograss Folder
                        //-------------------------------------------
                        temp = "";
                        Transleatv.Clear();
                        info.Clear();
                        Text.Clear();
                        //-------------------------------------------
                        //println(" Work Done! => " + v);
                        androidListView.Add(mylestFile[v]);
                    }
                    mylestFile.Clear();
                }
            }
            //else
            //{
            //    println("Path Not Selected!!!");
            //    SubTransleat(input , from , to , cotext);
            //}
            //println("Transleated Completed!!");
            //Console.ReadKey();
        }
    }
}