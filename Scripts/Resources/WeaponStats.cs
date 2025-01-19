using Godot;
using System;

[GlobalClass]
public partial class WeaponStats : Resource
{
	[Export] public int Speed { get; set; }

    [Export] public int AttackDamage { get; set; }

    [Export] public int Weight { get; set; }
}
