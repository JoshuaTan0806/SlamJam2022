using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KillBox : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerStats stats))
        {
            stats.TakeDamage(100000);
        }
    }
}
