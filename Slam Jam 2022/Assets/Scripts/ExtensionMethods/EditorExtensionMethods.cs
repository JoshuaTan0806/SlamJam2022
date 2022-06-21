using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public class EditorExtensionMethods : Editor
{
    public static List<T> GetAllInstances<T>() where T : ScriptableObject
    {
        return AssetDatabase.FindAssets($"t: {typeof(T).Name}").ToList().Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>).ToList();
    }

    public static bool CreateAsset<T, U>(T target, string path, out T newT, out U newU) where T : ScriptableObject where U : ScriptableObject
    {
        newT = target;
        newU = null;

        path += target.name;
        path += ".asset";

        if (!File.Exists(path))
        {
            U u = CreateInstance<U>();
            AssetDatabase.CreateAsset(u, path);
            newU = u;

            return true;
        }

        return false;
    }

    public static void SaveAsset<T>(T t) where T : ScriptableObject
    {
        EditorUtility.SetDirty(t);
        AssetDatabase.SaveAssets();
    }
}
