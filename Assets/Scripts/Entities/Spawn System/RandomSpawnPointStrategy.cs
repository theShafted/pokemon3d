using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pokemon
{
    public class RandomSpawnPointStrategy : ISpawnPointStrategy
    {
        List<Transform> _unusedSpawnPoints;
        Transform[] _spawnPoints;

        public RandomSpawnPointStrategy(Transform[] spawnPoints)
        {
            _spawnPoints = spawnPoints;
            _unusedSpawnPoints = new List<Transform>(_spawnPoints);
        }
        public Transform NextSpawnPoint()
        {
            if (!_unusedSpawnPoints.Any())
            {
                _unusedSpawnPoints = new List<Transform>(_spawnPoints);
            }

            int _randomIndex = Random.Range(0, _unusedSpawnPoints.Count);
            Transform point = _unusedSpawnPoints[_randomIndex];
            _unusedSpawnPoints.RemoveAt(_randomIndex);

            return point;
        }
    }
}