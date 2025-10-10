using Godot;
using System;

public partial class CharacterTmp : CharacterBody3D
{
    public override void _Ready()
    {
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

        float speed = 5.0f;
        Velocity = direction * speed;

        MoveAndSlide();
    }
}
