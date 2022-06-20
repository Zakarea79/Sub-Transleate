using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using sub_Transleator_x;
using Newtonsoft.Json;
using Java.Interop;
using System.IO;
using Android.Text;
using System;
using static Android.Views.View;
using System.Threading.Tasks;

namespace App1
{
    [Activity(Label = "Activity1", Theme = "@style/AppTheme.NoActionBar")]
    public class HandelSub : Activity
    {
        private LinearLayout  daealogmaneager , linearLayoutDalog;
        private TableLayout tableLayout;
        private readonly List<View> MyViewColection = new List<View>();
        private ImageView btn_bak, btn_Save, btn_Delet;
        private bool Chproject = false;
        private readonly List<EditText> editTextsList = new List<EditText>();
        private readonly List<myDictionary> FileInfov = new List<myDictionary>();
        private readonly string PathJsonFile = $"/storage/emulated/0/subproject/{Path.GetFileNameWithoutExtension(publicClassAndroid.info.pathSrtFile)}.json";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.handel_transleat);

            #region SetID
            tableLayout = FindViewById<TableLayout>(Resource.Id.tableLayoutMain);
            daealogmaneager = FindViewById<LinearLayout>(Resource.Id.Daealog);


            //-----------------------------------------------------------------------------------
            btn_Save = FindViewById<ImageView>(Resource.Id.buttonSave);
            btn_bak = FindViewById<ImageView>(Resource.Id.buttonBak);
            btn_Delet = FindViewById<ImageView>(Resource.Id.buttonDeleate);
            #endregion


            btn_bak.Click += (s, e) =>
            {
                backButtonControll = true;
                if (Chproject == true)
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(this);
                    View view = layoutInflater.Inflate(Resource.Layout.DalogSaveView, null);
                    daealogmaneager.AddView(view);
                    linearLayoutDalog = view as LinearLayout;
                    //---------------------------------------
                    btn_bak.Visibility = ViewStates.Invisible;
                    btn_Save.Visibility = ViewStates.Invisible;
                    btn_Delet.Visibility = ViewStates.Invisible;
                    //---------------------------------------
                }
                else
                {
                    StartActivity(typeof(CratenewProject));
                    Finish();
                }
            };
            btn_Save.LongClick += longClick;
            btn_Save.Click += (s, e) =>
            {
                Chproject = false;
                publicClassAndroid.info.Transleat.Clear();
                foreach (var item in editTextsList)
                {
                    publicClassAndroid.info.Transleat.Add(item.Text);
                }
                var tmp = JsonConvert.SerializeObject(publicClassAndroid.info);
                File.Delete(PathJsonFile);
                File.WriteAllText(PathJsonFile, tmp);
                Toast.MakeText(this, "ذخیره شد", ToastLength.Short).Show();
            };
            btn_Delet.Click += (s, e) =>
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View view = layoutInflater.Inflate(Resource.Layout.DalogDeleteView, null);
                daealogmaneager.AddView(view);
                linearLayoutDalog = view as LinearLayout;
                //------------------------------------------
                var scr = tableLayout.Parent as ScrollView; 
                scr.Enabled = false;
                //---------------------------------------
                btn_bak.Visibility = ViewStates.Invisible;
                btn_Save.Visibility = ViewStates.Invisible;
                btn_Delet.Visibility = ViewStates.Invisible;
                //---------------------------------------
                //linearLayoutDelet.Visibility = ViewStates.Visible;
            };

            if (File.Exists(PathJsonFile) == true)
            {
                var jsondata = File.ReadAllText(PathJsonFile);
                publicClassAndroid.info = JsonConvert.DeserializeObject<ProjectInfo>(jsondata);
            }

            FileInfov.AddRange(new ReadSub().ReadFile(publicClassAndroid.info.pathSrtFile));

            for (int i = 0; i < FileInfov.Count; i++)
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View view = layoutInflater.Inflate(Resource.Layout.CustomViewSubTransleat, null);

                LinearLayout tepm = view as LinearLayout;

                var tempTextView = tepm.GetChildAt(1) as TextView;
                tempTextView.Text = FileInfov[i].info; //item.Key;

                var tempview = tepm.GetChildAt(3) as TextView;
                tempview.Text = FileInfov[i].text;//item.Value;

                var linearLayout = tepm.GetChildAt(5) as LinearLayout;

                var EditText = linearLayout.GetChildAt(0) as EditText;

                EditText.TextChanged += changeText;

                editTextsList.Add(EditText);

                if (File.Exists(PathJsonFile) == false)
                    publicClassAndroid.info.Transleat.Add("");
                else
                {
                    var liner = tepm.GetChildAt(5) as LinearLayout;
                    var textEdit = liner.GetChildAt(0) as EditText;
                    textEdit.Text = publicClassAndroid.info.Transleat[i];
                }
                MyViewColection.Add(view);
                tableLayout.AddView(view);
            }

            if (File.Exists(PathJsonFile) == false)
            {
                string jsondata = JsonConvert.SerializeObject(publicClassAndroid.info);
                File.WriteAllText($"/storage/emulated/0/subproject/{Path.GetFileNameWithoutExtension(publicClassAndroid.info.pathSrtFile)}.json", jsondata);
            }
            Chproject = false;
        }

        [Export("BtnClick")]
        public async void BtnClickAsync(View v)
        {
            v.Enabled = false;
            LinearLayout view = (LinearLayout)v.Parent;
            LinearLayout mainParint = (LinearLayout)view.Parent;
            var tempview = mainParint.GetChildAt(3) as TextView;
            var TempEditText = view.GetChildAt(0) as EditText;
            await Task.Run(() => 
            {
                RunOnUiThread(() => 
                {
                    try
                    {
                        publicClassAndroid.From = publicClassAndroid.info.form;
                        publicClassAndroid.To = publicClassAndroid.info.to;
                        TempEditText.Text = TransleatClass.Translate(tempview.Text);
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(this, "اتصال اینترنت را برسی کنید", ToastLength.Short).Show();
                    }
                    
                });
            });
            v.Enabled = true;
        }

        [Export("Deleatroject")]
        public void Deleatroject(View v)
        {
            backButtonControll = true;
            File.Delete(PathJsonFile);
            Toast.MakeText(this, "حذف شد", ToastLength.Short).Show();
            StartActivity(typeof(CratenewProject));
            Finish();
        }
        [Export("CanselDeleatProject")]
        public void CanselDeleatProject(View v)
        {
            backButtonControll = false;
            daealogmaneager.RemoveView(linearLayoutDalog);
            //----------------------
            btn_bak.Visibility = ViewStates.Visible;
            btn_Save.Visibility = ViewStates.Visible;
            btn_Delet.Visibility = ViewStates.Visible;
            //-----------------------------------------------------
        }
        [Export("SaveProject")]
        public void SaveProject(View v)
        {
            publicClassAndroid.info.Transleat.Clear();
            foreach (var item in editTextsList)
            {
                publicClassAndroid.info.Transleat.Add(item.Text);
            }
            var tmp = JsonConvert.SerializeObject(publicClassAndroid.info);
            File.Delete(PathJsonFile);
            File.WriteAllText(PathJsonFile, tmp);
            Toast.MakeText(this, "ذخیره شد", ToastLength.Short).Show();
            StartActivity(typeof(CratenewProject));
            Finish();
        }
        [Export("CanselSaveProject")]
        public void CanselSaveProject(View v)
        {
            Toast.MakeText(this, "بازگشت بدون ذخیره سازی", ToastLength.Short).Show();
            StartActivity(typeof(CratenewProject));
            Finish();
        }
        [Export("SaveProjectFienall")]
        public void SaveProjectFienall(View v)
        {
            string strindata = "";
            for (int i = 0; i < FileInfov.Count; i++)
            {
                strindata += (FileInfov[i].info + editTextsList[i].Text + "\n\n");
            }
            try
            {
                File.WriteAllText("/storage/emulated/0/Download/"
                                + Path.GetFileNameWithoutExtension(publicClassAndroid.info.pathSrtFile)
                                + ".srt", strindata);
                Toast.MakeText(this, "با موفقیت در پوشه دانلود ذخیره شد", ToastLength.Short).Show();
                daealogmaneager.RemoveView(linearLayoutDalog);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
            btn_bak.Visibility = ViewStates.Visible;
            btn_Save.Visibility = ViewStates.Visible;
            btn_Delet.Visibility = ViewStates.Visible;
        }

        private void longClick(object sender , LongClickEventArgs e) 
        {
            btn_bak.Visibility = ViewStates.Invisible;
            btn_Save.Visibility = ViewStates.Invisible;
            btn_Delet.Visibility = ViewStates.Invisible;
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            View view = layoutInflater.Inflate(Resource.Layout.DalogSaveView, null);
            daealogmaneager.AddView(view);
            linearLayoutDalog = view as LinearLayout;
        }
        bool backButtonControll = false;
        public override void OnBackPressed()
        {
            if (backButtonControll == false)
            {
                backButtonControll = true;
                if (Chproject == true)
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(this);
                    View view = layoutInflater.Inflate(Resource.Layout.DalogSaveView, null);
                    daealogmaneager.AddView(view);
                    linearLayoutDalog = view as LinearLayout;
                    //---------------------------------------
                    btn_bak.Visibility = ViewStates.Invisible;
                    btn_Save.Visibility = ViewStates.Invisible;
                    btn_Delet.Visibility = ViewStates.Invisible;
                    //---------------------------------------
                }
                else
                {
                    StartActivity(typeof(CratenewProject));
                    Finish();
                }
            }
        }
        private void changeText(object sender, TextChangedEventArgs e)
        {
            Chproject = true;
        }
    }
}