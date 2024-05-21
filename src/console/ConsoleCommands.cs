using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public partial class Argument : Resource {
    public string name;
    public Type type;
    public bool optional;
}

public partial class Command : Resource {
    public string name;
    public MethodInfo methodInfo;
    public Array<Argument> arguments;
}

public partial class ConsoleCommands : Node
{
    private Console _console;

    public Array<Command> Commands { get; set; } = new();

    public ConsoleCommands(Console console) {
        _console = console;

        InitCommands();
    }

    private Argument CreateArgument(string name, Type type, bool optional) {
        return new Argument() { name = name, type = type, optional = optional};
    }

    private void AddCommand(string name, Delegate method) {
        var parameters = method.Method.GetParameters();

        Array<Argument> arguments = new();

        foreach (var parameter in parameters) {
            var paramName = parameter.Name;
            var type = parameter.ParameterType;
            var optional = parameter.IsOptional;

            arguments.Add(CreateArgument(paramName, type, optional));
        }
        
        // var callable = new Callable(this, method.Method.Name);
        var command = new Command { name = name, methodInfo = method.Method, arguments = arguments};
        Commands.Add(command);
    }

    public bool TryCallCommand(string commandName, Array<string> arguments) {
        foreach (var command in Commands) {
            if (commandName == command.name) {
                CallCommand(command, arguments);
                return true;
            }
        }
        return false;
    }

    private void CallCommand(Command command, Array<string> passedArguments) {
        var argCount = command.arguments.Count;
        var passedArgCount = passedArguments.Count;

        if (passedArgCount < argCount) {
            Console.LogError($"Missing argument for command \"{command.name}\"");
            return;
        }

        List<Object> typedArgs = new();

        for (int i = 0; i < argCount; i++) {
            Argument arg = command.arguments[i];
            string passedArg = passedArguments[i];

            try {
                var convertedArg = Convert.ChangeType(passedArg, arg.type);                
                typedArgs.Add(convertedArg);
            }
            catch (Exception e) {
                Console.LogError(e.ToString());
                return;
            }
        }

        try {
            command.methodInfo.Invoke(this, typedArgs.ToArray());
        }
        catch (Exception e) {
            Console.LogError(e.ToString());
            return;
        }

        return;
    }

    private void InitCommands() {
        AddCommand("fps", ToggleFPS);
        AddCommand("print", Print);
    }

    // --- Commands ---
    
    public void ToggleFPS() {
        Console.Log("ToggleFPS");
    }

    public void Print(string text) {
        Console.Log(text);
    }
}
