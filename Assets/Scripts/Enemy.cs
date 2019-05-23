using System.Collections;
using System.Collections.Generic;
using Core;
using Interfaces;
using UniRx;
using UnityEngine;
using UniRx.Triggers;

public class Enemy : Item, IEnemy
{
    [SerializeField] private int damage = 10;

    public int Damage => damage;

    public override void GetFromPoolReset()
    {
        transform.SetParent(null);
    }


    private void OnMouseDown()
    {
        EventContainer<Enemy>.Raise(Topics.EnemyDestroyed, this);
    }

}