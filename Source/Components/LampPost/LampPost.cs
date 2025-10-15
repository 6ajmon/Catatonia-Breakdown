using Godot;
using System;

public partial class LampPost : MeshInstance3D
{
    [Export] public SpotLight3D LightDown;
    [Export] public SpotLight3D LightUp;
    [Export] public float UpLightEnergyOn = 36.0f;
    [Export] public float DownLightEnergyOn = 6.0f;
    [Export] public bool EnableFlicker = true;
    [Export] public float FlickerMinInterval = 0.05f;
    [Export] public float FlickerMaxInterval = 0.3f;
    [Export] public float FlickerIntensity = 0.2f;
    
    private float flickerTimer = 0f;
    private float nextFlickerTime = 0f;
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    
    public override void _Ready()
    {
        rng.Randomize();
        nextFlickerTime = rng.RandfRange(FlickerMinInterval, FlickerMaxInterval);
    }
    
    public override void _Process(double delta)
    {
        if (!EnableFlicker) return;
        
        flickerTimer += (float)delta;
        
        if (flickerTimer >= nextFlickerTime)
        {
            flickerTimer = 0f;
            nextFlickerTime = rng.RandfRange(FlickerMinInterval, FlickerMaxInterval);
            
            float flickerAmount = rng.RandfRange(-FlickerIntensity, FlickerIntensity);
            
            if (LightUp != null)
            {
                LightUp.LightEnergy = UpLightEnergyOn + (UpLightEnergyOn * flickerAmount);
            }
            
            if (LightDown != null)
            {
                LightDown.LightEnergy = DownLightEnergyOn + (DownLightEnergyOn * flickerAmount);
            }
        }
    }
}