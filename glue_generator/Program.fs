// For more information see https://aka.ms/fsharp-console-apps

[<EntryPoint>]
let main args =
    let parsedArgs = argParser.parse args

    let files = projectExplorer.findFiles parsedArgs.projectRoot

    let assemblyPath =
        System.IO.Path.Combine [| parsedArgs.projectRoot; ".godot/mono/temp/bin/Debug"; parsedArgs.dllName |]

    let x = System.Reflection.Assembly.LoadFrom assemblyPath
    let types = x.ExportedTypes

    types
    |> Seq.filter csharpGenerator.isTypeValidForTranslation
    |> Seq.map csharpGenerator.fromTyToClass
    |> writer.writeFiles parsedArgs files
    |> writer.cleanupUnusedFiles files
    |> Seq.map (csharpGenerator.createTypeRegisterCalls parsedArgs.nameSpace)
    |> csharpGenerator.createTypeRegisterClass parsedArgs.nameSpace
    |> writer.writeTypeRegister parsedArgs

    0
