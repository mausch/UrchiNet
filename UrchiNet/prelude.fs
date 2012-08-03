namespace UrchiNet

module Helpers =

    open System
    open System.Globalization

    type Int32 with
        static member parseWithOptions style provider x =
            match Int32.TryParse(x, style, provider) with
            | true,v -> Some v
            | _ -> None
            
        static member parse x = 
            Int32.parseWithOptions NumberStyles.Integer CultureInfo.InvariantCulture x

    module Option =
        let fromNull =
            function 
            | null -> None
            | x -> Some x

