using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public const float Sensitivity = 0.005f;
    public float gravity = 9.8f;
	public Node3D head;
	public Camera3D camera;
	public RayCast3D interactionRay;

    public override void _Ready()
    {
		Input.MouseMode = Input.MouseModeEnum.Captured;
		this.head = GetNode<Node3D>("Head");
		this.camera = GetNode<Camera3D>("Head/Camera3D");
		this.interactionRay = GetNode<RayCast3D>("interaction");
	}

	public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
		{
			var yRotation = -mouseMotion.Relative.X * Sensitivity;
			var xRotation = -mouseMotion.Relative.Y * Sensitivity;
            head.RotateY(yRotation);
            camera.RotateX(xRotation);
			camera.Rotation = new Vector3(Mathf.Clamp(camera.Rotation.X, Mathf.DegToRad(-60), Mathf.DegToRad(60)), camera.Rotation.Y, camera.Rotation.Z);
			interactionRay.Rotation = new Vector3(camera.Rotation.X, head.Rotation.Y, 0);
        }
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("strafe_left", "strafe_right", "forward", "backward");
		Vector3 direction = (head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

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
