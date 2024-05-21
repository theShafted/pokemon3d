using UnityEngine;

namespace Pokemon
{
    public class PokemonDamageState : PokemonBaseState
    {
        public PokemonDamageState(Pokemon pokemon, Animator animator) : base(pokemon, animator)
        {
        }
        public override void OnEnter()
        {
            _animator.CrossFade(_damageHash, _fadeDuration);
        }
        public override void Update()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) _pokemon._damaged = false;
        }
    }
}