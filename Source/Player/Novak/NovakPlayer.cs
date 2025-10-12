using Godot;
using System;

public partial class NovakPlayer : CharacterBody3D
{
    [Export] public float Speed = 2.0f;
    [Export] public float RunSpeed = 3.0f;
    private bool _isRunning = false;
    [Export] public AnimationPlayer animationPlayer;
    [Export] public AnimationTree animationTree;
    [Export] public StateMachine stateMachine;
    [Export] public Camera3D firstPersonCamera;
    private Node3D skeleton;
    private float _walkBlendAmount = 0.0f;
    private float _runBlendAmount = 0.0f;
    [Export] public float BlendSpeed = 3.0f;
    public override void _Ready()
    {
        skeleton = GetNode<Node3D>("Skeleton");
        CameraManager.Instance.GetFirstPersonCamera();
        GameManager.Instance.PlayerInstance = this;
    }
    public override void _Process(double delta)
    {
        if (firstPersonCamera.Current)
        {
            skeleton.Visible = false;
        }
        else
        {
            skeleton.Visible = true;
        }
        Vector3 direction = Vector3.Zero;

        if (Input.IsActionPressed("MoveUp"))
            direction -= Transform.Basis.Z;
        if (Input.IsActionPressed("MoveDown"))
            direction += Transform.Basis.Z;
        if (Input.IsActionPressed("MoveLeft"))
            direction -= Transform.Basis.X;
        if (Input.IsActionPressed("MoveRight"))
            direction += Transform.Basis.X;

        direction = direction.Normalized();
        _isRunning = Input.IsActionPressed("RunToggle");
        var speed = _isRunning ? RunSpeed : Speed;
        Velocity = direction * speed;
        if (stateMachine != null)
        {
            if (direction == Vector3.Zero)
            {
                stateMachine.ChangeState(stateMachine.GetNode<PlayerIdleState>("Idle"));
            }
            else if (_isRunning)
            {
                stateMachine.ChangeState(stateMachine.GetNode<PlayerRunningState>("Running"));
            }
            else
            {
                stateMachine.ChangeState(stateMachine.GetNode<PlayerWalkingState>("Walking"));
            }
        }
        HandleAnimation(delta);
        MoveAndSlide();
    }

    public void HandleAnimation(double delta)
    {
        switch (stateMachine.CurrentState)
        {
            case PlayerIdleState:
                _walkBlendAmount = Mathf.MoveToward(_walkBlendAmount, 0.0f, (float)(BlendSpeed * delta));
                _runBlendAmount = Mathf.MoveToward(_runBlendAmount, 0.0f, (float)(BlendSpeed * delta));
                break;
            case PlayerWalkingState:
                _walkBlendAmount = Mathf.MoveToward(_walkBlendAmount, 1.0f, (float)(BlendSpeed * delta));
                _runBlendAmount = Mathf.MoveToward(_runBlendAmount, 0.0f, (float)(BlendSpeed * delta));
                break;
            case PlayerRunningState:
                _walkBlendAmount = Mathf.MoveToward(_walkBlendAmount, 0.0f, (float)(BlendSpeed * delta));
                _runBlendAmount = Mathf.MoveToward(_runBlendAmount, 1.0f, (float)(BlendSpeed * delta));
                break;
        }
        UpdateAnimationTreeParameters();
    }

    public void UpdateAnimationTreeParameters()
    {
        if (animationTree != null)
        {
            animationTree.Set("parameters/Walk/blend_amount", _walkBlendAmount);
            animationTree.Set("parameters/Run/blend_amount", _runBlendAmount);
        }
    }
}
