using Godot;
using System;

public partial class LevelExit : Area2D
{
	private LevelManager levelManager;

    public override void _Ready()
    {
        levelManager = GetNode<LevelManager>("/root/LevelManager");
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            levelManager.GoToNextLevel();
        }
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
        base._ExitTree();
    }
}
