using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public int Speed { get; set; } = 400;
	public int AttackDamage { get; set; } = 3;
	public int Weight { get; set; } = 3;
	private Node2D _layerFolder;
	private const int RADIUS = 1;

	private string currentweapon; // for testing right now
	private Area2D interactionArea;


	public override void _Ready()
	{
		_layerFolder = GetNode<Node2D>("../LayerFolder");
		YSortEnabled = true;
		SetupInteractionArea();
	}

	private void SetupInteractionArea()
	{
		interactionArea = new Area2D();
		var collisionShape = new CollisionShape2D();
		var circleShape = new CircleShape2D();

		circleShape.Radius = 10.0f;  // Interaction range
		collisionShape.Shape = circleShape;

		interactionArea.CollisionLayer = 0;
		interactionArea.CollisionMask = 2;   // Detect layer 2 (interactables)

		interactionArea.AddChild(collisionShape);
		AddChild(interactionArea);
	}

	private void UpdatePlayerZIndex()
	{
		// Go through layers from top to bottom
		//for (int i = _layerFolder.GetChildCount() - 1; i >= 0; i--)
		//{
		//	if (_layerFolder.GetChild(i) is TileMapLayer layer)
		//	{
		//		Vector2I tilePos = layer.LocalToMap(layer.ToLocal(GlobalPosition));

		//		// Check surrounding tiles
		//		for (int x = -RADIUS; x <= RADIUS; x++)
		//		{
		//			for (int y = -RADIUS; y <= RADIUS; y++)
		//			{
		//				if (layer.GetCellSourceId(tilePos + new Vector2I(x, y)) != -1)
		//				{
		//					ZIndex = layer.ZIndex + 1;
		//					return;
		//				}
		//			}
		//		}
		//	}
		//}
	}

	// ==========================
	// Weapon Transformation Logic

	public void OnChangeWeaponForm(int NewSpeed, int NewAttackDamage, int NewWeight, string CurrentWeapon)
	{
		Speed = NewSpeed;
		AttackDamage = NewAttackDamage;
		Weight = NewWeight;
		currentweapon = CurrentWeapon;
	}

	// ==========================
	// Player Movement
	public override void _Draw()
	{
		// Draw a circle representing the interaction range for debugging
		DrawArc(Vector2.Zero, 10.0f, 0, Mathf.Tau, 32, Colors.Yellow, 2.0f);
	}

	private void TryInteraction()
	{
		var areas = interactionArea.GetOverlappingAreas();
		foreach (var area in areas)
		{
			if (area is BaseInteractable interactable)
			{
				interactable.StartInteraction(currentweapon);

				break;  // Only interact with the first one found
			}
		}
	}

	// ==========================
	// Other Inputs

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact"))
		{
			TryInteraction();
		}
	}

	// ==========================
	// Player Movement

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Velocity = inputDirection * Speed;

		// idle
		if (inputDirection.X == 0 && inputDirection.Y == 0)
		{

			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
		}

		// move right
		else if (inputDirection.X > 0 && inputDirection.Y == 0)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("move_vertical");
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").RotationDegrees = -90;
		}

		// move left
		else if (inputDirection.X < 0 && inputDirection.Y == 0)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("move_vertical");
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").RotationDegrees = 90;
		}

		// move up
		else if (inputDirection.X == 0 && inputDirection.Y < 0)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("move_vertical");
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").RotationDegrees = 180;
		}

		// move down
		else if (inputDirection.X == 0 && inputDirection.Y > 0)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("move_vertical");
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").RotationDegrees = 0;
		}

		// move north west
		else if (inputDirection.X < 0 && inputDirection.Y < 0)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("move_diagonal");
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").RotationDegrees = 180;
		}

		// move north east
		else if (inputDirection.X > 0 && inputDirection.Y < 0)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("move_diagonal");
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").RotationDegrees = -90;
		}

		// move south west
		else if (inputDirection.X < 0 && inputDirection.Y > 0)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("move_diagonal");
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").RotationDegrees = 90;
		}

		// move south east
		else if (inputDirection.X > 0 && inputDirection.Y > 0)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("move_diagonal");
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").RotationDegrees = 0;
		}
	}



	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
		UpdatePlayerZIndex();
		QueueRedraw();
	}
}
