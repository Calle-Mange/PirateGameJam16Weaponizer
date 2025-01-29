using Godot;
using System;

public partial class DoorInteractable : BaseInteractable
{
    private AnimatedSprite2D sprite;
    private CollisionShape2D collisionShape;
    private StaticBody2D body2D;

    private int Health;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        AllowedWeapons = new[] { WeaponNamesResource.Axe };
        RequiresSpecificWeapon = true;

        CollisionLayer = 2;
        CollisionMask = 0;

        Health = 3;

        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        body2D = GetNode<StaticBody2D>("StaticBody2D");
        sprite.AnimationFinished += OnAnimationFinished;
        sprite.Play("idle_door");
    }

    private void OnAnimationFinished()
    {
        if (sprite.Animation == "break_door_three")
        {
            collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
            body2D.SetDeferred("disabled", true);
            RemoveChild(body2D);
            EmitSignal(SignalName.InteractionCompleted);
            sprite.Play("broken_door");
        }

        if (sprite.Animation == "break_door_one")
        {
            sprite.Pause();
        }

        if (sprite.Animation == "break_door_two")
        {
            sprite.Pause();
        }
    }

    protected override void PerformInteraction(string weaponType, Vector2 interactionDirection)
    {
        if (Health == 3)
        {
            sprite.Play("break_door_one");
        }

        if (Health == 2)
        {
            sprite.Play("break_door_two");
        }

        if (Health == 1)
        {
            sprite.Play("break_door_three");
        }

        Health--;
    }
}
