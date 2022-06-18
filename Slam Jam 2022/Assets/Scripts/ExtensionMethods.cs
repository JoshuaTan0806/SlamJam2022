using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public static class ExtensionMethods
{
    public static void ResetPosition(this Transform transform)
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        transform.localScale = Vector3.one;
    }

    public static T V2ToCoordinates<T>(this T[,] ts, Vector2Int vector2Int)
    {
        return ts[vector2Int.x, vector2Int.y];
    }

    public static string AddSpace(this string str)
    {
        string newString = "";

        for (int i = 0; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]))
                newString = " ";
            
            newString += str[i];
        }

        return newString;
    }

    public static void MoveToPosition(this Transform transform, Vector3 destination, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    public static void LerpToPosition(this Transform transform, Vector3 destination, float speed)
    {
        transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
    }

    public static bool IsEven(this int num)
    {
        return num % 2 == 0;
    }

    public static bool IsOdd(this int num)
    {
        return num % 2 == 1;
    }

    public static bool IsNull<T>(this T t)
    {
        return t == null;
    }

    public static bool IsNullOrEmpty<T>(this T[] array)
    {
        return array == null || array.Length == 0;
    }

    public static bool IsNullOrEmpty<T>(this List<T> list)
    {
        return list == null || list.Count == 0;
    }

    public static bool IsNullOrEmpty<T1, T2>(this Dictionary<T1, T2> dic)
    {
        return dic == null || dic.Count == 0;
    }

    public static void StartCoroutine(this MonoBehaviour monoBehaviour, System.Action action, float seconds)
    {
        monoBehaviour.StartCoroutine(Coroutine(action, seconds));
    }

    static IEnumerator Coroutine(System.Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }

    public static bool IsAtPosition(this Transform transform, Vector3 destination, float threshhold)
    {
        return Vector3.Distance(transform.position, destination) < threshhold;
    }

    public static Vector3 V2ToV3(this Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0);
    }

    public static Vector2 V3ToV2(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }

    public static Vector2Int V2ToV2Int(this Vector2 vector2)
    {
        return new Vector2Int((int)vector2.x, (int)vector2.y);
    }

    public static Vector3Int V3ToV3Int(this Vector3 vector3)
    {
        return new Vector3Int((int)vector3.x, (int)vector3.y, (int)vector3.z);
    }

    public static T ChooseRandomElementInList<T>(this List<T> ts, bool RemoveFromList = false)
    {
        T t = ts[UnityEngine.Random.Range(0, ts.Count)];

        if(RemoveFromList)
        ts.Remove(t);

        return t;
    }

    public static T ChooseRandomElementInArray<T>(this T[] ts)
    {
        T t = ts[UnityEngine.Random.Range(0, ts.Length)];

        return t;
    }

    public static float Rand(this Vector2 vector2)
    {
        return UnityEngine.Random.Range(vector2.x, vector2.y);
    }

    public static int Rand(this Vector2Int vector2Int)
    {
        return UnityEngine.Random.Range(vector2Int.x, vector2Int.y);
    }

    public static Vector2 OnUnitCircle()
    {
        return UnityEngine.Random.insideUnitCircle.normalized;
    }

    public static List<T> Clone<T>(this List<T> ts) where T : ScriptableObject
    {
        List<T> newTs = new List<T>();

        for (int i = 0; i < ts.Count; i++)
        {
            newTs.Add(UnityEngine.Object.Instantiate(ts[i]));
        }

        return newTs;
    }

    public static Transform NextChild(this Transform transform)
    {
        // Check where we are
        int thisIndex = transform.GetSiblingIndex();

        // We have a few cases to rule out
        if (transform.parent == null)
            return null;
        if (transform.parent.childCount <= thisIndex + 1)
            return null;

        // Then return whatever was next, now that we're sure it's there
        return transform.parent.GetChild(thisIndex + 1);
    }

    public static GameObject NextChild(this GameObject gameObject)
    {
        // Check where we are
        int thisIndex = gameObject.transform.GetSiblingIndex();

        // We have a few cases to rule out
        if (gameObject.transform.parent == null)
            return null;
        if (gameObject.transform.parent.childCount <= thisIndex + 1)
            return null;

        // Then return whatever was next, now that we're sure it's there
        return gameObject.transform.parent.GetChild(thisIndex + 1).gameObject;
    }

    public static Transform PreviousChild(this Transform transform)
    {
        // Check where we are
        int thisIndex = transform.GetSiblingIndex();

        // We have a few cases to rule out
        if (transform.parent == null)
            return null;
        if (thisIndex - 1 < 0)
            return null;

        // Then return whatever was next, now that we're sure it's there
        return transform.parent.GetChild(thisIndex - 1);
    }

    public static GameObject PreviousChild(this GameObject gameObject)
    {
        // Check where we are
        int thisIndex = gameObject.transform.GetSiblingIndex();

        // We have a few cases to rule out
        if (gameObject.transform.parent == null)
            return null;
        if (thisIndex - 1 < 0)
            return null;

        // Then return whatever was next, now that we're sure it's there
        return gameObject.transform.parent.GetChild(thisIndex - 1).gameObject;
    }

    public static void SetActive(this Transform transform, bool toggle)
    {
        transform.gameObject.SetActive(toggle);
    }

    public static void SetParent(this GameObject gameObject, Transform transform)
    {
        gameObject.transform.SetParent(transform);
    }

    public static void SetParent(this GameObject gameObject, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
    }

    public static int ChildCount(this GameObject gameObject)
    {
        return gameObject.transform.childCount;
    }

    public static GameObject GetChild(this GameObject gameObject, int index)
    {
        return gameObject.transform.GetChild(index).gameObject;
    }

    public static GameObject Parent(this GameObject gameObject)
    {
        return gameObject.transform.parent.gameObject;
    }

    public static int EnumLength(this Enum e)
    {
        return System.Enum.GetNames(e.GetType()).Length;
    }

    public static T Nothing<T>(this T t) where T : IConvertible
    {
        t = (T)(IConvertible)(0);
        return t;
    }

    public static T Everything<T>(this T t) where T : IConvertible
    {
        t = (T)(IConvertible)(-1);
        return t;
    }

    public static void ForEach(this Enum e, Action<Enum> action)
    {
        foreach (var type in Enum.GetValues(e.GetType()))
        {
            action((Enum)type);
        }
    }

    public static void DestroyRootObject(this GameObject gameObject)
    {
        UnityEngine.Object.Destroy(gameObject.transform.root.gameObject);
    }

    public static void DestroyRootObject(this GameObject gameObject, float seconds)
    {
        UnityEngine.Object.Destroy(gameObject.transform.root.gameObject, seconds);
    }

    public static void OnButtonClick(this MonoBehaviour monoBehaviour, System.Action action)
    {
        monoBehaviour.GetComponent<Button>().onClick.AddListener(() => action?.Invoke());
    }

    public static void PlayNewClip(this AudioSource audioSource, AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.time = 0;
        audioSource.Play();
    }

    public static T LastElement<T>(this T[] ts)
    {
        return ts[ts.Length - 1];
    }

    public static T LastElement<T>(this List<T> ts)
    {
        return ts[ts.Count - 1];
    }

    public static Vector3 GetPosition<T>(this T t) where T : Behaviour
    {
        return t.transform.position;
    }

    public static void SetPosition<T>(this T t, Vector3 vector3) where T : Behaviour
    {
        t.transform.position = vector3;
    }
}
