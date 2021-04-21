using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugs
{
    public static void Log(params object[] list)
    {
        var log = "";

        for (int i = 0; i < list.Length; i++)
        {
            log += list[i];
            if (i + 1 < list.Length) log += " --- ";
        }
        Debug.Log(log);
    }

    public static void Log<T>(List<T> main)
    {
        Debug.Log(LogList(main));
    }

    public static void Log<T>(T[] main)
    {
        Debug.Log(LogList(main));
    }

    private static string LogList<T>(List<T> list)
    {
        var log = "";

        for (int i = 0; i < list.Count; i++)
        {
            log += list[i];
            if (i + 1 < list.Count) log += " --- ";
        }
        return log;
    }

    private static string LogList<T>(T[] list)
    {
        var log = "";

        for (int i = 0; i < list.Length; i++)
        {
            log += list[i];
            if (i + 1 < list.Length) log += " --- ";
        }
        return log;
    }
}
