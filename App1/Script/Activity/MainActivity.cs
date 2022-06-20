using Android.App;
using Android.OS;
using Android.Runtime;
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
using System.IO;

namespace App1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            #region Buttons

            Android.Widget.Button btnaotutransleat = FindViewById<Android.Widget.Button>(Resource.Id.aouttransleat);
            Android.Widget.Button btncrateproject = FindViewById<Android.Widget.Button>(Resource.Id.CrateProject);

            btnaotutransleat.Click += (s, e) =>
            {
                publicClassAndroid.enumBtnStatuse = backButtonStatuse.autoTransleat;
                StartActivity(typeof(auto_transleat));
                Finish();
            };

            btncrateproject.Click += (s, e) =>
            {
                publicClassAndroid.enumBtnStatuse = backButtonStatuse.crateProject;
                StartActivity(typeof(CratenewProject));
                Finish();
            };

            #endregion
        }
        protected override void OnStart()
        {
            base.OnStart();
            if (Build.VERSION.SdkInt > BuildVersionCodes.Lollipop)
            {
                if (ContextCompat.CheckSelfPermission(this, "android.permission.WRITE_EXTERNAL_STORAGE") != Permission.Granted)
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
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                try
                {
                    File.WriteAllText("/storage/emulated/0/file.t", "");
                    File.Delete("/storage/emulated/0/file.t");
                }
                catch (Exception)
                {
                    Intent intent = new Intent(Android.Provider.Settings.ActionManageAppAllFilesAccessPermission);
                    intent.SetData(Android.Net.Uri.Parse($"package:{Application.Context.PackageName}"));
                    StartActivity(intent);
                }
            }
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
                    //await sherText(@"شده اموزش زبان اصلی دانلود کنی
                    //ولی زیر نویس فارسی نداشته باشه؟
                    //یا فیلم و سریال قدیمی که زیرنویس فارسی هماهنگ پیدا نکنی؟
                    //با دریافت این اپلیکشن راحت زیر نویس ها رو از هر زبانی به فارسی ترجمه کن
                    //https://www.charkhoneh.com/content/931172500");

                    await sherText(@"شده اموزش زبان اصلی دانلود کنی
                    ولی زیر نویس فارسی نداشته باشه؟
                    یا فیلم و سریال قدیمی که زیرنویس فارسی هماهنگ پیدا نکنی؟
                    با دریافت این اپلیکشن راحت زیر نویس ها رو از هر زبانی به فارسی ترجمه کن
                    https://myket.ir/app/myc.supernova.substitutetranslate");
                });
            }
            else if (id == Resource.Id.github)
            {
                var uri = Android.Net.Uri.Parse("https://github.com/Zakarea79");
                var inte = new Intent(Intent.ActionView, uri);
                StartActivity(inte);
            }
            else if (id == Resource.Id.myket)
            {
                var uri = Android.Net.Uri.Parse("https://myket.ir/developer/dev-65034");
                //var uri = Android.Net.Uri.Parse("https://www.charkhoneh.com/collection/78692/game/110806");
                var inte = new Intent(Intent.ActionView, uri);
                StartActivity(inte);
            }
            else if (id == Resource.Id.wondo)
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
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

