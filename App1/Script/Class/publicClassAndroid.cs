using System.Collections.Generic;
using sub_Transleator_x;
public enum backButtonStatuse : byte 
{
    mainLayout , autoTransleat , crateProject
}
namespace App1
{
    class publicClassAndroid
    {
        public static backButtonStatuse enumBtnStatuse;
        public static List<string> FileSelected = new List<string>();
        public static string Folderpath = "";
        public static int From = 0;
        public static int To = 0;
        public static bool StButtonStatuse = false;
        public static ProjectInfo info = new ProjectInfo();

        public static void ReseatData() 
        {
            info.form = 0;
            info.to = 0;
            info.Transleat.Clear();
            info.pathSrtFile = "";
            //-------------------------
            To = 0;
            From = 0;
            StButtonStatuse = false;
            Folderpath = "";
            FileSelected.Clear();
            enumBtnStatuse = backButtonStatuse.mainLayout;
        }

    }
}