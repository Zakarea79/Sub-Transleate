using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App1
{
    [Activity(Label = "Activity1" , MainLauncher = true)]
    public class Activity1 : Activity
    {

        private VideoView video;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VidoioPlayer);

            video = FindViewById<VideoView>(Resource.Id.videoView1);

            video.SetVideoPath("");
        }
    }
}