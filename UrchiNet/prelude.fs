namespace UrchiNet

module Helpers =
    module Option =
        let fromNull =
            function 
            | null -> None
            | x -> Some x
