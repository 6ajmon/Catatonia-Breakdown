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

    public void CreateAudioOneShotAtPosition(AudioStream sfx, Vector3 position, string bus = "SFX", float volumeDb = 0.0f, float lifespan = 0.0f)
    {
        if (sfx == null) return;
        
        GD.Print("Creating audio one shot at position");
        var audioPlayer = new AudioStreamPlayer3D();
        GetTree().Root.AddChild(audioPlayer);
        audioPlayer.Stream = sfx;
        audioPlayer.GlobalPosition = position;
        audioPlayer.Autoplay = false;
        audioPlayer.Bus = bus;
        audioPlayer.VolumeDb = volumeDb;
        audioPlayer.Playing = true;
        
        if (lifespan > 0.0f)
        {
            var timer = new Timer();
            audioPlayer.AddChild(timer);
            timer.WaitTime = lifespan;
            timer.OneShot = true;
            timer.Timeout += () => audioPlayer.QueueFree();
            timer.Start();
        }
        else
        {
            audioPlayer.Finished += () => audioPlayer.QueueFree();
        }
    }

}
