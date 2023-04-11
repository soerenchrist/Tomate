using Tomate.Services.Abstractions;

namespace Tomate.Services;

public class Output : IOutput
{
    public void Write(string message)
    {
        Console.Write(message);
        
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public void WriteError(string message)
    {
        Console.Error.Write(message);
    }

    public void WriteErrorLine(string message)
    {
        Console.Error.WriteLine(message);
    }
}