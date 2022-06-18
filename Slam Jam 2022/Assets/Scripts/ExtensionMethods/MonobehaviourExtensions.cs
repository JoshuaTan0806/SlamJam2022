using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonobehaviourExtensions
{
	/// <summary>
	/// Waits for a frame, and then performs an action
	/// </summary>
	/// <param name="mono"></param>
	/// <param name="action"></param>
	public static void PerformAtEndOfFrame(this MonoBehaviour mono, Action action)
	{
		mono.StartCoroutine(WaitForEndOfFrame(action));
	}
	static IEnumerator WaitForEndOfFrame(Action action)
	{
		yield return new WaitForEndOfFrame();

		action?.Invoke();
	}

	/// <summary>
	/// Waits a delay, and then performs an action
	/// </summary>
	/// <param name="mono"></param>
	/// <param name="action">Action to perform</param>
	/// <param name="delay">How long to wait before performing it</param>
	/// <param name="realTime">If true, delay is afftected by timeScale</param>
	public static void PerformAfterDelay(this MonoBehaviour mono, Action action, float delay, bool realTime = false)
	{
		mono.StartCoroutine(PerformAfterDelay(action, delay, realTime));
	}
	static IEnumerator PerformAfterDelay(Action action, float delay, bool realTime)
	{
		if (realTime == false)
			yield return new WaitForSeconds(delay);
		else
			yield return new WaitForSecondsRealtime(delay);

		action?.Invoke();
	}

	/// <summary>
	/// Returns true if the MonoBehavior has this component
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="mono"></param>
	/// <returns></returns>
	public static bool HasComponent<T>(this MonoBehaviour mono) where T : MonoBehaviour
	{
		return mono.GetComponent<T>() != null;
	}

	/// <summary>
	/// Returns a component if it is already on the MonoBehavior, or returns it
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="gameObject"></param>
	/// <returns></returns>
	public static T GetOrAddComponent<T>(this MonoBehaviour mono) where T : MonoBehaviour
	{
		var comp = mono.GetComponent<T>();

		return comp ?? mono.gameObject.AddComponent<T>();
	}

	public static bool IsOnScreen(this RectTransform rect)
	{
		Rect screenRect = new Rect(0,0,Screen.width, Screen.height);

		Vector3[] corners = new Vector3[4];
		rect.GetWorldCorners(corners);

		foreach (var corner in corners)
			if (!screenRect.Contains(corner))
				return false;

		return true;
	}
}
