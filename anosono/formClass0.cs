using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

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

    public class Config
    {
        public string? Comment_A { get; set; }
        public string? Comment_B { get; set; }
        public string? Comment_C { get; set; }

        public string? ProjectFolder { get; set; }
        public string? ImageFileFolder { get; set; }
        public float? MaxDistanceFromMouseToNode { get; set; }
        public int? MinimumLinkLength { get; set; }
        public string? AnnotationFileName { get; set; }


        public string? TrainImageFileFolder { get; set; }
        public string? ValidImageFileFolder { get; set; }
        public string? AnnotationFileFolder { get; set; }
        public string? TrainAnnotationFileName { get; set; }
        public string? ValidAnnotationFileName { get; set; }


        public Config()
        {
            ProjectFolder = @"I:\データサイエンス\TOOL\JsonAnnotator\";
            ImageFileFolder = "train2017";
            MaxDistanceFromMouseToNode = 10;
            MinimumLinkLength = 5;

            TrainImageFileFolder = "train2017";
            ValidImageFileFolder = "valid2017";

            AnnotationFileName = "all.json";
            TrainAnnotationFileName = "train.json";
            ValidAnnotationFileName = "valid.json";

        }
    }

}
