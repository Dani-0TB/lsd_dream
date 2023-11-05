using Godot;
using System;

public partial class Door : Node3D
{
	[Export]
	private PackedScene open;
	[Export]
	private PackedScene close;
	
	public enum State
	{
		Open,
		Closed
	}
	/*
	private State current_state {}
	[Export]
	private State start_as {
		get { return start_as; }
		set {
			if(value==State.Open){
				
			}
		}
	}
	
	[Export(PropertyHint.Flags, "open, close")]
	private int actions;
	*/
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
