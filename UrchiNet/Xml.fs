namespace UrchiNet

module Xml =
    open UrchiNet.Helpers
    open System.Xml.Linq

    let element (n: string) (e: XContainer) = 
        e.Element(XName.Get n)

    let elements (n: string) (e: XContainer) =
        e.Elements(XName.Get n)

    let tryElement (n: string) (e: XContainer) =
        element n e |> Option.fromNull

    let value (n: XElement) = n.Value

    /// Gets attributes of an element as a tuple list
    let getAttr (e: XElement) =
        e.Attributes() 
        |> Seq.map (fun a -> a.Name.LocalName,a.Value)
        |> Seq.toList

    /// <summary>
    /// Matches a <see cref="XElement"/>
    /// </summary>
    /// <param name="n"></param>
    let (|Tag|_|) (n: XNode) = 
        match n with
        | :? XElement as e -> Some e
        | _ -> None

    /// <summary>
    /// Matches a <see cref="XElement"/>, splitting name, attributes and children
    /// </summary>
    /// <param name="n"></param>
    let (|TagA|_|) (n : XNode) =
        match n with
        | Tag t -> 
            let name = t.Name.LocalName
            let attr = getAttr t
            let children = t.Nodes() |> Seq.toList
            Some(name,attr,children)
        | _ -> None

    let (|Elem|) (e: XElement) =
        let name = e.Name.LocalName
        let attr = getAttr e
        name,attr,e.Value