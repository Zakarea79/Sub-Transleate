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
    }
}