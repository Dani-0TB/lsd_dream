using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public const float Sensitivity = 0.005f;
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = 9.8f;
	

	public Node3D head;
	public Camera3D camera;

    public override void _Ready()
    {
		Input.MouseMode = Input.MouseModeEnum.Captured;
		this.head = GetNode<Node3D>("Head");
		this.camera = GetNode<Camera3D>("Head/Camera3D");

	}

	public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
		{
            head.RotateY(-mouseMotion.Relative.X * Sensitivity);
            camera.RotateX(-mouseMotion.Relative.Y * Sensitivity);
        }
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		// if (Input.IsActionJustPressed("jump") && IsOnFloor())
		// 	velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("strafe_left", "strafe_right", "forward", "backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
