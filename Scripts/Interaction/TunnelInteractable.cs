using Godot;
using System;

public partial class TunnelInteractable : BaseInteractable
{
	[Export] public string TunnelID { get; set; } = "Tunnel1";
	[Export] public float CameraSmoothSpeed {get; set;} = 5.0f;

	private Node2D exitNode;
	private AnimatedSprite2D wallSprite;
	private Player player;
	private Camera2D camera;

    public override void _Ready()
    {
        base._Ready();
		AllowedWeapons = new[]{WeaponNamesResource.Dagger};
		RequiresSpecificWeapon = true;
		CollisionLayer = 2;
		CollisionMask = 0;

		exitNode = GetNode<Node2D>("../../ExitNodeFolder/" + TunnelID + "_Exit");
		player = GetNode<Player>("../../Player");
		wallSprite = GetNode<AnimatedSprite2D>("Wall/WallSprite");
		camera = player.GetNode<Camera2D>("Camera2D");

		if(exitNode == null){
			GD.Print($"No Exit node found for tunnel {TunnelID}");
			return;
		}

		if (player == null){
			GD.Print("Player node not found");
			return;
		}

		if (wallSprite != null)
        {
            wallSprite.AnimationFinished += OnAnimationFinished;
        }
    }

    private void OnAnimationFinished()
    {
		TeleportPlayer();
		player.Visible = true;
    }


    protected override void PerformInteraction(string weaponType)
    {
        if (exitNode == null) return;

		if(wallSprite != null){
			player.Visible = false;
			wallSprite.Play("go_through_tunnel");
		} else {
			TeleportPlayer();
		}
    }

    private void TeleportPlayer()
    {
		var targetPos = exitNode.Position;
        player.GlobalPosition = targetPos;
		if(camera != null){
			camera.PositionSmoothingEnabled = true;
			camera.PositionSmoothingSpeed = CameraSmoothSpeed;
		}
		EmitSignal(SignalName.InteractionCompleted);
    }

    public override void _ExitTree()
    {
		if(wallSprite != null){
			wallSprite.AnimationFinished -= OnAnimationFinished;
		}
        base._ExitTree();
    }
}
