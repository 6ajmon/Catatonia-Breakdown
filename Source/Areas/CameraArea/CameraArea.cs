using Godot;
using System;

public partial class CameraArea : Area3D
{
    public Camera3D Camera;
    public override void _Ready()
    {
        Camera = GetNode<Camera3D>("StaticCamera3D");
        BodyEntered += OnBodyEntered;
    }
    private void OnBodyEntered(Node3D body)
    {
        var cameraIndex = CameraManager.Instance.Cameras.IndexOf(Camera);
        if (body is CharacterTmp)
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeCamera), cameraIndex);
        }
    }
}
