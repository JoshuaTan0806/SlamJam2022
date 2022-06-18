using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Summonable))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed;

    [Tooltip("How long of a delay before gravity is enabled for this projectile, (-1 for never on, 0 for instantly on, etc)")]
    [SerializeField] float gravityEnableDelay = -1;

    Summonable summonable;

    private void Awake()
    {
        summonable = GetComponent<Summonable>();

    }

}
