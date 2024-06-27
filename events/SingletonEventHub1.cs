using System;
using System.Collections.Generic;
using Events;

namespace SisyphusFramework
{
    public partial class SingletonEventHub<T1> : BaseSingleton<SingletonEventHub<T1>>
    {
        private readonly IDictionary<T1, EventHandler> Events = new Dictionary<T1, EventHandler>();

        public void Subscribe(T1 eventType, EventHandler listener)
        {
            if (!Events.ContainsKey(eventType))
            {
                Events[eventType] = null;
            }
            Events[eventType] += listener;
        }

        public  void Unsubscribe(T1 eventType, EventHandler listener)
        {
            if (Events.ContainsKey(eventType))
            {
                Events[eventType] -= listener;
            }
        }

        public  void Publish(T1 eventType)
        {
            if (Events.ContainsKey(eventType) && Events[eventType] != null)
            {
                Events[eventType]?.Invoke(null, EventArgs.Empty);
            }
        }
        public void Publish(T1 eventType, EventArgs param)
        {
            if (Events.ContainsKey(eventType) && Events[eventType] != null)
            {
                Events[eventType]?.Invoke(null, param);
            }
        }
    }
}