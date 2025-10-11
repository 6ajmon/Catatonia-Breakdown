using Godot;
using System;

public partial class CameraArea : Area3D
{
    public Camera3D Camera;
    [Export] public bool ShouldLookAtPlayer = false;
    [Export] public float CameraSmoothnessFactor = 4.0f;
    public Vector3 UpDir = Vector3.Up;

        private Basis _targetBasis = Basis.Identity;
    public override void _Ready()
    {
        Camera = GetNode<Camera3D>("StaticCamera3D");
        BodyEntered += OnBodyEntered;
    }
    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
    }
    public override void _Process(double delta)
    {
        if (Camera == null) return;

        if (ShouldLookAtPlayer && GameManager.Instance.PlayerInstance != null)
        {
            Vector3 camPos = Camera.GlobalPosition;
            Vector3 targetPos = GameManager.Instance.PlayerInstance.GlobalPosition;
            Vector3 dir = (targetPos - camPos).Normalized();

            _targetBasis = Basis.LookingAt(dir, UpDir);

            float w = 1.0f - Mathf.Exp(-CameraSmoothnessFactor * (float)delta);

            Basis blended = Camera.GlobalBasis.Slerp(_targetBasis, w).Orthonormalized();
            Camera.GlobalBasis = blended;
        }
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
