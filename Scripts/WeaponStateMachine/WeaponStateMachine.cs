using Godot;
using System;
using System.Collections.Generic;

public partial class WeaponStateMachine : Node
{
	[Export] public NodePath InitialState;
	private Dictionary<string, State> States;
	private State CurrentState;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		States = new Dictionary<string, State>();

		foreach (Node node in GetChildren())
		{
			if (node is State state)
			{
				States[node.Name] = state;
				state.WeaponStateMachine = this;
				state.StateReady();
				state.Exit();
			}
		}

		CurrentState = GetNode<State>(InitialState);
		CurrentState.Enter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("dagger_transform"))
		{
			TransitionTo("DaggerState");
		}

        if (Input.IsActionJustPressed("axe_transform"))
        {
            TransitionTo("AxeState");
        }

        if (Input.IsActionJustPressed("flail_transform"))
        {
            TransitionTo("FlailState");
        }

        if (Input.IsActionJustPressed("gun_transform"))
        {
            TransitionTo("GunState");
        }

        CurrentState._Process(delta);
		CurrentState._PhysicsProcess(delta);
	}

	public void TransitionTo(string state)
	{
		if (!States.ContainsKey(state) || CurrentState == States[state])
		{
			return;
		}

		CurrentState.Exit();
		CurrentState = States[state];
		CurrentState.Enter();
	}
}
