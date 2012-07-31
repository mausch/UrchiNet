namespace UrchiNet

module Helpers =
    let nullToOption = 
        function 
        | null -> None
        | x -> Some x
