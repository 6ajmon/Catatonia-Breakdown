using Godot;
using System;
using System.Threading.Tasks;

public partial class OutsideCutscene : Cutscene
{
    private bool gateClosed = false;
    public override void _Ready()
    {
        CameraManager.Instance.GetStaticCameras();
        base._Ready();
        SignalManager.Instance.GateClosed += OnGateClosed;
    }
    public override async Task RunSequence()
    {
        subtitlesOverlay.ShowSubtitle(StringManager.Instance.Names.Novak, StringManager.Instance.Dialogue.ItsSoCold, 7.0f);
        await WaitForPlayerInput();
        subtitlesOverlay.HideSubtitle();
        await WaitForSeconds(2.0f);
        subtitlesOverlay.ShowSubtitle(StringManager.Instance.Names.Novak, StringManager.Instance.Dialogue.IllCloseThisGate, 7.0f);
        await WaitForPlayerInput();
        subtitlesOverlay.HideSubtitle();

    }

    private async void OnGateClosed()
    {
        gateClosed = true;
        subtitlesOverlay.ShowSubtitle(StringManager.Instance.Names.Novak, StringManager.Instance.Dialogue.WhatWasThatNoise, 6.0f);
        await WaitForPlayerInput();
        subtitlesOverlay.HideSubtitle();
    }

    private async void OnReturningPlayerEntered(Node body)
    {
        if (!gateClosed) return;
        if (body is NovakPlayer)
        {
            subtitlesOverlay.ShowSubtitle(StringManager.Instance.Names.Novak, StringManager.Instance.Dialogue.WasThatAGorilla, 9.0f);
            await WaitForPlayerInput();
            subtitlesOverlay.HideSubtitle();
        }
    }
}
