using Godot;
using System;
using static Godot.TextServer;

public partial class BoxInteractable : BaseInteractable
{
	private bool IsMoving;
	private int PushSpeed;
	private Vector2 StartPosition;
	private Vector2 PushDirection;
	private Timer MoveTimer;
    private Node2D _layerFolder;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        RequiresSpecificWeapon = false;
		IsMoving = false;
		PushSpeed = 100;
		StartPosition = Position;
		PushDirection = Vector2.Zero;
		MoveTimer = GetNode<Timer>("Timer");
        _layerFolder = GetNode<Node2D>("../LayerFolder");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        
    }

    public override void _PhysicsProcess(double delta)
    {
		if (IsMoving)
		{
            Position += (PushDirection * PushSpeed) * (float)delta;
        }
    }

    protected override void PerformInteraction(string weaponType, Vector2 interactionDirection)
    {
		PushDirection = interactionDirection.DirectionTo(Position);
		IsMoving = true;
		MoveTimer.Start();
    }

	private void OnStopMovement()
	{
		IsMoving = false;
	}
}
