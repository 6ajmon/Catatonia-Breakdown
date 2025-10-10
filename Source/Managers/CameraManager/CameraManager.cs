using Godot;
using System;
using System.Collections.Generic;
public partial class CameraManager : Node
{
    public static CameraManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<CameraManager>("CameraManager");
    [Export] private NodePath _cameraPath;
    private List<Camera3D> _cameras = new();

    public override void _Ready()
    {
        SignalManager.Instance.ChangeCamera += OnChangeCamera;
    }

    public void GetCameras()
    {
        _cameras.Clear();
        foreach (Camera3D cam in GetTree().GetNodesInGroup("StaticCameras"))
        {
            _cameras.Add(cam);
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("NextCamera"))
        {
            OnNextCamera();
        }
        if (Input.IsActionJustPressed("PreviousCamera"))
        {
            OnPreviousCamera();
        }
    }

    public void OnNextCamera()
    {
        if (_cameras.Count == 0) return;
        int currentIndex = _cameras.FindIndex(cam => cam.Current);
        int nextIndex = (currentIndex + 1) % _cameras.Count;
        SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeCamera), nextIndex);
    }
    public void OnPreviousCamera()
    {
        if (_cameras.Count == 0) return;
        int currentIndex = _cameras.FindIndex(cam => cam.Current);
        int previousIndex = (currentIndex - 1 + _cameras.Count) % _cameras.Count;
        SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeCamera), previousIndex);
    }
    public void OnChangeCamera(int index)
    {
        if (_cameras.Count == 0 || index < 0 || index >= _cameras.Count) return;

        foreach (var cam in _cameras)
        {
            cam.Current = false;
        }
        _cameras[index].Current = true;
    }
}
