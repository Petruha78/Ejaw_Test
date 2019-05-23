using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class EventContainer
    {
        private static readonly Dictionary<string, List<Action>> Topics =
            new Dictionary<string, List<Action>>();

        private static readonly Dictionary<string, List<Action>> TopicsToAdd =
            new Dictionary<string, List<Action>>();

        private static readonly Dictionary<string, List<Action>> TopicsToAddCache =
            new Dictionary<string, List<Action>>();

        private static readonly Dictionary<string, List<Action>> TopicsToRemove =
            new Dictionary<string, List<Action>>();

        private static readonly Dictionary<string, bool> Raising =
            new Dictionary<string, bool>();

        private static readonly Dictionary<string, bool> Adding =
            new Dictionary<string, bool>();

        private static readonly Dictionary<string, bool> Removing =
            new Dictionary<string, bool>();

        public static void Subscribe(string topic, Action callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!Raising.ContainsKey(topic))
            {
                Raising.Add(topic, false);
            }

            var oldRaising = Raising[topic];

            if (!oldRaising)
            {
                if (Topics.ContainsKey(topic))
                {
                    Topics[topic].Add(callback);
                }
                else
                {
                    Topics.Add(topic, new List<Action>() {callback});
                }

                return;
            }

            if (!Adding.ContainsKey(topic))
            {
                Adding.Add(topic, true);
            }
            else
            {
                Adding[topic] = true;
            }

            if (TopicsToAdd.ContainsKey(topic))
            {
                TopicsToAdd[topic].Add(callback);
            }
            else
            {
                TopicsToAdd.Add(topic, new List<Action> {callback});
            }

            if (TopicsToRemove.ContainsKey(topic) && TopicsToRemove[topic].Contains(callback))
            {
                TopicsToRemove[topic].Remove(callback);
            }

            Adding[topic] = false;
        }

        public static void UnSubscribe(string topic, Action callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!Raising.ContainsKey(topic))
            {
                Raising.Add(topic, false);
            }

            var oldRaising = Raising[topic];

            if (!oldRaising)
            {
                if (Topics.ContainsKey(topic))
                {
                    Topics[topic].Remove(callback);
                }

                return;
            }

            if (!Removing.ContainsKey(topic))
            {
                Removing.Add(topic, true);
            }
            else
            {
                Removing[topic] = true;
            }

            if (TopicsToRemove.ContainsKey(topic))
            {
                TopicsToRemove[topic].Add(callback);
            }
            else
            {
                TopicsToRemove.Add(topic, new List<Action> {callback});
            }

            if (TopicsToAdd.ContainsKey(topic) && TopicsToAdd[topic].Contains(callback))
            {
                TopicsToAdd[topic].Remove(callback);
            }

            Removing[topic] = false;
        }

        public static void Raise(string topic)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            bool oldRaising = false;

            if (!Raising.ContainsKey(topic))
            {
                Raising.Add(topic, true);
            }
            else
            {
                oldRaising = Raising[topic];
                Raising[topic] = true;
            }

            TopicsToAddCache.Clear();
            foreach (var item in TopicsToAdd)
            {
                TopicsToAddCache.Add(item.Key, item.Value);
            }

            foreach (var item in TopicsToAddCache)
            {
                if (!Topics.ContainsKey(item.Key))
                {
                    Topics.Add(item.Key, item.Value);
                }
                else
                {
                    if (oldRaising)
                    {
                        continue;
                    }

                    Topics[item.Key].AddRange(item.Value);
                }

                TopicsToAdd.Remove(item.Key);
            }

            foreach (var item in TopicsToRemove)
            {
                if (Topics.ContainsKey(item.Key))
                {
                    foreach (var action in item.Value)
                    {
                        Topics[item.Key].Remove(action);
                    }
                }
            }

            TopicsToRemove.Clear();

            if (!Topics.ContainsKey(topic))
            {
                return;
            }

            try
            {
                foreach (var callback in Topics[topic])
                {
                    callback.Invoke();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            if (TopicsToAdd.ContainsKey(topic))
            {
                if (!((Adding.ContainsKey(topic) && Adding[topic]) ||
                      (Removing.ContainsKey(topic) && Removing[topic])))
                    try
                    {
                        foreach (var callback in TopicsToAdd[topic])
                        {
                            callback.Invoke();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
            }

            Raising[topic] = false;
        }
    }

    public class EventContainer<TArg>
    {
        private static readonly Dictionary<string, List<Action<TArg>>> Topics =
            new Dictionary<string, List<Action<TArg>>>();

        private static readonly Dictionary<string, List<Action<TArg>>> TopicsToAdd =
            new Dictionary<string, List<Action<TArg>>>();

        private static readonly Dictionary<string, List<Action<TArg>>> TopicsToAddCache =
            new Dictionary<string, List<Action<TArg>>>();

        private static readonly Dictionary<string, List<Action<TArg>>> TopicsToRemove =
            new Dictionary<string, List<Action<TArg>>>();

        private static readonly Dictionary<string, bool> Raising = 
            new Dictionary<string, bool>();
        
        private static readonly Dictionary<string, bool> Adding = 
            new Dictionary<string, bool>();
        
        private static readonly Dictionary<string, bool> Removing = 
            new Dictionary<string, bool>();

        public static void Subscribe(string topic, Action<TArg> callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!Raising.ContainsKey(topic))
            {
                Raising.Add(topic, false);
            }

            var oldRaising = Raising[topic];

            if (!oldRaising)
            {
                if (Topics.ContainsKey(topic))
                {
                    Topics[topic].Add(callback);
                }
                else
                {
                    Topics.Add(topic, new List<Action<TArg>>() {callback});
                }

                return;
            }

            if (!Adding.ContainsKey(topic))
            {
                Adding.Add(topic, true);
            }
            else
            {
                Adding[topic] = true;
            }

            if (TopicsToAdd.ContainsKey(topic))
            {
                TopicsToAdd[topic].Add(callback);
            }
            else
            {
                TopicsToAdd.Add(topic, new List<Action<TArg>> {callback});
            }

            if (TopicsToRemove.ContainsKey(topic) && TopicsToRemove[topic].Contains(callback))
            {
                TopicsToRemove[topic].Remove(callback);
            }

            Adding[topic] = false;
        }

        public static void UnSubscribe(string topic, Action<TArg> callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!Raising.ContainsKey(topic))
            {
                Raising.Add(topic, false);
            }

            var oldRaising = Raising[topic];

            if (!oldRaising)
            {
                if (Topics.ContainsKey(topic))
                {
                    Topics[topic].Remove(callback);
                }

                return;
            }

            if (!Removing.ContainsKey(topic))
            {
                Removing.Add(topic, true);
            }
            else
            {
                Removing[topic] = true;
            }

            if (TopicsToRemove.ContainsKey(topic))
            {
                TopicsToRemove[topic].Add(callback);
            }
            else
            {
                TopicsToRemove.Add(topic, new List<Action<TArg>> {callback});
            }

            if (TopicsToAdd.ContainsKey(topic) && TopicsToAdd[topic].Contains(callback))
            {
                TopicsToAdd[topic].Remove(callback);
            }

            Removing[topic] = false;
        }

        public static void Raise(string topic, TArg arg)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            bool oldRaising = false;

            if (!Raising.ContainsKey(topic))
            {
                Raising.Add(topic, true);
            }
            else
            {
                oldRaising = Raising[topic];
                Raising[topic] = true;
            }

            TopicsToAddCache.Clear();
            foreach (var item in TopicsToAdd)
            {
                TopicsToAddCache.Add(item.Key, item.Value);
            }

            foreach (var item in TopicsToAddCache)
            {
                if (!Topics.ContainsKey(item.Key))
                {
                    Topics.Add(item.Key, item.Value);
                }
                else
                {
                    if (oldRaising)
                    {
                        continue;
                    }

                    Topics[item.Key].AddRange(item.Value);
                }

                TopicsToAdd.Remove(item.Key);
            }

            foreach (var item in TopicsToRemove)
            {
                if (Topics.ContainsKey(item.Key))
                {
                    foreach (var action in item.Value)
                    {
                        Topics[item.Key].Remove(action);
                    }
                }
            }

            TopicsToRemove.Clear();

            if (!Topics.ContainsKey(topic))
            {
                return;
            }

            try
            {
                foreach (var callback in Topics[topic])
                {
                    callback.Invoke(arg);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            if (TopicsToAdd.ContainsKey(topic))
            {
                if (!((Adding.ContainsKey(topic) && Adding[topic]) ||
                      (Removing.ContainsKey(topic) && Removing[topic])))
                    try
                    {
                        foreach (var callback in TopicsToAdd[topic])
                        {
                            callback.Invoke(arg);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                else
                {
                }
            }

            Raising[topic] = false;
        }
    }
}