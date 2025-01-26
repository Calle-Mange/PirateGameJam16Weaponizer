using Godot;
using System;

public partial class HazardInteractable : BaseInteractable
{
	private ColorRect waterRect;
	private ShaderMaterial waterMaterial;
	private Player player;
	public override void _Ready()
	{
		base._Ready();
		CollisionLayer = 2;
		CollisionMask = 0;

		waterRect = GetNode<ColorRect>("ColorRect");
		waterMaterial = (ShaderMaterial)waterRect.Material;
		player = GetNode<Player>("../../Player");

		RequiresSpecificWeapon = false;
	}

    protected override void PerformInteraction(string weaponType)
    {
		if(player != null){
			player.Respawn();
		}
    }

}
