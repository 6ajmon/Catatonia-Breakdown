using Godot;
using System;

public partial class Level : Node3D
{
    public override void _Ready()
    {
        CameraManager.Instance.GetCameras();
    }
    private void OnArea1Entered(Node3D body)
    {
        if (body is CharacterTmp)
        {
            GD.Print("Entered Area 1");
            CameraManager.Instance.OnChangeCamera(1);
        }
    }
    private void OnArea2Entered(Node3D body)
    {
        if (body is CharacterTmp)
        {
            GD.Print("Entered Area 2");
            CameraManager.Instance.OnChangeCamera(0);
        }
    }
}
