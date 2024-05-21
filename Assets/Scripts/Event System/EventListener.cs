using UnityEngine;
using UnityEngine.Events;

namespace Pokemon
{
    public abstract class EventListener<T>: MonoBehaviour
   { 
        [SerializeField] EventChannel<T> _eventChannel;
        [SerializeField] UnityEvent<T> _event;

        protected void Awake()
        {
            _eventChannel.Register(this);
        }
        protected void OnDestroy()
        {
            _eventChannel.Unregister(this);
        }

        public void Raise(T value)
        {
            _event?.Invoke(value);
        }
   } 

   public class EventListener: EventListener<Empty> {}
}
