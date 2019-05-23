using Core;
using Interfaces;
using UnityEngine;

public class Player : Movable, IDestructable
{
    [SerializeField] private int hitPoints = Constants.MaxPlayerHealth;
    public int Damage { get; set; }

    public int HitPoints => hitPoints - Damage;

    private void Awake()
    {
        EventContainer.Subscribe(Topics.GameReload, Die);
        DependencyContainer.Add<Player>(this);
    }

    public void Die()
    {
        Damage = 0;
        transform.position = new Vector3(0, transform.position.y, 0);
    }
}