using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public bool CanJump = false;

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

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		HandleInteraction();
        if (CanJump) velocity = Jump(velocity);
        velocity = AddGravity(velocity, delta);
		velocity = HandleInput(velocity);
		
		Velocity = velocity;
		MoveAndSlide();
	}

    /****************************************************
     * Handles camera rotation given a mouse motion event
	 */
    public override void _UnhandledInput(InputEvent @event)
    {
        // Rotate head and camera with mouse
        if (@event is InputEventMouseMotion mouseMotion)
        {
            var yRotation = -mouseMotion.Relative.X * Sensitivity;
            var xRotation = -mouseMotion.Relative.Y * Sensitivity;

            head.RotateY(yRotation);
            camera.RotateX(xRotation);
            camera.Rotation = ClampCameraY(-80, 80);
        }
    }

    /****************************************************
     * Handles interact action calling OnInteract if
     * looking at an interactable object.
	 */
    private void HandleInteraction()
	{
        if (Input.IsActionJustPressed("interact"))
        {
            var other = interactionRay.GetCollider();
            if (other != null)
            {
                OnInteract(other);
            }
        }
    }

	/****************************************************
	 * Handles user interaction with the players raycast
	 * node. It calls the OnInteraction method of the
	 * interaction component on the object pointed at by
	 * the ray. Return void.
	 */
    private static void OnInteract(GodotObject other)
	{
		if (other.HasMethod("OnInteraction"))
		{
            other.Call("OnInteraction");
        }
	}

    /****************************************************
	 *	Recieves velocity: Vector3
	 *	Returns velocity modified by JumpVelocity
	 */
    private Vector3 Jump(Vector3 velocity)
	{
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
            velocity.Y = JumpVelocity;
        }
		return velocity;
    }

    /****************************************************
	 *	Recieves velocity: Vector3 and delta: double
	 *	Returns velocity after applying gravity
	 */
    private Vector3 AddGravity(Vector3 velocity, double delta)
	{
        if (!IsOnFloor())
        {
			if (Input.IsActionJustReleased("jump"))
			{
				velocity.Y *= 0.5f;
			}

            if (velocity.Y > 0)
            {
                velocity.Y -= gravity * (float)delta;
            }
            else
            {
                velocity.Y -= gravity * (float)delta * 2;
            }
        }
		return velocity;
    }

    /****************************************************
	 *	Recieves min: int, max:int in degrees
	 *	Returns Vector3 with clamped values for the camera
	 */
    private Vector3 ClampCameraY(int min, int max)
	{
		return new Vector3(
			Mathf.Clamp(camera.Rotation.X, Mathf.DegToRad(min), Mathf.DegToRad(max)), 
			camera.Rotation.Y, 
			camera.Rotation.Z
			);
    }

	private Vector3 HandleInput(Vector3 velocity)
	{
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
		return velocity;
    }
}
