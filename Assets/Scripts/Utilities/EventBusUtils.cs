using System;
using System.Collections.Generic;
using System.Reflection;
using Pokemon;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public static class EventBusUtils
    {
        public static IReadOnlyList<Type> EventTypes { get; set; }
        public static IReadOnlyList<Type> EventBusTypes { get; set; }

#if UNITY_EDITOR
    
    public static PlayModeStateChange PlayModeState { get; set; }

    [InitializeOnLoadMethod]
    public static void InitEditor()
    {
        EditorApplication.playModeStateChanged -= PlayModeStateChanged;
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
    }

    private static void PlayModeStateChanged(PlayModeStateChange state)
    {
        PlayModeState = state;
        if (state == PlayModeStateChange.ExitingPlayMode) ClearAllBuses();
    }

#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            EventTypes = PreDefinedAssembliesUtils.GetTypes(typeof(IEvent));
            EventBusTypes = InitAllBuses();
        }

        private static List<Type> InitAllBuses()
        {
            List<Type> eventBusTypes = new();

            var typedef = typeof(EventBus<>);
            foreach( var eventType in EventTypes)
            {
                var busType = typedef.MakeGenericType(eventType);
                eventBusTypes.Add(busType);
            }

            return eventBusTypes;
        }

        public static void ClearAllBuses()
        {
            foreach(var busType in EventBusTypes)
            {
                var clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
                clearMethod.Invoke(null, null);
            }
        }
    }
}