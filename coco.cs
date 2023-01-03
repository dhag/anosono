﻿//https://iwasiman.hatenablog.com/entry/20210614-CSharp-json#%E8%BF%BD%E5%8A%A0%E6%96%B9%E6%B3%95
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
//[参照]-[参照の追加],[アセンブリ]-[拡張]で System.Text.Encodings.Web, System.Text.Jsonにチェック
using System.Text.Json;
using System.Text.Json.Serialization;
//下記はエラー修正で自動的に追加された。
using System.Text.Encodings.Web;
using System.Text.Unicode;
//using System.IO;
//System.Threading.Tasks.ExtensionsをNUGETから
using System.Numerics;
//using System.Net;

//namespace COCOJsonAnnotation
//{
public partial class Coco
{
    /// <summary>
    /// 入力のJSON文字列をクラスに変換します。
    /// </summary>
    /// <param name="json">JSON文字列</param>
    /// <returns>SampleUserPoco型の出力</returns>
    public static Coco JsonToCoco(string json)
    {
        if (String.IsNullOrEmpty(json))
        {
            return null;
        }
        try
        {
            Coco coco = JsonSerializer.Deserialize<Coco>(json, GetOption());
            coco.ParseCoco();
            return coco;
        }
        catch (JsonException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }


    /// <summary>
    /// 入力をJSON文字列に変換します。
    /// </summary>
    /// <param name="dict">Dictionary<string, string>型の入力</param>
    /// <returns>JSON文字列</returns>
    public static string ToJson(Dictionary<string, string> dict)
    {
        var json = JsonSerializer.Serialize(dict, GetOption());
        return json;
    }

    /// <summary>
    /// 入力をJSON文字列に変換します。
    /// </summary>
    /// <param name="dict">Dictionary<string, int>型の入力</param>
    /// <returns>JSON文字列</returns>
    public static string ToJson(Dictionary<string, int> dict)
    {
        var json = JsonSerializer.Serialize(dict, GetOption());
        return json;
    }

    /// <summary>
    /// オプションを設定します。内部メソッドです。
    /// </summary>
    /// <returns>JsonSerializerOptions型のオプション</returns>
    public static JsonSerializerOptions GetOption()
    {
        // ユニコードのレンジ指定で日本語も正しく表示、インデントされるように指定
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true,
        };
        return options;
    }

    /// <summary>
    /// 入力をJSON文字列に変換します。
    /// </summary>
    /// <param name="poco">定義済みのクラスオブジェクト</param>
    /// <returns>JSON文字列 (入力が異常な場合はnull)</returns>
    public static string ToJson(Object poco)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(poco, GetOption());
            return jsonString;
        }
        catch (JsonException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    /*

    /// <summary>
    /// ユーザー情報を表すJSON全体に対応したPOCOなクラスの例。
    /// フィールド(メンバ変数名)がJSONのキーと対応しています。このクラスはデータのみで処理を行いません。
    /// </summary>
    public class SampleUserPoco
    {
        // 文字列型
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        // 真偽値型
        [JsonPropertyName("isExcellent")]
        public bool IsExcellent { get; set; }
        // 数値型
        [JsonPropertyName("someIntValue")]
        public int SomeIntValue { get; set; }
        [JsonPropertyName("someDoubleValue")]
        public double? SomeDoubleValue { get; set; }
        // リスト型
        [JsonPropertyName("kvs")]
        public IList<SampleKvPoco> Kvs { get; set; }
    }


    /// <summary>
    /// キーと値を持つJSON全体に対応したPOCOなクラスの例。
    /// フィールド(メンバ変数名)がJSONのキーと対応しています。このクラスはデータのみで処理を行いません。
    /// </summary>
    public class SampleKvPoco
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
    */
    //public class Coco
    //{
    public Coco()
    {
        info = new Info();
        images = new List<Images>();
        licenses = new List<Licenses>();
        type = "instances";
        annotations = new List<Annotation>();
        categories = new List<Categories>();
    }

    public void ParseCoco()
    {
        //Vector2にする
        foreach (var a in annotations)
        {
            a.ToVector2();
        }
        //イメージやカテゴリにリンク
        foreach (var img in images)
        {
            img.annotationList.Clear();
        }
        foreach (var a in annotations)
        {
            int imageID = (int)a.image_id;
            foreach (var img in images)
            {
                if (img.id == imageID)
                {
                    img.annotationList.Add(a);
                    break;
                }
            }

        }
    }


    [JsonPropertyName("info")]
    public Info info { get; set; }
    [JsonPropertyName("licenses")]
    public IList<Licenses> licenses { get; set; }
    [JsonPropertyName("categories")]
    public IList<Categories> categories { get; set; }
    [JsonPropertyName("images")]
    public IList<Images> images { get; set; }
    [JsonPropertyName("type")]
    public string type { get; set; }

    [JsonPropertyName("annotations")]
    public IList<Annotation> annotations { get; set; }
    //}

    public class Info
    {
        public Info()
        {
            year = 0;
            version = "0.0";
            description = "";
            date_created = "";
        }
        [JsonPropertyName("year")]
        public int year { get; set; }
        [JsonPropertyName("version")]
        public string version { get; set; }
        [JsonPropertyName("description")]
        public string description { get; set; }
        [JsonPropertyName("date_created")]
        public string date_created { get; set; }
    }

    public class Images
    {
        public Images()
        {
            date_captured = "";
            file_name = "";
            id = 0;
            height = 0;
            width = 0;
            isActive = false;
            annotationList= new List<Annotation>();
        }
        [JsonPropertyName("date_captured")]
        public string date_captured { get; set; }
        [JsonPropertyName("file_name")]
        public string file_name { get; set; }
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("height")]
        public int height { get; set; }
        [JsonPropertyName("width")]
        public int width { get; set; }


        [JsonIgnore]
        public bool isActive ;
        [JsonIgnore]
        public List<Annotation> annotationList;
    }

    public class Licenses
    {
        public Licenses()
        {
            id = 0;
            name = "";
            url = "";
        }
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("name")]
        public string name { get; set; }//"GNU General Public License v3.0",
        [JsonPropertyName("url")]
        public string url { get; set; }//"https://github.com/zhiqwang/yolov5-rt-stack/blob/master/LICENSE"
    }
    public partial class Annotation
    {
        public Annotation()
        {
            segmentation = new List<IList<double>>();
            bbox = new List<double>();
            area = 0;
            iscrowd = 0;
            image_id = 0;
            category_id = 0;
            id = 0;

            _BoundigBoxPosition = new Vector2();
            _BoundigBoxSize = new Vector2();
            //_Segmentation = new List<List<Vector2>>();
        }

        public Annotation CloneMyself()
        {
            Annotation a = new Annotation();
            foreach (var item in this.segmentation)
            {
                a.segmentation.Add(new List<double>(item));
            }
            a.bbox = new List<double>(this.bbox);
            a.area = this.area;
            a.iscrowd = this.iscrowd;
            a.image_id = this.image_id;
            a.category_id = this.category_id;
            a.id = this.id;

            a._BoundigBoxPosition = this._BoundigBoxPosition;
            a._BoundigBoxSize = this._BoundigBoxSize;
            //a._Segmentation = this._Segmentation;

            return a;
        }

        //アノテーションリストのクローン
        public static List<Coco.Annotation> CloneAnnotationList(List<Coco.Annotation> annotationList)
        {
            List<Coco.Annotation> cloneAnnotationList = new List<Coco.Annotation>();
            foreach (var a in annotationList)
            {
                cloneAnnotationList.Add(a.CloneMyself());
            }
            return cloneAnnotationList;
        }

        /*いらない。ていうか使っちゃだめだと思う。
        //アノテーションリストのクローン
        //リストA1のクローンを返す。ただし、リストA2,リストB2に現物が入っており、リンクを保持する。
        public static List<Coco.Annotation> CloneAnnotationList(
            List<Coco.Annotation> annotationListA1,
            List<Coco.Annotation> annotationListA2,
            List<Coco.Annotation> annotationListB2
            )
        {
            List<Coco.Annotation> cloneAnnotationList = new List<Coco.Annotation>();
            foreach (var a in annotationListA1)
            {
                var i = annotationListA2.IndexOf(a);
                cloneAnnotationList.Add(annotationListB2[i]);
            }
            return cloneAnnotationList;
        }
        */


        public void ToVector2()
        {
            _BoundigBoxPosition = new Vector2((float)bbox[0], (float)bbox[1]);
            _BoundigBoxSize = new Vector2((float)bbox[2], (float)bbox[3]);
        }
        public void FromVector2()
        {
            area= _BoundigBoxSize.X * _BoundigBoxSize.Y;

             var x0 = _BoundigBoxPosition.X;
            var y0 = _BoundigBoxPosition.Y;
            var x1 = _BoundigBoxSize.X;
            var y1 = _BoundigBoxSize.Y;
           var seg = new List<double>();
            seg.Add(x0);seg.Add(y0);
            seg.Add(x0);seg.Add(y1);
            seg.Add(x1);seg.Add(y1);
            seg.Add(x1);seg.Add(y0);
            segmentation.Clear();
            segmentation.Add(seg);

            bbox.Clear();
            bbox.Add(x0);
            bbox.Add(y0);
            bbox.Add(x1);
            bbox.Add(y1);
        }
    }
    public partial class Annotation
    {
        [JsonPropertyName("segmentation")]
        public IList<IList<double>> segmentation { get; set; }
        [JsonPropertyName("area")]
        public double area { get; set; }
        [JsonPropertyName("iscrowd")]
        public int iscrowd { get; set; }
        [JsonPropertyName("image_id")]
        public int image_id { get; set; }
        [JsonPropertyName("bbox")]
        public IList<double> bbox { get; set; }
        [JsonPropertyName("category_id")]
        public int category_id { get; set; }
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonIgnore]
        public Vector2 _BoundigBoxPosition { get; set; }
        [JsonIgnore]
        public Vector2 _BoundigBoxSize { get; set; }
        //[JsonIgnore] 
        //public List<List<Vector2>> _Segmentation { get; set; }

        //[JsonIgnore] 
        //public Categories? category = null;

    }
    public class Categories
    {
        public Categories()
        {
            id = 0;
            name = "";
            supercategory = "";
        }
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("name")]
        public string name { get; set; }
        [JsonPropertyName("supercategory")]
        public string supercategory { get; set; }

    }
}
//}


/*
{
    "info": {
        "year": 2021,
    "version": "1.0",
    "description": "For object detection",
    "date_created": "2021"
    },
  "images": [
    {
        "date_captured": "2021",
      "file_name": "000000000001.jpg",
      "id": 1,
      "height": 480,
      "width": 640
    },
    {
      "date_captured": "2021",
      "file_name": "000000000002.jpg",
      "id": 2,
      "height": 426,
      "width": 640
    },
    {
      "date_captured": "2021",
      "file_name": "000000000003.jpg",
      "id": 3,
      "height": 428,
      "width": 640
    },
    {
      "date_captured": "2021",
      "file_name": "000000000004.jpg",
      "id": 4,
      "height": 425,
      "width": 640
    },
    {
        "date_captured": "2021",
      "file_name": "000000000005.jpg",
      "id": 5,
      "height": 640,
      "width": 481
    }
  ],
  "licenses": [
    {
        "id": 1,
      "name": "GNU General Public License v3.0",
      "url": "https://github.com/zhiqwang/yolov5-rt-stack/blob/master/LICENSE"
    }
  ],
  "type": "instances",
  "annotations": [
    {
        "segmentation": [ [ 1.0799999999999272, 187.69008000000002, 612.66976, 187.69008000000002, 612.66976, 473.53008000000005, 1.0799999999999272, 473.53008000000005 ] ],
      "area": 174816.81699840003,
      "iscrowd": 0,
      "image_id": 1,
      "bbox": [ 1.0799999999999272, 187.69008000000002, 611.5897600000001, 285.84000000000003 ],
      "category_id": 19,
      "id": 1
    },
    {
        "segmentation": [ [ 311.73024, 4.310159999999996, 631.0102400000001, 4.310159999999996, 631.0102400000001, 232.99032, 311.73024, 232.99032 ] ],
      "area": 73013.00148480001,
      "iscrowd": 0,
      "image_id": 1,
      "bbox": [ 311.73024, 4.310159999999996, 319.28000000000003, 228.68016 ],
      "category_id": 50,
      "id": 2
    },
    {
        "segmentation": [ [ 249.60032, 229.27031999999997, 565.84032, 229.27031999999997, 565.84032, 474.35015999999996, 249.60032, 474.35015999999996 ] ],
      "area": 77504.04860159999,
      "iscrowd": 0,
      "image_id": 1,
      "bbox": [ 249.60032, 229.27031999999997, 316.24, 245.07984 ],
      "category_id": 70,
      "id": 3
    },
    {
        "segmentation": [ [ 0.00031999999998788553, 13.510079999999988, 434.48032, 13.510079999999988, 434.48032, 388.63008, 0.00031999999998788553, 388.63008 ] ],
      "area": 162982.13760000002,
      "iscrowd": 0,
      "image_id": 1,
      "bbox": [ 0.00031999999998788553, 13.510079999999988, 434.48, 375.12 ],
      "category_id": 38,
      "id": 4
    },
    {
        "segmentation": [ [ 376.2, 40.36008, 451.75007999999997, 40.36008, 451.75007999999997, 86.88983999999999, 376.2, 86.88983999999999 ] ],
      "area": 3515.3270903807993,
      "iscrowd": 0,
      "image_id": 1,
      "bbox": [ 376.2, 40.36008, 75.55008, 46.529759999999996 ],
      "category_id": 33,
      "id": 5
    },
    {
        "segmentation": [ [ 465.77984, 38.97, 523.8496, 38.97, 523.8496, 85.63991999999999, 465.77984, 85.63991999999999 ] ],
      "area": 2710.1110536191995,
      "iscrowd": 0,
      "image_id": 1,
      "bbox": [ 465.77984, 38.97, 58.069759999999995, 46.66992 ],
      "category_id": 8,
      "id": 6
    },
    {
        "segmentation": [ [ 385.70016, 73.65984, 469.71999999999997, 73.65984, 469.71999999999997, 144.16992, 385.70016, 144.16992 ] ],
      "area": 5924.245639987201,
      "iscrowd": 0,
      "image_id": 1,
      "bbox": [ 385.70016, 73.65984, 84.01984, 70.51008 ],
      "category_id": 62,
      "id": 7
    },
    {
        "segmentation": [ [ 364.0496, 2.49024, 458.80992000000003, 2.49024, 458.80992000000003, 73.56, 364.0496, 73.56 ] ],
      "area": 6734.593199923201,
      "iscrowd": 0,
      "image_id": 1,
      "bbox": [ 364.0496, 2.49024, 94.76032000000001, 71.06976 ],
      "category_id": 45,
      "id": 8
    },
    {
        "segmentation": [ [ 385.52992, 60.030002999999994, 600.50016, 60.030002999999994, 600.50016, 357.19013700000005, 385.52992, 357.19013700000005 ] ],
      "area": 63880.58532441216,
      "iscrowd": 0,
      "image_id": 2,
      "bbox": [ 385.52992, 60.030002999999994, 214.97024, 297.160134 ],
      "category_id": 71,
      "id": 9
    },
    {
        "segmentation": [ [ 53.01024000000001, 356.49000599999994, 185.04032, 356.49000599999994, 185.04032, 411.6800099999999, 53.01024000000001, 411.6800099999999 ] ],
      "area": 7286.7406433203205,
      "iscrowd": 0,
      "image_id": 2,
      "bbox": [ 53.01024000000001, 356.49000599999994, 132.03008, 55.190004 ],
      "category_id": 27,
      "id": 10
    },
    {
        "segmentation": [ [ 204.86016, 31.019728000000015, 459.74016, 31.019728000000015, 459.74016, 355.13984800000003, 204.86016, 355.13984800000003 ] ],
      "area": 82611.73618559999,
      "iscrowd": 0,
      "image_id": 3,
      "bbox": [ 204.86016, 31.019728000000015, 254.88, 324.12012 ],
      "category_id": 27,
      "id": 11
    },
    {
        "segmentation": [ [ 237.56032, 155.809976, 403.96032, 155.809976, 403.96032, 351.060152, 237.56032, 351.060152 ] ],
      "area": 32489.6292864,
      "iscrowd": 0,
      "image_id": 3,
      "bbox": [ 237.56032, 155.809976, 166.4, 195.25017599999998 ],
      "category_id": 58,
      "id": 12
    },
    {
        "segmentation": [ [ 0.960000000000008, 20.060000000000002, 442.19007999999997, 20.060000000000002, 442.19007999999997, 399.21015, 0.960000000000008, 399.21015 ] ],
      "area": 167292.451016512,
      "iscrowd": 0,
      "image_id": 4,
      "bbox": [ 0.960000000000008, 20.060000000000002, 441.23008, 379.15015 ],
      "category_id": 19,
      "id": 13
    },
    {
        "segmentation": [ [ 0, 50.11967999999999, 457.680158, 50.11967999999999, 457.680158, 480.46975999999995, 0, 480.46975999999995 ] ],
      "area": 196962.69260971263,
      "iscrowd": 0,
      "image_id": 5,
      "bbox": [ 0, 50.11967999999999, 457.680158, 430.35008 ],
      "category_id": 35,
      "id": 14
    },
    {
        "segmentation": [ [ 167.5801595, 162.88991999999993, 478.19023849999996, 162.88991999999993, 478.19023849999996, 628.0796799999999, 167.5801595, 628.0796799999999 ] ],
      "area": 144492.62810359104,
      "iscrowd": 0,
      "image_id": 5,
      "bbox": [ 167.5801595, 162.88991999999993, 310.610079, 465.18976000000004 ],
      "category_id": 57,
      "id": 15
    }
  ],
  "categories": [
    {
        "id": 1,
      "name": "0",
      "supercategory": "0"
    },
    {
        "id": 2,
      "name": "1",
      "supercategory": "1"
    },
    {
        "id": 3,
      "name": "2",
      "supercategory": "2"
    },
    {
        "id": 4,
      "name": "3",
      "supercategory": "3"
    },
    {
        "id": 5,
      "name": "4",
      "supercategory": "4"
    },
    {
        "id": 6,
      "name": "5",
      "supercategory": "5"
    },
    {
        "id": 7,
      "name": "6",
      "supercategory": "6"
    },
    {
        "id": 8,
      "name": "7",
      "supercategory": "7"
    },
    {
        "id": 9,
      "name": "8",
      "supercategory": "8"
    },
    {
        "id": 10,
      "name": "9",
      "supercategory": "9"
    },
    {
        "id": 11,
      "name": "10",
      "supercategory": "10"
    },
    {
        "id": 12,
      "name": "11",
      "supercategory": "11"
    },
    {
        "id": 13,
      "name": "12",
      "supercategory": "12"
    },
    {
        "id": 14,
      "name": "13",
      "supercategory": "13"
    },
    {
        "id": 15,
      "name": "14",
      "supercategory": "14"
    },
    {
        "id": 16,
      "name": "15",
      "supercategory": "15"
    },
    {
        "id": 17,
      "name": "16",
      "supercategory": "16"
    },
    {
        "id": 18,
      "name": "17",
      "supercategory": "17"
    },
    {
        "id": 19,
      "name": "18",
      "supercategory": "18"
    },
    {
        "id": 20,
      "name": "19",
      "supercategory": "19"
    },
    {
        "id": 21,
      "name": "20",
      "supercategory": "20"
    },
    {
        "id": 22,
      "name": "21",
      "supercategory": "21"
    },
    {
        "id": 23,
      "name": "22",
      "supercategory": "22"
    },
    {
        "id": 24,
      "name": "23",
      "supercategory": "23"
    },
    {
        "id": 25,
      "name": "24",
      "supercategory": "24"
    },
    {
        "id": 26,
      "name": "25",
      "supercategory": "25"
    },
    {
        "id": 27,
      "name": "26",
      "supercategory": "26"
    },
    {
        "id": 28,
      "name": "27",
      "supercategory": "27"
    },
    {
        "id": 29,
      "name": "28",
      "supercategory": "28"
    },
    {
        "id": 30,
      "name": "29",
      "supercategory": "29"
    },
    {
        "id": 31,
      "name": "30",
      "supercategory": "30"
    },
    {
        "id": 32,
      "name": "31",
      "supercategory": "31"
    },
    {
        "id": 33,
      "name": "32",
      "supercategory": "32"
    },
    {
        "id": 34,
      "name": "33",
      "supercategory": "33"
    },
    {
        "id": 35,
      "name": "34",
      "supercategory": "34"
    },
    {
        "id": 36,
      "name": "35",
      "supercategory": "35"
    },
    {
        "id": 37,
      "name": "36",
      "supercategory": "36"
    },
    {
        "id": 38,
      "name": "37",
      "supercategory": "37"
    },
    {
        "id": 39,
      "name": "38",
      "supercategory": "38"
    },
    {
        "id": 40,
      "name": "39",
      "supercategory": "39"
    },
    {
        "id": 41,
      "name": "40",
      "supercategory": "40"
    },
    {
        "id": 42,
      "name": "41",
      "supercategory": "41"
    },
    {
        "id": 43,
      "name": "42",
      "supercategory": "42"
    },
    {
        "id": 44,
      "name": "43",
      "supercategory": "43"
    },
    {
        "id": 45,
      "name": "44",
      "supercategory": "44"
    },
    {
        "id": 46,
      "name": "45",
      "supercategory": "45"
    },
    {
        "id": 47,
      "name": "46",
      "supercategory": "46"
    },
    {
        "id": 48,
      "name": "47",
      "supercategory": "47"
    },
    {
        "id": 49,
      "name": "48",
      "supercategory": "48"
    },
    {
        "id": 50,
      "name": "49",
      "supercategory": "49"
    },
    {
        "id": 51,
      "name": "50",
      "supercategory": "50"
    },
    {
        "id": 52,
      "name": "51",
      "supercategory": "51"
    },
    {
        "id": 53,
      "name": "52",
      "supercategory": "52"
    },
    {
        "id": 54,
      "name": "53",
      "supercategory": "53"
    },
    {
        "id": 55,
      "name": "54",
      "supercategory": "54"
    },
    {
        "id": 56,
      "name": "55",
      "supercategory": "55"
    },
    {
        "id": 57,
      "name": "56",
      "supercategory": "56"
    },
    {
        "id": 58,
      "name": "57",
      "supercategory": "57"
    },
    {
        "id": 59,
      "name": "58",
      "supercategory": "58"
    },
    {
        "id": 60,
      "name": "59",
      "supercategory": "59"
    },
    {
        "id": 61,
      "name": "60",
      "supercategory": "60"
    },
    {
        "id": 62,
      "name": "61",
      "supercategory": "61"
    },
    {
        "id": 63,
      "name": "62",
      "supercategory": "62"
    },
    {
        "id": 64,
      "name": "63",
      "supercategory": "63"
    },
    {
        "id": 65,
      "name": "64",
      "supercategory": "64"
    },
    {
        "id": 66,
      "name": "65",
      "supercategory": "65"
    },
    {
        "id": 67,
      "name": "66",
      "supercategory": "66"
    },
    {
        "id": 68,
      "name": "67",
      "supercategory": "67"
    },
    {
        "id": 69,
      "name": "68",
      "supercategory": "68"
    },
    {
        "id": 70,
      "name": "69",
      "supercategory": "69"
    },
    {
        "id": 71,
      "name": "70",
      "supercategory": "70"
    }
  ]
}
*/
