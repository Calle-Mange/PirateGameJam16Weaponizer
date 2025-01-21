using Godot;
using System;

public partial class GlobalGameVariables : Node
{
	[Export]
	public int PlayerHealth { get; set; }

	[Export]
	public bool AxeUnlocked { get; set; }

    [Export]
	public bool FlailUnlocked { get; set; }

	[Export]
	public bool GunUnlocked { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayerHealth = 5;
		AxeUnlocked = false;
		FlailUnlocked = false;
		GunUnlocked = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
