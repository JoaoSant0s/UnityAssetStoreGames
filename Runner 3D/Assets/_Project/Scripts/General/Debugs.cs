using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JoaoSantos.General
{
    public class Debugs
    {
        public static void Log(params object[] list)
        {
            var log = "";

            for (int i = 0; i < list.Length; i++)
            {
                log += list[i];

                if (i + 1 < list.Length) log += ", ";
            }

            Debug.Log(log);
        }
    }
}