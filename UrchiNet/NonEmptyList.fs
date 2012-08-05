namespace UrchiNet

type 'a NonEmptyList = {
    Head: 'a
    Tail: 'a list
}

module NonEmptyList =
    let inline create head tail = { Head = head; Tail = tail }
    let inline head (x: _ NonEmptyList) = x.Head
    let inline tail (x: _ NonEmptyList) = x.Tail

    let inline toList (x: _ NonEmptyList) = x.Head :: x.Tail

    let inline length (x: _ NonEmptyList) = x.Tail.Length + 1

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

    let toSeq x = 
        seq { yield head x
              yield! tail x }

    let map f x = 
        let newHead = f (head x)
        let newTail = List.map f (tail x)
        create newHead newTail
    
    let cons h t =        
        create h (toList t)
        