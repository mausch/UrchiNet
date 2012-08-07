namespace UrchiNet

module Xml =
    open UrchiNet.Helpers
    open System.Xml.Linq

    let element (n: string) (e: XContainer) = 
        e.Element(XName.Get n)

    let elements (n: string) (e: XContainer) =
        e.Elements(XName.Get n)

    let tryElement (n: string) (e: XContainer) =
        match element n e with
        | null -> None
        | x -> Some x

    let value (n: XElement) = n.Value

    /// Gets attributes of an element as a tuple list
    let getAttr (e: XElement) =
        e.Attributes() 
        |> Seq.map (fun a -> a.Name.LocalName,a.Value)
        |> Seq.toList
