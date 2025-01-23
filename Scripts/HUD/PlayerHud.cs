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
        GD.Print(WeaponIconsArray.Count);

        for (int i = 1; i <= WeaponIconsArray.Count - 1; i++)
        {
            WeaponIconsArray[i].Visible = false;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        UpdateHeartIcons();
        UpdateWeaponIcons();
    }

    public void UpdateHeartIcons()
    {
        int healthIndex = GlobalGameVariables.Instance.PlayerHealth;

        for (int i = 0; i < HeartIconsArray.Count; i++)
        {
            if (healthIndex <= 0)
            {
                foreach (TextureRect heart in HeartIconsArray) 
                { 
                    heart.Visible = false; 
                }
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
        if (GlobalGameVariables.Instance.PlayerInventory[GlobalGameVariables.Instance.WeaponList.Axe])
        {
            WeaponIconsArray[1].Visible = true;
        }
    }
}
