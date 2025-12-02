namespace FSGlue

type FSharpSignalAttribute() =
    inherit System.Attribute()

module initialization =

    let private types = System.Collections.Generic.Dictionary()

    let registerType<'a, 'b when 'a: (new: unit -> 'a) and 'b: (new: unit -> 'b)> () =
        let a = typeof<'a>.FullName
        types.Add(a, fun () -> new 'b () :> obj)

    let createInstance<'a> () =
        let a = typeof<'a>.FullName
        let f = types.[a]
        f () :?> 'a
