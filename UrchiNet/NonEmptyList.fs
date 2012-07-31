namespace UrchiNet

type 'a NonEmptyList = {
    Head: 'a
    Tail: 'a list
}

module NonEmptyList =
    let create head tail = { Head = head; Tail = tail }
    let head (x: _ NonEmptyList) = x.Head
    let tail (x: _ NonEmptyList) = x.Tail
    let toList (x: _ NonEmptyList) = x.Head :: x.Tail
    let toSeq x = 
        seq { yield head x
              yield! tail x }
    let map f x = 
        let newHead = f (head x)
        let newTail = List.map f (tail x)
        create newHead newTail