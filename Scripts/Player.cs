using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed { get; set; } = 400;
    public int AttackDamage { get; set; } = 3;
    public int Weight { get; set; } = 3;
    private Node2D _layerFolder;
	private const int RADIUS = 1;

    private string currentweapon = "knife"; // for testing right now
    private Area2D interactionArea;


    public override void _Ready()
    {
        _layerFolder = GetNode<Node2D>("../LayerFolder");
        YSortEnabled = true;
        SetupInteractionArea();
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

<<<<<<< HEAD
    // ==========================
    // Weapon Transformation Logic

    public void OnChangeWeaponForm(int NewSpeed, int NewAttackDamage, int NewWeight)
    {
        Speed = NewSpeed;
        AttackDamage = NewAttackDamage;
        Weight = NewWeight;
    }
	
	// ==========================
	// Player Movement
=======
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
>>>>>>> 994bbdd (base of interaction system)

    public void GetInput()
    {
        Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        Velocity = inputDirection * Speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
		UpdatePlayerZIndex();
        QueueRedraw();
    }
}
