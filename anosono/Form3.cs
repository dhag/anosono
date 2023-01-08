using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace anosono
{
    public partial class Form3 : Form
    {

        public Form1 form1;
        public Form3()
        {
            InitializeComponent();
        }

        private void button1__1_Click(object sender, EventArgs e)
        {

            var initialFolder = Environment.CurrentDirectory;
            if (!string.IsNullOrWhiteSpace(textBox1__1.Text))
            {
                initialFolder = textBox1__1.Text;
            }
            FolderDialog.Bll.FolderDialog.ISelect dialog = new FolderDialog.Bll.FolderDialog.Select();
            dialog.InitialFolder = initialFolder;// "C:\\";
            if (DialogResult.OK == dialog.ShowDialog())
            {
                var rootFolder = dialog.Folder;
                var config=form1.GetConfigClone();
                var aFolder = rootFolder + @"\" + config.AnnotationFileFolder;
                var tFolder = rootFolder + @"\" + config.TrainImageFileFolder;
                var vFolder = rootFolder + @"\" + config.ValidImageFileFolder;
                if ((Directory.Exists(vFolder)) || (Directory.Exists(tFolder)) || (Directory.Exists(aFolder))) 
                {
                    MessageBox.Show("このフォルダはすでにあるプロジェクトフォルダのようです。別のフォルダ（たぶん、もう一つ上のフォルダ？）を指定してください。");

                }
                else
                {
                    textBox1__1.Text = rootFolder;

                    List<string> nameList = new List<string>();
                    //TextBox2Config();
                    DirectoryInfo di = new DirectoryInfo(dialog.Folder);
                    // ディレクトリ直下のすべてのディレクトリ一覧を取得する
                    DirectoryInfo[] diAlls = di.GetDirectories();
                    foreach (DirectoryInfo diFile in diAlls)
                    {
                        nameList.Add(diFile.Name);
                    }


                    var path = NewNameCreator.CreateA(nameList, Path.GetFileName(config.ProjectFolderFullPath), 3);
                    //Directory.CreateDirectory(path);
                    textBox1__2.Text = path;


                    textBox1__1.Visible = true;
                    textBox1__2.Visible = true;
                    button1__2.Visible = true;
                }

            }

        }

        private void button1__2_Click(object sender, EventArgs e)
        {
            string fullPath = textBox1__1.Text+ @"\" + textBox1__2.Text;

            if (Directory.Exists(fullPath))
            {
                Console.WriteLine("すでにそのフォルダは存在します");
            }
            else
            {
                //Console.WriteLine("存在しません");
                Directory.CreateDirectory(fullPath);
                Directory.CreateDirectory(fullPath+ @"\"+ "annotations");
                Directory.CreateDirectory(fullPath + @"\" + "train_and_val");
                Directory.CreateDirectory(fullPath + @"\" + "train2017");
                Directory.CreateDirectory(fullPath+ @"\" + "val2017");

                button1__2.Visible = false;
                textBox2__1.Text = fullPath;
                panel1__1.Visible = true;
                localConfigMode = new List<bool>() { false, false, false };
            }


        }

        private void button0__1_Click(object sender, EventArgs e)
        {
            SetFolders();
            this.Close();
        }


        List<bool> checkFoloders(string fullPath,bool isCreateFolder)
        {
            bool mm0 = false;
            bool mm1 = false;
            bool mm2 = false;
            var config =form1.GetConfigClone();
            var AnnotationFileFolderFullpath = fullPath + @"\" + config.AnnotationFileFolder;
            var AllImageFileFolderFullpath = fullPath + @"\" + config.AllImageFileFolder;
            var TrainImageFileFolderFullpath = fullPath + @"\" + config.TrainImageFileFolder;
            var ValidImageFileFolderFullpath = fullPath + @"\" + config.ValidImageFileFolder;

            var AllAnnotationFileNameFullpath = AnnotationFileFolderFullpath + @"\" + config.AllAnnotationFileName;
            var TrainAnnotationFileNameFullpath = AnnotationFileFolderFullpath + @"\" + config.TrainAnnotationFileName;
            var ValidAnnotationFileNameFullpath = AnnotationFileFolderFullpath + @"\" + config.ValidAnnotationFileName;
            if (!Directory.Exists(AnnotationFileFolderFullpath))
            {
                if (isCreateFolder)
                {
                    MessageBox.Show("フォルダ" + config.AnnotationFileFolder + "がありません.作成します");
                    Directory.CreateDirectory(AnnotationFileFolderFullpath);
                }
            }
            if (!Directory.Exists(AllImageFileFolderFullpath))
            {
                if (isCreateFolder)
                {
                    MessageBox.Show("フォルダ" + config.AllImageFileFolder + "がありません.作成します");
                    Directory.CreateDirectory(AllImageFileFolderFullpath);
                }
            }
            else if(File.Exists(AllAnnotationFileNameFullpath))
            {//フォルダあり。ファイルあり。
                mm0 = true;
            }
            if (!Directory.Exists(TrainImageFileFolderFullpath))
            {
                if (isCreateFolder)
                {
                    MessageBox.Show("フォルダ" + config.TrainImageFileFolder + "がありません.作成します");
                    Directory.CreateDirectory(TrainImageFileFolderFullpath);
                }
            }
            else if (File.Exists(TrainAnnotationFileNameFullpath))
            {
                mm1 = true;
            }

            if (!Directory.Exists(ValidImageFileFolderFullpath))
            {
                if (isCreateFolder)
                {
                    MessageBox.Show("フォルダ" + config.ValidImageFileFolder + "がありません.作成します");
                    Directory.CreateDirectory(ValidImageFileFolderFullpath);
                }
            }
            else if (File.Exists(ValidAnnotationFileNameFullpath))
            {
                mm2 = true;
            }

            return new List<bool>() { mm0, mm1, mm2 };
        }


        private void button2__1_Click(object sender, EventArgs e)
        {
            var config = form1.GetConfigClone();

            var initialFolder = Environment.CurrentDirectory;
            if (!string.IsNullOrWhiteSpace(textBox2__1.Text))
            {
                initialFolder = textBox2__1.Text;
                if (!Directory.Exists(initialFolder))
                {
                    initialFolder=Path.GetDirectoryName(initialFolder);
                }


            }
            FolderDialog.Bll.FolderDialog.ISelect dialog = new FolderDialog.Bll.FolderDialog.Select();
            dialog.InitialFolder = initialFolder;// "C:\\";
            if (DialogResult.OK == dialog.ShowDialog())
            {
                var fullPath = dialog.Folder;
                textBox2__1.Text = fullPath;
                if (Directory.Exists(fullPath))
                {

                    localConfigMode= checkFoloders(fullPath,false);
                    if (localConfigMode[2] == true)
                    {
                        radioButton2__3.Visible = true;
                        radioButton2__3.Enabled = true;
                        radioButton2__3.Text = config.ValidImageFileFolder;
                        radioButton2__3.Checked = true;
                    }
                    if (localConfigMode[1] == true)
                    {
                        radioButton2__2.Visible = true;
                        radioButton2__2.Enabled = true;
                        radioButton2__2.Text = config.TrainImageFileFolder;
                        radioButton2__2.Checked = true;

                    }
                    if (localConfigMode[0] == true)
                    {
                        radioButton2__1.Visible = true;
                        radioButton2__1.Enabled=true;
                        radioButton2__1.Text = config.AllImageFileFolder;
                        radioButton2__1.Checked = true;
                    }

                    textBox2__1.Visible = true;
                    button0__1.Visible = true;
                }
                else
                {
                    MessageBox.Show("フォルダがありません");
                }
            }
        }
        List<bool> localConfigMode = new List<bool>() { false, false, false };
        void SetFolders()
        {
            Config _config = form1.GetConfigClone();
            _config.ProjectFolderFullPath = textBox2__1.Text;
            var m = 0;
            if (radioButton2__1.Checked) {
                m = 1;
            }
            else if (radioButton2__2.Checked)
            {
                m = 2;
            }
            else if (radioButton2__3.Checked)
            {
                m = 3;
            }
            _config.annotationMode = m;
             form1.UpdateConfig(_config,false);
        }


        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            // ドラッグ＆ドロップされたファイル
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            Config _config = form1.GetConfigClone();
            var projectFolder= textBox2__1.Text; 
            var allFolder= _config.AllImageFileFolder;
            string er = "";
            foreach (var f in files)
            {
                try
                {
                    var fname=Path.GetFileName(f);
                    File.Copy(f, projectFolder + @"\" + allFolder + @"\"+fname);//上書き禁止, true);
                }
                catch (Exception ex)
                { 
                    if(ex is System.IO.IOException)
                    {
                        er+= Path.GetFileName(f)+System.Environment.NewLine;
                    }
                }

            }
            if(!string.IsNullOrWhiteSpace(er))
            {
                MessageBox.Show("すでに存在するファイルはコピーしません。" + System.Environment.NewLine + er);
            }
            button0__1.Visible = true;
        }


        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.FileDrop))
            //
            //    e.Effect = DragDropEffects.Copy;
            //}

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                // ドラッグ中のファイルやディレクトリの取得
                string[] drags = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string d in drags)
                {
                    if (!System.IO.File.Exists(d))
                    {
                        // ファイル以外であればイベント・ハンドラを抜ける.ドロップは発生しない。
                        return;
                    }
                }
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void Form3_Shown(object sender, EventArgs e)
        {
            var config = form1.GetConfigClone();
            textBox1__1.Text = Path.GetDirectoryName(config.ProjectFolderFullPath);
            textBox1__2.Text = Path.GetFileName(config.ProjectFolderFullPath);
            textBox2__1.Text = config.ProjectFolderFullPath ;
        }
    }
}
