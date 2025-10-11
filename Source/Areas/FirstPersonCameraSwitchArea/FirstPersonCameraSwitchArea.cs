using Godot;
using System;

public partial class FirstPersonCameraSwitchArea : Area3D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }
    private void OnBodyEntered(Node3D body)
    {
        CameraManager.Instance.GetFirstPersonCamera();
        if (CameraManager.Instance.FirstPersonCamera == null) return;
        if (body is CharacterTmp)
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.SwitchToFirstPersonCamera));
        }
    }
}
