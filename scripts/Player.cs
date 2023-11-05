using Godot;
using System;

public partial class Player : CharacterBody3D
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
		
		this.head = GetNode<Node3D>("head");
		this.camera = GetNode<Camera3D>("head/camera");
		this.interactionRay = GetNode<RayCast3D>("head/camera/interaction");

		head.Rotation = new Vector3(0, Rotation.Y, 0);
		Rotation = Vector3.Zero;
		var mesh = GetNode<MeshInstance3D>("pill");
		mesh.Visible = false;

	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// Rotate head and camera with mouse
		if (@event is InputEventMouseMotion mouseMotion)
		{
			var yRotation = -mouseMotion.Relative.X * Sensitivity;
			var xRotation = -mouseMotion.Relative.Y * Sensitivity;
			head.RotateY(yRotation);
			camera.RotateX(xRotation);
			camera.Rotation = new Vector3(Mathf.Clamp(camera.Rotation.X, Mathf.DegToRad(-60), Mathf.DegToRad(60)), camera.Rotation.Y, camera.Rotation.Z);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (Input.IsActionJustPressed("interact"))
		{
			var other = interactionRay.GetCollider();
			if (other != null)
			{
                OnInteraction(other);
			}
		}

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		// if (Input.IsActionJustPressed("jump") && IsOnFloor())
		//		velocity.Y = JumpVelocity;

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

	private static void OnInteraction(GodotObject other)
	{
		if (other.HasMethod("OnInteraction"))
		{
            other.Call("OnInteraction");
        }
	}
}
