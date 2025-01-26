using Godot;
using System;

public partial class GateInteractable : StaticBody2D
{
	private bool IsOpen = false;
	private Transform2D startTransform;
	private Transform2D endTransform;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startTransform = Transform;
		endTransform.Y = startTransform.Y - new Vector2(0, -50);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnOpenGate(double delta)
	{
		if (Transform.Y != endTransform.Y)
		{
            Position = Position.MoveToward(endTransform.Y, (float)delta);
        }

	}

	public void OnCloseGate(double delta)
	{
		if (Transform.Y != startTransform.Y)
		{
            Position = Position.MoveToward(startTransform.Y, (float)delta);
        }
	}
}
