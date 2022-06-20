using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summonable : MonoBehaviour
{
    [System.Serializable]
    public enum SummonPlace
    {
        OnPlayer,
        InFrontOfPlayer,
        RandomlyAroundPlayer,
    }

    [SerializeField] SummonPlace summonPlace;

    [SerializeField] bool parentToPlayer;

    [SerializeField] Vector3 spawnOffset;

    [Space]

    [SerializeField] float summonLifeTime = -1;
    float aliveTime;
    public float AliveTime => aliveTime;

    PlayerStats summoner;
    public PlayerStats Summoner => summoner;
    SummonSpill spill;
    public SummonSpill Spill => spill;

    public event System.Action OnSummonDestroyed;

    public virtual void Initialise(PlayerStats summoner, SummonSpill spill)
    {
        this.summoner = summoner;
        this.spill = spill;

        summoner.CurrentActiveSummons.Add(this);

        var casterAsSummonable = summoner.GetComponent<Summonable>();
        if (casterAsSummonable && casterAsSummonable.summoner)
        {
            casterAsSummonable.Summoner.CurrentActiveSummons.Add(this);
        }

        if (parentToPlayer)
            transform.SetParent(summoner.transform);

        switch (summonPlace)
        {
            case SummonPlace.OnPlayer:
                transform.position = summoner.transform.position;
                break;
            case SummonPlace.InFrontOfPlayer:
                transform.position = summoner.transform.position + summoner.transform.forward;
                break;
            case SummonPlace.RandomlyAroundPlayer:
                transform.position = summoner.transform.position + new Vector3((Random.insideUnitCircle * 2).x, 0, (Random.insideUnitCircle * 2).y);
                break;
        }
        transform.position += 
            transform.right * spawnOffset.x
            + transform.up* spawnOffset.y 
            + transform.forward * spawnOffset.z;

        transform.forward = summoner.transform.forward;
    }

    public virtual void DestroySummon()
    {
        OnSummonDestroyed?.Invoke();

        summoner.CurrentActiveSummons.Remove(this);

        var casterAsSummonable = summoner.GetComponent<Summonable>();
        if (casterAsSummonable && casterAsSummonable.summoner)
        {
            casterAsSummonable.Summoner.CurrentActiveSummons.Remove(this);
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        aliveTime += Time.deltaTime;

        if(summonLifeTime > 0)
        {
            if(aliveTime >= summonLifeTime)
            {
                DestroySummon();
            }
        }
    }
}
