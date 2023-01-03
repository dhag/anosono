using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using System.Text.Json;
using System.Text.Json.Serialization;
//下記はエラー修正で自動的に追加された。
using System.Text.Encodings.Web;
using System.Text.Unicode;


namespace anosono
{
    internal class formClass1
    {
    }

    public partial class Form1 
    {

        bool disposeMessage()
        {
            if (isActiveData())
            {
                var r=MessageBox.Show(@"今までの作業が水の泡ですが。ほんとにいいんですね。","あのその.　　　by はぎけん！", MessageBoxButtons.OKCancel);
                if (r == DialogResult.OK) return true;
                return false;

            }
            return true;
        }

        bool isActiveData()
        {
            bool isActive = false;
            foreach (var img in imagePackList)
            {
                foreach (var ann in img.annotationList)
                {
                    isActive = true;
                }
            }
            return isActive;
        }


        //マウス位置ex0,ey0に近いやつがあれば求める。あればカテゴリIDも。
        private bool checkRect(int ex0, int ey0, out int xbb0, out int ybb0, out Coco.Annotation aa)
        {
            aa = null;
            //categoryID = -1;
            xbb0 = 0;
            ybb0 = 0;
            //近い点を求める
            float dmin = float.MaxValue;
            int ni = -1;
            int nj = -1;
            var distance = config.MaxDistanceFromMouseToNode* config.MaxDistanceFromMouseToNode;
            for (int j = 0; j < currentImagePack.annotationList.Count; j++)
            {
                var a = currentImagePack.annotationList[j];

                var p0 = a._BoundigBoxPosition;
                var p1 = p0 + a._BoundigBoxSize;
                //2点の情報しかないので4点にする。
                List<float> xx = new List<float>();
                List<float> yy = new List<float>();
                xx.Add(p0.X); yy.Add(p0.Y);//x0y0.対角はx1y1
                xx.Add(p0.X); yy.Add(p1.Y);//x0y1.対角はx1y0
                xx.Add(p1.X); yy.Add(p0.Y);//x1y0.対角はx0y1
                xx.Add(p1.X); yy.Add(p1.Y);//x1y1.対角はx0y0
                for (int i = 0; i < xx.Count; i++)
                {
                    var x = xx[i]; var y = yy[i];
                    var d = (x - ex0) * (x - ex0) + (y - ey0) * (y - ey0);
                    if (d <= distance)
                    {
                        if (d < dmin)
                        {
                            dmin = d;
                            ni = i;
                            nj = j;
                        }
                    }
                }
            }
            //対角点を求める
            if (0 <= nj)
            {
                //push();//退避
                aa = currentImagePack.annotationList[nj];
                //currentImagePack.annotationList.RemoveAt(nj);
                var p0 = aa._BoundigBoxPosition;
                var p1 = p0 + aa._BoundigBoxSize;
                if (ni == 0)
                {//x0y0.対角はx1y1
                    xbb0 = (int)(p1.X);
                    ybb0 = (int)(p1.Y);
                }
                else if (ni == 1)
                {//x0y1.対角はx1y0
                    xbb0 = (int)(p1.X);
                    ybb0 = (int)(p0.Y);
                }
                else if (ni == 2)
                {//x1y0.対角はx0y1
                    xbb0 = (int)(p0.X);
                    ybb0 = (int)(p1.Y);
                }
                else if (ni == 3)
                {//x1y1.対角はx0y0
                    xbb0 = (int)(p0.X);
                    ybb0 = (int)(p0.Y);
                }
                //カテゴリーをセットする。
                //categoryID = a.category_id;
                //ひとつ削除されたので再描画。
                //pictureBox1.Refresh();
                return true;
            }
            return false;

        }

        //大小関係を整理する。サイズも求める。
        void SwapPosition(float p0X, float p0Y, float p1X, float p1Y, out Vector2 po0, out Vector2 po1, out Vector2 ps)
        {
            po0 = new Vector2((p0X <= p1X) ? p0X : p1X, (p0Y <= p1Y) ? p0Y : p1Y);
            po1 = new Vector2((p0X > p1X) ? p0X : p1X, (p0Y > p1Y) ? p0Y : p1Y);
            ps = po1 - po0;
        }
        //大小関係を整理する。サイズも求める。
        void SwapPosition(Vector2 p0, Vector2 p1, out Vector2 po0, Vector2 po1, out Vector2 ps)
        {
            po0 = new Vector2((p0.X <= p1.X) ? p0.X : p1.X, (p0.Y <= p1.Y) ? p0.Y : p1.Y);
            po1 = new Vector2((p0.X > p1.X) ? p0.X : p1.X, (p0.Y > p1.Y) ? p0.Y : p1.Y);
            ps = po0 - po1;
        }

        void pauseEventA(int n)
        {//ある処理中はイベントを一時無効にする
            //https://itthestudy.com/csharp-event-stop/
            // イベントを無効化したいコントロールをListに追加
            List<Control> controlList = new List<Control> { comboBox1_4__1 };// textBox1, button1};
            // 関数呼び出し
            DisableFormEvent.DoSomethingWithoutEvents(
                controlList,
                () =>
                {
                    if ((0 <= n) && (n < categoryList.Count) && (n < comboBox1_4__1.Items.Count))
                    {
                        //currentCategoryID = categoryIDs[n];
                        comboBox1_4__1.SelectedIndex = n;
                    }
                    else
                    {
                        //currentCategoryID = 0;
                    }
                    //extBox1.Text = "イベントを無効化している間に値を代入する。"; 

                }
            );


        }

        //int currentCategoryID = 0;
        //List<int> categoryIDs = new List<int>();

        void limitPosition()
        {
            if (pictureBox1.Width <= mouseStartPositionX) mouseStartPositionX = pictureBox1.Width - 1;
            if (mouseStartPositionX < 0) mouseStartPositionX = 0;

            if (pictureBox1.Height <= mouseStartPositionY) mouseStartPositionY = pictureBox1.Height - 1;
            if (mouseStartPositionY < 0) mouseStartPositionY = 0;

            if (pictureBox1.Width <= mouseCurrentPositionX) mouseCurrentPositionX = pictureBox1.Width - 1;
            if (mouseCurrentPositionX < 0) mouseCurrentPositionX = 0;

            if (pictureBox1.Height <= mouseCurrentPositionY) mouseCurrentPositionY = pictureBox1.Height - 1;
            if (mouseCurrentPositionY < 0) mouseCurrentPositionY = 0;
        }


        int currentCategoryID()
        {
            var i = comboBox1_4__1.SelectedIndex;
            if (i < 0) return 0;
            if (categoryList.Count <= i) return 0;
            var id = categoryList[i].id;
            return id;
        }

        String createJsonCoco()
        {
            Coco coco = new Coco();
            //int imageID = 1;
            int annotationID = 1;
            foreach (var imagePack in imagePackList)
            {
                if (imagePack.isActive)
                {
                    var cocoImage = new Coco.Images();

                    cocoImage.date_captured = imagePack.date_captured;
                    cocoImage.id = imagePack.id;
                    if (cocoImage.id <= 0)
                    {//ImageIDは1から
                        cocoImage.id = 1;
                    }
                    cocoImage.file_name = imagePack.file_name;
                    cocoImage.width = imagePack.width;
                    cocoImage.height = imagePack.height;
                    coco.images.Add(cocoImage);

                    for (int i = 0; i < imagePack.annotationList.Count; i++)
                    {
                        var a = imagePack.annotationList[i];//一次的なので現物でよし。
                        if (a.category_id <= 0)
                        {//カテゴリーIDは1から
                            a.category_id = 1;
                        }
                        a.id = annotationID;
                        a.FromVector2();
                        coco.annotations.Add(a);
                        annotationID++;
                    }
                }
            }
             ((List<Coco.Categories>)coco.categories).AddRange(categoryList);
            if ((categoryList.Count == 0) && (0 < coco.annotations.Count))
            {
                coco.categories.Add(new Coco.Categories() { id = 1, name = "NoName", supercategory = "NoName" });
            }
            var jsonString = Coco.ToJson(coco);
            return jsonString;

        }


        void setBbox(Vector2 pos, Vector2 size, int categoryID)
        {
            if (!isCorrectMode)
            {//追加モードのとき。
                currentImagePack.pushNew();
            }
            var a = new Coco.Annotation();
            a.image_id = currentImagePack.id;
            a.category_id = categoryID;
            a._BoundigBoxPosition = pos;
            a._BoundigBoxSize = size;
            a.FromVector2();//セグメントの簡易計算など
            currentImagePack.annotationList.Add(a);
        }



        void gazoUpdate()
        {
            var id = imagePackList.IndexOf(currentImagePack);
            if (id == -1) return;
            var currentImage = pictureBox1.Image;
            //結果を表示する
            var fullpath = textBox1_1__1.Text + "/" + textBox1_1__2.Text + "/" + currentImagePack.file_name;
            textBox1_6__1.Text = fullpath;
            Image currentImage0 = null;
            try
            {
                currentImagePack.isActive = false;
                //画像ファイルを読み込む
                currentImage0 = Image.FromFile(fullpath);
                //成功したら(読めたら)
                currentImagePack.isActive = true;
                currentImagePack.width = currentImage0.Width;
                currentImagePack.height = currentImage0.Height;

            }
            catch { currentImage0 = null; }

            if (currentImage != null)
            {
                currentImage.Dispose();
            }
            pictureBox1.Image = currentImage0;
            if (currentImage0 != null)
            {
                pictureBox1.Size = currentImage0.Size;
            }
        }



        private void Form1_Load(string fileName)
        {
            try
            {
                var jsonString = File.ReadAllText(fileName);
                config = JsonSerializer.Deserialize<Config>(jsonString, Coco.GetOption());
            }
            catch { }
            Config2TextBox(fileName);
        }
        void Config2TextBox(string fileName) {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                textBox2_3__1.Text = fileName;
            }
            textBox1_1__1.Text = config.ProjectFolder;
            textBox1_1__2.Text = config.ImageFileFolder;
            textBox2_1__1.Text = config.MaxDistanceFromMouseToNode.ToString();
            textBox2_2__1.Text = config.MinimumLinkLength.ToString();

        }

        void TextBox2Config()
        {
            config.ProjectFolder = textBox1_1__1.Text;
            config.ImageFileFolder = textBox1_1__2.Text;
            config.MaxDistanceFromMouseToNode = float.Parse(textBox2_1__1.Text);
            config.MinimumLinkLength = int.Parse(textBox2_2__1.Text);

        }
        private void Form1_Save(string fileName)
        {
/*          var config = new Config
            {
            };
*/


            string jsonString = JsonSerializer.Serialize(config, Coco.GetOption());
            try
            {
                File.WriteAllText(fileName, jsonString);
            }catch { }

            //Console.WriteLine(File.ReadAllText(fileName));

        }



    }




}
