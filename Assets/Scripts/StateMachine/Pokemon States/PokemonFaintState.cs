using UnityEngine;

namespace Pokemon
{
    public class PokemonFaintState : PokemonBaseState
    {
        public PokemonFaintState(Pokemon pokemon, Animator animator) : base(pokemon, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(_dieHash, _fadeDuration);
        }
    }
}