using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
public partial class CameraManager : Node
{
    public static CameraManager Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<CameraManager>("CameraManager");
    [Export] private NodePath _cameraPath;
    public List<Camera3D> Cameras = new();
    public Camera3D FirstPersonCamera;

    public override void _Ready()
    {
        SignalManager.Instance.ChangeCamera += OnChangeCamera;
        SignalManager.Instance.SwitchToFirstPersonCamera += OnSwitchToFirstPersonCamera;
    }

    public void GetStaticCameras()
    {
        Cameras.Clear();
        foreach (Camera3D cam in GetTree().GetNodesInGroup("StaticCameras"))
        {
            Cameras.Add(cam);
        }
    }
    public void GetFirstPersonCamera()
    {
        var nodesInFirstPersonGroup = GetTree().GetNodesInGroup("FirstPersonCamera");
        if (nodesInFirstPersonGroup.Count == 0) return;
        FirstPersonCamera = nodesInFirstPersonGroup.FirstOrDefault() as Camera3D;
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
        if (Input.IsActionJustPressed("DebugSwitchToFirstPersonCamera"))
        {
            SignalManager.Instance.EmitSignal(nameof(SignalManager.SwitchToFirstPersonCamera));
        }
    }

    public void OnNextCamera()
    {
        if (Cameras.Count == 0) return;
        int currentIndex = Cameras.FindIndex(cam => cam.Current);
        int nextIndex = (currentIndex + 1) % Cameras.Count;
        SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeCamera), nextIndex);
    }
    public void OnPreviousCamera()
    {
        if (Cameras.Count == 0) return;
        int currentIndex = Cameras.FindIndex(cam => cam.Current);
        int previousIndex = (currentIndex - 1 + Cameras.Count) % Cameras.Count;
        SignalManager.Instance.EmitSignal(nameof(SignalManager.ChangeCamera), previousIndex);
    }
    public void OnChangeCamera(int index)
    {
        if (Cameras.Count == 0 || index < 0 || index >= Cameras.Count) return;

        foreach (var cam in Cameras)
        {
            cam.Current = false;
        }
        Cameras[index].Current = true;
    }
    public void OnSwitchToFirstPersonCamera()
    {
        if (FirstPersonCamera == null) return;

        foreach (var cam in Cameras)
        {
            cam.Current = false;
        }
        FirstPersonCamera.Current = true;
    }
}
