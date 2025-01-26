using Godot;

public partial class GateInteractable : StaticBody2D
{
	private bool IsOpen = false;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private CollisionShape2D collisionShape;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        startPosition = Position;
        endPosition = new Vector2(Position.X, Position.Y - 32);
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (IsOpen)
        {
            Position = endPosition;
            collisionShape.Disabled = true;
        }

        if (!IsOpen)
        {
            Position = startPosition;
            collisionShape.Disabled = false;
        }
    }

    private void OnOpenGate()
    {
        IsOpen = true;
    }

    private void OnCloseGate()
    {
        IsOpen = false;
    }
}
