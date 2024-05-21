using UnityEngine;
using UnityEngine.AI;

namespace Pokemon
{

    public class PokemonChaseState : PokemonBaseState
    {
        private NavMeshAgent _agent;
        private Transform _target;

        public PokemonChaseState(Pokemon pokemon, Animator animator, NavMeshAgent agent, Transform target) : base(pokemon, animator)
        {
            _agent = agent;
            _target = target;
        }

        public override void OnEnter()
        {
            _animator.CrossFade(_moveHash, _fadeDuration);
        }
        public override void Update()
        {
            _agent.SetDestination(_target.position);
        }
    }
}