using Godot;

public partial class WaterHazard : Area2D
{
	public override void _Ready()
    {
        CollisionLayer = 0;
        CollisionMask = 1;
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is CharacterBody2D)
        {
            var player = body as Player;
            if (player != null)
            {
                player.TakeDamage();
            }
        }
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
    }
}
