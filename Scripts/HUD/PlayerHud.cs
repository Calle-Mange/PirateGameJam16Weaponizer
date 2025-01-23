using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerHud : CanvasLayer
{
	[Export] Godot.Collections.Array<TextureRect> HeartIconsArray;
	[Export] Godot.Collections.Array<TextureRect> WeaponIconsArray;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalGameVariables.Instance.PlayerHealth = HeartIconsArray.Count;

		for (int i = 0; i < WeaponIconsArray.Count; i++)
		{
			if (i != 0)
			{
				WeaponIconsArray[i].Visible = false;
			}
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void UpdateHeartIcons()
	{
		int healthIndex = GlobalGameVariables.Instance.PlayerHealth;

		for (int i = 0; i < HeartIconsArray.Count; i++)
		{
            if ( healthIndex <= 0)
			{

			}
            {
                
            }
            if (i > (healthIndex - 1))
			{
				HeartIconsArray[i].Visible = false;
			}

			if (i < (healthIndex - 1))
			{
				HeartIconsArray[i].Visible = true;
			}
		}
	}

	public void UpdateWeaponIcons()
	{
		if (GlobalGameVariables.Instance.AxeUnlocked)
		{
			WeaponIconsArray[1].Visible = true;
		}

		if (GlobalGameVariables.Instance.FlailUnlocked)
		{
			WeaponIconsArray[2].Visible = true;
		}

		if (GlobalGameVariables.Instance.GunUnlocked)
		{
			WeaponIconsArray[3].Visible = true;
		}
	}
}
