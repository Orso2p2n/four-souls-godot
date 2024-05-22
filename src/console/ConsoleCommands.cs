using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public partial class Argument : Resource {
    public string name;
    public Variant.Type type;
    public bool optional;
}

public partial class Command : Resource {
    public string name;
    public Callable callable;
    public Array<Argument> arguments;
    public bool allArgsAsOne;
}

public partial class ConsoleCommands : Node
{
    private Console _console;

    public Array<Command> Commands { get; set; } = new();

    public ConsoleCommands(Console console) {
        _console = console;

        InitCommands();
    }

    private void AddCommand(string name, Delegate method, bool allArgsAsOne = false) {
        var methodName = method.Method.Name;

        var allMethods = GetMethodList();
        Dictionary foundMethod = null;

        foreach (var gdMethod in allMethods) {
            var gdMethodName = (string) gdMethod["name"];

            if (gdMethodName == methodName) {
                foundMethod = gdMethod;
                break;
            }
        }

        if (foundMethod == null) {
            Console.LogError($"Method \"{methodName}\" not found, could not create command.");
            return;
        }

        Array<Argument> arguments = new();

        var args = (Array<Dictionary>) foundMethod["args"];
        var parameters = method.Method.GetParameters();
        var i = 0;
        foreach (var arg in args) {
            var argName = (string) arg["name"];
            var argType = (Variant.Type) (int) arg["type"];
            var argOptional = parameters[i].IsOptional;

            var argument = new Argument() { name = argName, type = argType, optional = argOptional};
            arguments.Add(argument);

            i++;
        }
        
        var callable = new Callable(this, methodName);
        var command = new Command { name = name, callable = callable, arguments = arguments, allArgsAsOne = allArgsAsOne};
        Commands.Add(command);
    }

    public Command GetCommand(string commandName) {
        foreach (var command in Commands) {
            if (commandName == command.name) {
                return command;
            }
        }

        return null;
    }

    public void CallCommand(Command command, Array<string> passedArguments) {
        var argCount = command.arguments.Count;
        var passedArgCount = passedArguments.Count;

        if (passedArgCount < argCount) {
            Console.LogError($"Missing argument for command \"{command.name}\".");
            return;
        }

        Variant[] typedArgs = new Variant[argCount];

        for (int i = 0; i < argCount; i++) {
            var arg = (Argument) command.arguments[i];
            var passedArg = (string) passedArguments[i];

            var typedArg = GD.Convert(passedArg, arg.type);
            typedArgs[i] = typedArg;
        }

        try {
            command.callable.Call(typedArgs);
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
        AddCommand("chat", Chat, true);
    }

    // --- Commands ---
    
    public void ToggleFPS() {
        Console.LogInfo("Toggled FPS Counter.");
        FpsCounter.Toggle();
    }

    public void Print(string text) {
        Console.LogInfo(text);
    }

    public void Chat(string text) {
        Console.ME.LogRpc(text);
    }
}
