# Tomate

Simple pomodoro timer for the command line. It is built using .NET 7.

## How to install

The tool can be installed as a dotnet tool:

```
dotnet tool install -g tomate
```

The tool does currently only support notifications for Linux systems. As an alterantive on Windows, you can download the application from the `Releases` section.

## How to use

```
start          Start a new pomodoro session

config         Configure the pomodoro timer

list-config    Show the configuration

help           Display more information on a specific command.

version        Display version information.
```

For example to start a new pomodoro session with your configured default settings simply call
```
tomate start
```
You also have the option to pass the parameters directly to the `start` command:

```
-f, --focus-minutes          Set the length of your focus time in
                               minutes

-s, --short-break-minutes    Set the length of your short break in
                               minutes

-l, --long-break-minutes     Set the length of your long break in
                               minutes

-i, --long-break-interval    Set the number of focus sessions
                               before a long break

-c, --cycles                 Set the amount of cycles to run
```

### Contribute / Build it yourself
Clone the repository and use the NUKE pipeline to compile or pack the application:

```
nuke pack
```
