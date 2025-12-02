# Fsharp Godot

This project makes it easier/possible to work with F# in Godot. It reuses the existing C# bindings to work.

## Warning: Proof of concept

This project is still a proof of concept, many things are still missing, bugs are bound to crop up.

## How it works

Like the old existing F# plugin for Godot 3, this new method relies C# files/classes with the same name as the F# one.

Unlike the old one however, having this by itself is not enough. As the new bindings use source generators to tell Godot what properties and methods a class has.

These source generators do not work with F# classes, nor will they look up higher in the class hierarchy for methods/properties.

As such, this new method not only creates C# classes with the same name extending the F# ones. It _also_ creates proxy methods,properties and even signals on said class and automatically regenerates them every time you compile.

It is admittedly a bit hacky but... so was the existing plugin and, if it works then why break it?

## Example of an F# node:

```fs
open Godot;
open FSGlue;

//nodes are entirely handled by the C# layer now. As such, they must be marked as Abstract.
[<AbstractClass>]
type public Test() =
    inherit Node2D()

    //export properties work just as you are used to
    [<Export(PropertyHint.Flags, "Self:4,Allies:8,Foes:16")>]
    member val public SpellTargets = 0 with get, set

    //This will create a signal named EmitSignal, taking 2 string arguments
    [<FSharpSignal>]
    abstract member EmitMySignal: arg1: string * arg2: string -> unit

    //"special" methods work just as normal
    override this._Input(ev: InputEvent) : unit =
        //technically not needed in this case but always a good one to have.
        base._Input (ev: InputEvent)

        if ev.IsActionPressed "ui_accept" then
            //emits our custom signal.
            this.EmitMySignal("Custom Signal!", this.Text)
```

As your nodes are Abstract classes, you can not just initialize them, you _have_ to initialize the C# class.

To make this easier, the generated glue comes with a workaround. While it is far from the most elegant solution, it is still quite easy. Just do:

```fs
open FSGlue.initialization

let myNode = createInstance<Test> ()
```

The generated glue should have registered the corresponding C# class and initialize it for you.

## How to get it working.

First: Head over to the releases (to be done) and download the zip file. Then, extract it into the "plugins" folder in your godot project. As such, the file directory should look like this:

```
/
    plugins/
        fsglue/
            init.fs
            glue_generator/

    {yourProject}.csproj
```

Next, copy the below fsproj file and place it next to your csproj file.

Yes _next_ to the csproj file

```xml
<Project>
  <PropertyGroup>
    <BaseOutputPath>.godot\mono\temp\fsharp\bin\</BaseOutputPath>
    <BaseIntermediateOutputPath>.godot\mono\temp\fsharp\obj\</BaseIntermediateOutputPath>
    <RestorePackagesPath>.godot\mono\temp\fsharp\packages</RestorePackagesPath>
  </PropertyGroup>
  <Target Name="FSToGodotGlueGeneration" AfterTargets="PostBuildEvent">
    <Exec Command="dotnet ./plugins/fsglue/glue_generator/generate_glue ./ &quot;{fsProjectName}.dll&quot; {NamespaceCSharpCode}" />
  </Target>
  <Import Project="Sdk.props" Sdk="Godot.NET.Sdk" Version="{GodotVersion}" />
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <EnableDynamicLoading>true</EnableDynamicLoading>
  </PropertyGroup>
  <Import Project="Sdk.targets" Sdk="Godot.NET.Sdk" Version="{GodotVersion}" />
  <ItemGroup>
    <!-- make sure that this is the _first_ file to be included -->
    <Compile Include="plugins/fsglue/init.fs" />
  </ItemGroup>
</Project>
```

Then, replace the following things:
`{fsProjectName}`: the name of your F# project.

`{NamespaceCSharpCode}`: the namespace that the generated C# classes should fall under.

`{GodotVersion}`: The version of godotsharp you are using. You can look at the .csproj file if you are unsure

Then, head over to the csproj file and add the F# project as a dependency. The below can be copied to do this (don't forget to change `{fsProjectName}`)

```xml
<ItemGroup>
    <ProjectReference Include="{fsProjectName}.fsproj" />
</ItemGroup>
```

So, in the end, your folder structure should look like this:

```
/
    plugins/
        fsglue/
            init.fs
            glue_generator/

    {yourProject}.csproj
    {fsProjectName}.fsproj
```

Now, simply build and if everything went well, there shouldn't be any errors.
