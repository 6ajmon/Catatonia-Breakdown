using Godot;
using System;

public partial class NovakPlayer : CharacterBody3D
{
    [Export] public float Speed = 3.0f;
    [Export] public AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        CameraManager.Instance.GetFirstPersonCamera();
        GameManager.Instance.PlayerInstance = this;
        if (animationPlayer != null)
        {
            animationPlayer.Play("Novak/Idle");
        }
    }
    public override void _Process(double delta)
    {
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

        Velocity = direction * Speed;

        MoveAndSlide();
    }
}
