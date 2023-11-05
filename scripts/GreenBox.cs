using Godot;
using System;

public partial class GreenBox : RigidBody3D
{
    public void OnInteraction()
    {
        GD.Print("I'm a green box!");
    }
}
