# Introduction

This section describes the physical and logical organization of the `hashfields` source code.

## .NET Core

[.NET Core][dotnet] is Microsoft's modular, open source, cross-platform development framework for building many kinds of
software systems and applications.

.NET Core apps are often written in the [C# programming language][csharp], a strongly and statically-typed, compiled
programming language with a heavy emphasis on Object Oriented principles. .NET Core programs can be compiled and deployed into
a single, zero-dependency executable for Windows, macOS, and Linux.

[NuGet](https://www.nuget.org/) is the package manager for .NET. NuGet is both an online, public directory of packages and a
set of interfaces for interacting with the online directory (via e.g. package files in a project or command line tooling).
NuGet is thus similar to `npm` in the Node.js ecosystem and `pip`/PyPI in the Python ecosystem.

## Source code structure

.NET Core apps are organized into a number of physical and logical constructs that allow for code reuse and a variety of
packaging and runtime scenarios.

The following description follows an "inside-out" perspective, where lower-level concepts are mentioned before their
higher-level containers.

### Namespaces

A *namespace* logically organizes a set of common functionality in C# code, for example a set of related classes. A namespace
may be defined across several physical source code files, each contributing parts of the complete set. Most code (e.g. classes)
*cannot* be defined "outside" of a namespace.

#### Namespace syntax

Inside a code file, a namespace is explicitly declared with a keyword, using curly braces to denote the *scope*, usually as one
of the very first lines of code:

```csharp
namespace HashFields
{
    // HashFields code goes here
}
```

Namespaces are hierarchical, using dot-notation left to right to denote top to bottom:

```csharp
namespace Parent.Child.Grandchild
{
    // Grandchild code goes here
}
```

Or

```csharp
namespace HashFields.Data.Csv
{
    // Csv code goes here
}
```

Note the namespace hierarchy _does not_ have to follow the physical directory structure (e.g. folders and subfolders) that
organizes the individual code files.

#### Using namespaces

To make the types defined in a source namespace available to code in another namespace, the source namespace can be "imported"
with a `using` statement, similar to Python's `import` statement or Node's `require` statement:

```csharp
using Parent.Child;
```

Or

```csharp
using HashFields.Data.Csv;
```

Note that any level of the hierarchy may be imported and used.

Conventionally, `using` statements are the one exception and are most often written "outside" the namespace that needs them:

```csharp
using SomeNamespace;

namespace AnotherNamespace
{
    // code from SomeNamespace is available here
}
```

#### Built-in .NET namespaces

.NET/C# comes with a number of built-in namespaces defining the base types and functionality needed to build new apps. `System`
is the top-level namespace under which most of the built-ins are organized.

Some examples used in the `hashfields` project include:

| Namespace                                | Description                                                                                                |
| ---------------------------------------- | ---------------------------------------------------------------------------------------------------------- |
| [`System`][system]                       | Core data types like `int`, `string`, and `DateTime`                                                       |
| [`System.IO`][io]                        | Reading and writing files and data streams                                                                 |
| [`System.Linq`][linq]                    | Language-Integrated Query is a SQL-like interface to nearly any source (e.g. database, file, in-memory)    |
| [`System.Security.Cryptography`][crypto] | Hashing and other cryptographic services                                                                   |
| [`System.Text`][text]                    | Text encoding/decoding, bytes <--> characters conversions, formatting                                      |

### Code files

A C# code file, denoted by the `.cs` extension, contains the `using` statements and `namespace` declaration, plus any class
definitions and other code for a given component of an app.

Remember that a single namespace can be defined over many files. It is common to define a single class per file, with multiple
classes (files) making up a single namespace.

### Projects

.NET Projects organize a common set of files into a single compilation target. The projects in `hashfields` have the `.csproj`
extension and they list key attributes for build-time, like:

* The version of .NET to compile for (the "Target Framework")
* The output type (e.g. reusable "class library", executable)
* References to other projects (e.g. to reuse a "class library")
* References to external packages from NuGet

Individual projects can be built using the `dotnet` command line tool:

```bash
dotnet build /path/to/ProjectName.csproj
```

The two main projects in `hashfields` are documented on their own pages:

* [`Cli`](./cli.md)
* [`Data`](./data.md)

### Solution

The *Solution* or *Solution file*, with an `.sln` extension, is the top-level organizational structure in .NET. A solution is
a collection of one or more projects plus the supported build architectures (e.g. x86, x64) and modes (e.g. Debug, Release).

Using the *Solution Explorer* tool in Visual Studio and VS Code (included with the `hashfields` devcontainer) gives the
developer a focused view of just the solution content apart from the rest of the repository.

The entire solution (all projects in the solution) can be built using the `dotnet` command line tool within a directory
containing a solution file:

```bash
dotnet build
```

Or by passing the solution file path directly:

```bash
dotnet build /path/to/SolutionName.sln
```

[csharp]: https://docs.microsoft.com/en-us/dotnet/csharp/
[crypto]: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography?view=net-5.0
[dotnet]: https://docs.microsoft.com/en-us/dotnet/core/introduction
[io]: https://docs.microsoft.com/en-us/dotnet/api/system.io?view=net-5.0
[linq]: https://docs.microsoft.com/en-us/dotnet/api/system.linq?view=net-5.0
[system]: https://docs.microsoft.com/en-us/dotnet/api/system?view=net-5.0
[text]: https://docs.microsoft.com/en-us/dotnet/api/system.text?view=net-5.0
