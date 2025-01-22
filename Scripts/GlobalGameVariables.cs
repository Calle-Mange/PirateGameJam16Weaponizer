using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalGameVariables : Node
{
	public static GlobalGameVariables Instance {  get; private set; }

	public Dictionary<string, bool> PlayerInventory;

	public  WeaponList WeaponList { get; private set; }

	private List<string> Weapons;

	public int PlayerHealth { get; set; }

	public bool AxeUnlocked { get; set; }

	public bool FlailUnlocked { get; set; }

	public bool GunUnlocked { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Instance = this;
        
		PlayerInventory = new Dictionary<string, bool>();
		WeaponList = GD.Load<WeaponList>("res://Scripts/Resources/WeaponNames.tres");
        Weapons = new List<string>();
		Weapons.Add(WeaponList.Dagger);
		Weapons.Add(WeaponList.Axe);
		Weapons.Add(WeaponList.Flail);
		Weapons.Add(WeaponList.Gun);

		foreach (string weapon in Weapons)
		{
			if (weapon != WeaponList.Dagger)
			{
                PlayerInventory[weapon] = false;
            }
			else
			{
				PlayerInventory[weapon] = true;
			}
		}

		PlayerHealth = 5;
		AxeUnlocked = false;
		FlailUnlocked = false;
		GunUnlocked = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
