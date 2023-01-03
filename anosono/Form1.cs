using System.Numerics;

namespace anosono
{
    public partial class Form1 : Form
    {
        Form3 form3;

        public Form1()
        {
            form3= new Form3();
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
            var imagePackList_= new List<ImagePack>();
            //var categoryList_ = new List<Coco.Categories>();

            if (checkBox1_2__1.Checked) {//追加モード
                imagePackList_ = imagePackList;
                //categoryList_ = categoryList;
            }
            else
            {
                if(!disposeMessage())return;
            }

            string[] files = new string[0];
            try
            {
                files = System.IO.Directory.GetFiles(textBox1_1__1.Text+"/" + textBox1_1__2.Text, "*");
            }
            catch
            {

            }
            imagePackList = ImagePack.CreateNewImagePackList_(imagePackList_,files);//

            currentImagePack = imagePackList[0];
            comboBox1__1_Update(); 
            //ファイルの追加のみなので、カテゴリリストは不要
        }




        private void button1_3__1_Click(object sender, EventArgs e)
        {
                try
                {            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {

                    openFileDialog.InitialDirectory = textBox1_1__1.Text ;
                    openFileDialog.Filter = "json files (*.json)|*.json|txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    //openFileDialog.FilterIndex = 2;
                    //openFileDialog.RestoreDirectory = true;

                    //アノテーションファイルの読み取り
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
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
                        var filePath = openFileDialog.FileName;//Get the path of specified file                    
                        var fileStream = openFileDialog.OpenFile();//Read the contents of the file into a stream
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            var fileContent = reader.ReadToEnd();//丸ごと読む
                            Coco cc = Coco.JsonToCoco(fileContent);//JSON読む

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
                if(isCorrectMode)mainPen=mainPenB;
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
            var i=comboBox1__1.SelectedIndex;
            if((0<=i)&& (i < imagePackList.Count))
            {
                currentImagePack=imagePackList[i];
            }
            gazoUpdate();
        }

        void comboBox1__1_Update()
        {
            int ip = -1;
            comboBox1__1.Items.Clear();
            foreach(var img in imagePackList)
            {
                comboBox1__1.Items.Add(img.file_name);
                if (img == currentImagePack)
                {
                    ip = imagePackList.IndexOf(img);
                }
            }
            if (0 <= ip)
            {
                comboBox1__1.SelectedIndex= ip;
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
            if (comboBox1__1.SelectedIndex+1< comboBox1__1.Items.Count)
            {
                comboBox1__1.SelectedIndex++;
            }
        }

        private void button1__1_Click(object sender, EventArgs e)
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
                        String s = createJsonCoco();
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
                initialFolder = textBox1_1__1.Text+"/"+textBox1_1__2.Text;
            }
            FolderDialog.Bll.FolderDialog.ISelect dialog = new FolderDialog.Bll.FolderDialog.Select();
            dialog.InitialFolder = initialFolder;// "C:\\";
            if (DialogResult.OK == dialog.ShowDialog())
            {
                var fpath= dialog.Folder;
                textBox1_1__2.Text = System.IO.Path.GetFileName(fpath);
                TextBox2Config();
            }
        }



        private void button1_7__2_Click(object sender, EventArgs e)
        {
            currentImagePack.ReDo();
            pictureBox1.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var s = "config.json";
            try
            {
                s = File.ReadAllText("configbase.txt");

            } catch
            { 
            }


            Form1_Load(s);

            form3.getfolders(
                out string projectFolder,
                out string AnnotationFolder,
                out string TrainingFolder,
                out string ValidationFolder);
            textBox1_1__1.Text = projectFolder;
            textBox1_1__2.Text = TrainingFolder;

            TextBox2Config();

        }



        private void button1_Click(object sender, EventArgs e)
        {
            TextBox2Config();
            Form1_Save(textBox2_3__1.Text);
        }

        private void button1_3__2_Click(object sender, EventArgs e)
        {
            var imagePackList_ = new List<ImagePack>();
            string[] files = new string[0];
            try
            {
                files = System.IO.Directory.GetFiles(textBox1_1__1.Text +"/"+ textBox1_1__2.Text, "*");
            }
            catch
            {

            }
            //仮データ作成。ここでファイルチェックも行われる。
            var imagePackList__ = ImagePack.CreateNewImagePackList_(imagePackList_, files);//
            //既存と仮の照合
            List<bool> result = new List<bool>();
            foreach (var img in imagePackList)
            {
                bool imgIsActive = false;

                img.isActive = false;
                foreach (var img__ in imagePackList__)
                {
                    if (img__.isActive)
                    {
                        if (img.file_name == img__.file_name)
                        {

                            imgIsActive = true;
                            break;
                        }
                    }
                }
                result.Add(imgIsActive);
            }
            int imageCount = 0;
            int annCount=0;
            for(int i = 0; i < result.Count; i++)
            {
                var img = imagePackList[i];
                if (!result[i])
                {//ない、または無効
                    annCount += img.annotationList.Count;
                    imageCount++;
                }
            }
            if (0 < imageCount)
            {
                var msg = imageCount.ToString()+ @"個の画像がありません。"+ annCount.ToString()+@"個のアノテーションデータが削除されますが、続行しますか";
                DialogResult dr = MessageBox.Show(msg,@"実在の画像ファイルとの照合",MessageBoxButtons.YesNo);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    var imagePackList___ = new List<ImagePack>();
                    for (int i = 0; i < result.Count; i++)
                    {
                        var img = imagePackList[i];
                        if (!result[i])
                        {//ない、または無効

                        }
                        else {
                            imagePackList___.Add(img);
                        }
                    }
                    imagePackList = imagePackList___;
                    
                    if (imagePackList_.Count == 0)//ひとつは必要。
                    {
                        imagePackList_.Add(new ImagePack());
                    }
                    currentImagePack = imagePackList[0];
                    comboBox1__1_Update();
                    MessageBox.Show("完了しました");
                }
                else 
                {
                    
                }
            }
            else
            {
                MessageBox.Show(@"画像ファイルとの照合完了。不具合はありません");
            }


        }

        private void button1_4__2_Click(object sender, EventArgs e)
        {
            int n = categoryList.Count;


            var nl = System.Environment.NewLine;
            string text = "#!/usr/bin/env python3"+nl+"# -*- coding:utf-8 -*-"+nl+"# .\r\n";
            text += "COCO_CLASSES = (" + nl;
            for(int i = 0; i < n; i++)
            {
                text += "'" + categoryList[i].name + "'";
                if (i < n - 1)
                {
                    text += ",";
                    if (((i+1) %10)==0)
                    {
                        text += nl;
                    }
                }
            }
            text += nl+") " ;
            var f2=new Form2();
            f2.Show();
            f2.WriteTextSafe(text);

        }
    }




}