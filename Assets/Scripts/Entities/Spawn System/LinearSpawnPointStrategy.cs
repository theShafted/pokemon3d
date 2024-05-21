using UnityEngine;

namespace Pokemon
{
    public class LinearSpawnPointStrategy : ISpawnPointStrategy
    {
        int _index = 0;
        Transform[] _spawnPoints;

        public LinearSpawnPointStrategy(Transform[] spawnPoints)
        {
            _spawnPoints = spawnPoints;
        }
        public Transform NextSpawnPoint()
        {
            Transform point = _spawnPoints[_index];
            _index = (_index + 1) % _spawnPoints.Length;

            return point;
        }
    }
}