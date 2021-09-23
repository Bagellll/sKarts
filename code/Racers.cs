using Sandbox;

namespace SKarts
{
	partial class Racer : Player
	{
		[Net] public PawnController VehicleController { get; set; }
		[Net] public PawnAnimator VehicleAnimator { get; set; }
		[Net, Predicted] public ICamera VehicleCamera { get; set; }
		[Net, Predicted] public Entity Vehicle { get; set; }
		[Net, Predicted] public ICamera MainCamera { get; set; }

		public ICamera LastCamera { get; set; }

		public Clothing.Container Clothing = new();

		public Racer()
		{
			Inventory = new Inventory( this );
		}

		public Racer( Client cl ) : this()
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
		}
	}
}
