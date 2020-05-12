using System;
using System.Collections.Generic;

namespace FridgeLogic.Patterns.FSM
{
    // Based on https://www.youtube.com/watch?v=V75hgcsCGOM
    public class StateMachine
    {
        private static readonly IList<Transition> EMPTY_TRANSITIONS = new List<Transition>();

        private IState currentState = null;
        private IDictionary<Type, IList<Transition>> transitions = new Dictionary<Type, IList<Transition>>();
        private IList<Transition> anyTransitions = new List<Transition>();
        private IList<Transition> currentTransitions = new List<Transition>();

        public void OnUpdate(float dt)
        {
            var transition = CheckTransitions();
            if (transition != null)
            {
                SetState(transition.To);
            }

            currentState?.OnUpdate(dt);
        }

        public void SetState(IState state)
        {
            if (state == currentState)
            {
                return;
            }

            currentState?.OnExit();
            currentState = state;
            currentTransitions = GetTransitions(currentState, false);
            currentState.OnEnter();
        }

        public void AddTransition(IState from, IState to, Func<bool> when)
        {
            GetTransitions(from).Add(new Transition(to, when));
        }

        public void AddTransition(IState to, Func<bool> when)
        {
            anyTransitions.Add(new Transition(to, when));
        }

        private Transition CheckTransitions()
        {
            foreach (var transition in anyTransitions)
            {
                if (transition.IsConditionMet())
                {
                    return transition;
                }
            }

            foreach (var transition in currentTransitions)
            {
                if (transition.IsConditionMet())
                {
                    return transition;
                }
            }

            return null;
        }

        private IList<Transition> GetTransitions(IState state, bool createOnEmpty = true)
        {
            if (!this.transitions.TryGetValue(state.GetType(), out var transitions))
            {
                if (!createOnEmpty)
                {
                    return EMPTY_TRANSITIONS;
                }

                transitions = new List<Transition>();
                this.transitions[state.GetType()] = transitions;
            }

            return transitions;
        }

        private class Transition
        {
            public IState To { get; }
            public Func<bool> IsConditionMet { get; }

            public Transition(IState to, Func<bool> when)
            {
                To = to;
                IsConditionMet = when;
            }
        }
    }
}