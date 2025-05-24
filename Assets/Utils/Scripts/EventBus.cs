using System;
using System.Collections.Generic;

namespace BroCollie
{
    public class EventBus : IPublisher, ISubscriber, IDisposable
    {
        private readonly Dictionary<Type, List<Delegate>> _eventHandlers = new();

        public void Publish<T>(T eventData)
        {
            if (_eventHandlers.TryGetValue(typeof(T), out List<Delegate> handlers))
            {
                foreach (Delegate handler in handlers.ToArray())
                    (handler as Action<T>)?.Invoke(eventData);
            }
        }

        public void Subscribe<T>(Action<T> handler)
        {
            Type eventType = typeof(T);
            if (!_eventHandlers.ContainsKey(eventType))
                _eventHandlers[eventType] = new List<Delegate>();
            _eventHandlers[eventType].Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            Type eventType = typeof(T);
            if (_eventHandlers.TryGetValue(eventType, out List<Delegate> handlers))
            {
                handlers.Remove(handler);
                if (handlers.Count == 0)
                    _eventHandlers.Remove(eventType);
            }
        }

        public void Dispose()
        {
            _eventHandlers.Clear();
        }
    }

    public interface IPublisher
    {
        void Publish<T>(T eventData);
    }

    public interface ISubscriber
    {
        void Subscribe<T>(Action<T> handler);
        void Unsubscribe<T>(Action<T> handler);
    }
}
