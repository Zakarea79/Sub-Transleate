using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace sub_Transleator_x
{
    internal class RequestSetting
    {
        public string text;
        public string source;
        public string target;
    }
    internal class RequestSupernova
    {
        public static RequestSetting requestSetting = new RequestSetting();
        public static string sendData(string Url, Dictionary<string, string> data)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("AndroidTranslate/5.3.0.RC02.130475354-53000263 5.1 phone TRANSLATE_OPM5_TEST_1");
            FormUrlEncodedContent Content = new FormUrlEncodedContent(data);
            HttpResponseMessage Response = client.PostAsync(Url, Content).Result;
            return Response.Content.ReadAsStringAsync().Result;
        }
        static void CallWithTimeout(Action action, int timeoutMilliseconds)
        {
            Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                try
                {
                    action();
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                //throw new TimeoutException();
            }
        }
        static string Data = "";
        static void GetData()
        {
            string url = "https://translate.google.com/translate_a/single?client=at&dt=t&dt=ld&dt=qca&dt=rm&dt=bd&dj=1&hl=es-ES&ie=UTF-8&oe=UTF-8&inputm=2&otf=2&iid=1dd3b944-fa62-4b55-b330-74909a99969e";
            string data = sendData(url, new Dictionary<string, string>
            {
                { "sl" , requestSetting.source},
                { "tl" , requestSetting.target},
                {  "q" , requestSetting.text}
            });
            Data = data;
        }
        public static string Translate()
        {
            while (true)
            {
                if (Data == "")
                {
                    CallWithTimeout(GetData, 1000);
                }
                else
                {
                    string temp = Data;
                    Data = "";
                    return temp;
                }
            }
        }
    }
}
