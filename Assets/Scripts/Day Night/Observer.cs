using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor.Events;

#if false
using UnityEditor;
#endif

namespace Pokemon
{


    [Serializable]
    public class Observer<T>
    {
        [SerializeField] T _value;
        [SerializeField] public UnityEvent<T> _onValueChanged;

        public T Value
        {
            get => _value;
            set => Set(value);
        }
        public static implicit operator T(Observer<T> observer) => observer._value;

        public Observer(T value, UnityAction<T> callback = null)
        {
            _value = value;
            _onValueChanged = new UnityEvent<T>();

            if (callback != null) _onValueChanged.AddListener(callback);
        }
        public void AddListener(UnityAction<T> callback)
        {
            if (callback == null) return;
            if (_onValueChanged == null) _onValueChanged = new UnityEvent<T>();

#if false
            UnityEventTools.AddPersistentListener(_onValueChanged, callback);
#else
            _onValueChanged.AddListener(callback);
#endif
        }
        public void RemoveListener(UnityAction<T> callback)
        {
            if (callback == null) return;
            if (_onValueChanged == null) _onValueChanged = new UnityEvent<T>();

#if false
            UnityEventTools.RemovePersistentListener(_onValueChanged, callback);
#else
            _onValueChanged.RemoveListener(callback);
#endif
        }
        public void RemoveAllListeners()
        {
            if (_onValueChanged == null) return;

#if false
            FieldInfo fieldInfo = typeof(T).GetField("m_PersistentCalls", BindingFlags.Instance | BindingFlags.NonPublic);
            
            object value = fieldInfo.GetValue(_onValueChanged);
            value.GetType().GetMethod("Clear").Invoke(_value, null);
#else
            _onValueChanged.RemoveAllListeners();   
#endif
        }
        public void Dispose()
        {
            RemoveAllListeners();
            _onValueChanged = null;
            _value = default;
        }

        public void Set(T value)
        {
            if (Equals(_value, value)) return;

            _value = value;
            Invoke();
        }
        public void Invoke()
        {
            _onValueChanged.Invoke(_value);
        }
    }
}