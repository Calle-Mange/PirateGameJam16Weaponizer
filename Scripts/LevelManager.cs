using Godot;
using System.Threading.Tasks;

public partial class LevelManager : Node
{
    private AnimationPlayer transitionPlayer;
    private Node transitionScene;

    public override void _Ready()
    {
        transitionScene = GetNode("/root/TransitionScene");
        transitionPlayer = transitionScene.GetNode<AnimationPlayer>("AnimationPlayer");
    }

	public void ChangeSceneToFile(string scenePath){
		CallDeferred(nameof(ChangeSceneDeferred), scenePath);
	}

	private async void ChangeSceneDeferred(string scenePath){
		transitionPlayer.Play("fade_out");
		await ToSignal(transitionPlayer, "animation_finished");

		var result = GetTree().ChangeSceneToFile(scenePath);
		if (result != Error.Ok)
        {
            GD.PrintErr($"Failed to change scene to {scenePath}");
            return;
        }

		transitionPlayer.Play("fade_in");
	}
}
	
