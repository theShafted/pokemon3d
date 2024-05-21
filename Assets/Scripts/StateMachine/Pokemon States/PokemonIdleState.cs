using System.Collections;
using System.Collections.Generic;
using Pokemon;
using UnityEngine;
using UnityEngine.AI;

namespace Pokemon
{
    public class PokemonIdleState : PokemonBaseState
    {
        private Transform _target;
        public PokemonIdleState(Pokemon pokemon, Animator animator, Transform target=null) : base(pokemon, animator)
        {
            _target = target;
        }

        public override void OnEnter()
        {
            _animator.CrossFade(_idleHash, _fadeDuration);
        }
        public override void Update()
        {
            if (_target != null) _pokemon.transform.LookAt(_target);
        }
    }
}