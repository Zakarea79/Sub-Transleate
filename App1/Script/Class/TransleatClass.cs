using Newtonsoft.Json;

namespace sub_Transleator_x
{
    public class TransleatClass : Loger
    {
        #region Code Language
        public static string[] Language = new string[]
        {
            "ar" , "bg" , "zh-CN" , "hr"  , "cs",
            "sd" , "nl" , "en"    , "fi"  , "fr",
            "de" , "el" , "hi"    , "it"  , "ja",
            "ko" , "no" , "fa"    , "pl"  , "pt",
            "ro" , "ru" , "es"    , "sv"  , "Unknown"
        };
        #endregion
        #region List name Language
        public string[] slect_lang = new string[]
        {
            "Arabic"   , "Bulgarian" , "Chinese" , "Croatian" , "Czech"     ,
            "Danish"   , "Dutch"     , "English" , "Finnish"  , "French"    ,
            "German"   , "Greek"     , "Hindi"   , "Italian"  , "Japanese"  ,
            "Korean"   , "Norwegian" , "Persian" , "Polish"   , "Portuguese",
            "Romanian" , "Russian"   , "Spanish" , "Swedish"  , "Unknown"
        };
        #endregion
        public void ShowMenueItem(out int from, out int to)
        {
            int contTabel = 1;
            for (int i = 0; i < slect_lang.Length; i++)
            {
                print(i, " : ", slect_lang[i], "\t");
                contTabel++;
                if (contTabel == 4)
                {
                    println("");
                    contTabel = 1;
                }
            }
            print('\n');
            print("Enter from Language :");
            from = System.Convert.ToInt32(input());
            print('\n');
            print("Enter To Language :");
            to = System.Convert.ToInt32(input());
            print('\n');
        }

        private static string JsonDecod(string JsonData)
        {
            var tmp = JsonConvert.DeserializeObject<Root>(JsonData);
            return tmp.sentences[0].trans;
        }
        public static string Translate(string input)
        {
            RequestSupernova.requestSetting.source = Language[App1.publicClassAndroid.From];
            RequestSupernova.requestSetting.target = Language[App1.publicClassAndroid.To];
            RequestSupernova.requestSetting.text = input;
            return JsonDecod(RequestSupernova.Translate());
        }
    }
}
