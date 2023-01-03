using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Text.Json.Serialization;

namespace anosono
{
    internal class formClass0
    {
    }

    public partial class Form1 : Form
    {
        List<Coco.Categories> categoryList = new List<Coco.Categories>();

        bool isCorrectMode = false;//修正のときはtrue,追加の時はfalse
        bool mouseMode1 = false;//ダウンされるとtrue.アップでfalse
        bool mouseMode2 = false;//ムーブするとtrue.アップでfalse
        int mouseStartPositionX, mouseStartPositionY, mouseCurrentPositionX, mouseCurrentPositionY;

        ImagePack currentImagePack = new ImagePack();
        List<ImagePack> imagePackList = new List<ImagePack>();
        Config config = new Config();
    }
}

public class Config
{
    public string? Comment_A { get; set; }
    public string? Comment_B { get; set; }
    public string? Comment_C { get; set; }

    public string? ProjectFolderFullPath { get; set; }
    public string? ImageFileFolder { get; set; }
    public float? MaxDistanceFromMouseToNode { get; set; }
    public int? MinimumLinkLength { get; set; }
    public string? AnnotationFileFolder { get; set; }

    public string? AllImageFileFolder { get; set; }

    public string? TrainImageFileFolder { get; set; }
    public string? ValidImageFileFolder { get; set; }
    public string? AnnotationFileName { get; set; }
    public string? AllAnnotationFileName { get; set; }
    public string? TrainAnnotationFileName { get; set; }
    public string? ValidAnnotationFileName { get; set; }
    public string? CategoryPythonFileName { get; set; }

    public string? DefinedPythonFileName { get; set; }

    [JsonIgnore]
    public int mode = 0;//0新規生成

    [JsonIgnore]
    public string thisFileName;


    public Config()
    {
        ProjectFolderFullPath = @"I:\データサイエンス\TOOL\JsonAnnotator\";
        ImageFileFolder = "train2017";
        MaxDistanceFromMouseToNode = 10;
        MinimumLinkLength = 5;

        AnnotationFileFolder = "annotations";
        AllImageFileFolder = "train_and_val";
        TrainImageFileFolder = "train2017";
        ValidImageFileFolder = "val2017";

        AnnotationFileName = "all.json";
        AllAnnotationFileName = "all.json";
        TrainAnnotationFileName = "train.json";
        ValidAnnotationFileName = "val.json";
        CategoryPythonFileName="iwatedemo_category.py";
        DefinedPythonFileName = "my_yolox_s_C.py";
        mode = 0;
    }

    public void CopyFrom(Config c)
    {
        ProjectFolderFullPath = c.ProjectFolderFullPath;// @"I:\データサイエンス\TOOL\JsonAnnotator\";
        ImageFileFolder = c.ImageFileFolder;//"train2017";
        MaxDistanceFromMouseToNode = c.MaxDistanceFromMouseToNode;//10;
        MinimumLinkLength = c.MinimumLinkLength;//5;

        AnnotationFileFolder = c.AnnotationFileFolder;
        AllImageFileFolder = c.AllImageFileFolder;//"train_and_val";
        TrainImageFileFolder = c.TrainImageFileFolder;//"train2017";
        ValidImageFileFolder = c.ValidImageFileFolder;//"val2017";

        AnnotationFileName = c.AnnotationFileName;//"all.json";
        AllAnnotationFileName = c.AllAnnotationFileName;//"all.json";
        TrainAnnotationFileName = c.TrainAnnotationFileName;//"train.json";
        ValidAnnotationFileName = c.ValidAnnotationFileName;//"valid.json";
        CategoryPythonFileName = c.CategoryPythonFileName;
        DefinedPythonFileName = c.DefinedPythonFileName;

        mode = c.mode;
        thisFileName = c.thisFileName;

    }

    public Config CloneMyself()
    {
        var c= new Config();
        c.CopyFrom(this);
        return c;
    }


}
