using Godot;
using System;

public partial class State : Node
{
	public WeaponStateMachine WeaponStateMachine;

	[Signal]
	public delegate void SignalPlayerStateChangeEventHandler(int Speed, int AttackDamage, int Weight, string StateName, SpriteFrames AnimationSet);

	protected WeaponStats WeaponStats;

	protected int Speed;
	protected int AttackDamage;
	protected int Weight;
	protected SpriteFrames AnimationSet;

	public override void _Ready() { }

	public virtual void Enter() 
	{
        EmitSignal(SignalName.SignalPlayerStateChange, Speed, AttackDamage, Weight, this.Name, AnimationSet);
    }

	public virtual void Exit() { }

    public virtual void StateReady() { }

    public virtual void StateProcess(double delta) { }

	public virtual void StatePhysicsProcess(double delta) { }

	public override void _Process(double delta) { }

}
