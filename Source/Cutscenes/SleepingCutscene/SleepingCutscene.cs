using Godot;
using System;
using System.Threading.Tasks;

public partial class SleepingCutscene : Cutscene
{
    [Export] public Camera3D bedCamera;
    [Export] public Camera3D windowCamera;
    [Export] private InteractableDoor interactableDoor;
    public override void _Ready()
    {
        base._Ready();
        GameManager.Instance.IsCutscenePlaying = true;
    }
    public override async Task RunSequence()
    {
        interactableDoor.interactionEnabled = false;
        CameraManager.Instance.TransitionToCamera(bedCamera);
        subtitlesOverlay.ShowSubtitle(names.Novak, dialogue.WhatsThatNoise);
        await WaitForPlayerInput();
        subtitlesOverlay.ShowSubtitle(names.Novak, dialogue.ItsProbablyTrashDoor);
        await WaitForSeconds(2.0f);
        CameraManager.Instance.TransitionToCamera(windowCamera);
        await WaitForPlayerInput();
        CameraManager.Instance.TransitionToCamera(bedCamera);
        subtitlesOverlay.HideSubtitle();
        await WaitForSeconds(3.0f);
        subtitlesOverlay.ShowSubtitle(names.Novak, dialogue.FuckICantSleep);
        await WaitForPlayerInput();
        subtitlesOverlay.HideSubtitle();
        await WaitForSeconds(2.0f);
        GameManager.Instance.PlayerInstance.Rotation = new Vector3(0, 0, 0);
        SignalManager.Instance.EmitSignal(nameof(SignalManager.CutsceneEnded));
        await WaitForSeconds(3.0f);
        subtitlesOverlay.ShowSubtitle(names.Novak, dialogue.ImGonnaCheckItOut, 5.0f);
        await WaitForPlayerInput();
        subtitlesOverlay.HideSubtitle();
    }

    private async void OnPlayerEnteredWindowArea(Node body)
    {
        if (body is NovakPlayer)
        {
            interactableDoor.interactionEnabled = true;
            subtitlesOverlay.ShowSubtitle(names.Novak, dialogue.TheGateIsOpen, 5.0f);
            await WaitForPlayerInput();
            subtitlesOverlay.HideSubtitle();
        }
    }
}
