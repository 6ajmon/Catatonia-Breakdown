using Godot;
using System;

public partial class Bigfoot : CharacterBody3D
{
    [Export] public AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        if (animationPlayer != null)
        {
            animationPlayer.Play("Bigfoot/Idle");
        }
    }
}
