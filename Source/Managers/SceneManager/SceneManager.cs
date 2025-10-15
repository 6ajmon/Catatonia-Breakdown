using Godot;
using System;

public partial class SceneManager : Node
{
    public static SceneManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<SceneManager>("SceneManager");
    public bool IsMenuOpen { get; set; }
    public Node CurrentMenu { get; set; }

    public override void _Ready()
    {

    }

    public void ReplaceScene(string scenePath)
    {
        if (IsMenuOpen)
        {
            CurrentMenu?.QueueFree();
            CurrentMenu = null;
            IsMenuOpen = false;
        }

        GetTree().Paused = false;
        GetTree().ChangeSceneToFile(scenePath);
    }

    public void AddChildScene(string scenePath)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>(scenePath);
        if (scene != null)
        {
            Node instance = scene.Instantiate();
            if (instance != null)
            {
                GetTree().Root.AddChild(instance);
                CurrentMenu = instance;
                IsMenuOpen = true;
            }
            else
            {
                GD.PrintErr($"Failed to instantiate scene at {scenePath}");
            }
        }
        else
        {
            GD.PrintErr($"Failed to load scene at {scenePath}");
        }
    }
}