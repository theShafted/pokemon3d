using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

namespace Pokemon
{
    public abstract partial class EventChannel<T> : ScriptableObject
    {
        readonly HashSet<EventListener<T>> _observers = new();

        public void Invoke(T value)
        {
            foreach (var observer in _observers)
            {
                observer.Raise(value);
            }
        }

        public void Register(EventListener<T> observer) => _observers.Add(observer);
        public void Unregister(EventListener<T> observer) => _observers.Remove(observer);
    }

    public readonly struct Empty {}
    [CreateAssetMenu(menuName = "Events/EventChannel")]
    public class EventChannel: EventChannel<Empty> {}
}