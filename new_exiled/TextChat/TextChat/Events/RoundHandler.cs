using static MultiPlugin22.Database;

namespace MultiPlugin22.Events
{
	public class RoundHandler
	{
		public void OnRoundRestart() => LiteDatabase.Checkpoint();
	}
}
