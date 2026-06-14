//#define LOGGING_ON

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DTools.FSM
{
    public class StateMachine<TState> : IDisposable where TState : State<TState>
    {
        private Dictionary<Type, TState> _states;
        private bool _showErrors;
        private bool _debug;

        public TState CurrentState { get; private set; }

        public StateMachine(bool debug = false)
        {
	        _debug = debug;
        }

        public void SetStates(IReadOnlyCollection<TState> states, bool showErrors = true)
	    {
		    ClearStates();
		    _showErrors = showErrors;
		    _states = new Dictionary<Type, TState>(states.Count);
		    foreach (var state in states)
		    {
			    state.SetStateMachine(this);
				_states.Add(state.GetType(), state);
		    }
	    }
        
        public T SetState<T>(object parameter = null, bool allowTransitionToSelf = false) where T : TState
        {
	        var stateType = typeof(T);
			if (!_states.TryGetValue(stateType, out var newState))
	        {
		        throw new Exception($"Unknown state with type {stateType.Name}");
	        }

	        if (CurrentState != null)
	        {
		        if (!allowTransitionToSelf && CurrentState.Equals(newState))
		        {
			        if (_showErrors)
						Debug.LogError($"Trying to change state from {CurrentState.GetType().Name} to the same({newState.GetType().Name})");
			        return default;
		        }

		        CurrentState.Exit(newState);
	        }
	        
	        Log($"From {CurrentState?.GetType().Name} to {stateType.Name} at {Time.frameCount}");
	        var oldState = CurrentState;
	        CurrentState = newState;
	        newState.Enter(oldState, parameter);

	        return newState as T;
        }

        public void Dispose()
        {
			ClearStates();
        }

        private void ClearStates()
        {
	        if (_states == null) return;
	        
	        foreach (var state in _states.Values)
	        {
		        state.Dispose();
	        }

	        _states.Clear();
        }

        [Conditional("LOGGING_ON")]
        private void Log(string message)
        {
	        if (_debug)
				Debug.Log(message);
        }
    }
}