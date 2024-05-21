using System;
using System.Collections.Generic;

namespace Pokemon
{
    public static class EventBus<T> where T : IEvent 
    { 
        static readonly HashSet<IEventBinding<T>> _bindings = new();

        public static void Register(EventBinding<T> binding) => _bindings.Add(binding);
        public static void UnRegister(EventBinding<T> binding) => _bindings.Remove(binding);

        public static void Raise(T @event)
        {
            foreach (var binding in _bindings)
            {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }

        static void Clear()
        {
            _bindings.Clear();
        }
    }
}