﻿using Android.App;
using Android.OS;
using Android.Widget;
using sub_Transleator_x;
using System.IO;
using System.Collections.Generic;
namespace App1
{
    [Activity(Label = "CratenewProject", Theme = "@style/AppTheme.NoActionBar")]
    public class CratenewProject : Activity
    {
        private Button buttonSelectFile, buttonCrateProject;
        private ImageView backVisullButton;
        private TextView TextFilePath;
        private Spinner FirstLang, secendLang;
        private ListView ProjectListView;
        private string PathJsonFile = $"/storage/emulated/0/Android/data/myc.supernova.substitutetranslate/";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.crate_new_project);

            #region Set ID
            buttonSelectFile = FindViewById<Button>(Resource.Id.chFile);
            buttonCrateProject = FindViewById<Button>(Resource.Id.CarteProject);
            backVisullButton = FindViewById<ImageView>(Resource.Id.buttonBak);
            TextFilePath = FindViewById<TextView>(Resource.Id.Textselectfile);
            FirstLang = FindViewById<Spinner>(Resource.Id.spinnerFromProject);
            secendLang = FindViewById<Spinner>(Resource.Id.spinnerToProject);
            ProjectListView = FindViewById<ListView>(Resource.Id.listViewProject);
            #endregion

            backVisullButton.Click += (s, e) =>
            {
                StartActivity(typeof(MainActivity));
                Finish();
            };

            string[] array = Directory.GetFiles(PathJsonFile);
            List<string> tmp = new List<string>();
            foreach (var item in array)
            {
                if (Path.GetExtension(item).ToUpper() == ".JSON")
                {
                    tmp.Add(Path.GetFileNameWithoutExtension(item));
                }
            }
            ProjectListView.ItemClick += Click_List_View;
            ProjectListView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, tmp);

            if (publicClassAndroid.FileSelected.Count != 0)
                TextFilePath.Text = publicClassAndroid.FileSelected[0];

            FirstLang.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, new TransleatClass().slect_lang);
            secendLang.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, new TransleatClass().slect_lang);

            FirstLang.SetSelection(publicClassAndroid.From);
            secendLang.SetSelection(publicClassAndroid.To);

            FirstLang.ItemSelected += (s, e) =>
            {
                publicClassAndroid.From = e.Position;
            };
            secendLang.ItemSelected += (s, e) =>
            {
                publicClassAndroid.To = e.Position;
            };

            buttonSelectFile.Click += (s, e) =>
            {
                publicClassAndroid.enumBtnStatuse = backButtonStatuse.crateProject;
                StartActivity(typeof(FolderPicker));
                Finish();
            };
            buttonCrateProject.Click += (s, e) =>
            {
                if (publicClassAndroid.FileSelected.Count != 0)
                {
                    publicClassAndroid.info.pathSrtFile = publicClassAndroid.FileSelected[0];
                    publicClassAndroid.info.form = publicClassAndroid.From;
                    publicClassAndroid.info.to = publicClassAndroid.To;
                    if (publicClassAndroid.info.pathSrtFile != null)
                    {
                        StartActivity(typeof(HandelSub));
                        Finish();
                    }
                }
            };
        }
        private void Click_List_View(object sender, AdapterView.ItemClickEventArgs e)
        {
            TextView textView = e.View as TextView;

            var jsonformat = File.ReadAllText(PathJsonFile + $"{textView.Text}.json");
            var jsondata = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectInfo>(jsonformat);

            publicClassAndroid.info.pathSrtFile = jsondata.pathSrtFile;
            publicClassAndroid.info.form = jsondata.form;
            publicClassAndroid.info.to = jsondata.to;
            StartActivity(typeof(HandelSub));
            Finish();
        }
        public override void OnBackPressed()
        {
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }
}