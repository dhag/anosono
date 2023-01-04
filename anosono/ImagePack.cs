using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Coco;

public class ImagePack:Coco.Images
{
    public static List<ImagePack> CreateCone(List<ImagePack>_imagepack)
    {
        var c = new List<ImagePack>();
        foreach (var n in _imagepack)
        {
            c.Add(n.CloneMyself());
        }
        return c;
    }
    public ImagePack CloneMyself()
    {
        ImagePack c = new ImagePack();

        c.date_captured = date_captured;
        c.file_name = this.file_name;
        c.id = id;
        c.height = height;
        c.width = width;
        c.isActive = isActive;
        //アノテーションリストのクローン
        c.annotationList.AddRange(Coco.Annotation.CloneAnnotationList(this.annotationList));
        //スタックのクローン1。アンドゥバッファ。
        var thisAry1 = this.annotationListHistoryForUndo.ToArray();
        foreach (var aList in thisAry1)
        {
            var cAry = Coco.Annotation.CloneAnnotationList(aList);
            c.annotationListHistoryForUndo.Push(cAry);
        }
        //スタックのクローン2。リドゥバッファ。
        var thisAry2 = this.annotationListHistoryForRedo.ToArray();
        foreach (var aList in thisAry2)
        {
            var cAry = Coco.Annotation.CloneAnnotationList(aList);
            c.annotationListHistoryForRedo.Push(cAry);
        }
        return c;
    }
    public void pushNew()
    {
        //クローンを生成してUndo準備
        var cAry = Coco.Annotation.CloneAnnotationList(annotationList);
        annotationListHistoryForUndo.Push(cAry);
        //Redoクリア
        annotationListHistoryForRedo.Clear();

    }
    public void UnDo()
    {
        if(annotationListHistoryForUndo.Count > 0)
        {
            //annotationListHistoryForRedo.Push(Coco.Annotation.CloneAnnotationList(annotationList));
            annotationListHistoryForRedo.Push(annotationList);
            this.annotationList=annotationListHistoryForUndo.Pop();
        }
    }
    public void ReDo()
    {
        if (annotationListHistoryForRedo.Count > 0)
        {
            //annotationListHistoryForUndo.Push(Coco.Annotation.CloneAnnotationList(annotationList));
            annotationListHistoryForUndo.Push(annotationList);
            this.annotationList = annotationListHistoryForRedo.Pop();
        }
    }

    //public string _imageFileName;
    //public int _width;
    //public int _height;
    //public int _ID = -1;
    //public bool isActive = false;

    //List<Coco.Annotations> a= new List<Coco.Annotations>();
    public Stack<List<Coco.Annotation>> annotationListHistoryForUndo = new Stack<List<Coco.Annotation>>();
    public Stack<List<Coco.Annotation>> annotationListHistoryForRedo = new Stack<List<Coco.Annotation>>();



    //public int currentCommand = 0;//初期状態ではゼロとする
    //public List<List<Vector2>> bBoxList = new List<List<Vector2>>();//初期状態では要素0個
    //public Stack<int> UndoCommandBuffer = new Stack<int>();
    //public Stack<int> RedoCommandBuffer = new Stack<int>();
    //public Stack<List<List<Vector2>>> bBoxListHistoryForUndo = new Stack<List<List<Vector2>>>();
    //public Stack<List<List<Vector2>>> bBoxListHistoryForRedo = new Stack<List<List<Vector2>>>();
    //public List<int> categoryList = new List<int>();
    //public Stack<List<int>> categoryListHistoryForUndo = new Stack<List<int>>();
    //public Stack<List<int>> categoryListHistoryForRedo = new Stack<List<int>>();
    //public Stack<List<float>> AreaListHistoryForUndoYet = new Stack<List<float>>();
    //public Stack<List<float>> AreaListHistoryForRedoYet = new Stack<List<float>>();
    public static List<ImagePack> CreateImagePackList_(List<ImagePack> imagePackList_0, Coco cc)
    {
        imagePackList_0 = RemoveDumy(imagePackList_0);
        int imageIDmin = 0;
        int anotationIDmin = 0;
        foreach (var img in imagePackList_0)
        {
            if (imageIDmin <= img.id)
            {
                imageIDmin = img.id;
            }
            foreach (var ann in img.annotationList)
            {
                if (anotationIDmin <= ann.id)
                {
                    anotationIDmin = ann.id;
                }

            }
        }
        int fileIDOffset = imageIDmin;
        int annIDOffset = anotationIDmin;
        List<ImagePack> imagePackList_ = imagePackList_0;// new List<ImagePack>();

        //いらないと思うがアノテーションIDを修正しておく。
        foreach (var img in cc.images)
        {
            foreach (var ann in img.annotationList)
            {
                ann.id += annIDOffset;
            }
        }

        foreach (var img in cc.images)
        {
            if (!isListedFileName(img.file_name, imagePackList_0))//重複を避けて追加
            {//未知のファイル
                ImagePack imagePack = new ImagePack();
                imagePack.isActive = true;
                imagePack.date_captured = img.date_captured;
                imagePack.file_name = img.file_name;
                imagePack.id = img.id+ fileIDOffset;//画像IDを修正する。
                imagePack.width = img.width;
                imagePack.height = img.height;
                imagePack.annotationList.AddRange(img.annotationList);
                imagePackList_.Add(imagePack);
            }
            else
            {//すでに同名のファイルがある。
                var masterImagePack = findImagePack(img.file_name, imagePackList_0);
                foreach (var a in img.annotationList)//リンク用Dの修正
                {
                    a.image_id = masterImagePack.id;//画像IDを修正する。
                }
                masterImagePack.annotationList.AddRange(img.annotationList);//まとめて修正。以後使わない
            }
        }

        
        if (imagePackList_.Count == 0)//ひとつは必要。
        {
            imagePackList_.Add(new ImagePack());
        }
        return imagePackList_;
    }



    public static List<ImagePack> CreateNewImagePackList_(List<ImagePack> imagePackList_0,IEnumerable<string> files)
    {
        imagePackList_0 = RemoveDumy(imagePackList_0);
        int idmin = 0;
        foreach (var img in imagePackList_0)
        {
            if (idmin<=img.id)
            {
                idmin=img.id;
            }
        }
        int fileIDOffset = idmin;
        int id = 1;
        List<ImagePack> imagePackList_ = imagePackList_0;// new List<ImagePack>();
        foreach (var fullPathName in files)
        {
            ImagePack imagePack = new ImagePack();//無効でも作って追加
            var fileName_ = Path.GetFileName(fullPathName);
            if (!isListedFileName(fileName_, imagePackList_0))//重複を避けて追加
            {
                try
                {

                    imagePack.isActive = false;
                    //画像ファイルを読み込む
                    var tmpImage0 = Image.FromFile(fullPathName);
                    //成功したら(読めたら)
                    imagePack.isActive = true;
                    imagePack.date_captured = "";
                    imagePack.file_name = fileName_;
                    imagePack.id = id + fileIDOffset;
                    imagePack.width = tmpImage0.Width;
                    imagePack.height = tmpImage0.Height;
                    id++;
                }
                catch { }
                imagePackList_.Add(imagePack);
            }
        }
        
        if (imagePackList_.Count == 0)//ひとつは必要。
        {
            imagePackList_.Add(new ImagePack());
        }
        return imagePackList_;
    }



    //ダミーの場合削除
    public static List<ImagePack> RemoveDumy(List<ImagePack> imagePackList_0)
    {

        List<ImagePack> imagePackList_ = new List<ImagePack>();
        foreach (var img in imagePackList_0)
        {
            if (!string.IsNullOrWhiteSpace(img.file_name))
            {//ダミーでなければ
                imagePackList_.Add(img);
            }
        }
        return imagePackList_;
    }


    //ファイル名重複検査
    //引数fileNameがホワイトスペースの場合falseになるので注意
    public static bool isListedFileName(string fileName, List<ImagePack> imagePackList_0)
    {
        bool isListed_ = false;
        foreach (var img in imagePackList_0)
        {
            if (!string.IsNullOrWhiteSpace(img.file_name))
            {//ダミーでなければ
                if (fileName == img.file_name)
                {
                    isListed_ = true;
                }
            }
        }
        return isListed_;
    }

    public static ImagePack findImagePack(string fileName, List<ImagePack> imagePackList_0)
    {
        ImagePack imagePack_ = null;
        bool isListed_ = false;
        foreach (var img in imagePackList_0)
        {
            if (!string.IsNullOrWhiteSpace(img.file_name))
            {//ダミーでなければ
                if (fileName == img.file_name)
                {
                    imagePack_ = img;
                }
            }
        }
        return imagePack_;
    }

    //カテゴリ名重複検査。
    //引数categoryNameがホワイトスペースの場合falseになるので注意
    public static bool isListedCategory(string categoryName, List<Coco.Categories> categoryList_0)
    {
        bool isListed_ = false;
        foreach (var cat in categoryList_0)
        {
            if (!string.IsNullOrWhiteSpace(cat.name))
            {//ダミーでなければ
                if (categoryName == cat.name)
                {
                    isListed_ = true;
                }
            }
        }
        return isListed_;
    }


}
//EditPack currentEditPack = new EditPack();

//List<EditPack> editPackList = new List<EditPack>();


