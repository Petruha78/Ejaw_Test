using System;
using Core;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Vector3 _nextPoint;
    private int _currentTime;
    private DateTimeOffset _lastTimeUpdated;
    private Player Player => DependencyContainer.Resolve<Player>();
    private WaypointManager WaypointManager => DependencyContainer.Resolve<WaypointManager>();

    private void Start()
    {
        Player.FixedUpdateAsObservable().Subscribe(_ => { Move(); });

        Player.ObserveEveryValueChanged(_ => Player.HitPoints)
            .Where(_ => Player.HitPoints <= 0)
            .Subscribe(_ => { ReloadGame(); });

        this.ObserveEveryValueChanged(_ => _currentTime)
            .Where(_ => _currentTime >= Constants.MaxTime).Subscribe(_ => ReloadGame());

        this.UpdateAsObservable()
            .Where(_ => (Input.GetKeyDown(KeyCode.Space) && Player.HitPoints > 0 && _currentTime < Constants.MaxTime) || 
                        Player.HitPoints <= 0 || _currentTime >= Constants.MaxTime)
            .Subscribe(_ => Pause());

        this.FixedUpdateAsObservable()
            .Where(_ => _currentTime < Constants.MaxTime)
            .Timestamp()
            .Where(x => x.Timestamp > _lastTimeUpdated.AddSeconds(1))
            .Subscribe(x =>
            {
                _lastTimeUpdated = x.Timestamp;
                EventContainer<int>.Raise(Topics.TimerUpdate, _currentTime += 1);
            });
        
        EventContainer.Subscribe(Topics.GameReload, ResetTimeCounter);
        EventContainer.Subscribe(Topics.GamePaused, Pause);
        EventContainer.Subscribe(Topics.GameExit, ExitGame);
    }

    private void Move()
    {
        if (!Player.IsMoving)
        {
            _nextPoint = WaypointManager.GetNextWayPoint();
            _nextPoint.y = (Player.transform.position.y);
        }

        Player.Move(_nextPoint);
    }

    private void ReloadGame()
    {
        EventContainer.Raise(Topics.GameReload);    
    }

    private void Pause()
    {
        Time.timeScale = (int) Time.timeScale == 1 ? 0 : 1;
        EventContainer<bool>.Raise(Topics.ResetButtonHide, (int) Time.timeScale == 0);
        GetComponent<BoxCollider>().enabled = (int) Time.timeScale == 0;
    }

    private void ResetTimeCounter()
    {
        _currentTime = 0;
    }
    
    private void ExitGame()
    {
        Application.Quit();
    }
}