using Godot;
using System;

public partial class PressurePlateInteractable : Area2D
{
	[Signal]
	public delegate void PressurePlateActiveEventHandler();

	[Signal]
	public delegate void PressurePlateInactiveEventHandler();

	private Sprite2D texture;
	private Texture2D activeTexture;
	private Texture2D inactiveTexture;

	private bool IsActive = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		texture = GetNode<Sprite2D>("Sprite2D");
		activeTexture = GD.Load<Texture2D>("res://Assets/Graphics/WouldBuilding/pressure-plate-on.png");
		inactiveTexture = GD.Load<Texture2D>("res://Assets/Graphics/WouldBuilding/pressure-plate.png");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsActive)
		{
			texture.Texture = activeTexture;
		}
		else
		{
			texture.Texture = inactiveTexture;
		}

		var OverlappingBodies = GetOverlappingBodies();

		foreach (PhysicsBody2D body in OverlappingBodies)
		{
			if (body.IsInGroup("Player") || body.IsInGroup("Moveable"))
			{
				IsActive = true;
				EmitSignal(SignalName.PressurePlateActive);
			}
		}

		if (OverlappingBodies.Count <= 0)
		{
			IsActive = false;
			EmitSignal(SignalName.PressurePlateInactive);
		}
	}
}
