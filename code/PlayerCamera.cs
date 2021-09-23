using Sandbox;

namespace SKarts
{
	public class PlayerCamera : Camera
	{
		public override void Update()
		{
			if ( Local.Pawn is not AnimEntity pawn )
				return;

			Pos = pawn.Position;
			Vector3 targetPos;

			var center = pawn.Position + Vector3.Up * 64;

			Pos = center;
			Rot = Rotation.FromAxis( Vector3.Up, 0 ) * Input.Rotation;

			float distance = 256.0f * pawn.Scale;
			targetPos = Pos + Input.Rotation.Right * pawn.Scale;
			targetPos += Input.Rotation.Forward * -distance;

			var tr = Trace.Ray( Pos, targetPos ).Ignore( pawn ).Radius( 8 ).Run();

			Pos = tr.EndPos;

			//Pos = targetPos;

			FieldOfView = 70;

			Viewer = null;
		}
	}
}
