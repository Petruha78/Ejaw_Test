using System;
using System.Collections.Generic;
using Core;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Range(1, 10)] private int maxCount;
    [SerializeField] private Item[] prefabs;
    private DateTimeOffset _lastInstantiated;
    private int _currentScore;

    private ObjectPool ObjectPool => DependencyContainer.Resolve<ObjectPool>();
    private Player Target => DependencyContainer.Resolve<Player>();
    private WaypointManager WaypointManager => DependencyContainer.Resolve<WaypointManager>();

    private List<Enemy> _enemies;
    private List<Enemy> _enemiesToDestroy;

    private void Start()
    {
        _enemiesToDestroy = new List<Enemy>();
        _enemies = new List<Enemy>();
        EventContainer.Subscribe(Topics.GameReload, Reset);
        EventContainer<Enemy>.Subscribe(Topics.EnemyDestroyed, DestroyEnemy);

        this.FixedUpdateAsObservable()
            .Where(w => _enemies.Count < maxCount)
            .Timestamp()
            .Where(x => x.Timestamp > _lastInstantiated.AddSeconds(1))
            .Subscribe(x =>
            {
                GetEnemy();
                _lastInstantiated = x.Timestamp;
            });

        this.FixedUpdateAsObservable().Subscribe(_ =>
        {
            MoveEnemies();
            DestroyEnemies();
        });
    }

    private void GetEnemy()
    {
        var index = Random.Range(0, prefabs.Length);

        var newEnemy = ObjectPool.GetFromPool(prefabs[index]) as Enemy;

        if (newEnemy != null)
        {
            _enemies.Add(newEnemy);
            var enemyStartPos = WaypointManager.GetEnemyStartPos();
            enemyStartPos.y = (newEnemy.transform.localScale.y);
            newEnemy.transform.position = enemyStartPos;
        }
    }

    private void MoveEnemies()
    {
        if (_enemies.Count > 0)
        {
            foreach (var enemy in _enemies)
            {
                enemy.Move(Target.transform.position);

                if (!enemy.IsMoving)
                {
                    Target.Damage += enemy.Damage;
                    EventContainer<int>.Raise(Topics.HealthBarValuesChange, Target.HitPoints);
                    _enemiesToDestroy.Add(enemy);
                }
            }
        }
    }

    private void DestroyEnemies()
    {
        foreach (var enemy in _enemiesToDestroy)
        {
            ObjectPool.ReturnToPool(enemy);
            if (_enemies.Contains(enemy))
            {
                _enemies.Remove(enemy);
            }
        }

        _enemiesToDestroy.Clear();
    }

    private void DestroyEnemy(Enemy enemy)
    {
        _enemiesToDestroy.Add(enemy);
        EventContainer<int>.Raise(Topics.ScoreUpdate, _currentScore += Constants.ScorePointsPerEnemy);
    }

    private void Reset()
    {
        _currentScore = 0;
        _enemies.Clear();
        ObjectPool.AddToPoolAll();
    }
}