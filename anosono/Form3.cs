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
                textBox1__1.Text = dialog.Folder;

                List <string> nameList = new List<string>();
                //TextBox2Config();
                DirectoryInfo di = new DirectoryInfo(dialog.Folder);
                // ディレクトリ直下のすべてのディレクトリ一覧を取得する
                DirectoryInfo[] diAlls = di.GetDirectories();
                foreach (DirectoryInfo diFile in diAlls)
                {
                    nameList.Add(diFile.Name);
                }
                var path = NewNameCreator.CreateA(nameList, "YoloXTrainning",3);
                //Directory.CreateDirectory(path);
                textBox1__2.Text = path;


                textBox1__1.Visible = true;
                textBox1__2.Visible = true;
                button1__2.Visible=true;


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
                Directory.CreateDirectory(fullPath+ @"\" + "train2017");
                Directory.CreateDirectory(fullPath+ @"\" + "val2017");

                textBox2__1.Text = fullPath;
                panel1__1.Visible = true;
            }

        }

        private void button0__1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2__1_Click(object sender, EventArgs e)
        {


            var initialFolder = Environment.CurrentDirectory;
            if (!string.IsNullOrWhiteSpace(textBox2__1.Text))
            {
                initialFolder = textBox2__1.Text;
            }
            FolderDialog.Bll.FolderDialog.ISelect dialog = new FolderDialog.Bll.FolderDialog.Select();
            dialog.InitialFolder = initialFolder;// "C:\\";
            if (DialogResult.OK == dialog.ShowDialog())
            {
                var fullPath = dialog.Folder;
                textBox2__1.Text = fullPath;
                if (Directory.Exists(fullPath))
                {
                    //Console.WriteLine("すでにそのフォルダは存在します");
                    if (!Directory.Exists(fullPath + @"\" + "annotations"))
                    {
                        MessageBox.Show("フォルダannotationsがありません.作成します");
                        Directory.CreateDirectory(fullPath + @"\" + "annotations");
                    }
                    if (!Directory.Exists(fullPath + @"\" + "train2017"))
                    {
                        MessageBox.Show("フォルダtrain2017がありません.作成します");
                        Directory.CreateDirectory(fullPath + @"\" + "train2017");

                    }
                    if (!Directory.Exists(fullPath + @"\" + "val2017"))
                    {
                        MessageBox.Show("フォルダval2017がありません.作成します");
                        Directory.CreateDirectory(fullPath + @"\" + "val2017");

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

            public void getfolders(
            out string projectFolder,
            out string AnnotationFolder,
            out string TrainingFolder,
            out string ValidationFolder
            )
        {
            projectFolder = textBox2__1.Text;
            AnnotationFolder="annotations";
            TrainingFolder = "train2017";
            ValidationFolder= "val2017";
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            // ドラッグ＆ドロップされたファイル
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            getfolders(
            out string projectFolder,
            out string AnnotationFolder,
            out string TrainingFolder,
            out string ValidationFolder
            );

            string er = "";
            foreach (var f in files)
            {
                try
                {
                    var fname=Path.GetFileName(f);
                    File.Copy(f, projectFolder + @"\" + TrainingFolder+ @"\"+fname);//上書き禁止, true);
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




    }
}
