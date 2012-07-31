namespace UrchiNet

module Xml =
    open System.Xml.Linq

    let descendants (n: string) (a: #seq<XElement>) = 
        Extensions.Descendants(a, XName.Get n)

    let element (n: string) (e: XContainer) = 
        e.Element(XName.Get n)

    let value (n: XElement) = n.Value