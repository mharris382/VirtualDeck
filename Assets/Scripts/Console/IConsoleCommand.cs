//TODO: move this to core
public interface IConsoleCommand
{
    bool TryCommand(ref string command);
}