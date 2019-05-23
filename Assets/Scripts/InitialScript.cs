using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialScript : MonoBehaviour
{
    [SerializeField] private PoolItemsInfo poolItemsInfo;
    
    private GameObject _poolGameObject;
    
    private GameObject ObjectPoolGameObject => _poolGameObject ?? (_poolGameObject = new GameObject("Pool")); 
    
    private void Awake()
    {
        InitInstances();
        InitGameSettings();
    }

    private void InitGameSettings()
    {
        
    }


    private void InitInstances()
    {
        DependencyContainer.Add<PoolItemsInfo>(poolItemsInfo);
        ObjectPoolGameObject.AddComponent<ObjectPool>();
    }
}
