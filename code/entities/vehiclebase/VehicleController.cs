using Sandbox;

namespace SKarts
{
	[Library]
	public class SKartsVehicleController : PawnController
	{
		public override void FrameSimulate()
		{
			base.FrameSimulate();

			Simulate();
		}

		public override void Simulate()
		{
			var player = Pawn as SKartsRacer;
			if ( !player.IsValid() ) return;

			var vEnt = player.Vehicle as SKartsVehicleBase;
			if ( !vEnt.IsValid() ) return;

			vEnt.Simulate( Client );

			if ( player.Vehicle == null )
			{
				Position = vEnt.Position + vEnt.Rotation.Up * 100 * vEnt.Scale;
				Velocity += vEnt.Rotation.Right * 200 * vEnt.Scale;
				return;
			}

			EyeRot = Input.Rotation;
			EyePosLocal = Vector3.Up * (64 - 10) * vEnt.Scale;
			Velocity = vEnt.Velocity;

			SetTag("noclip");
			SetTag("sitting");
		}
	}
}
