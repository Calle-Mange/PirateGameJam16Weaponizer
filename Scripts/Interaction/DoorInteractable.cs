using Godot;
using System;

public partial class DoorInteractable : BaseInteractable
{
    private AnimatedSprite2D sprite;
    private CollisionShape2D collisionShape;
    private StaticBody2D body2D;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        base._Ready();
        AllowedWeapons = new[] { WeaponNamesResource.Axe };
        RequiresSpecificWeapon = true;

        CollisionLayer = 2;
        CollisionMask = 0;

        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        sprite.Play();
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        body2D = GetNode<StaticBody2D>("StaticBody2D");
    }

    protected override void PerformInteraction(string weaponType)
    {
        collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        body2D.SetDeferred("disabled", true);
        RemoveChild(body2D);
        EmitSignal(SignalName.InteractionCompleted);
    }
}
