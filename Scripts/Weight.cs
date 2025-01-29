using Godot;

public partial class Weight : Node2D
{
	private Area2D collisionArea;
    
    public override void _Ready()
    {
        collisionArea = GetNode<Area2D>("Area2D");
        collisionArea.AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area.IsInGroup("Button"))
        {
            var button = area.GetParent<DoorOpenButton>();
            button.OnWeightCollision();
        }
    }

    public override void _ExitTree()
    {
        if (collisionArea != null)
        {
            collisionArea.AreaEntered -= OnAreaEntered;
        }
        base._ExitTree();
    }
}
