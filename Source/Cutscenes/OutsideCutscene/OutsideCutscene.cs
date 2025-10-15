using Godot;
using System;
using System.Threading.Tasks;

public partial class OutsideCutscene : Cutscene
{
    public override void _Ready()
    {
        CameraManager.Instance.GetStaticCameras();
        base._Ready();
    }
    public override async Task RunSequence()
    {
        await Task.CompletedTask;
    }
}
