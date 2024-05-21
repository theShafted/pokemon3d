using UnityEngine;

namespace Pokemon
{

    public class PokemonAttackState : PokemonBaseState
    {
        public PokemonAttackState(Pokemon pokemon, Animator animator) : base(pokemon, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(_attackHash, _fadeDuration);
        }
        public override void Update()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) _pokemon._moveUsed = null;
        }
    }
}