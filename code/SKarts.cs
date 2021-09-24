using Sandbox;

namespace SKarts
{
	[Library("skarts", Title = "sKarts")]
	partial class SKartsGame : Game
	{
		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );

			var player = new SKartsRacer( cl );
			cl.Pawn = player;

			player.Respawn();
		}

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			base.ClientDisconnect( cl, reason );

			if ( cl.Pawn is SKartsRacer player && player.Vehicle != null )
			{
				player.Vehicle.Delete();
			}
		}
	}
}
