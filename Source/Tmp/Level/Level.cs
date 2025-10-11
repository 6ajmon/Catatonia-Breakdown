using Godot;
using System;

public partial class Level : Node3D
{
    public override void _Ready()
    {
        CameraManager.Instance.GetStaticCameras();
    }
}
