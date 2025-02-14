using System;
using System.Collections.Generic;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Managers
{
    public class EventManager : MonoBehaviour
    {
        private Dictionary<(EventChannelType, string), Action<object>> events = new();

        #region Subscriber
        public void Subscriber<T>(EventChannelType channel, Action<T> fn)
        {
            if (!events.ContainsKey((channel, typeof(T).ToString())))
            {
                events.Add((channel, typeof(T).ToString()), (object data) => fn((T)data));
                return;
            }

            events[(channel, typeof(T).ToString())] += (object data) => fn((T)data);
        }

        public void Subscriber(EventChannelType channel, Action fn)
        {
            if (!events.ContainsKey((channel, "")))
            {
                events.Add((channel, ""), (object data) => fn());
                return;
            }

            events[(channel, "")] += (object data) => fn();
        }
        #endregion

        #region Publisher
        public void Publisher<T>(EventChannelType channel, T data)
        {
            if (!events.ContainsKey((channel, typeof(T).ToString()))) return;
            if (events[(channel, typeof(T).ToString())] == null) return;

            events[(channel, typeof(T).ToString())]?.Invoke(data);
        }

        public void Publisher(EventChannelType channel)
        {
            if (!events.ContainsKey((channel, ""))) return;
            if (events[(channel, "")] == null) return;

            events[(channel, "")]?.Invoke(null);
        }
        #endregion
    }
}