using Sandbox;

namespace SKarts
{
	public partial class SKartsGame : Game
	{
		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );

			var player = new Racer( cl );
			cl.Pawn = player;

			player.Respawn();
		}
	}
}
