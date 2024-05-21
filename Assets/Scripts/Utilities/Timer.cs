using System;

namespace Utilities
{
    public abstract class Timer
    {
        protected float _initialTime;
        public float _time { get; protected set; }
        public bool _isRunning { get; protected set; }

        public float _progress => _time / _initialTime;

        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };

        protected Timer(float value)
        {
            _initialTime = value;
            _isRunning = false;
        }

        public void Start()
        {
            _time = _initialTime;
            if (!_isRunning)
            {
                _isRunning = true;
                OnTimerStart.Invoke();
            }
        }
        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                OnTimerStop.Invoke();
            }
        }
        public void Pause() => _isRunning = false;
        public void Resume() => _isRunning = true;

        public abstract void Tick(float deltaTime);

    }

    public class CountDownTimer: Timer
    {
        public CountDownTimer(float value) : base(value) {}

        public override void Tick(float deltaTime)
        {
            if (_isRunning && _time > 0f)
            {
                _time -= deltaTime;
            }

            if (_isRunning && _time <= 0f)
            {
                Stop();
            }
        }

        public bool IsFinished() => _time <= 0;
        public void Reset() => _time = _initialTime;
        public void Reset(float time)
        {
            _initialTime = time;
            Reset();
        }
    }

    public class StopWatchTimer: Timer
    {
        public StopWatchTimer(float value) : base(0f) {}

        public override void Tick(float deltaTime)
        {
            if (_isRunning && _time <= 0f)
            {
                _time += deltaTime;
            }
        }

        public float GetTime() => _time;
        public void Reset() => _time = 0f;
    }

}