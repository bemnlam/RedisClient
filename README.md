# RedisElectron

A .NET Core Redis desktop client.

## Up and run

You need `dotnet`, `node`, `npm`.

### Tools required

```zsh
❯ dotnet --version
3.1.101

❯ node --version
v10.19.0

❯ npm --version
6.13.4
```

### ElectronNet.CLI

Install `ElectronNet.CLI`:

```zsh
dotnet tool install ElectronNet.CLI -g
```

```zsh
❯ dotnet tool list -g
Package Id           Version      Commands
---------------------------------------------
electronnet.cli      7.30.2       electronize

❯ electronize version
ElectronNET.CLI Version: 7.30.2.0
```

You may also need to add PATH in `~/.zshrc` or `~/.bash_profile` (depends on which shell you are using) if you got the `command not found` error.

```
# e.g. in ~/.zshrc

# .NET
# For tools installed with `dotnet tool` command
export PATH="$HOME/.dotnet/tools:$PATH"%
```

### Run

```zsh
cd src/
dotnet restore
dotnet build
electronize start
```

you can also run the app in a browser:

```
cd src/
dotnet run
```

