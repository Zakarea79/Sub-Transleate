//using Android.App;
//using Android.Content;
//using Android.Webkit;
//using Android.Widget;

//namespace WebView
//{
//    class MyJSInterface : Java.Lang.Object
//    {
//        Context context;

//        public MyJSInterface(Context context)
//        {
//            this.context = context;
//        }

//        public void ShowToast()
//        {
//            Toast.MakeText(context, "Hello from C#", ToastLength.Short).Show();
//        }
//    }


//    class EvaluateBack : Java.Lang.Object, IValueCallback
//    {
//        public void OnReceiveValue(Java.Lang.Object value)
//        {
//            //if (value.ToString() != "null")
//            {
//                App1.MainActivity.WebResult = value.ToString();
//                Toast.MakeText(Application.Context, value.ToString(), ToastLength.Short).Show();
//            }
//        }
//    }

//}