using Tomate;

var handler = new CliArgumentHandler();

var result = await handler.Handle(args);

return result;