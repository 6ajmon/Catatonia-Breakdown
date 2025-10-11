using Godot;
using System;

public partial class FirstPersonCameraSwitchArea : Area3D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
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
    private void OnBodyExited(Node3D body)
    {
        if (CameraManager.Instance.FirstPersonCamera == null) return;
        if (body is CharacterTmp && CameraManager.Instance.FirstPersonCamera.Current)
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeCamera), CameraManager.Instance.PreviousCameraIndex);
        }
    }
}
