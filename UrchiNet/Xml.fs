namespace UrchiNet

module Xml =
    open System.Xml.Linq

    /// <summary>
    /// Try to get an element in a XML container
    /// </summary>
    /// <param name="n">Element name</param>
    /// <param name="e">Container</param>
    let tryElement (n: string) (e: XContainer) = 
        match e.Element(XName.Get n) with
        | null -> None
        | x -> Some x

    /// <summary>
    /// Gets an element from a container by element name. Throws if not found.
    /// </summary>
    /// <param name="n">Element name</param>
    /// <param name="e">Container</param>
    let element (n: string) (e: XContainer) = 
        match tryElement n e with
        | None -> failwithf "Element %s not found" n
        | Some x -> x

    /// <summary>
    /// Gets an element from a container by namespace and element name.
    /// Throws if not found.
    /// </summary>
    /// <param name="ns">Namespace</param>
    /// <param name="n">Element name</param>
    /// <param name="e">Container</param>
    let elementNS (ns: string) (n: string) (e: XContainer) = 
        let ns = XNamespace.Get ns
        let nname = ns + n
        match e.Element nname with
        | null -> failwithf "Element %s not found" n
        | x -> x

    let elements (n: string) (e: XContainer) =
        e.Elements(XName.Get n)


    let value (n: XElement) = n.Value

    /// Gets attributes of an element as a tuple list
    let getAttr (e: XElement) =
        e.Attributes() 
        |> Seq.map (fun a -> a.Name.LocalName,a.Value)
        |> Seq.toList
