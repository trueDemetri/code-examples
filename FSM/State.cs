using System;

namespace DTools.FSM
{
	public abstract class State<TState>: IDisposable where TState : State<TState>
	{
		protected StateMachine<TState> StateMachine { get; private set; }
		protected bool Active { get; private set; }

		public void Enter(TState prevState, object arg)
		{
			Active = true;
			OnEnter(prevState, arg);
		}

		public void Exit(TState nextState)
		{
			Active = false;
			OnExit(nextState);
		}
		
		protected virtual void OnEnter(TState prevState, object arg)
		{}

		protected virtual void OnExit(TState nextState)
		{}
		
		public virtual void Dispose()
		{}

		public void SetStateMachine(StateMachine<TState> stateMachine)
		{
			StateMachine = stateMachine;
		}
	}
}