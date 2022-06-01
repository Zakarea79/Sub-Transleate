using Android.App;
using Android.OS;
using System.IO;
using Android.Widget;
using System.Collections.Generic;

namespace App1
{
    [Activity(Label = "@string/app_name")]
    public class FolderPicker : Activity
    {
        private string path_maian = "/storage/emulated/0/";
        private string this_pats = "/storage/emulated/0/";
        private ListView listFolder;
        private readonly List<string> FolderArrayAddresFull = new List<string>();
        private readonly List<string> name_Folder = new List<string>();

        private Button btnbackFolder;
        private Button btnChose;

        [System.Obsolete]
        #pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        protected override void OnCreate(Bundle savedInstanceState)
        #pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.folderpicker);

            btnbackFolder = FindViewById<Button>(Resource.Id.buttonFolderBack);
            btnChose = FindViewById<Button>(Resource.Id.buttonSelect);
            listFolder = FindViewById<ListView>(Resource.Id.listViewDirectory);

            show_Folder(path_maian);

            listFolder.ItemClick += Click_List_View;

            #region GetPermison
            
            #endregion

            btnChose.Click += (s, e)=>
            {
                StartActivity(typeof(MainActivity));
                publicClassAndroid.Folderpath = this_pats;
                Finish();
            };

            btnbackFolder.Click += (s, e) => 
            {
                if (this_pats == "/storage/emulated/")
                    return;
                string bakfol = "";
                for (int i = 0; i < this_pats.Split('/').Length -2; i++)
                {
                    bakfol += this_pats.Split('/')[i] + '/';
                }
                show_Folder(bakfol);
            };
        }
        public override void OnBackPressed()
        {
            StartActivity(typeof(MainActivity));
            Finish();
        }
        private void Click_List_View(object sender , AdapterView.ItemClickEventArgs e) 
        {
            show_Folder($"{this_pats}{name_Folder[e.Position]}/");
        }

        private void show_Folder(string path) 
        {
            if (path == "/storage/emulated/")
                return;
            name_Folder.Clear();

            FolderArrayAddresFull.Clear();
            //Toast.MakeText(this, path, ToastLength.Long).Show();
            try 
            {
                FolderArrayAddresFull.AddRange(Directory.GetDirectories(path));
            }
            catch(System.Exception ex) 
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                return;
            }
            
            foreach (var item in FolderArrayAddresFull)
            {
                name_Folder.Add(item.Split('/')[item.Split('/').Length - 1]);
            }
            this_pats = path;
            name_Folder.Sort();
            listFolder.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, name_Folder);
        }
    }
}