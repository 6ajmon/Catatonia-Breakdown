using Godot;
using System;

public partial class FreeRoamCamera : Camera3D
{
    [Export] public float moveSpeed = 10.0f;
    [Export] public float minSpeed = 1.0f;
    [Export] public float maxSpeed = 100.0f;
    [Export] public float speedChangeRate = 2.0f;
    [Export] public float mouseSensitivity = 0.003f;
    
    private bool isActive = false;
    private Camera3D originalCamera;
    private Vector2 rotation = Vector2.Zero;
    
    public override void _Ready()
    {
        Current = false;
        Visible = false;
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("DebugSwitchToFreeRoamCamera"))
        {
            ToggleCamera();
        }
        
        if (!isActive) return;
        
        if (@event.IsActionPressed("ui_cancel")) // Escape key
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        
        if (@event is InputEventMouseButton && @event.IsPressed())
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
        
        if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            rotation.X -= mouseMotion.Relative.Y * mouseSensitivity;
            rotation.Y -= mouseMotion.Relative.X * mouseSensitivity;
            rotation.X = Mathf.Clamp(rotation.X, -Mathf.Pi / 2, Mathf.Pi / 2);
            
            Transform3D transform = Transform;
            transform.Basis = Basis.FromEuler(new Vector3(rotation.X, rotation.Y, 0));
            Transform = transform;
        }
        
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.WheelUp)
            {
                moveSpeed = Mathf.Min(moveSpeed + speedChangeRate, maxSpeed);
            }
            else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
            {
                moveSpeed = Mathf.Max(moveSpeed - speedChangeRate, minSpeed);
            }
        }
    }
    
    public override void _Process(double delta)
    {
        if (!isActive) return;
        
        Vector3 velocity = Vector3.Zero;
        
        if (Input.IsActionPressed("MoveUp")) velocity -= Transform.Basis.Z;
        if (Input.IsActionPressed("MoveDown")) velocity += Transform.Basis.Z;
        if (Input.IsActionPressed("MoveLeft")) velocity -= Transform.Basis.X;
        if (Input.IsActionPressed("MoveRight")) velocity += Transform.Basis.X;
        if (Input.IsActionPressed("DebugFlyUp")) velocity += Vector3.Up;
        if (Input.IsActionPressed("DebugFlyDown")) velocity -= Vector3.Up;
        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized();
        }
        
        GlobalPosition += velocity * moveSpeed * (float)delta;
    }
    
    private void ToggleCamera()
    {
        isActive = !isActive;
        
        if (isActive)
        {
            // Find and store current active camera
            var viewport = GetViewport();
            originalCamera = viewport.GetCamera3D();
            
            if (originalCamera != null && originalCamera != this)
            {
                GlobalTransform = originalCamera.GlobalTransform;
                Vector3 euler = originalCamera.GlobalTransform.Basis.GetEuler();
                rotation = new Vector2(euler.X, euler.Y);
            }
            
            Current = true;
            Visible = true;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
        else
        {
            Current = false;
            Visible = false;
            Input.MouseMode = Input.MouseModeEnum.Visible;
            
            if (originalCamera != null)
            {
                originalCamera.Current = true;
            }
        }
    }
}
