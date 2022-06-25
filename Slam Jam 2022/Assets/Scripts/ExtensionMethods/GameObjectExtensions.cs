using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
	public static void DestroyChildren(this Transform transform)
	{
		foreach (Transform t in transform)
			Object.Destroy(t.gameObject);
	}
}
