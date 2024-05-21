using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Pokemon
{
    public partial class PlayerDetectionController : MonoBehaviour
    {
        [SerializeField] private float _detectionAngle = 0f;
        [SerializeField] private float _outerRadius = 10f;
        [SerializeField] private float _innerRadius = 5f;
        [SerializeField] private float _cooldown = 1f;

        public Transform _player { get; private set; }

        private CountDownTimer _timer;
        private IDetectionStrategy _detectionStrategy;

        void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        void Start()
        {
            _timer = new CountDownTimer(_cooldown);
            _detectionStrategy = new ConeDetectionStrategy(_detectionAngle, _outerRadius, _innerRadius);
        }

        // Update is called once per frame
        void Update() => _timer.Tick(Time.deltaTime);

        public bool PlayerDetectable()
        {
            return _timer._isRunning || _detectionStrategy.Execute(_player, transform, _timer);
        }
        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => _detectionStrategy = detectionStrategy;
    }
}