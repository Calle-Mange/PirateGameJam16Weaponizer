using Godot;
using System.Threading.Tasks;

public partial class LevelManager : Node
{
    private AnimationPlayer transitionPlayer;
    private Node transitionScene;
    private int currentLevel = 0;

    private readonly string mainMenuPath = "res://Scenes/Menu/main_menu.tscn";
    private readonly string storyPath = "res://Scenes/Menu/story_scene.tscn";
	private readonly string previousScene;

	private readonly string endScenePath = "res://Scenes/Menu/end_scene.tscn";

    private readonly string[] levelPaths = new[]
    {
        // "res://Scenes/TileMapLevels/map0.tscn", test level
        "res://Scenes/TileMapLevels/Level1.tscn",
        "res://Scenes/TileMapLevels/Level2.tscn",
    };

    public void StartNewGame()
    {
        ChangeSceneToFile(storyPath);
    }

    public void StartFirstLevel()
    {
        // Called from the story scene when it's finished
        currentLevel = 1;
        ChangeSceneToFile(levelPaths[0]);
    }

    public override void _Ready()
    {
        transitionScene = GetNode("/root/TransitionScene");
        transitionPlayer = transitionScene.GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void GoToNextLevel()
    {
        currentLevel++;
        if (currentLevel <= levelPaths.Length)
        {
            ChangeSceneToFile(levelPaths[currentLevel - 1]);
        }
        else
        {
            GD.Print("Game Completed!");
            ChangeSceneToFile(endScenePath);
        }
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
            return;
        }

		transitionPlayer.Play("fade_in");

        if(scenePath.EndsWith("Level1.tscn")){
            await ToSignal(GetTree(), "process_frame");
            ShowTutorial();
        }
	}

    private void ShowTutorial()
    {
        var tutorialScene = ResourceLoader.Load<PackedScene>("res://Scenes/tutorial_overlay.tscn");
        var tutorial = tutorialScene.Instantiate<TutorialOverlay>();
        GetTree().Root.AddChild(tutorial);
    }
}
	
