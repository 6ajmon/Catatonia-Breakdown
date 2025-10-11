using Godot;
using System;

public partial class Apartment : Node3D
{
    public override void _Ready()
    {
        CameraManager.Instance.GetStaticCameras();
    }
}
