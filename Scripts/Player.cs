using Godot;
using System;
using System.Reflection;
using System.Threading.Tasks;

public partial class Player : CharacterBody2D
{
	#region Constants
    private const int RADIUS = 1;
    private const float GRAVITY = 980.0f;
    private const uint NORMAL_COLLISION_MASK = 2;
    private const uint NO_COLLISION_MASK = 0;
    private const float SPEED_MULTIPLIER = 0.5f;
    #endregion

    #region Export Properties
    [Export] public int Speed { get; set; } = 120;
    [Export] public int AttackDamage { get; set; } = 3;
    [Export] public int Weight { get; set; } = 3;
    #endregion

    #region Node References
    private Node2D _layerFolder;
    private SpawnManager spawnManager;
    private AnimatedSprite2D animatedSprite;
    private Area2D interactionArea;
    private Node transitionScene;
    private AnimationPlayer transitionPlayer;
	private Timer hurtTimer;
    #endregion

    #region Movement and Physics
    private Vector2 movementVelocity;
    private float gravityVelocity = 0f;
    private bool isFalling = false;
    private Vector2 fallStartPosition;
    private TileMapLayer currentLayer;
	private Vector2 externalForce = Vector2.Zero;
	private float externalForceDrag = 0.95f;
    #endregion

    #region Interaction System
    private string currentweapon; // for testing right now
	private BaseInteractable currentInteractable;
	private Label interactionPrompt;
	#endregion

	#region HUD
	private bool Damageable = true;
    #endregion

    public override void _Ready()
    {
        _layerFolder = GetNode<Node2D>("../LayerFolder");
        YSortEnabled = true;
        SetupInteractionArea();
        transitionScene = GetNode("/root/TransitionScene");
        transitionPlayer = transitionScene.GetNode<AnimationPlayer>("AnimationPlayer"); 
        spawnManager = GetNode<SpawnManager>("../../SpawnManager");
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		hurtTimer = GetNode<Timer>("HurtTimer");
		AddToGroup("Player");
    }

	private void SetupInteractionArea()
	{
		interactionArea = new Area2D();
		var collisionShape = new CollisionShape2D();
        var circleShape = new CircleShape2D
        {
            Radius = 10.0f  // Interaction range
        };
        collisionShape.Shape = circleShape;

		interactionArea.CollisionLayer = 0;
		interactionArea.CollisionMask = 2;   // Detect layer 2 (interactables)

        interactionPrompt = new Label
        {
            Text = "[F]",
            Position = new Vector2(0, -40),
            Scale = new Vector2(1, 1),
            Modulate = Colors.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            ZIndex = 1000,
            YSortEnabled = false
        };

        interactionArea.AreaEntered += (area) => 
    	{
        	if (area is BaseInteractable interactable)
        	{
            	currentInteractable = interactable;
            	interactionPrompt.Visible = interactable.CanInteract(currentweapon);
        	}
    	};

    	interactionArea.AreaExited += (area) => 
    	{
        	if (area is BaseInteractable)
        	{
				currentInteractable = null;
            	interactionPrompt.Visible = false;
        	}
    	};

			interactionArea.AddChild(collisionShape);
			interactionArea.AddChild(interactionPrompt);
			AddChild(interactionArea);
	}

	private void UpdatePlayerZIndex()
	{
        if (_layerFolder == null) return;
		if (isFalling && fallStartPosition.Y < 0)
		{
			ZIndex = -1000;
			return;
		}

		Vector2I playerPos = Vector2I.Zero;
		bool isPositionCached = false;

		for (int i = _layerFolder.GetChildCount() - 1; i >= 0; i--)
		{
			if (_layerFolder.GetChild(i) is not TileMapLayer layer) continue;

			if (!isPositionCached)
			{
				playerPos = layer.LocalToMap(layer.ToLocal(GlobalPosition));
				isPositionCached = true;
			}

			// Check center tile first
			if (layer.GetCellSourceId(playerPos) != -1)
			{
				ZIndex = layer.ZIndex + 1;
				return;
			}

			// Check adjacent tiles only if center wasn't found
			Vector2I[] adjacentOffsets = { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
			foreach (var offset in adjacentOffsets)
			{
				if (layer.GetCellSourceId(playerPos + offset) != -1)
				{
					ZIndex = layer.ZIndex + 1;
					return;
				}
			}
		}
	}

	private bool HasTileBelowPlayer()
	{
		if (isFalling && fallStartPosition.Y < 0)
		{
			return false;
		}

		for (int i = _layerFolder.GetChildCount() - 1; i >= 0; i--)
		{
			if (_layerFolder.GetChild(i) is TileMapLayer layer)
			{
				Vector2I centerTile = layer.LocalToMap(layer.ToLocal(GlobalPosition));
				Vector2I[] checkPositions = new[]
				{
					centerTile,                        // Center
                    centerTile + new Vector2I(0, 1),  // Right
                    centerTile + new Vector2I(0, -1), // Left
                    centerTile + new Vector2I(1, 0),  // Down
                    centerTile + new Vector2I(-1, 0)  // Up
                };

				foreach (var pos in checkPositions)
				{
					if (layer.GetCellSourceId(pos) != -1)
					{
						currentLayer = layer;
						return true;
					}
				}
			}
		}
		return false;
	}

	// ==========================
	// Weapon Transformation Logic

	public void OnChangeWeaponForm(int NewSpeed, int NewAttackDamage, int NewWeight, string CurrentWeapon, SpriteFrames AnimationSet)
	{
		Speed = NewSpeed;
		AttackDamage = NewAttackDamage;
		Weight = NewWeight;
		currentweapon = CurrentWeapon;
		animatedSprite.SpriteFrames = AnimationSet;

		if (currentInteractable != null)
        {
            interactionPrompt.Visible = currentInteractable.CanInteract(currentweapon);
        }
	}

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

	private void HandleGravity(double delta)
	{
        if(_layerFolder == null || spawnManager == null) return;

		if (!HasTileBelowPlayer())
		{
			if (!isFalling)
			{
				isFalling = true;
				fallStartPosition = GlobalPosition;
				movementVelocity = Vector2.Zero;
				AudioManager.Instance.PlaySound("Falling");
			}

			if (fallStartPosition.Y < 0)
			{
				CollisionMask = NO_COLLISION_MASK;
			}
			gravityVelocity += GRAVITY * (float)delta;

			if (Position.Y > 1000)
			{
				Respawn();
			}
		}
		else
		{
			if (!isFalling)
			{
				gravityVelocity = 0;
				CollisionMask = NORMAL_COLLISION_MASK;
			}
		}
	}

	private async void Respawn()
	{
		transitionPlayer.Play("fade_out");
        await ToSignal(transitionPlayer, "animation_finished");

		Position = spawnManager.GetRespawnPosition(fallStartPosition);

		TakeDamage();

		isFalling = false;
        movementVelocity = Vector2.Zero;
		CollisionMask = NORMAL_COLLISION_MASK;
		transitionPlayer.Play("fade_in");
	}

	private async void LevelReset() {

		transitionPlayer.Play("fade_out");
		await ToSignal(transitionPlayer, "animation_finished");

		Position = spawnManager.GetInitalRespawnPosition();

		GD.Print(GlobalGameVariables.Instance.PlayerHealth);
		Position = spawnManager.GetInitalRespawnPosition();
		GlobalGameVariables.Instance.PlayerHealth = 5;

		isFalling = false;
		movementVelocity = Vector2.Zero;
		CollisionMask = NORMAL_COLLISION_MASK;
		transitionPlayer.Play("fade_in");
	}

	private void OnHurtTimerTimeout()
	{
		Damageable = true;
	}

	public void TakeDamage()
	{
		if (Damageable)
		{
			AudioManager.Instance.PlaySound("taking_damage");
			GlobalGameVariables.Instance.PlayerHealth--;
			Damageable = false;
			hurtTimer.Start(2);
			SetSpriteInvinsibility(true);

			if (GlobalGameVariables.Instance.PlayerHealth == 0)
			{
				LevelReset();
			}
		}
	}

	public void SetSpriteInvinsibility(bool isInvnsible)
	{
		if (isInvnsible)
		{
			var modulate = animatedSprite.Modulate;
			modulate.A = 0.5f;
			animatedSprite.Modulate = modulate;
		}
		else
		{
			var modulate = animatedSprite.Modulate;
			modulate.A = 1.0f;
			animatedSprite.Modulate = modulate;
		}	
	}

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        inputDirection = inputDirection.Normalized();

        Vector2 isoDirection = new Vector2(
            inputDirection.X + inputDirection.Y,
            (-inputDirection.X + inputDirection.Y) * 0.5f
        );

		if (Damageable)
		{
			SetSpriteInvinsibility(false);
		}

        isoDirection = isoDirection.Normalized();
    
        movementVelocity = isoDirection * Speed;

        if (inputDirection == Vector2.Zero)
        {
            animatedSprite.Play("idle");
            return;
        }

		// idle
		if (inputDirection.X == 0 && inputDirection.Y == 0)
		{
			animatedSprite.Play("idle");
		}

		// move right
		else if (inputDirection.X > 0 && inputDirection.Y == 0)
		{
			animatedSprite.Play("move_vertical");
			animatedSprite.RotationDegrees = -120;
		}

		// move left
		else if (inputDirection.X < 0 && inputDirection.Y == 0)
		{
			animatedSprite.Play("move_vertical");
			animatedSprite.RotationDegrees = 60;
		}

		// move up
		else if (inputDirection.X == 0 && inputDirection.Y < 0)
		{
			animatedSprite.Play("move_vertical");
			animatedSprite.RotationDegrees = 125;
		}

		// move down
		else if (inputDirection.X == 0 && inputDirection.Y > 0)
		{
			animatedSprite.Play("move_vertical");
			animatedSprite.RotationDegrees = -60;
		}

		// move north west
		else if (inputDirection.X < 0 && inputDirection.Y < 0)
		{
			animatedSprite.Play("move_diagonal");
			animatedSprite.RotationDegrees = 90;
		}

		// move north east
		else if (inputDirection.X > 0 && inputDirection.Y < 0)
		{
			animatedSprite.Play("move_diagonal");
			animatedSprite.RotationDegrees = 180;
		}

		// move south west
		else if (inputDirection.X < 0 && inputDirection.Y > 0)
		{
			animatedSprite.Play("move_diagonal");
			animatedSprite.RotationDegrees = 0;
		}

		// move south east
		else if (inputDirection.X > 0 && inputDirection.Y > 0)
		{
			animatedSprite.Play("move_diagonal");
			animatedSprite.RotationDegrees = -90;
		}
	}

	public void AddExternalForce(Vector2 force)
    {
        externalForce += force;
    }
    public override void _PhysicsProcess(double delta)
    {
		if (TutorialOverlay.IsTutorialActive) return;
        GetInput();
        HandleGravity(delta);
        Velocity = new Vector2(movementVelocity.X + externalForce.X, movementVelocity.Y + gravityVelocity + externalForce.Y);
		externalForce *= externalForceDrag;
		
		if (externalForce.Length() < 0.01f)
        {
            externalForce = Vector2.Zero;
        }

        MoveAndSlide();
		UpdatePlayerZIndex();
		QueueRedraw();
	}
}
