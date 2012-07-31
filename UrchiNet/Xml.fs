namespace UrchiNet

module Xml =
    open UrchiNet.Helpers
    open System.Xml.Linq

    let descendants (n: string) (a: #seq<XElement>) = 
        Extensions.Descendants(a, XName.Get n)

    let element (n: string) (e: XContainer) = 
        e.Element(XName.Get n)

    let tryElement (n: string) (e: XContainer) =
        element n e |> nullToOption

    let value (n: XElement) = n.Value