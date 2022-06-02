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
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private Android.Widget.ListView listView_;
        private Android.Widget.TextView TxtOpenFolder;
        private Android.Widget.TextView Prograss;
        private Android.Widget.Button BtnOpenFolder, BtnOpenStart;
        private Android.Widget.Spinner SpierFrom, SpierTo;
        private Android.Widget.ProgressBar progressB;

        private List<string> ComplectFile = new List<string>();

        public static Data data = new Data();


        private bool btnChack = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);


            //threadGetText = new Thread(new ParameterizedThreadStart(GetTextThread));

            #region ElemetPage
            BtnOpenFolder = FindViewById<Android.Widget.Button>(Resource.Id.MainActivityBtnOpenFolder);
            BtnOpenStart = FindViewById<Android.Widget.Button>(Resource.Id.MainActivityBtnStartTranslet);
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


            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);


            #region AddSpener

            SpierFrom.Adapter = new Android.Widget.ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, new TransleatClass().slect_lang);
            SpierTo.Adapter = new Android.Widget.ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, new TransleatClass().slect_lang);

            SpierFrom.SetSelection(publicClassAndroid.From);
            SpierTo.SetSelection(publicClassAndroid.To);
            //SpierFrom.setSelectedPositionInt();

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
            BtnOpenFolder.Click += (s, e) =>
            {
                
                StartActivity(typeof(FolderPicker));
                Finish();
            };

            BtnOpenStart.Click += (s, e) =>
            {
                if(btnChack == false) 
                {
                    StartTransleat();
                    btnChack = true;
                    publicClassAndroid.StButtonStatuse = false;
                }
                else if(publicClassAndroid.StButtonStatuse == false)
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
                if(btnChack == true)
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
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (ContextCompat.CheckSelfPermission(this, "ndroid.permission.WRITE_EXTERNAL_STORAGE") != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.WriteExternalStorage }, 0);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Permission Granted!!!");
            }
            if (ContextCompat.CheckSelfPermission(this, "android.permission.READ_EXTERNAL_STORAGE") != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.ReadExternalStorage }, 0);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Permission Granted!!!");
            }
        }
        
        public void addItem(ref List<string> list, ref MainActivity act)
        {
            listView_.Adapter = new Android.Widget.ArrayAdapter(act, Android.Resource.Layout.SimpleListItem1, list);
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            //int id = item.ItemId;
            //if (id == Resource.Id.action_settings)
            //{
            //    return true;
            //}

            return base.OnOptionsItemSelected(item);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                var uri = Android.Net.Uri.Parse("https://t.me/Supernovaofficiall");
                var inte = new Intent(Intent.ActionView, uri);
                StartActivity(inte);
            }
            else if (id == Resource.Id.nav_gallery)
            {
                var uri = Android.Net.Uri.Parse("https://instagram.com/supernovaofficiall");
                var inte = new Intent(Intent.ActionView, uri);
                StartActivity(inte);
            }
            else if (id == Resource.Id.nav_slideshow)
            {
                var uri = Android.Net.Uri.Parse("https://youtube.com/channel/UCuHvo9ZigMJYyrtQpEPwflw");
                var inte = new Intent(Intent.ActionView, uri);
                StartActivity(inte);
            }
            else if (id == Resource.Id.nav_share)
            {
                Task.Run(async () =>
                {
                    await sherText(@"شده اموزش زبان اصلی دانلود کنی
ولی زیر نویس فارسی نداشته باشه؟
یا فیلم و سریال قدیمی که زیرنویس فارسی هماهنگ پیدا نکنی؟
با دریافت اپلیکش راحت زیر نویس ها رو از هر زبانی به فارسی برگردون
https://myket.ir/app/myc.supernova.substitutetranslate");
                });
            }
            else if(id == Resource.Id.github) 
            {
                var uri = Android.Net.Uri.Parse("https://github.com/Zakarea79");
                var inte = new Intent(Intent.ActionView, uri);
                StartActivity(inte);
            }
            else if(id == Resource.Id.myket) 
            {
                var uri = Android.Net.Uri.Parse("https://myket.ir/developer/dev-65034");
                var inte = new Intent(Intent.ActionView, uri);
                StartActivity(inte);
            }
            else if(id == Resource.Id.wondo) 
            {
                var uri = Android.Net.Uri.Parse("https://supernovaofficial.blogsky.com/Getwindowsversion");
                var inte = new Intent(Intent.ActionView, uri);
                StartActivity(inte);
            }
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
        private async Task sherText(string text)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "اشتراک برنامه"
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

