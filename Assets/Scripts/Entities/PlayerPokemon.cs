using UnityEngine;

namespace Pokemon
{
    public class PlayerPokemon: Pokemon
    {
        [SerializeField] private Rigidbody _rigidbody;
        public bool _isAlive = true;

        protected override void OnEnable() => _inBattle = true;
        protected override void OnDisable() => _inBattle = false;
        protected void Awake()
        {
            _rigidbody.freezeRotation = true;
        }
        protected override void Start()
        {     
            base.Start();       
            
            var idleState = new PokemonIdleState(this, _animator);
            Any(idleState, new FuncPredicate(() => _inBattle && !_damaged && _moveUsed == null));

            _stateMachine.SetState(idleState);
        }
        
    }
}