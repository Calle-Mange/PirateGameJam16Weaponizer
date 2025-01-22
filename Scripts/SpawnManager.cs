using Godot;
using System;
using System.Linq;

public partial class SpawnManager : Node
{
	private Node spawnPointContainer;

    public override void _Ready()
    {
		spawnPointContainer = GetNode("../SpawnPoints");
    }

	public Vector2 GetRespawnPosition(Vector2 playerPosition){
		float closestDistance = float.MaxValue;
		SpawnPoint closest = null;
		int highestPriority = -1;

		foreach (SpawnPoint spawn in spawnPointContainer.GetChildren().Cast<SpawnPoint>()){
			float distance = playerPosition.DistanceTo(spawn.GlobalPosition);

			if(spawn.Priority > highestPriority || 
			(spawn.Priority == highestPriority && distance < closestDistance)){
				closestDistance = distance;
				closest = spawn;
				highestPriority = spawn.Priority;
			}
		}

		return closest?.GlobalPosition ?? Vector2.Zero;
	}
}
