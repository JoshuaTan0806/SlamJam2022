using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summonable : MonoBehaviour
{
    public enum SummonPlace
    {
        OnPlayer,
        InFrontOfPlayer
    }

    [SerializeField] SummonPlace summonPlace;

    [SerializeField] bool parentToPlayer;

    [Space]

    [SerializeField] float summonLifeTime = -1;
    float aliveTime;

    PlayerStats summoner;
    SummonSpill spill;

    public virtual void Initialise(PlayerStats summoner, SummonSpill spill)
    {
        this.summoner = summoner;
        this.spill = spill;

        summoner.CurrentActiveSummons.Add(this);

        if (parentToPlayer)
            transform.SetParent(summoner.transform);

        switch (summonPlace)
        {
            case SummonPlace.OnPlayer:
                transform.localPosition = Vector3.zero;
                break;
        }
    }

    public virtual void DestroySummon()
    {
        summoner.CurrentActiveSummons.Remove(this);
    }

    private void Update()
    {
        aliveTime += Time.deltaTime;

        if(summonLifeTime > 0)
        {
            if(aliveTime >= summonLifeTime)
            {
                DestroySummon();
                Destroy(gameObject);
            }
        }
    }
}
