using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Web;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace anosono
{
    public partial class Form1 : Form
    {
        Form3 form3;

        public Config GetConfigClone()
        {
            Config c;
            lock (config)
            {
                c = config.CloneMyself();
            }
            return c;
        }
        public Form1()

        {
            Form1_LoadConfig();

            form3 = new Form3() { form1 = this };
            form3.ShowDialog();

            InitializeComponent();
        }

        private void button1_1__1_Click(object sender, EventArgs e)
        {
            var initialFolder = Environment.CurrentDirectory;
            if (!string.IsNullOrWhiteSpace(textBox1_1__1.Text))
            {
                initialFolder = textBox1_1__1.Text;
            }
            FolderDialog.Bll.FolderDialog.ISelect dialog = new FolderDialog.Bll.FolderDialog.Select();
            dialog.InitialFolder = initialFolder;// "C:\\";
            if (DialogResult.OK == dialog.ShowDialog())
            {
                textBox1_1__1.Text = dialog.Folder;
                TextBox2Config();

            }


            /*

            var dialog = new FolderSelectDialog
            {

                InitialDirectory = initialDirectory,// Environment.CurrentDirectory,
                Title = "Select a folder "
            };
            if (dialog.Show(Handle))
            {
                textBox1_1__1.Text = dialog.FileName;
            }
            */
        }

        private void button1_2__1_Click(object sender, EventArgs e)
        {
            viewImageList();
        }
        private void viewImageList()
        {
            var imagePackList_ = new List<ImagePack>();
            //var categoryList_ = new List<Coco.Categories>();

            if (checkBox1_2__1.Checked)
            {//追加モード
                imagePackList_ = imagePackList;
                //categoryList_ = categoryList;
            }
            else
            {
                if (!disposeMessage()) return;
            }

            string[] files = new string[0];
            try
            {
                files = System.IO.Directory.GetFiles(textBox1_1__1.Text + "/" + textBox1_1__2.Text, "*");
            }
            catch
            {

            }
            //ファイル名一覧を追加
            imagePackList = ImagePack.CreateNewImagePackList_(imagePackList_, files);//

            currentImagePack = imagePackList[0];
            comboBox1__1_Update();
            //ファイルの追加のみなので、カテゴリリストは不要
        }

        void loadAnnotationFile(string filename)
        {

            var imagePackList_ = new List<ImagePack>();
            var categoryList_ = new List<Coco.Categories>();
            if (checkBox1_3__1.Checked)
            {//追加モード
                imagePackList_ = imagePackList;
                categoryList_ = categoryList;
            }
            else
            {
                if (!disposeMessage()) return;
            }

            using (StreamReader reader = new StreamReader(filename))
            {
                var fileContent = reader.ReadToEnd();//丸ごと読む
                Coco cc = Coco.JsonToCoco(fileContent);//JSON読む
                if (cc != null)
                {
                    //----------------------------------
                    int categoryIDmin = 0;
                    foreach (var cat in categoryList)
                    {
                        if (categoryIDmin <= cat.id)
                        {
                            categoryIDmin = cat.id;
                        }
                    }
                    //カテゴリID修正A
                    int categoryIDOffset = categoryIDmin;
                    foreach (var cat in cc.categories)
                    {
                        cat.id += categoryIDOffset;
                    }
                    //カテゴリID修正B
                    foreach (var ann in cc.annotations)
                    {
                        ann.category_id += categoryIDOffset;
                    }

                    categoryList_.AddRange(cc.categories);
                    //----------------------------------
                    //イメージパックの差し替え
                    imagePackList = ImagePack.CreateImagePackList_(imagePackList_, cc);//
                    currentImagePack = imagePackList[0];
                    comboBox1__1_Update();
                    //categoryList_.AddRange(cc.categories);


                    categoryList = categoryList_;
                    comboBox1_4__1.Items.Clear();
                    foreach (var cat in categoryList)
                    {
                        comboBox1_4__1.Items.Add(cat.name);
                    }
                    comboBox1_4__1.Refresh();
                }
            }

        }




        private void button1_3__1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {

                    openFileDialog.InitialDirectory = textBox1_1__1.Text;
                    openFileDialog.Filter = "json files (*.json)|*.json|txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    //openFileDialog.FilterIndex = 2;
                    //openFileDialog.RestoreDirectory = true;

                    //アノテーションファイルの読み取り
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        loadAnnotationFile(openFileDialog.FileName);
                    }
                }

            }
            catch
            {

            }
        }



        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var mainPenA = System.Drawing.Pens.Red;
            var mainPenB = System.Drawing.Pens.Orange;
            var subPen = System.Drawing.Pens.Blue;
            Graphics g = e.Graphics;

            // Draw a string on the PictureBox.
            //g.DrawString("This is a diagonal line drawn on the control",
            //    fnt, System.Drawing.Brushes.Blue, new Point(30, 30));
            // Draw a line in the PictureBox.
            //g.DrawLine(System.Drawing.Pens.Red, pictureBox1.Left, pictureBox1.Top,
            //    pictureBox1.Right, pictureBox1.Bottom);

            //既存のバウンディングボックス描画
            foreach (var a in currentImagePack.annotationList)
            {
                var bX0 = a._BoundigBoxPosition.X;
                var bY0 = a._BoundigBoxPosition.Y;
                var bX1 = bX0 + a._BoundigBoxSize.X;
                var bY1 = bY0 + a._BoundigBoxSize.Y;
                paintRect(g, subPen, (int)bX0, (int)bY0, (int)bX1, (int)bY1);
            }
            //マウス移動中ならそれを書く
            if (mouseMode1)
            {
                var mainPen = mainPenA;
                if (isCorrectMode) mainPen = mainPenB;
                paintRect(g, mainPen, mouseStartPositionX, mouseStartPositionY, mouseCurrentPositionX, mouseCurrentPositionY);
            }
            //mainPenA.Dispose();
            //mainPenB.Dispose();
            //subPen.Dispose();
            //g.Dispose();

        }

        private void paintRect(Graphics g, Pen mainPen, int xx0, int yy0, int xx1, int yy1)
        {
            g.DrawLine(mainPen, xx0, yy0, xx0, yy1);
            g.DrawLine(mainPen, xx0, yy1, xx1, yy1);
            g.DrawLine(mainPen, xx1, yy1, xx1, yy0);
            g.DrawLine(mainPen, xx1, yy0, xx0, yy0);
        }


        private void button2_1__1_Click(object sender, EventArgs e)
        {
            TextBox2Config();
        }

        private void buttonx1_4__1_Click(object sender, EventArgs e)
        {
            var newName = comboBox1_4__1.Text;
            var idx = comboBox1_4__1.Items.IndexOf(newName);
            if ((idx < 0) && (!string.IsNullOrWhiteSpace(comboBox1_4__1.Text)))
            {
                comboBox1_4__1.Items.Add(newName);

                Coco.Categories c = new Coco.Categories();
                c.name = newName;
                c.supercategory = newName;
                //カテゴリIDの最大値を求め、その値+１を新たなカテゴリーにする。
                int id = 0;
                foreach (var cat in categoryList)
                {
                    var idc = cat.id;
                    if (id < idc)
                    {
                        id = idc;
                    }
                }
                id = id + 1;
                c.id = id;
                categoryList.Add(c);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var eX = e.X;
            var eY = e.Y;
            var s = "Move: X=" + eX + "Y=" + eY;

            textBox1_5__1.Text = s;
            textBox1_5__2.Text = s;

            if (checkRect(eX, eY, out int xbb0, out int ybb0, out Coco.Annotation a))
            {


                int index = 0;
                for (int i = 0; i < categoryList.Count; i++)
                {
                    var cat = categoryList[i];
                    if (cat.id == a.category_id)
                    {
                        index = i;
                    }
                }

                pauseEventA(index);
                //}

                mouseStartPositionX = xbb0;
                mouseStartPositionY = ybb0;
                mouseCurrentPositionX = eX;
                mouseCurrentPositionY = eY;
                limitPosition();
                isCorrectMode = true;

                currentImagePack.pushNew();//修正もーどのときはここでアンドゥバッファに。
                currentImagePack.annotationList.Remove(a);
                //ひとつ削除されたので再描画。
                pictureBox1.Refresh();

            }
            else
            {

                mouseStartPositionX = eX;
                mouseStartPositionY = eY;
                mouseCurrentPositionX = eX;
                mouseCurrentPositionY = eY;
                limitPosition();
                isCorrectMode = false;
            }


            mouseMode1 = true;
            pictureBox1.Refresh();

        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseMode1) return;

            mouseCurrentPositionX = e.X;
            mouseCurrentPositionY = e.Y;
            limitPosition();
            pictureBox1.Refresh();
            var s = "Move: X=" + mouseCurrentPositionX + "Y=" + mouseCurrentPositionY;
            textBox1_5__1.Text = s;
            mouseMode2 = true;
        }




        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

            if (!mouseMode1) return;
            if (!mouseMode2) return;//移動必須
            mouseCurrentPositionX = e.X;
            mouseCurrentPositionY = e.Y;
            limitPosition();

            SwapPosition(mouseStartPositionX, mouseStartPositionY, mouseCurrentPositionX, mouseCurrentPositionY, out Vector2 pos, out Vector2 pos1, out Vector2 size);
            //小さすぎるときは登録しない。Undoも処理しない。
            if (size.X >= config.MinimumLinkLength && size.Y >= config.MinimumLinkLength)
            {
                setBbox(pos, size, currentCategoryID());
            }
            var s = "Move: X=" + mouseCurrentPositionX + "Y=" + mouseCurrentPositionY;
            textBox1_5__1.Text = s;
            mouseMode2 = false;
            mouseMode1 = false;

            pictureBox1.Refresh(); //mouseModeを戻してから。
        }

        private void button2_2__1_Click(object sender, EventArgs e)
        {
            TextBox2Config();
        }

        private void comboBox1__1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var i = comboBox1__1.SelectedIndex;
            if ((0 <= i) && (i < imagePackList.Count))
            {
                currentImagePack = imagePackList[i];
            }
            gazoUpdate();
        }

        void comboBox1__1_Update()
        {
            int ip = -1;
            comboBox1__1.Items.Clear();
            foreach (var img in imagePackList)
            {
                comboBox1__1.Items.Add(img.file_name);
                if (img == currentImagePack)
                {
                    ip = imagePackList.IndexOf(img);
                }
            }
            if (0 <= ip)
            {
                comboBox1__1.SelectedIndex = ip;
            }

            comboBox1__1.Refresh();
        }

        private void button1_7__1_Click(object sender, EventArgs e)
        {
            currentImagePack.UnDo();
            pictureBox1.Refresh();
        }

        private void button1_6__1_Click(object sender, EventArgs e)
        {
            if (0 < comboBox1__1.SelectedIndex)
            {
                comboBox1__1.SelectedIndex--;
            }
        }

        private void button1_6__2_Click(object sender, EventArgs e)
        {
            if (comboBox1__1.SelectedIndex + 1 < comboBox1__1.Items.Count)
            {
                comboBox1__1.SelectedIndex++;
            }
        }

        private void button1_0__1_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                //openFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "json files (*.json)|*.json|txt files (*.txt)|*.txt|All files (*.*)|*.*";
                //openFileDialog.FilterIndex = 2;
                //openFileDialog.RestoreDirectory = true;

                //アノテーションファイル
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = saveFileDialog.FileName;//Get the path of specified file                    
                    var fileStream = saveFileDialog.OpenFile();//Read the contents of the file into a stream
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        String s = createJsonCoco(imagePackList);
                        writer.WriteLine(s);
                    }
                }
            }
        }

        private void button1_1__2_Click(object sender, EventArgs e)
        {

            var initialFolder = Environment.CurrentDirectory;
            if (!string.IsNullOrWhiteSpace(textBox1_1__2.Text))
            {
                initialFolder = textBox1_1__1.Text + "/" + textBox1_1__2.Text;
            }
            FolderDialog.Bll.FolderDialog.ISelect dialog = new FolderDialog.Bll.FolderDialog.Select();
            dialog.InitialFolder = initialFolder;// "C:\\";
            if (DialogResult.OK == dialog.ShowDialog())
            {
                var fpath = dialog.Folder;
                textBox1_1__2.Text = System.IO.Path.GetFileName(fpath);
                TextBox2Config();
            }
        }



        private void button1_7__2_Click(object sender, EventArgs e)
        {
            currentImagePack.ReDo();
            pictureBox1.Refresh();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            textBox1_1__1.Text = config.ProjectFolderFullPath;
            textBox1_1__2.Text = config.AllImageFileFolder;
            config.ImageFileFolder = config.TrainImageFileFolder;
            config.AnnotationFileName = config.AllAnnotationFileName;

            var annotationFolderFullPath = config.ProjectFolderFullPath +@"\"+ config.AnnotationFileFolder;
                
            if (config.mode == 0)
            {
                viewImageList();
            }
            else if (config.mode == 1)
            {
                var fullpath=annotationFolderFullPath + @"\" + config.AnnotationFileName;
                loadAnnotationFile(fullpath);
            }
            else if (config.mode == 2)
            {
                textBox1_1__2.Text = config.TrainImageFileFolder;
                config.ImageFileFolder= config.TrainImageFileFolder;
                config.AnnotationFileName = config.TrainAnnotationFileName;
                var fullpath = annotationFolderFullPath + @"\" + config.AnnotationFileName;
                loadAnnotationFile(fullpath);
            }
        }

        public void UpdateConfig(Config _config, bool isUpdateTextBox)
        {
            lock (config)
            {
                config.CopyFrom(_config);
            }
            if (isUpdateTextBox)
            {
                UpdateConfig2TextBox();
            }
        }
        public void UpdateConfig2TextBox()
        {
            //label1.Text = string.Format("{0}", count);
            if (this.InvokeRequired)
            {
                this.Invoke(() =>
                {
                    this.UpdateConfig2TextBox();
                });
                return;
            }
            //label1.Text = string.Format("{0}", count);
            Config2TextBox(null);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            TextBox2Config();
            Form1_Save(textBox2_3__1.Text);
        }

        private void button1_3__2_Click(object sender, EventArgs e)
        {
            var imageFolderFullpath = textBox1_1__1.Text + "/" + textBox1_1__2.Text;
            int isChange = chackImage(imageFolderFullpath, imagePackList,true);
            if (isChange == 0)
            {
                MessageBox.Show(@"画像ファイルとの照合完了。不具合はありません");
            }
            else if (isChange==1)
            {
                currentImagePack = imagePackList[0];
                comboBox1__1_Update();
                MessageBox.Show("完了しました");
            }
        }

        int chackImage(string imageFolderFullpath, List<ImagePack> _imagePackList,bool isUI) {
            int  isChange = 0;
            var imagePackList_ = new List<ImagePack>();
            //ファイル一覧
            string[] files = new string[0];
            try
            {
                files = System.IO.Directory.GetFiles(imageFolderFullpath, "*");
            }
            catch
            {
                isChange = 2;
            }
            //仮データ作成。ここでファイル読み取りチェックも行われる。
            var imagePackList__ = ImagePack.CreateNewImagePackList_(imagePackList_, files);//
            //既存と仮の照合
            List<bool> result = new List<bool>();
            foreach (var img in _imagePackList)
            {//既存一覧
                bool imgIsActive = false;
                img.isActive = false;
                foreach (var img__ in imagePackList__)
                {
                    if (img__.isActive)
                    {//仮データあり
                        if (img.file_name == img__.file_name)
                        {//同名仮データあり

                            img.isActive = true;
                            imgIsActive = true;
                            break;
                        }
                    }
                }
                result.Add(imgIsActive);
            }
            int imageCount = 0;
            int annCount = 0;
            for (int i = 0; i < result.Count; i++)
            {
                var img = _imagePackList[i];
                if (!result[i])
                {//ない、または無効
                    annCount += img.annotationList.Count;
                    imageCount++;
                }
            }
            if (0 < imageCount)
            {
                bool recover = true;
                if (isUI)
                {
                    var msg = imageCount.ToString() + @"個の画像がありません。" + annCount.ToString() + @"個のアノテーションデータが削除されますが、続行しますか";
                    DialogResult dr = MessageBox.Show(msg, @"実在の画像ファイルとの照合", MessageBoxButtons.YesNo);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        //recover = true;
                    }
                    else
                    {
                        recover = false;
                    }
                }
                if (recover) { 
                    var imagePackList___ = new List<ImagePack>();
                    for (int i = 0; i < result.Count; i++)
                    {
                        var img = _imagePackList[i];
                        if (!result[i])
                        {//ない、または無効

                        }
                        else
                        {
                            imagePackList___.Add(img);
                        }
                    }
                    _imagePackList.Clear();
                    _imagePackList.AddRange(imagePackList___);

                    if (_imagePackList.Count == 0)//ひとつは必要。
                    {
                        _imagePackList.Add(new ImagePack());
                    }

                    isChange = 1;

                }
                else
                {
                    isChange = -1;
                }
            }
            else
            {
            }
            return isChange;

        }

        private void button1_4__2_Click(object sender, EventArgs e)
        {
            var text = cocoCat(categoryList);
            var f2 = new Form2();
            f2.Show();
            f2.WriteTextSafe(text);

        }

        private void button1_0__2_Click(object sender, EventArgs e)
        {
            var r=MessageBox.Show("既存のファイルがあっても上書きします","",MessageBoxButtons.OKCancel);
            if (r == DialogResult.OK)
            {
                var aFilename =
                    config.ProjectFolderFullPath + @"\"
                    + config.AnnotationFileFolder + @"\"
                    + config.AnnotationFileName;
                var atFilename =
                    config.ProjectFolderFullPath + @"\"
                    + config.AnnotationFileFolder + @"\"
                    + config.TrainAnnotationFileName;

                var avFilename =
                    config.ProjectFolderFullPath + @"\"
                    + config.AnnotationFileFolder + @"\"
                    + config.ValidAnnotationFileName;


                //アノテーションファイル
                var jsonCoco = createJsonCoco(imagePackList);
                String jsonCocoT = jsonCoco;
                String jsonCocoV = jsonCoco;
                using (StreamWriter writer = new StreamWriter(aFilename))
                {
                    String s = jsonCoco;
                    writer.WriteLine(s);
                }

                var pFilename =
                    config.ProjectFolderFullPath + @"\"
                    + config.DefinedPythonFileName;

                using (StreamWriter writer = new StreamWriter(pFilename))
                {
                    String s = py();
                    writer.WriteLine(s);
                }


                var cFilename =
                    config.ProjectFolderFullPath + @"\"
                    + config.CategoryPythonFileName;

                using (StreamWriter writer = new StreamWriter(cFilename))
                {
                    String s = cocoCat(categoryList);
                    writer.WriteLine(s);
                }



                if ((int.Parse(textBox1_0__2.Text) == 100)||((int.Parse(textBox1_0__2.Text) == 0)))
                {
                    //ファイルのコピー
                    if ((config.mode == 0) || (config.mode == 1))
                    {
                        var iAllDirName =
                        config.ProjectFolderFullPath + @"\"
                        + config.AllImageFileFolder;
                        var itDirName =
                        config.ProjectFolderFullPath + @"\"
                        + config.TrainImageFileFolder;
                        var ivDirName =
                        config.ProjectFolderFullPath + @"\"
                        + config.ValidImageFileFolder;
                        CopyAll(iAllDirName, itDirName);
                        CopyAll(iAllDirName, ivDirName);
                    }
                }
                else
                {
                    var ratio = float.Parse(textBox1_0__2.Text) / 100f;

                    var iAllDirName =
                    config.ProjectFolderFullPath + @"\"
                    + config.AllImageFileFolder;
                    var itDirName =
                    config.ProjectFolderFullPath + @"\"
                    + config.TrainImageFileFolder;
                    var ivDirName =
                    config.ProjectFolderFullPath + @"\"
                    + config.ValidImageFileFolder;

                    var files = System.IO.Directory.GetFiles(iAllDirName);
                    var n = files.Length;
                    var nv = (int)(n * (1 - ratio));
                    var nt = n - nv;

                    List<string> ft = new List<string>();
                    List<string> fv = new List<string>();
                    for (var i = 0; i < nt; i++)
                    {
                        ft.Add(files[i]);
                    }
                    for (var i = nt; i < n; i++)
                    {
                        fv.Add(files[i]);
                    }
                    Copy(ft, itDirName);
                    Copy(fv, ivDirName);

                    var _imagePackListT = ImagePack.CreateCone(imagePackList);
                    var _imagePackListV = ImagePack.CreateCone(imagePackList);
                    var ct = chackImage(itDirName, _imagePackListT, false);
                    var cv = chackImage(ivDirName, _imagePackListV, false);
                    jsonCocoT = createJsonCoco(_imagePackListT);
                    jsonCocoV = createJsonCoco(_imagePackListV);

                }

                //アノテーションファイル
                using (StreamWriter writer = new StreamWriter(atFilename))
                {
                    writer.WriteLine(jsonCocoT);
                }
                //アノテーションファイル
                using (StreamWriter writer = new StreamWriter(avFilename))
                {
                    writer.WriteLine(jsonCocoV);
                }
            }
        }

        public void Copy(List<string>files , string targetDirFullpath)
        {
            string er = "";
            foreach (var f in files)
            {
                try
                {
                    var fname = Path.GetFileName(f);
                    System.IO.File.Copy(f, targetDirFullpath+@"\" + fname, true);//上書きOK;
                }
                catch (Exception ex)
                {
                    if (ex is System.IO.IOException)
                    {
                        er += Path.GetFileName(f) + System.Environment.NewLine;
                    }
                }

            }
        }

        //リカーシブなコピーだがそのまま。
        public static void CopyAll(string sourceDirectory, string targetDirectory)
        {
            //string sourceDirectory = @"d:\DemoSourceDirectory";
            //string targetDirectory = @"d:\DemoTargetDirectory";
            DirectoryInfo sourceDircetory = new DirectoryInfo(sourceDirectory);
            DirectoryInfo targetDircetory = new DirectoryInfo(targetDirectory);
            CopyAll(sourceDircetory, targetDircetory);
            Console.ReadLine();
        }
        //リカーシブなコピーだがそのまま。
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }


        string cocoCat(List<Coco.Categories> _categoryList)
        {


            int n = _categoryList.Count;


            var nl = System.Environment.NewLine;
            string text = "#!/usr/bin/env python3" + nl + "# -*- coding:utf-8 -*-" + nl + "# .\r\n";
            text += "COCO_CLASSES = (" + nl;
            for (int i = 0; i < n; i++)
            {
                text += "'" + _categoryList[i].name + "'";
                if (i < n - 1)
                {
                    text += ",";
                    if (((i + 1) % 10) == 0)
                    {
                        text += nl;
                    }
                }
            }
            text += nl + ") ";
            return text;
        }
        string py()
        { 
            var s = textBox2_4__1.Text;
            var nl = System.Environment.NewLine;
            var tab = "        ";
            var s1 = "self.depth = 0.33";
            var s2 = "self.width = 0.50";
            var s3 = "# Define yourself dataset path";
            var s4 = "self.data_dir = \"datasets/my_data_C\" # <------------ your dataset folder";
            var s5 = "self.train_ann = \"train.json\"# <------------ your annotations filename for training";
            var s6 = "self.val_ann = \"val.json\"# <------------ your annotations filename for validation";
            var s7 = "self.test_ann = \"test.json\"# <------------ NotUse?";
            var s8 = "self.num_classes = 1  # <------------ number of your classes";
            var s9 = "self.max_epoch = 10 # <------------ number of your epochs.300";
            var s10 = "self.data_num_workers = 2";
            var s11 = "self.eval_interval = 1";

            s += tab + s1 + nl;
            s += tab + s2 + nl;
            s += tab + s3 + nl;
            s += tab + s4 + nl;
            s += tab + s5 + nl;
            s += tab + s6 + nl;
            s += tab + s7 + nl;
            s += tab + s8 + nl;
            s += tab + s9 + nl;
            s += tab + s10+ nl;
            s += tab + s11+ nl;
            return s;
        }
    }




}