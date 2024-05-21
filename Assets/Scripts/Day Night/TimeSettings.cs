using UnityEngine;

namespace Pokemon
{
    [CreateAssetMenu(fileName = "TimeSettings", menuName = "TimeSettings")]
    public class TimeSettings: ScriptableObject
    {
        public float _timeMultiplier = 2000f;
        public float _startHour = 12;
        public float _sunriseHour = 6;
        public float _sunsetHour = 18;
        
    }
}