namespace UrchiNet

type Account = {
    Id: int
    Name: string
}

[<AutoOpen>]
module Functions =
    open System
    open System.Xml
    open System.Xml.Linq
    open System.IO
    open System.Net
    open Microsoft.FSharp.Control.WebExtensions
    open UrchiNet

    let newHttpRequest (uri: string) =
        WebRequest.Create uri :?> HttpWebRequest

    let parseAccount (x: XElement) = 
        let accountId = x |> Xml.element "accountId" |> Xml.value |> int
        let accountName = x |> Xml.element "accountName" |> Xml.value
        { Account.Id = accountId
          Name = accountName }

    let parseAccounts (x: XDocument) =
        x.Root.Elements() |> Seq.map parseAccount

    let dorequestAsync urchinUrl login password =
        let url = sprintf "http://%s/services/v1/adminservice/accounts/?login=%s&password=%s" urchinUrl login password
        let request = newHttpRequest url
        async {
            use! response = request.AsyncGetResponse()
            use responseStream = response.GetResponseStream()
            use xmlReader = new XmlTextReader(responseStream)
            let xdoc = XDocument.Load xmlReader
            return xdoc.ToString()
        }
