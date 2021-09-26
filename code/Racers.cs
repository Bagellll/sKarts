using Sandbox;

namespace SKarts
{
	partial class SKartsRacer : Player
	{
		public bool IsUseDisabled()
		{
			return this == null;
		}

		[Net] public PawnController VehicleController { get; set; } = null;
		[Net] public PawnAnimator VehicleAnimator { get; set; } = null;
		[Net, Predicted] public ICamera VehicleCamera { get; set; } = null;
		[Net, Predicted] public Entity Vehicle { get; set; } = null;
		[Net, Predicted] public ICamera MainCamera { get; set; } = null;

		public ICamera LastCamera { get; set; }

		public Clothing.Container Clothing = new();

		public SKartsRacer()
		{
			Inventory = new Inventory( this );
		}

		public SKartsRacer( Client cl ) : this()
		{
			Clothing.LoadFromClient( cl );
		}

		public override void Spawn()
		{
			MainCamera = new PlayerCamera();
			LastCamera = MainCamera;

			base.Spawn();
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			Controller = new WalkController();
			Animator = new StandardPlayerAnimator();

			MainCamera = LastCamera;
			Camera = MainCamera;

			if ( DevController is NoclipController )
			{
				DevController = null;
			}

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			Clothing.DressEntity( this );

			base.Respawn();

			SKartsVehicleBase vEnt;

			if ( Vehicle == null )
			{
				vEnt = Library.Create<SKartsVehicleBase>( "ent_vehiclebase" );
			}
			else
			{
				vEnt = Vehicle as SKartsVehicleBase;
			}

			vEnt.Position = Position + Vector3.Up * 32;
			vEnt.Rotation = Rotation.From( new Angles( 0, this.EyeRot.Angles().yaw, 0 ) );

			vEnt.SetDriver(this);
		}

		public override PawnController GetActiveController()
		{
			if ( VehicleController != null ) return VehicleController;

			return base.GetActiveController();
		}

		public ICamera GetActiveCamera()
		{
			if ( VehicleCamera != null ) return VehicleCamera;

			return MainCamera;
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			Camera = GetActiveCamera();
		}
	}
}
