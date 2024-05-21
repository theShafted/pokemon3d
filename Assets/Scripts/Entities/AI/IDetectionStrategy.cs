using UnityEngine;
using Utilities;

namespace Pokemon
{
    public interface IDetectionStrategy
    {
        bool Execute(Transform player, Transform detector, CountDownTimer timer);
    }
}