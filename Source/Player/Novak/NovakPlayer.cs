using Godot;
using System;

public partial class NovakPlayer : CharacterBody3D
{
    [Export] public float Speed = 2.0f;
    [Export] public float RunSpeed = 3.0f;
    [Export] public float RotationSpeed = 2.0f;
    [Export] public float RunRotationSpeed = 3.0f;
    [Export] public float MouseSensitivity = 0.16f;
    [Export] public float MaxCameraAngle = 60.0f;
    [Export] public float GravityForce = 9.8f;
    Vector3 direction = Vector3.Zero;
    private bool _isRunning = false;
    private bool _movingBackwards = false;
    private bool isFirstPerson = false;
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
        isFirstPerson = firstPersonCamera.Current;
        if (isFirstPerson)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
        else
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        
        updateInput(delta);
        updateStateMachine();
        HandleAnimation(delta);
        skeleton.Visible = !isFirstPerson;
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

    public void updateInput(double delta)
    {
        direction = Vector3.Zero;
        _movingBackwards = false;
        _isRunning = Input.IsActionPressed("RunToggle");
        var rotationSpeed = _isRunning ? RunRotationSpeed : RotationSpeed;
        
        if (Input.IsActionPressed("MoveUp"))
            direction += Transform.Basis.Z;
        if (Input.IsActionPressed("MoveDown"))
        {
            direction -= Transform.Basis.Z;
            _movingBackwards = true;
        }
        
        if (!isFirstPerson)
        {
            if (Input.IsActionPressed("MoveLeft"))
                RotateY(Mathf.DegToRad(rotationSpeed)); 
            if (Input.IsActionPressed("MoveRight"))
                RotateY(Mathf.DegToRad(-rotationSpeed));
            
        }
        else
        {
            if (Input.IsActionPressed("MoveLeft"))
            {
                direction += Transform.Basis.X;
            }

            if (Input.IsActionPressed("MoveRight"))
            {
                direction -= Transform.Basis.X;
            }

            
        }
        
        direction = direction.Normalized();
        
        var  speed = _isRunning ? RunSpeed : Speed;;
        if (_movingBackwards)
        {
            _isRunning = false;
            speed = Speed;
        }

        if (!IsOnFloor())
        {
            direction.Y = -GravityForce * (float)delta;
        }
        
        Velocity = direction * speed;
        MoveAndSlide();
    }

    public void updateStateMachine()
    {
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
    }

    public override void _Input(InputEvent @event)
    {
        if (isFirstPerson)
        {
            if (@event is InputEventMouseMotion mouseMotion)
            {
                RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * MouseSensitivity));
                float newX = firstPersonCamera.RotationDegrees.X - mouseMotion.Relative.Y * MouseSensitivity;
                newX = Mathf.Clamp(newX, -MaxCameraAngle, MaxCameraAngle);
                var rot = firstPersonCamera.RotationDegrees;
                rot.X = newX;
                firstPersonCamera.RotationDegrees = rot;
            }
            
        }
    }
}
