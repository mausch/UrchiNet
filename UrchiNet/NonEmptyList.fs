namespace UrchiNet

type 'a NonEmptyList = {
    Head: 'a
    Tail: 'a list
}

module NonEmptyList =
    [<CompiledName("Create")>]
    let inline create head tail = { Head = head; Tail = tail }

    [<CompiledName("Singleton")>]
    let inline singleton value = create value []

    [<CompiledName("Head")>]
    let inline head (x: _ NonEmptyList) = x.Head

    [<CompiledName("Tail")>]
    let inline tail (x: _ NonEmptyList) = x.Tail

    [<CompiledName("ToList")>]
    let inline toList (x: _ NonEmptyList) = x.Head :: x.Tail

    [<CompiledName("Length")>]
    let inline length (x: _ NonEmptyList) = x.Tail.Length + 1

    [<CompiledName("ToArray")>]
    let toArray x =
        let r = Array.zeroCreate (length x)
        r.[0] <- head x
        let rec loop i = 
            function
            | [] -> ()
            | h::t -> 
                r.[i] <- h
                loop (i+1) t
        loop 1 (tail x)
        r

    [<CompiledName("AsEnumerable")>]
    let toSeq x = 
        seq { yield head x
              yield! tail x }

    [<CompiledName("Select")>]
    let map f x = 
        let newHead = f (head x)
        let newTail = List.map f (tail x)
        create newHead newTail
    
    [<CompiledName("Cons")>]
    let cons h t =        
        create h (toList t)
        