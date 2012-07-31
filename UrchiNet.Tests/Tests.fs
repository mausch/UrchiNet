open System
open System.Configuration
open System.Xml
open System.Xml.Linq
open Fuchu
open UrchiNet

let tests = 
    TestList [
        testCase "start" <| fun _ ->
            let s = ConfigurationManager.AppSettings
            let q = dorequestAsync s.["urchin.hostport"] s.["urchin.login"] s.["urchin.password"] |> Async.RunSynchronously
            printfn "%s" q

        testCase "parse accountlist" <| fun _ ->
            let rawXml = @"<tns:getAccountListResponse xmlns:tns='https://urchin.com/api/urchin/v1/'>
  <account>
    <accountId>1</accountId>
    <accountName>(NONE)</accountName>
  </account>
</tns:getAccountListResponse>"
            let xml = XDocument.Parse rawXml
            let result = parseAccounts xml |> Seq.toList
            Assert.Equal("account count", 1, result.Length)
            Assert.Equal("account id", 1, result.[0].Id)
            Assert.Equal("account name", "(NONE)", result.[0].Name)

        testCase "parse accountlist 2" <| fun _ ->
            let rawXml = @"<tns:getAccountListResponse xmlns:tns='https://urchin.com/api/urchin/v1/'>
             <account>
                <accountId>1</accountId>
                <accountName>(NONE)</accountName>
             </account>
             <account>
                <accountId>2</accountId>
                <accountName>account_soap</accountName>
                <contactName>SOAP Administrator Account</contactName>
                <emailAddress>administrator@urchin.com</emailAddress>
             </account>
             <account>
                <accountId>3</accountId>
                <accountName>PM_account</accountName>
                <contactName>Simple account</contactName>
             </account>
          </tns:getAccountListResponse>"
            let xml = XDocument.Parse rawXml
            let result = parseAccounts xml |> Seq.toList
            Assert.Equal("account count", 3, result.Length)
            Assert.Equal("account 1", 
                { Account.Id = 1
                  Name = "(NONE)"
                  Contact = None
                  Email = None }, 
                result.[0])
            Assert.Equal("account 2", 
                { Account.Id = 2
                  Name = "account_soap"
                  Contact = Some "SOAP Administrator Account"
                  Email = Some "administrator@urchin.com" }, 
                result.[1])
    ]

[<EntryPoint>]
let main _ = run tests