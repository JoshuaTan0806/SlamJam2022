using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBody : MonoBehaviour
{
    Animator animator;
    Rigidbody[] rbs;

    PlayerStats stats;

    private void Awake()
    {
        stats = GetComponentInParent<PlayerStats>();
        animator = GetComponent<Animator>();
        rbs = GetComponentsInChildren<Rigidbody>();

        stats.OnDeath += Ragdoll;

        UnRagdoll();
    }

    private void OnDestroy()
    {
        stats.OnDeath -= Ragdoll;
    }

    void Ragdoll()
    {
        animator.enabled = false;
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
        }
    }

    void UnRagdoll()
    {
        animator.enabled = true;
        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
        }

        //transform.rotation = Quaternion.identity;
    }

    public void SetMoving(bool val)
    {
        animator.SetBool("Run", val);
    }
}
