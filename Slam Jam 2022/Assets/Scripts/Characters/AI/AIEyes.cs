using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIEyes : MonoBehaviour
{
	[Tooltip("How close to the AI something can be before it is automatically noticed")]
	[SerializeField] float autoViewDist = 15;

	[Range(1, 179)]
	[SerializeField] float fieldOfView = 45;
	float dotView;

	[SerializeField] float viewDistance = 50;

	[Space]

	[Tooltip("What layers can block our view")]
	[SerializeField] LayerMask visionMask;

	private void Awake()
	{
		dotView = (((Mathf.Abs(fieldOfView) - 180f) * (1f / 90f)) * -1f) - 1f;
	}

	public bool CanSeeTransform(Transform target)
	{
		var dist = Vector3.Distance(target.position, transform.position);

		if (dist < autoViewDist)
			return true;

		if (dist > viewDistance)
			return false;

		//If we are facing away from the target
		if (!IsLookingTowards(target))
			return false;

		//If there are no obstructables blocking the view
		if (Physics.Raycast(transform.position, target.position, dist, visionMask) == false)
			return true;

		return false;
	}

	public List<Transform> GetAllInSight()
	{
		List<Transform> seen = Physics.OverlapSphere(transform.position, viewDistance, ~visionMask).Select(t => t.transform).ToList();

		List<Transform> inSight = new List<Transform>();
		foreach (var s in seen)
		{
			if (IsLookingTowards(s.transform))
			{
				inSight.Add(s);
			}
		}

		return inSight;
	}

	bool IsLookingTowards(Transform target)
	{
		float dot = Vector3.Dot(transform.forward, Vector3.Normalize(target.position - transform.position));

		return dot > dotView;
	}
}
