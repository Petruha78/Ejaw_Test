using System;
using System.Collections.Generic;
using UnityEngine;

public class DependencyContainer
{
    private static Dictionary<Type, object> _container = new Dictionary<Type, object>();

    public static void Add<T>(object obj, bool replace = true)
    {
        Add(typeof(T), obj, replace);
    }

    private static void Add(Type type, object obj, bool replace = true)
    {
        if (_container.ContainsKey(type))
        {
            if (replace)
            {
                _container[type] = obj;
            }

            return;
        }

        _container.Add(type, obj);
    }

    public static T Resolve<T>()
    {
        if (_container != null && _container.ContainsKey(typeof(T)))
        {
            try
            {
                return (T) _container[typeof(T)];
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}{Environment.NewLine}{e.StackTrace}");
            }
        }

        return default(T);
    }
}