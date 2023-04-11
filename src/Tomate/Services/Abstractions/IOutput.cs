namespace Tomate.Services.Abstractions;

public interface IOutput
{
    void Write(string message);
    void WriteLine(string message);
    void WriteError(string message);
    void WriteErrorLine(string message); 
}