using UnityEngine;
using Utilities;

namespace Pokemon
{
    public class ConeDetectionStrategy : IDetectionStrategy
    {
        readonly float _detectionAngle;
        readonly float _outerRadius;
        readonly float _innerRadius;

        public ConeDetectionStrategy(float detectionAngle, float outerRadius, float innerRadius)
        {
            _detectionAngle = detectionAngle;
            _outerRadius = outerRadius;
            _innerRadius = innerRadius;
        }
        public bool Execute(Transform player, Transform detector, CountDownTimer timer)
        {
            if (timer._isRunning) return false;

            var playerDir = player.position - detector.position;
            var playerAngle = Vector3.Angle(playerDir, detector.forward);

            if (!(playerAngle < _detectionAngle / 2f) || !(playerDir.magnitude < _outerRadius)) return false;
            
            if (!(playerDir.magnitude < _innerRadius)) return false;

            timer.Start();
            return true;

        }
    }
}
