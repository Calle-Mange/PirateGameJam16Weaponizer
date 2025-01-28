using Godot;
using System;

public partial class CutAnimation : AnimatedSprite2D
{
	[Signal]
    public delegate void CutAnimationFinishedEventHandler();
    
    public override void _Ready()
    {
		Visible = false;
        AnimationLooped += OnAnimationFinished;
        AnimationFinished += OnAnimationFinished;
    }

    public void PlayCutAnimation()
    {
        Visible = true;
        Play("cut");
    }

    private void OnAnimationFinished()
    {
        Visible = false;
        EmitSignal(SignalName.CutAnimationFinished);
    }

    public override void _ExitTree()
    {
        AnimationLooped -= OnAnimationFinished;
        AnimationFinished -= OnAnimationFinished;
        base._ExitTree();
    }
}
