using UnityEngine;
using UnityEngine.AI;

namespace Pokemon
{
    public class PokemonWanderState : PokemonBaseState
    {
        private readonly NavMeshAgent _agent;
        private readonly float _radius;
        private readonly Vector3 _initialPosition;

        public PokemonWanderState(Pokemon pokemon, Animator animator, NavMeshAgent agent, float radius) : base(pokemon, animator)
        {
            _agent = agent;
            _radius = radius;
            _initialPosition = pokemon.transform.position;
        }

        public override void OnEnter()
        {
            _animator.CrossFade(_moveHash, _fadeDuration);
        }
        public override void Update()
        {
            if (ReachedDestination())
            {
                var randomDir = Random.insideUnitSphere * _radius;
                randomDir += _initialPosition;

                NavMesh.SamplePosition(randomDir, out NavMeshHit hit, _radius, 1);
                var targetPosition = hit.position;

                _agent.SetDestination(targetPosition);
            }
        }

        private bool ReachedDestination()
        {
            return !_agent.pathPending
                && _agent.remainingDistance <= _agent.stoppingDistance
                && (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f);
        }
    }
}