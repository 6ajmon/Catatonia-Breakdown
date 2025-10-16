using Godot;
using System;
using System.Threading.Tasks;

public partial class OutsideCutscene : Cutscene
{
    [Export] private Node3D BigfootNode;
    [Export(PropertyHint.File, "*.wav")] private string bigfootGateNoiseSoundPath;
    [Export(PropertyHint.File, "*.wav")] private string bigfootJumpscareNoiseSoundPath;
    [Export] private InteractableDoor InteractableDoorNode;
    private AudioStreamWav bigfootNoiseSound;
    private AudioStreamWav bigfootJumpscareNoiseSound;
    private bool gateClosed = false;
    public override void _Ready()
    {
        bigfootNoiseSound = ResourceLoader.Load<AudioStreamWav>(bigfootGateNoiseSoundPath);
        bigfootJumpscareNoiseSound = ResourceLoader.Load<AudioStreamWav>(bigfootJumpscareNoiseSoundPath);
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
        AudioManager.Instance.CreateAudioOneShotAtPosition(bigfootNoiseSound, BigfootNode.GlobalPosition);
        subtitlesOverlay.ShowSubtitle(StringManager.Instance.Names.Novak, StringManager.Instance.Dialogue.WhatWasThatNoise, 6.0f);
        await WaitForPlayerInput();
        subtitlesOverlay.HideSubtitle();
    }

    private async void OnReturningPlayerEntered(Node body)
    {
        if (!gateClosed) return;
        if (body is NovakPlayer)
        {
            BigfootNode.Visible = true;
            AudioManager.Instance.CreateAudioOneShotAtPosition(bigfootJumpscareNoiseSound, BigfootNode.GlobalPosition, lifespan: 0.43f);
            await WaitForSeconds(0.4f);
            BigfootNode.QueueFree();
            subtitlesOverlay.ShowSubtitle(StringManager.Instance.Names.Novak, StringManager.Instance.Dialogue.WasThatAGorilla, 9.0f);
            InteractableDoorNode.interactionEnabled = true;
            await WaitForPlayerInput();
            subtitlesOverlay.HideSubtitle();
        }
    }
}
