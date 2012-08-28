namespace UrchiNet

module Helpers =

    open System
    open System.Globalization

    module Choice =
        let get =
            function
            | Choice1Of2 ok -> ok
            | Choice2Of2 error -> failwith error

    type Int32 with
        static member parseWithOptions style provider x =
            match Int32.TryParse(x, style, provider) with
            | true,v -> Some v
            | _ -> None
            
        static member parse x = 
            Int32.parseWithOptions NumberStyles.Integer CultureInfo.InvariantCulture x

    type Int64 with
        static member parseWithOptions style provider x =
            match Int64.TryParse(x, style, provider) with
            | true,v -> Some v
            | _ -> None
            
        static member parse x = 
            Int64.parseWithOptions NumberStyles.Integer CultureInfo.InvariantCulture x
