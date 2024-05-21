using UnityEngine;
using Utilities;

namespace Pokemon
{
    public class CollectibleSpawnController : EntitySpawnController
    {
        [SerializeField] CollectibleData[] _collectibleData;
        [SerializeField] float _interval = 1f;

        EntitySpawner<Collectible> _spawner;
        CountDownTimer _timer;
        int _counter;

        protected override void Awake()
        {
            base.Awake();

            _spawner = new EntitySpawner<Collectible>(new EntityFactory<Collectible>(_collectibleData), _spawnPointStrategy);
            
            _timer = new CountDownTimer(_interval);
            _timer.OnTimerStop += () =>
            {
                if (_counter++ >= _spawnPoints.Length)
                {
                    _timer.Stop();
                    return;
                }
                Spawn();
                _timer.Start();
            };

        }
        void Start() => _timer.Start();
        void Update() => _timer.Tick(Time.deltaTime);
        public override void Spawn() => _spawner.Spawn();
    }
}