using Godot;
using System;
public abstract partial class BaseInteractable : Area2D, IInteractable
{
	[Signal]
    public delegate void InteractionStartedEventHandler();
    
    [Signal]
    public delegate void InteractionCompletedEventHandler();
    
    [Signal]
    public delegate void InteractionFailedEventHandler();

	[Export]
    public string[] AllowedWeapons { get; set; } = new string[] { };
    
    [Export]
    public bool RequiresSpecificWeapon { get; set; } = false;

    protected WeaponList WeaponNamesResource;

    public override void _Ready()
    {
        WeaponNamesResource = GD.Load<WeaponList>("res://Scripts/Resources/WeaponNames.tres");
    }

    public bool CanInteract(string weaponType){
		if (!RequiresSpecificWeapon){
			return true;
		}
		return Array.Exists(AllowedWeapons, weapon => weapon == weaponType);
	}

	public void StartInteraction(string weaponType){
		if(CanInteract(weaponType)){
			EmitSignal(SignalName.InteractionStarted);
			PerformInteraction(weaponType);
		} else {
			EmitSignal(SignalName.InteractionFailed);
		}
	}

	protected abstract void PerformInteraction(string weaponType);
}
