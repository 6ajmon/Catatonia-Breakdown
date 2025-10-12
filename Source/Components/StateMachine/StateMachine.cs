using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : Node
{
    [Export] public State initialState;
    public State CurrentState { get; private set; }
    private Godot.Collections.Dictionary<string, State> _states = new();
    public override void _Ready()
    {
        foreach (Node child in GetChildren())
        {
            if (child is State state)
            {
                _states[state.Name] = state;
                state._stateMachine = this;
                state.Exit();
            }
        }

        if (initialState != null)
        {
            ChangeState(initialState);
        }
        else if (_states.Count > 0)
        {
            var enumerator = _states.Values.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ChangeState(enumerator.Current);
            }
        }
    }

    public void ChangeState(State newState)
    {
        if (newState == null)
        {
            GD.PrintErr("Cannot change to null state");
            return;
        }

        if (!_states.ContainsKey(newState.Name))
        {
            GD.PrintErr($"State '{newState.Name}' is not registered in the state machine");
            return;
        }

        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        var previousState = CurrentState;
        CurrentState = _states[newState.Name];
        CurrentState.Enter(previousState);
    }
    public override void _Process(double delta)
    {
        CurrentState?.StateProcess(delta);
    }
    public override void _PhysicsProcess(double delta)
    {
        CurrentState?.StatePhysicsProcess(delta);
    }
}
