using System;
using UnityEngine;

[CreateAssetMenu]
public class PoolItemsInfo : ScriptableObject
{
    public ItemInfo[] itemsInfo;

    private void OnValidate()
    {
        foreach (var itemInfo in itemsInfo)
        {
            if (itemInfo != null && itemInfo.prefab != null)
            {
                itemInfo.name = itemInfo.prefab.name;
            }            
        }
    }
}

[Serializable]
public class ItemInfo
{
    public string name;
    public Item prefab;
    public int preWarmCount;
}