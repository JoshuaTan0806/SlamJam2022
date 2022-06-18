using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class GenericExtensions
{
    public static bool IsEven(this int num)
    {
        return num % 2 == 0;
    }

    public static bool IsOdd(this int num)
    {
        return num % 2 == 1;
    }

    public static string AddSpaces(string str)
    {
        return AddCharBeforeCaptitals(str, " ");
    }

    public static string AddNewLines(string str)
    {
        return AddCharBeforeCaptitals(str, "\n");
    }

    public static string AddCharBeforeCaptitals(string str, string toAdd)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(str[0]);

        for (int i = 1; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]))
                sb.Append(toAdd);
            sb.Append(str[i]);
        }

        return sb.ToString();
    }
}
