using System;
using Godot;

public partial class RopeInteractable : BaseInteractable
{
	private AnimationPlayer animationPlayer;
	private Sprite2D sprite;
	private Sprite2D noRopeSprite;
	private CollisionShape2D collisionShape;
	private CutAnimation cutAnimation;
	private Player player;

	public override void _Ready(){
		base._Ready();
		AllowedWeapons = new [] {WeaponNamesResource.Dagger};
		RequiresSpecificWeapon = true;

		CollisionLayer = 2;
        CollisionMask = 0;

		animationPlayer = GetNode<AnimationPlayer>("../AnimationPlayer");
		animationPlayer.AnimationFinished += OnAnimationFinished;
		sprite = GetNode<Sprite2D>("../with_rope_sprite");
		noRopeSprite = GetNode<Sprite2D>("../no_rope_sprite");
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		cutAnimation = GetNode<CutAnimation>("/root/main/IsometricLevel/Interactables/WeightHolder/DaggerCut");
		player = GetNode<Player>("../../../Player");

		if (cutAnimation != null)
        {
            cutAnimation.CutAnimationFinished += OnCutAnimationFinished;
        }
	}
	protected override void PerformInteraction(string weaponType, Vector2 interactionDirection){
		if (cutAnimation != null)
        {
			player.Visible = false;
			Vector2 cutPosition = new Vector2(-64, -16);
            cutAnimation.GlobalPosition = GlobalPosition - cutPosition;
            cutAnimation.PlayCutAnimation();
        }
        else
        {
            StartWeightFall();
        }
	}

	private void OnCutAnimationFinished()
    {
        StartWeightFall();
		player.Visible = true;
    }

    private void StartWeightFall()
    {
        animationPlayer.Play("weight_fall");
    }

	private void OnAnimationFinished(StringName animationName){
		if (animationName == "weight_fall"){
			collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			sprite.Visible = false;
			noRopeSprite.Visible = true;
			EmitSignal(SignalName.InteractionCompleted);
		}
	}

	public override void _ExitTree(){
		if (animationPlayer != null){
			animationPlayer.AnimationFinished -= OnAnimationFinished;
		}
		if (cutAnimation != null)
        {
            cutAnimation.CutAnimationFinished -= OnCutAnimationFinished;
        }
		base._ExitTree();
	}
}
