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
        private readonly List<int> IdFile = new List<int>();

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

            btnChose.Click += (s, e)=>
            {
                if(IdFile.Count != 0) 
                {
                    foreach (var item in IdFile)
                    {
                        publicClassAndroid.FileSelected.Add($"{this_pats}{name_Folder[item]}");
                    }
                    this_pats = "selected file " + '\u2713';
                }
                publicClassAndroid.Folderpath = this_pats;
                StartActivity(typeof(MainActivity));
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
                IdFile.Clear();
                btnChose.Text = "انتخاب پوشه";
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
            if(Path.GetExtension($"{this_pats}{name_Folder[e.Position]}").ToUpper() == ".SRT") 
            {
                foreach (var item in IdFile)
                {
                    if(item == e.Position) 
                    {
                        TextView txtv = e.View as TextView;
                        txtv.Text = name_Folder[e.Position];
                        IdFile.Remove(item);
                        if(IdFile.Count == 0) 
                        {
                            btnChose.Text = "انتخاب پوشه";
                        }
                        return;
                    }
                }
                IdFile.Add(e.Position);
                TextView txt = e.View as TextView;
                txt.Text = '\u2713' + " " + txt.Text;
                btnChose.Text = "انتخاب فایل ها";

            }
            else
            {
                IdFile.Clear();
                show_Folder($"{this_pats}{name_Folder[e.Position]}/");
            }
            
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

                List<string> ListStr = new List<string>();

                ListStr.AddRange(Directory.EnumerateFiles(path));

                foreach (var item in ListStr)
                {
                    if(Path.GetExtension(item).ToUpper() == ".SRT") 
                    {
                        FolderArrayAddresFull.Add(item);
                    }
                }
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