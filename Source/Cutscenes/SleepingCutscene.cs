using Godot;
using System;
using System.Threading.Tasks;

public partial class SleepingCutscene : Cutscene
{
    [Export] public Camera3D bedCamera;
    [Export] public Camera3D windowCamera;
    public override void _Ready()
    {
        base._Ready();
    }
    public override async void RunSequence()
    {
        subtitlesOverlay.ShowSubtitle(names.Novak, dialogue.WhatsThatNoise);
        await WaitForPlayerInput();
        subtitlesOverlay.ShowSubtitle(names.Novak, dialogue.ItsProbablyTrashDoor);
        await WaitForSeconds(1.0f);
        CameraManager.Instance.TransitionToCamera(windowCamera);
        
    }
}
