using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed { get; set; } = 400;
    public int AttackDamage { get; set; } = 3;
    public int Weight { get; set; } = 3;
    private Node2D _layerFolder;
	private const int RADIUS = 1;

    public override void _Ready()
    {
        _layerFolder = GetNode<Node2D>("../LayerFolder");
        YSortEnabled = true;
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
    }
}
