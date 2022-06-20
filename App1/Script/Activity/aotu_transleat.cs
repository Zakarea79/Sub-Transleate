using Android.App;
using Android.OS;
using System.Collections.Generic;
using Android.Runtime;
using sub_Transleator_x;
using System.Threading;
using Xamarin.Essentials;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.Navigation;
using Android.Content;
using System.Threading.Tasks;
using AndroidX.Core.App;
using Android.Content.PM;
using Android;
using AndroidX.Core.Content;
using System;

namespace App1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]//, MainLauncher = true)]
    public class auto_transleat : Activity
    {
        private Android.Widget.ListView listView_;
        private Android.Widget.TextView TxtOpenFolder;
        private Android.Widget.TextView Prograss;
        private Android.Widget.Button BtnOpenFolder, BtnOpenStart;
        private Android.Widget.ImageView buttonVisullback;
        private Android.Widget.Spinner SpierFrom, SpierTo;
        private Android.Widget.ProgressBar progressB;

        private List<string> ComplectFile = new List<string>();

        public static Data data = new Data();


        private bool btnChack = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.auto_transleat);

            #region ResatData
            publicClassAndroid.info.form = 0;
            publicClassAndroid.info.to = 0;
            publicClassAndroid.info.Transleat.Clear();
            publicClassAndroid.info.pathSrtFile = "";

            publicClassAndroid.To = 0;
            publicClassAndroid.From = 0;
            publicClassAndroid.StButtonStatuse = false;
            publicClassAndroid.Folderpath = "";
            publicClassAndroid.FileSelected.Clear();
            publicClassAndroid.enumBtnStatuse = backButtonStatuse.mainLayout;
            #endregion

            //threadGetText = new Thread(new ParameterizedThreadStart(GetTextThread));

            #region ElemetPage
            BtnOpenFolder = FindViewById<Android.Widget.Button>(Resource.Id.MainActivityBtnOpenFolder);
            BtnOpenStart = FindViewById<Android.Widget.Button>(Resource.Id.MainActivityBtnStartTranslet);
            buttonVisullback = FindViewById<Android.Widget.ImageView>(Resource.Id.buttonBak);
            TxtOpenFolder = FindViewById<Android.Widget.TextView>(Resource.Id.MainActivityTxtOpenFolder);
            listView_ = FindViewById<Android.Widget.ListView>(Resource.Id.listView1);
            SpierFrom = FindViewById<Android.Widget.Spinner>(Resource.Id.spinnerFrom);
            SpierTo = FindViewById<Android.Widget.Spinner>(Resource.Id.spinnerTo);
            progressB = FindViewById<Android.Widget.ProgressBar>(Resource.Id.progressBar1);
            Prograss = FindViewById<Android.Widget.TextView>(Resource.Id.textViewprogressBarPersent);
            #endregion

            TxtOpenFolder.Text = publicClassAndroid.Folderpath == "" ? "Unselected folder" : publicClassAndroid.Folderpath;

            progressB.Visibility = ViewStates.Invisible;
            Prograss.Visibility = ViewStates.Invisible;

            #region AddSpener

            SpierFrom.Adapter = new Android.Widget.ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, new TransleatClass().slect_lang);
            SpierTo.Adapter = new Android.Widget.ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, new TransleatClass().slect_lang);

            SpierFrom.SetSelection(publicClassAndroid.From);
            SpierTo.SetSelection(publicClassAndroid.To);

            SpierFrom.ItemSelected += (s, e) =>
            {
                publicClassAndroid.From = e.Position;
            };
            SpierTo.ItemSelected += (s, e) =>
            {
                publicClassAndroid.To = e.Position;
            };

            #endregion

            #region Button
            buttonVisullback.Click += (s, e) =>
            {
                publicClassAndroid.StButtonStatuse = true;
                btnChack = false;
                StartActivity(typeof(MainActivity));
                Finish();
            };

            BtnOpenFolder.Click += (s, e) =>
            {
                publicClassAndroid.enumBtnStatuse = backButtonStatuse.autoTransleat;
                StartActivity(typeof(FolderPicker));
                Finish();
            };

            BtnOpenStart.Click += (s, e) =>
            {
                if (btnChack == false)
                {
                    btnChack = true;
                    StartTransleat();
                    publicClassAndroid.StButtonStatuse = false;
                }
                else if (publicClassAndroid.StButtonStatuse == false)
                {
                    btnChack = false;
                    BtnOpenStart.Enabled = false;
                    publicClassAndroid.StButtonStatuse = true;
                }
            };
            #endregion
        }


        private async void StartTransleat()
        {
            if (TxtOpenFolder.Text != "Unselected folder")
            {
                bool chFile()
                {
                    if (TxtOpenFolder.Text == "selected file " + '\u2713')
                        return true;
                    var a = new List<string>();

                    foreach (var element in System.IO.Directory.GetFiles(TxtOpenFolder.Text))
                    {
                        if (System.IO.Path.GetExtension(element).ToUpper() == ".srt".ToUpper())
                        {
                            return true;
                        }
                    }
                    return false;
                }

                if (chFile() == false)
                {
                    Android.Widget.Toast.MakeText(this, "در مسیر انتخاب شده فایل معتبری پیدا نشد", Android.Widget.ToastLength.Long).Show();
                    btnChack = false;
                    return;
                }

                progressB.Progress = 0;
                Prograss.Text = "0%";

                progressB.Visibility = ViewStates.Visible;
                Prograss.Visibility = ViewStates.Visible;

                BtnOpenFolder.Enabled = false;
                BtnOpenStart.Text = "توقف ترجمه";

                SpierFrom.Enabled = false;
                SpierTo.Enabled = false;

                listView_.Adapter = new Android.Widget.ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, ComplectFile);

                var thread = new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        RunOnUiThread(() =>
                        {
                            Prograss.Text = data.Persent + "%";
                            progressB.Progress = data.Persent;
                            if (data.Name != null)
                            {
                                ComplectFile.Add(System.IO.Path.GetFileName(data.Name));
                                listView_.Adapter = new Android.Widget.ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, ComplectFile);
                                data.Name = null;
                            }
                        });
                        Thread.Sleep(20);
                    }
                }));

                thread.Start();

                await SubTransletor_Class.SubTransleatAsync(TxtOpenFolder.Text, publicClassAndroid.FileSelected);

                thread.Abort();
                if (btnChack == true)
                    Android.Widget.Toast.MakeText(this, "ترجمه انجام شد", Android.Widget.ToastLength.Long).Show();
                else
                    Android.Widget.Toast.MakeText(this, "ترجمه متوقف شد", Android.Widget.ToastLength.Long).Show();

                btnChack = false;
                BtnOpenFolder.Enabled = true;
                BtnOpenStart.Enabled = true;
                BtnOpenStart.Text = "شروع ترجمه";
                TxtOpenFolder.Text = "Unselected folder";
                progressB.Progress = 0;
                Prograss.Text = "0%";
                SpierFrom.Enabled = true;
                SpierTo.Enabled = true;

                progressB.Visibility = ViewStates.Invisible;
                Prograss.Visibility = ViewStates.Invisible;

                publicClassAndroid.FileSelected.Clear();
                ComplectFile.Clear();
            }
            else
            {
                Android.Widget.Toast.MakeText(this, "لطفا مسیری انتخاب کنید", Android.Widget.ToastLength.Long).Show();
                btnChack = false;
            }
        }
        //public void addItem(ref List<string> list, ref MainActivity act)
        //{
        //    listView_.Adapter = new Android.Widget.ArrayAdapter(act, Android.Resource.Layout.SimpleListItem1, list);
        //}
        public override void OnBackPressed()
        {
            publicClassAndroid.StButtonStatuse = true;
            btnChack = false;
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }
}

