using Godot;

public partial class RopeInteractable : BaseInteractable
{
	private AnimationPlayer animationPlayer;
	private Sprite2D sprite;
	private CollisionShape2D collisionShape;

	public override void _Ready(){
		AllowedWeapons = new [] {"knife"};
		RequiresSpecificWeapon = true;

		CollisionLayer = 2;
        CollisionMask = 0;

		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.AnimationFinished += OnAnimationFinished;
		animationPlayer.Play("idle_shine");
		sprite = GetNode<Sprite2D>("Sprite2D");
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
	}
	protected override void PerformInteraction(string weaponType){
		animationPlayer.Play("cut_rope");
	}

	private void OnAnimationFinished(StringName animationName){
		if (animationName == "cut_rope"){
			collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			sprite.Visible = false;
			QueueFree();
			EmitSignal(SignalName.InteractionCompleted);
		}
	}

	public override void _ExitTree(){
		if (animationPlayer != null){
			animationPlayer.AnimationFinished -= OnAnimationFinished;
		}
		base._ExitTree();
	}
}
