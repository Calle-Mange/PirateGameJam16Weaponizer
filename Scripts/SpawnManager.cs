using Godot;
using System;
using System.Linq;

public partial class SpawnManager : Node
{
	private Node spawnPointContainer;
	private Node2D layerFolder;

    public override void _Ready()
    {
		spawnPointContainer = GetNode("../SpawnPoints");
		layerFolder = GetNode<Node2D>("../IsometricLevel/LayerFolder");
	}

	public Vector2 GetRespawnPosition(Vector2 playerPosition){
		var children = spawnPointContainer.GetChildren();
    	int count = children.Count;
    	if (count == 0) return Vector2.Zero;

    	SpawnPoint closest = (SpawnPoint)children[0];
    	float closestDistance = playerPosition.DistanceTo(closest.GlobalPosition);

    	for (int i = 1; i < count; i++)
    	{
       		SpawnPoint spawn = (SpawnPoint)children[i];
        	float distance = playerPosition.DistanceTo(spawn.GlobalPosition);
        
        	if (distance < closestDistance || 
            	(Mathf.IsEqualApprox(distance, closestDistance) && 
             	spawn.Priority > closest.Priority))
        	{
            	closestDistance = distance;
            	closest = spawn;
        	}
    }

    return closest.GlobalPosition;
	}

    public Vector2 GetInitalRespawnPosition() {
		var initialSpawnPoint = (SpawnPoint)spawnPointContainer.GetChildren()[0];
		return initialSpawnPoint.GlobalPosition;
	}

	private int GetCurrentLayer(Vector2 position){
		for (int i = layerFolder.GetChildCount() - 1; i >= 0; i--)
    	{
        	if (layerFolder.GetChild(i) is TileMapLayer layer)
        	{
            	Vector2I tilePos = layer.LocalToMap(layer.ToLocal(position));
            	if (layer.GetCellSourceId(tilePos) != -1)
            	{
                	return i;
            	}
        	}
    	}
    	return 0;
	}
}
