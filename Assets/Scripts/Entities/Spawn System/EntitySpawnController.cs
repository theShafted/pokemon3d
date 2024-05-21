using UnityEngine;

namespace Pokemon
{
    public abstract class EntitySpawnController: MonoBehaviour
    {
        [SerializeField] protected SpawnStrategyType _spawnStrategyType = SpawnStrategyType.Linear;
        [SerializeField] protected Transform[] _spawnPoints;

        protected ISpawnPointStrategy _spawnPointStrategy;

        protected enum SpawnStrategyType
        {
            Linear,
            Random
        }

        protected virtual void Awake()
        {
            _spawnPointStrategy = _spawnStrategyType switch
            {
                SpawnStrategyType.Linear => new LinearSpawnPointStrategy(_spawnPoints),
                SpawnStrategyType.Random => new RandomSpawnPointStrategy(_spawnPoints),
                _ => _spawnPointStrategy
            };
        }

        public abstract void Spawn();
    }
}