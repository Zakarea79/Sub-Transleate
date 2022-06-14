using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Interop;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace App1
{
    [Activity(Label = "Activity1", Theme = "@style/AppTheme")]
    public class VidoPlayer : Activity
    {
        private VideoView video;
        private TableLayout tableLayout;
        private TextView textViewSub;
        private List<View> MyViewColection = new List<View>();
        private ImageView ButtonPlay;
        private SeekBar TimeLine;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.AppTheme);
            SetContentView(Resource.Layout.VidoioPlayer);

            video = FindViewById<VideoView>(Resource.Id.videoView1);
            tableLayout = FindViewById<TableLayout>(Resource.Id.tableLayoutMain);
            textViewSub = FindViewById<TextView>(Resource.Id.textViewSub);
            ButtonPlay = FindViewById<ImageView>(Resource.Id.playPuseButton);
            TimeLine = FindViewById<SeekBar>(Resource.Id.timeline);

            TimeLine.Progress = 0;

            ButtonPlay.Click += (s, e) =>
            {
                if(video.IsPlaying == true)
                {
                    video.Pause();
                }
                else
                {
                    video.Start();
                }
            };


            LayoutInflater layoutInflater = LayoutInflater.From(this);
            View view = layoutInflater.Inflate(Resource.Layout.CustomView, null);

            RelativeLayout tepm = view as RelativeLayout;

            var tempview = tepm.GetChildAt(1) as TextView;
            tempview.Visibility = ViewStates.Invisible;
            tempview.Text = "1";
            MyViewColection.Add(view);
            tableLayout.AddView(view);


            video.SetVideoPath("/storage/emulated/0/Download/supernova.mp4");
            Task.Run(() => 
            {
                while (true)
                {
                    Thread.Sleep(900);
                    RunOnUiThread(() =>
                    {
                        int s = (video.CurrentPosition / 1000);
                        textViewSub.Text = s.ToString();
                        TimeLine.Progress = s;
                    });
                }
            });
            
            //video.pro
        }


        [Export("BtnClick")]
        public void BtnClick(View v)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            View view = layoutInflater.Inflate(Resource.Layout.CustomView, null);

            RelativeLayout tepm = view as RelativeLayout;

            var tempview = tepm.GetChildAt(1) as TextView;
            tempview.Text = (MyViewColection.Count + 1).ToString();

            tempview.Visibility = ViewStates.Invisible;
            MyViewColection.Add(view);
            tableLayout.AddView(view);
        }
    }
}