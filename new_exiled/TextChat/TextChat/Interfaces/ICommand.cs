namespace MultiPlugin22.Interfaces
{
	public interface ICommand
	{
		(string response, string color) OnCall(ReferenceHub sender, string[] args);

		string Usage { get; }

		string Description { get; }
	}
}
