using Godot;
public partial class SpawnPoint : Node2D
{
	[Export] public string AreaName { get; set; }
    [Export] public int Priority { get; set; } = 0;
}
