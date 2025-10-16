using Godot;
using System;
using System.Collections.Generic;

public partial class AudioManager : Node
{
    public static AudioManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<AudioManager>("AudioManager");
    [Export] public AudioStreamPlayer MusicPlayer;
    [Export] public AudioStreamPlayer SFXPlayer;

    public void PlayMusic(AudioStream music)
    {
        if (MusicPlayer == null) return;
        MusicPlayer.Stream = music;
        MusicPlayer.Play();
    }
    public void PlaySFX(AudioStream sfx)
    {
        if (SFXPlayer == null) return;
        SFXPlayer.Stream = sfx;
        SFXPlayer.Play();
    }

    public void CreateAudioOneShotAtPosition(AudioStream sfx, Vector3 position, string bus = "SFX", float volumeDb = 0.0f)
    {
        var audioPlayer = new AudioStreamPlayer3D();
        audioPlayer.Stream = sfx;
        audioPlayer.GlobalPosition = position;
        audioPlayer.Autoplay = false;
        audioPlayer.Bus = bus;
        audioPlayer.VolumeDb = volumeDb;
        GetTree().Root.AddChild(audioPlayer);
        audioPlayer.Play();
        audioPlayer.Finished += () => audioPlayer.QueueFree();
    }

}
