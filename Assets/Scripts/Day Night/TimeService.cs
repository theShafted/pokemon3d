using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Utilities;

namespace Pokemon
{
    public class TimeService
    {
        readonly TimeSettings _settings;
        DateTime _currentTime;
        readonly TimeSpan _sunriseTime;
        readonly TimeSpan _sunsetime;

        public  Action Sunrise = delegate { };
        public Action Sunset = delegate { };
        public UnityAction HourChange = delegate { };

        Observer<bool> _isDayTime;
        Observer<int> _currentHour;

        public TimeService(TimeSettings settings)
        {
            _settings = settings;

            _currentTime = DateTime.Now.Date + TimeSpan.FromHours(_settings._startHour);
            _sunriseTime = TimeSpan.FromHours(_settings._sunriseHour);
            _sunsetime = TimeSpan.FromHours(_settings._sunsetHour);

            _isDayTime = new Observer<bool>(IsDayTime());
            _currentHour = new Observer<int>(_currentTime.Hour);

            _isDayTime.AddListener(day => (day ? Sunrise : Sunset)?.Invoke());
            _currentHour.AddListener(_ => HourChange?.Invoke());

        }

        public void UpdateTime(float dt)
        {
            _currentTime = _currentTime.AddSeconds(dt * _settings._timeMultiplier);
            
            _isDayTime.Value = IsDayTime();
            _currentHour.Value = _currentTime.Hour;
        }
        public float SunAngle()
        {
            bool isDay = IsDayTime();
            float initialDegrees = isDay ? 0 : 180;
            
            TimeSpan startTime = isDay ? _sunriseTime : _sunsetime;
            TimeSpan endTime = isDay ? _sunsetime : _sunriseTime;

            TimeSpan totalTime = GetDifference(startTime, endTime);
            TimeSpan elapsedTime = GetDifference(startTime, _currentTime.TimeOfDay);

            Double percentage = elapsedTime.TotalMinutes / totalTime.TotalMinutes;

            return Mathf.Lerp(initialDegrees, initialDegrees + 180, (float)percentage);
        }
        public DateTime CurrentTime => _currentTime;
        
        bool IsDayTime() => _currentTime.TimeOfDay > _sunriseTime && _currentTime.TimeOfDay < _sunsetime;
        TimeSpan GetDifference(TimeSpan from, TimeSpan to)
        {
            TimeSpan difference = to - from;
            return difference.TotalHours < 0 ? difference + TimeSpan.FromHours(24) : difference;
        }
    }
}