using System;
using System.Collections.Generic;
using Events;

namespace SisyphusFramework
{
    public partial class SingletonEventHub<T, T1 > : BaseSingleton<SingletonEventHub<T, T1>> where T1 : EventArgs
    {
        private readonly IDictionary<T, EventHandler<T1>> Events = new Dictionary<T, EventHandler<T1>>();

        public void Subscribe(T eventType, EventHandler<T1> listener)
        {
            if (!Events.ContainsKey(eventType))
            {
                Events[eventType] = null;
            }
            Events[eventType] += listener;
        }

        public  void Unsubscribe(T eventType, EventHandler<T1> listener)
        {
            if (Events.ContainsKey(eventType))
            {
                Events[eventType] -= listener;
            }
        }

        public  void Publish(T eventType, T1 param)
        {
            if (Events.ContainsKey(eventType) && Events[eventType] != null)
            {
                Events[eventType]?.Invoke(null, param);
            }
        }
    }
}