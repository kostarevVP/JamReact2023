using System;
using UnityEngine;

namespace WKosArch.Extentions
{
    public static class Log 
    {
        public static void Print(string text) => 
            Debug.Log(text);

        public static void Print(string text, GameObject gameObject) => 
            Debug.Log(text, gameObject);

        public static void Print(string text, object obj) => 
            Debug.Log(text + "log from = " + obj.ToString());

        public static void PrintColor(string text, Color color) => 
            Debug.Log("<color=#" + ColorUtility.ToHtmlStringRGBA(color) + $">{text}" + "</color>");

        public static void PrintWarning(string text) => 
            Debug.LogWarning(text);

        public static void PrintWarning(string text, GameObject gameObject) => 
            Debug.LogWarning(text, gameObject);

        public static void PrintWarning(string text, object obj) => 
            Debug.Log(text + "log from = " + obj.ToString());

        public static void PrintError(string text) => 
            Debug.LogError(text);
        public static void PrintError(string text, object obj) =>
           Debug.LogError(text + "log from = " + obj.ToString());

        public static void PrintError(string text, GameObject gameObject) => 
            Debug.LogError(text, gameObject);

        public static void CheckForNull<T>(T o, string errorMessage)
        {
            if (o == null)
            {
                Debug.Log(errorMessage);
            }
        }
    }
}
