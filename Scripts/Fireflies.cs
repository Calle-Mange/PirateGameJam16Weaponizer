using Godot;
using System;

public partial class Fireflies : CpuParticles2D
{
	public override void _Ready()
    {
        Emitting = true;
        Amount = 80;
        Lifetime = 5.0f;
        
        Direction = Vector2.Up;
        Spread = 180;
        Gravity = Vector2.Zero;
        InitialVelocityMin = 20.0f;
        InitialVelocityMax = 20.0f;
        
        Modulate = new Color(0.9f, 1.0f, 0.3f, 0.8f);
        Scale = new Vector2(2, 2);
        
        AngularVelocityMin = -45.0f;
        AngularVelocityMax = 45.0f;
        
        Explosiveness = 0.0f;
        Randomness = 1.0f;
    }
}
