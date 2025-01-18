using Godot;
public interface IInteractable
{
	bool CanInteract(string weaponType);
	void StartInteraction(string weaponType);
}
