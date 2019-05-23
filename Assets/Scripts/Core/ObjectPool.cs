using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
   private readonly List<Item> _takenObjects = new List<Item>();
    private Dictionary<Item, List<Item>> _pool;

    private PoolItemsInfo ItemsInfo => DependencyContainer.Resolve<PoolItemsInfo>();


    private void Awake()
    {
        if (DependencyContainer.Resolve<ObjectPool>() != null) return;
        DontDestroyOnLoad(this.gameObject);
        DependencyContainer.Add<ObjectPool>(this);
        _pool = new Dictionary<Item, List<Item>>();
        PrewarmPool();
    }

    private void PrewarmPool()
    {
        foreach (var itemInfo in ItemsInfo.itemsInfo)
        {
            if (itemInfo != null && itemInfo.prefab != null)
            {
                for (int i = 0; i < itemInfo.preWarmCount; i++)
                {
                    Item clone = Instantiate(itemInfo.prefab);
                    clone.name = itemInfo.prefab.name;
                    AddToPool(clone);
                }
            }
        }
    }

    public void AddToPool(Item item)
    {
        item.transform.SetParent(transform);
        item.gameObject.SetActive(false);
        var prefab = GetPrefab(item.name);
        
        if (!_pool.ContainsKey(prefab))
        {
            _pool.Add(prefab, new List<Item>() {item});
        }
        else
        {
            if (!_pool[prefab].Contains(item))
            {
                _pool[prefab].Add(item);
            }
        }
    }


    public Item GetFromPool(Item prefab)
    {
        if (_pool.ContainsKey(prefab))
        {
            List<Item> list = _pool[prefab];

            foreach (var itm in list)
            {
                if (itm != null && !itm.gameObject.activeSelf)
                {
                    itm.gameObject.SetActive(true);
                    itm.GetFromPoolReset();
                    _takenObjects.Add(itm);
                    return itm;
                }
            }

            Item item = Instantiate(prefab);
            item.name = prefab.name;
            AddToPool(item);
            _takenObjects.Add(item);
            item.gameObject.SetActive(true);
            return item;
        }

        Debug.LogError($"Pool doesn't contain game object named '{name}'  ");
        return null;
    }

    public bool ReturnToPool(Item item)
    {
        if (_takenObjects.Contains(item))
        {
            AddToPool(item);
            _takenObjects.Remove(item);
            return true;
        }

        return false;
    }

    private Item GetPrefab(string prefabName)
    {
        foreach (var item in ItemsInfo.itemsInfo)
        {
            if (item.name == prefabName)
            {
                return item.prefab;
            }
        }

        return null;
    }

    public void AddToPoolAll()
    {
        foreach (var item in _takenObjects)
        {
            if (item != null)
            {
                AddToPool(item);
                continue;
            }

            Destroy(item);
        }

        _takenObjects.Clear();
    }
}
