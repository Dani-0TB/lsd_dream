using Godot;
using System;

public partial class Box : RigidBody3D
{
	public void OnInteraction()
	{
		GD.Print("I'm a purple box!");
	}
}
