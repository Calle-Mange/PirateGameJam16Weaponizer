using Godot;
using System;
using System.Threading.Tasks;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed { get; set; } = 400;
    public int AttackDamage { get; set; } = 3;
    public int Weight { get; set; } = 3;
    private Node2D _layerFolder;
	private const int RADIUS = 1;

    private string currentweapon; // for testing right now
    private Area2D interactionArea;
    private const float GRAVITY = 980.0f;
    private TileMapLayer currentLayer;
    private float gravityVelocity = 0f;
    private Vector2 movementVelocity;
    private bool isFalling = false;

    private LevelManager levelManager;
    private AnimationPlayer transitionPlayer;
    private Node transitionScene;

    public override void _Ready()
    {
        _layerFolder = GetNode<Node2D>("../LayerFolder");
        YSortEnabled = true;
        SetupInteractionArea();
        transitionScene = GetNode("/root/TransitionScene");
        transitionPlayer = transitionScene.GetNode<AnimationPlayer>("AnimationPlayer"); 
    }

    private void SetupInteractionArea(){
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
        for (int i = _layerFolder.GetChildCount() - 1; i >= 0; i--)
        {
            if (_layerFolder.GetChild(i) is TileMapLayer layer)
            {
                Vector2I tilePos = layer.LocalToMap(layer.ToLocal(GlobalPosition));
                
                // Check surrounding tiles
                for (int x = -RADIUS; x <= RADIUS; x++)
                {
                    for (int y = -RADIUS; y <= RADIUS; y++)
                    {
                        if (layer.GetCellSourceId(tilePos + new Vector2I(x, y)) != -1)
                        {
                            ZIndex = layer.ZIndex + 1;
                            return;
                        }
                    }
                }
            }
        }
    }

    private bool HasTileBelowPlayer()
    {
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

                foreach(var pos in checkPositions)
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

    public void OnChangeWeaponForm(int NewSpeed, int NewAttackDamage, int NewWeight, string CurrentWeapon)
    {
        Speed = NewSpeed;
        AttackDamage = NewAttackDamage;
        Weight = NewWeight;
        currentweapon = CurrentWeapon;
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
        if(@event.IsActionPressed("interact")){
            TryInteraction();
        }
    }

    // ==========================
    // Player Movement

    private void HandleGravity(double delta){
        if (!HasTileBelowPlayer())
        {
            if(!isFalling)
            {
                isFalling = true;
                movementVelocity = Vector2.Zero;
            }
            gravityVelocity += GRAVITY * (float)delta;

            if(Position.Y > 1000)
            {
                Respawn();
            }
        } else {
            if(!isFalling)
            {
                gravityVelocity = 0;
            }
        }
    }

    private async void Respawn(){
        transitionPlayer.Play("fade_out");
        await ToSignal(transitionPlayer, "animation_finished");

        Position = new Vector2(0,0);
        isFalling = false;
        movementVelocity = Vector2.Zero;

        transitionPlayer.Play("fade_in");
    }

    public void GetInput()
    {
        if(!isFalling){
            Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
            movementVelocity = inputDirection * Speed;
        } else {
            movementVelocity = Vector2.Zero;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        HandleGravity(delta);
        Velocity = new Vector2(movementVelocity.X, movementVelocity.Y + gravityVelocity);
        MoveAndSlide();
		UpdatePlayerZIndex();
        QueueRedraw();
    }
}
