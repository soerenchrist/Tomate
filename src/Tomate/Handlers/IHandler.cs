namespace Tomate.Handlers;

public interface IHandler<TArgs>
{
    int Handle(TArgs args); 
}