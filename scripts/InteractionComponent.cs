using Godot;
using System;

public partial class InteractionComponent : Area3D
{
	public void OnInteraction()
	{
		var parent = GetParent();
		if (parent.HasMethod("OnInteraction"))
		{
			parent.Call("OnInteraction");
		}
	}
}
