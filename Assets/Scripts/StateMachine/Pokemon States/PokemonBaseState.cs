using UnityEngine;

namespace Pokemon
{
    public abstract class PokemonBaseState : IState
    {
        protected readonly Pokemon _pokemon;
        protected readonly Animator _animator;

        protected static readonly int _idleHash = Animator.StringToHash("Idle");
        protected static readonly int _moveHash = Animator.StringToHash("Move");
        protected static readonly int _attackHash = Animator.StringToHash("Attack");
        protected static readonly int _damageHash = Animator.StringToHash("Damage");
        protected static readonly int _dieHash = Animator.StringToHash("Die");

        protected const float _fadeDuration = 0.1f;

        protected PokemonBaseState(Pokemon pokemon, Animator animator)
        {
            _pokemon = pokemon;
            _animator = animator;
        }
        public virtual void FixedUpdate()
        {
            
        }

        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public virtual void Update()
        {
            
        }
    }
}