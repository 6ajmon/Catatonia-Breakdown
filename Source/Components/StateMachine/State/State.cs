using Godot;
using System;

public abstract partial class State : Node
{
    public StateMachine _stateMachine { get; set; }
    public virtual void Enter(State previousState = null) { }
    public virtual void Exit() { }
    public virtual void StateProcess(double delta) { }
    public virtual void StatePhysicsProcess(double delta) { }
}
