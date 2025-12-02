# Glue generator

This project generates the C# classes based on the existing F# code.

If everything works as expected this will run automatically at build time.

Because of this, the argument system is very simplistic and no help is available in the program itself. There is _no_ point in wasting time using a fancy system to parse arguments when 99.99% of the time the tool is run fully automatic!

## Arguments

Still, the arguments should be documented. So, here is the order, their names and short description.

1. `projectRoot`: The root of the Godot project. This is _not_ the root of the F# project, regardless if you put it at the root of the Godot project.

2. `dllName`: The name of the dll file that your F# project will compile to. It is used together with `projectRoot` to locate and read it. `GlueGenerator` uses the resulting .dll file to generate the C# classes rather than the raw F# code.

3. `nameSpace`: The namespace that the C# classes will be placed under.

Run it like

```bash
dotnet generate_glue {projectRoot} {dllName} {nameSpace}
```

Example:

```bash
dotnet generate_glue ./ example_project.dll CSharpClasses
```
