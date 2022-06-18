using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIEyes : MonoBehaviour
{
    [Tooltip("How close to the AI something can be before it is automatically noticed")]
    [SerializeField] float autoViewDist = 15;

    [Range(1, 359)]
    [SerializeField] float fieldOfView = 45;
    float dotView;
    [SerializeField] float viewDistance = 50;

    [Space]

    [Tooltip("What layers can block our view")]
    [SerializeField] LayerMask visionMask;

    private void Awake()
    {
        dotView = Mathf.Cos(fieldOfView);
    }

    public bool CanSeeTransform(Transform target)
    {
        var dist = Vector3.Distance(target.position, transform.position);

        if (dist < autoViewDist)
            return true;

        if (dist > viewDistance)
            return false;

        //If we are facing towards the target
        if (Vector3.Dot(transform.forward, Vector3.Normalize(target.position - transform.position)) > dotView)
            return true;

        //If there are no obstructables blocking the view
        if (Physics.Raycast(transform.position, target.position, dist, visionMask) == false)
            return true;

        return false;
    }

    public Transform[] GetAllInSight()
    {
        Transform[] seen = Physics.OverlapSphere(transform.position, viewDistance, ~visionMask).Select(t => t.transform).ToArray();

        return seen;
    }
}
