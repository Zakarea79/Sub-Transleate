using System;
namespace sub_Transleator_x
{

    public class Data 
    {
        public int Persent { set; get; }
        public string Name { set; get; }
    }

    public class Loger
    {
        public static void println(params object[] argsme)
        {
            foreach (var element in argsme)
            {
                Console.Write(element);
            }
            Console.WriteLine();
        }
        public static void printZ(object Input, int Z)
        {
            for (int i = 0; i < Z; i++)
            {
                print(Input);
            }
        }

        public static void printZL(object Input, int Z)
        {
            for (int i = 0; i < Z; i++)
            {
                println(Input);
            }
        }
        public static string input()
        {
            var data = Console.ReadLine();
            return data == null ? "" : data;
        }
        public static void printlnE(params object[] argsme)
        {
            foreach (var element in argsme)
            {
                Console.WriteLine(element);
            }
        }
        public static void print(params object[] argsme)
        {
            foreach (var element in argsme)
            {
                Console.Write(element);
            }
        }
        public static void AndroidLog(Android.Content.Context cotext , string text) 
        {
            Android.Widget.Toast.MakeText(cotext, text, Android.Widget.ToastLength.Long).Show();
        }

        public static void PrograssBar(string unicode, int i, int Length, ref int prograss)
        {
            int x = i * 100;
            x = x / Length;
            if (x > prograss)
            {
                printZ(unicode, x - prograss);
                prograss = x;
            }
        }
    }
}
