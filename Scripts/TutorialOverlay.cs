using Godot;
using System;
using System.Collections.Generic;

public partial class TutorialOverlay : CanvasLayer
{
	 private Panel highlightPanel;
    private Label tutorialText;
    private Button nextButton;
    private int currentStep = 0;
    public static bool IsTutorialActive { get; private set; } = false;
    
    private readonly (string node, string text)[] tutorialSteps = new[]
    {
        ("PlayerHud/MarginContainer/HBoxContainer", "Health meter - You start with 5 hearts. Losing all hearts means game over. Try not to fall of the map."),
        ("PlayerHud/MarginContainer2/HBoxContainer", "Weapon inventory - Currently equipped with a dagger. Find more weapons to transform into as you explore. Each weapon has unique mechanics")
    };

    public override void _Ready()
    {
        highlightPanel = GetNode<Panel>("HighlightPanel");
        tutorialText = GetNode<Label>("TutorialText");
        nextButton = GetNode<Button>("NextButton");
        
        nextButton.Pressed += OnNextPressed;
        ShowCurrentStep();

        tutorialText.CustomMinimumSize = new Vector2(300, 0);
        tutorialText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
    
        nextButton.AnchorLeft = 0.5f;
        nextButton.AnchorRight = 0.5f;
        nextButton.AnchorTop = 0.8f;
        nextButton.GrowHorizontal = Control.GrowDirection.Both;
        
        IsTutorialActive = true;

        SetupTextLabel();
    }

    private void SetupTextLabel()
    {
        var style = new StyleBoxFlat();
        style.BgColor = new Color(0, 0, 0, 0.7f);
        style.BorderColor = Color.FromHtml("#FFD700");
        style.SetBorderWidthAll(2);
        style.SetCornerRadiusAll(5);
        style.ContentMarginLeft = 10;
        style.ContentMarginRight = 10;
        style.ContentMarginTop = 5;
        style.ContentMarginBottom = 5;

        tutorialText.AddThemeStyleboxOverride("normal", style);
        tutorialText.AnchorLeft = 0.1f;
        tutorialText.AnchorRight = 0.9f;
        tutorialText.AnchorTop = 0.2f;
        tutorialText.GrowHorizontal = Control.GrowDirection.Both;
        tutorialText.HorizontalAlignment = HorizontalAlignment.Center;
        tutorialText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
    }

    private void ShowCurrentStep()
    {
        if (currentStep >= tutorialSteps.Length)
        {
            QueueFree();
            return;
        }

        if (currentStep == 0)
        {
            highlightPanel.Position = new Vector2(10, 5);
            highlightPanel.Size = new Vector2(330, 45);
        }
        else
        {
            highlightPanel.Position = new Vector2(15, 46);
            highlightPanel.Size = new Vector2(50, 50);
        }
        tutorialText.Text = tutorialSteps[currentStep].text;
    }

    private void OnNextPressed()
    {
        currentStep++;
        if (currentStep >= tutorialSteps.Length)
        {
            IsTutorialActive = false;
        }
        ShowCurrentStep();
    }
}
