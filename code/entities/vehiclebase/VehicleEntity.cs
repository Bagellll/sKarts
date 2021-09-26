using Sandbox;

namespace SKarts
{
	[Library( "ent_vehiclebase", Title = "Base Vehicle", Spawnable = true )]
	public partial class SKartsVehicleBase : ModelEntity
	{
		private struct InputState
		{
			public float throttle;
			public float turning;
			public float braking;
			public float tilt;
			public float roll;

			public void Reset()
			{
				throttle = 0;
				turning = 0;
				braking = 0;
				tilt = 0;
				roll = 0;
			}
		}

		private InputState currentInput;

		[Net] public Player Driver { get; private set; }

		public override void Spawn()
		{
			base.Spawn();

			MoveType = MoveType.Physics;
			CollisionGroup = CollisionGroup.Interactive;
			PhysicsEnabled = true;
			UsePhysicsCollision = true;
			
			SetModel( "models/citizen_props/crate01.vmdl" );
		}

		public void ResetInput()
		{
			currentInput.Reset();
		}

		public override void Simulate( Client cl )
		{
			if ( cl == null ) return;
			if ( !IsServer ) return;

			using ( Prediction.Off() )
			{
				currentInput.Reset();

				currentInput.throttle = ( Input.Down( InputButton.Forward ) ? 1 : 0 ) + ( Input.Down( InputButton.Back ) ? -1 : 0);
				currentInput.turning = ( Input.Down( InputButton.Left ) ? 1 : 0 ) + ( Input.Down( InputButton.Right ) ? -1 : 0);
				currentInput.braking = Input.Down( InputButton.Jump ) ? 1 : 0;
				currentInput.tilt = ( Input.Down( InputButton.Run ) ? 1 : 0 ) + ( Input.Down( InputButton.Duck ) ? -1 : 0);
				currentInput.roll = ( Input.Down( InputButton.Left ) ? 1 : 0 ) + ( Input.Down( InputButton.Right ) ? -1 : 0);
			}
		}

		[Event.Physics.PreStep]
		public void OnPrePhysicsStep()
		{
			if ( !IsServer ) return;

			var selfBody = PhysicsBody;
			if ( !selfBody.IsValid() ) return;

			var body = selfBody.SelfOrParent;
			if ( !body.IsValid() ) return;

			body.Velocity += currentInput.throttle.Clamp( -1, 1 ) * body.Rotation.Forward.Normal * 25;
			body.AngularVelocity += currentInput.turning.Clamp( -1, 1 ) * body.Rotation.Up.Normal * 2;
		}

		public void SetDriver( Player user )
		{
			if ( user is SKartsRacer player && player.Vehicle == null )
			{
				player.Vehicle = this;
				player.VehicleController = new SKartsVehicleController();
				player.VehicleAnimator = player.Animator;
				player.VehicleCamera = new VehicleCamera();
				player.Parent = this;
				player.LocalPosition = PhysicsBody.LocalMassCenter;
				player.LocalRotation = Rotation.Identity;
				player.LocalScale = 1;
				player.PhysicsBody.Enabled = false;

				Driver = player;
			}
		}
	}
}
