# Robot factory management system

This project is an application written in C# designed to manage a robot factory.
It provides commands to assemble new robots

# Prerequisites
- [dotnet  version 8.0.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed (chose the 'SDK')
- git installed


_ps: if you have a more recent version of .NET that you want to keep you don't have to uninstall it._

# How to use this project

1) clone the repository
```shell
git clone https://github.com/ethxn-frs/RobotFactory.git
```

2) Open the cloned project on your favorite IDE


3) Run the entrypoint ``Program.cs``
```shell
cd .\RobotFactory\
dotnet run
```

# Commands

### Syntax of a command
```
[USER_INSTRUCTION] ARGS
```

Where ``ARGS`` can have the following form

- we want ``<nb1>`` robots named ``Robot1`` and ``<nb2>`` robots named ``Robot2``
```
<nb1> Robot1 [, <nb2> Robot2, ...]
```



### All commands

- List inventory
```
> STOCKS
```

- Get the list of pieces needed to build (a) robot(s)
```
> NEEDED_STOCKS ARGS
```

- (NOT IMPLEMENTED) Get the list of instructions needed to build (a) robot(s)
```
> INSTRUCTIONS ARGS
```

- (IMPLEMENTATION TO FIX) Verify the validity of an order
```
> VERIFY ARGS
```

- (IMPLEMENTATION TO FIX) Produce (a) robot(s)
```
> PRODUCE ARGS
```