using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class NewNameCreator
{
    public static string CreateWithRemoveSafix
           (string target, List<string> targetList, int numericLength, string safix)
    {
        if (safix == null)
        {
            safix = "";
        }
        var pattern = "(^" + safix + "$)";
        var prefix = Regex.Replace(target, pattern, "", RegexOptions.None);
        return Create
        (targetList, prefix, numericLength);
    }
    public static string RemoveSafix
    (string target, string safix)
    {
        if (safix == null)
        {
            safix = "";
        }
        var pattern = "(^" + safix + "$)";
        return Regex.Replace(target, pattern, "", RegexOptions.None);

    }

    public static string CreateA
(List<string> targetList, string prefix, int numericLength, string safix = "")
    {

        if (safix == null)
        {
            safix = "";
        }
        //リスト中になければ、そのままprefix+safixでよい
        //var s0 = prefix + safix;
        //if (targetList.IndexOf(s0) < 0)
        //{
        /// /   return s0;
        //}


        //既製品リスト
        List<int> list = new List<int>();
        foreach (string s in targetList)
        {
            int n = CheckName(s, prefix, safix);
            if (n >= 0)
            {
                list.Add(n);
            }
        }

        //候補名
        int i = 0;
        do
        {
            if (list.IndexOf(i) < 0)
            {
                //var numericLength = 4;
                var ss = String.Format("{0:D" + numericLength.ToString() + "}", i);
                var target = prefix + ss + safix;
                return target;
            };
            i++;
        } while (true);
    }
    public static string Create
    (List<string> targetList, string prefix, int numericLength, string safix = "")
    {

        if (safix == null)
        {
            safix = "";
        }
        //リスト中になければ、そのままprefix+safixでよい
        var s0 = prefix + safix;
        if (targetList.IndexOf(s0) < 0)
        {
            return s0;
        }


        //既製品リスト
        List<int> list = new List<int>();
        foreach (string s in targetList)
        {
            int n = CheckName(s, prefix, safix);
            if (n >= 0)
            {
                list.Add(n);
            }
        }

        //候補名
        int i = 0;
        do
        {
            if (list.IndexOf(i) < 0)
            {
                //var numericLength = 4;
                var ss = String.Format("{0:D" + numericLength.ToString() + "}", i);
                var target = prefix + ss + safix;
                return target;
            };
            i++;
        } while (true);
    }
    public static int CheckName
       (string target, string prefix, string safix)
    {

        if (safix == null)
        {
            safix = "";
        }
        //var target = "test005bak";        
        //ar prefix = "test";        
        //var safix = "bak";
        int number = -1;
        var pattern = "(^" + prefix + "[0-9]*" + safix + "$)";
        var regex = Regex.Match(target, pattern);
        if (regex.Success)
        {
            pattern = "(" + "[0-9]*" + safix + "$)";
            regex = Regex.Match(target, pattern);
            var iIndex = regex.Index;
            var iLength = regex.Length - safix.Length;
            var ss = target.Substring(iIndex, iLength);
            if (int.TryParse(ss, out int num))
            {
                number = num;
            }
        }
        return number;
    }

}

