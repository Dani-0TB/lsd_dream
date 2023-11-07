using Godot;

[Tool]
public partial class Door : Node3D
{
	[Export]
	private PackedScene open;
	[Export]
	private PackedScene close;
	private Node3D mesh;
	public void set_mesh(Node3D value){
		if(IsInstanceValid(mesh)) mesh.QueueFree();
		mesh = value;
		AddChild( value );
	}
	
	public enum State
	{
		Open,
		Close
	}
	private State state_current;
	public void set_state_current(State value){
		//if(state_current == value) return;
		switch(value)
			{
				case State.Open:
					if(!IsInstanceValid(open)){
						GD.PushError("No <open> PackedScene loaded");
						return;
					}
					set_mesh( open.Instantiate<StaticBody3D>() );
					break;
				case State.Close:
					if(!IsInstanceValid(close)){
						GD.PushError("No <close> PackedScene loaded");
						return;
					}
					set_mesh( close.Instantiate<StaticBody3D>() );
					break;
			}
			state_current = value;
	}

	[Export]
	private State state_start{ get; set; } = State.Close;
	
	[Export(PropertyHint.Flags, "open, close")]
	private int actions = 0b11;
	public bool can_open(){ return ((actions>>0)&1)==1; }
	public bool can_close(){return ((actions>>1)&1)==1; }

	public void OnInteraction()
	{
		switch(state_current)
		{
			case State.Open:
				if(can_close())set_state_current(State.Close);
				break;
			case State.Close:
				if(can_open()) set_state_current(State.Open);
				break;
		}
	}
	public override void _Ready()
	{
		set_state_current(state_start);
	}
}
