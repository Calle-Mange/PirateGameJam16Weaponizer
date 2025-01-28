using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerHud : CanvasLayer
{
    private List<TextureRect> heartIcons = new();
    private List<TextureRect> weaponIcons = new();
    public override void _Ready()
    {
        var heartsContainer = GetNode<HBoxContainer>("MarginContainer/HBoxContainer");
        if (heartsContainer != null)
        {
            foreach (Node child in heartsContainer.GetChildren())
            {
                if (child is TextureRect textureRect)
                {
                    heartIcons.Add(textureRect);
                }
            }
        }

        var weaponsBox = GetNode<HBoxContainer>("MarginContainer2/HBoxContainer");
        if (weaponsBox != null)
        {
            var weaponIcon0 = weaponsBox.GetNode<MarginContainer>("MarginContainer")?.GetChildren().OfType<TextureRect>().FirstOrDefault();
            var weaponIcon1 = weaponsBox.GetNode<MarginContainer>("MarginContainer2")?.GetChildren().OfType<TextureRect>().FirstOrDefault();
            
            if (weaponIcon0 != null) weaponIcons.Add(weaponIcon0);
            if (weaponIcon1 != null) weaponIcons.Add(weaponIcon1);
        }

        if (heartIcons.Count > 0)
        {
            GlobalGameVariables.Instance.PlayerHealth = heartIcons.Count;
            for (int i = 1; i < weaponIcons.Count; i++)
            {
                weaponIcons[i].Visible = false;
            }
        }
    }

    public override void _Process(double delta)
    {
        UpdateHeartIcons();
        UpdateWeaponIcons();
    }

    public void UpdateHeartIcons()
    {
        int healthIndex = GlobalGameVariables.Instance.PlayerHealth;
        
        for (int i = 0; i < heartIcons.Count; i++)
        {
            heartIcons[i].Visible = i < healthIndex;
        }
    }

    public void UpdateWeaponIcons()
    {
         if (weaponIcons.Count > 1 && 
            GlobalGameVariables.Instance.PlayerInventory[GlobalGameVariables.Instance.WeaponList.Axe])
        {
            weaponIcons[1].Visible = true;
        }
    }
}
