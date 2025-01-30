using Godot;
using System.Threading.Tasks;

public partial class StoryScene : Node
{
	[Export] string[] storyTexts = {
		"I was once known as the greatest blacksmith in the realm… Crafting weapons for all who sought my work.",
		// "But my rival couldn’t accept my choice to serve all sides of the great war. In his jealousy he cursed me…",
		// "Now I am bound to become the very weapons I once forged. Trapped in this battlefield echo.",
		// "My only hope lies in reaching his tower using the scattered weapons of war to break his curse."
	};

	private Label storyLabel;
	private Timer typewriterTimer;
	private int currentTextIndex = 0;
	private int currentCharIndex = 0;

	private LevelManager levelManager;

    public override void _Ready()
    {
		storyLabel = GetNode<Label>("Control/Label");
		storyLabel.Text = "";
		levelManager = GetNode<LevelManager>("/root/LevelManager");
		StartStorySequence();
    }

	private async void StartStorySequence(){
		await ToSignal(GetTree().CreateTimer(1.0f), "timeout");

		foreach(string text in storyTexts){
			await TypeWriterEffect(text);
			await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
			await FadeOutText();
		}

		levelManager.StartFirstLevel();
	}

	private async Task TypeWriterEffect(string text){
		storyLabel.Modulate = new Color(1, 1, 1, 1);
		storyLabel.Text = "";

		foreach (char c in text){
			storyLabel.Text += c;
			await ToSignal(GetTree().CreateTimer(0.04f), "timeout");
		}
	}

	private async Task FadeOutText(){
		var tween = CreateTween();
		tween.TweenProperty(storyLabel, "modulate:a", 0.0f, 1.0f);
		await ToSignal(tween, "finished");
	}
}
