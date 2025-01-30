using Godot;

public partial class DoorOpenButton : Node2D
{
    private Sprite2D unpressedSprite;
    private Sprite2D pressedSprite;
    private Area2D collisionArea;
	private AnimationPlayer levelAnimationPlayer;
    
    public override void _Ready()
    {
        unpressedSprite = GetNode<Sprite2D>("unpressed_sprite");
        pressedSprite = GetNode<Sprite2D>("pressed_sprite");
        collisionArea = GetNode<Area2D>("Area2D");
		levelAnimationPlayer = GetNode<AnimationPlayer>("/root/main3/AnimationPlayer");

		Node current = this;
        while (current != null)
        {
            current = current.GetParent();
        }
    }
    
    public void OnWeightCollision()
    {
        unpressedSprite.Visible = false;
        pressedSprite.Visible = true;
        
        if (levelAnimationPlayer != null)
        {
			AudioManager.Instance.PlaySound("floor_switch");
			levelAnimationPlayer.Play("move_down_tiles");
        }
    }
}
