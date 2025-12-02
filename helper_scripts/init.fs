namespace FSGlue

/// <summary>
/// This attribute is placed on an abstract method, it signals the glue generator that a signal should be created that this method emits
/// </summary>
type FSharpSignalAttribute() =
    inherit System.Attribute()

module initialization =

    let private types = System.Collections.Generic.Dictionary()

    /// <summary>
    /// This function is called by the glue generator to register a type
    ///
    /// It should _not_ be called by you unless you know what you are doing.
    /// </summary>
    let registerType<'a, 'b when 'a: (new: unit -> 'a) and 'b: (new: unit -> 'b)> () =
        let a = typeof<'a>.FullName
        types.Add(a, fun () -> new 'b () :> obj)

    /// <summary>
    /// Returns a function that creates an instance of the specified type, if it is registered.
    ///
    /// Useful if the same type has to be created multiple times or the constructor has to be passed around as a function
    /// </summary>
    let getConstructor<'a when 'a: (new: unit -> 'a)> () =
        let a = typeof<'a>.FullName
        let f = types.[a]
        (fun () -> f () :?> 'a)

    /// <summary>
    /// Creates an instance of the specified type, if it is registered.
    ///
    /// Same as calling `getConstructor<'a> () ()
    /// </summary>
    let createInstance<'a when 'a: (new: unit -> 'a)> () = getConstructor<'a> () ()
