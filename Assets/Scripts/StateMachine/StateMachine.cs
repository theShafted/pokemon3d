using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pokemon
{
    public class StateMachine 
    {
        StateNode _current;
        Dictionary<Type, StateNode> _nodes = new();
        HashSet<ITransition> _anyTransitions = new();

        public IState _defaultState;

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
            {
                ChangeState(transition.To);
            }

            _current.State?.Update();
        }
        public void FixedUpdate()
        {
            _current.State?.FixedUpdate();
        }

        public void SetState(IState state)
        {
            _current = _nodes[state.GetType()];
            _current.State?.OnEnter();
        }
        public void SetDefaultState(IState state=null)
        {
            if (state == null && _defaultState == null) return;
            if (_defaultState == null) _defaultState = state;;
            
            SetState(_defaultState);
        }
        private void ChangeState(IState state)
        {
            if (state == _current.State) return;

            var previousState = _current.State;
            var nextState = _nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();

            _current = _nodes[state.GetType()];
        }
        private ITransition GetTransition()
        {
            foreach (var transition in _anyTransitions)
                if (transition.Condition.Evaluate()) return transition;

            foreach (var transition in _current.Transitions)
                if (transition.Condition.Evaluate()) return transition;

            return null;
        }
        public void AddTransition(IState from, IPredicate condition, IState to)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }
        public void AddAnyTransition(IState to, IPredicate condition)
        {
            _anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }
        StateNode GetOrAddNode(IState state)
        {
            StateNode node = _nodes.GetValueOrDefault(state.GetType());

            if (node == null)
            {
                node = new StateNode(state);
                _nodes.Add(state.GetType(), node);
            }

            return node;
        }

        class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}