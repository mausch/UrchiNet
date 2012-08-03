open System
open System.Configuration
open System.Xml
open System.Xml.Linq
open Fuchu
open UrchiNet

let pintegrationTests hostport login password = 
    testList "integration" [
        testCase "start" <| fun _ ->
            let s = ConfigurationManager.AppSettings
            let q = dorequestAsync hostport login password "adminservice/accounts" [] |> Async.RunSynchronously
            printfn "%s" q
    ]

let integrationTests = 
    let s = ConfigurationManager.AppSettings
    pintegrationTests s.["urchin.hostport"] s.["urchin.login"] s.["urchin.password"]

let tests = 
    TestList [
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

        testCase "parse profiles" <| fun _ ->
            let rawXml = @"<tns:getProfileListResponse xmlns:tns='https://urchin.com/api/urchin/v1/'>
             <profile>
                <accountId>1</accountId>
                <accountName>(NONE)</accountName>
                <profileId>1</profileId>
                <profileName>tets.profile</profileName>
             </profile>
             <profile>
                <accountId>1</accountId>
                <accountName>(NONE)</accountName>
                <profileId>3</profileId>
                <profileName>ttets.profile (utm)</profileName>
             </profile>
          </tns:getProfileListResponse>"
            let xml = XDocument.Parse rawXml
            let result = parseProfiles xml |> Seq.toList
            Assert.Equal("profile count", 2, result.Length)
            match result.[0] with
            | { Profile.Id = 1; Name = "tets.profile" } -> ()
            | x -> failtestf "Unexpected %A" x
            match result.[1] with
            | { Profile.Id = 3; Name = "ttets.profile (utm)" } -> ()
            | x -> failtestf "Unexpected %A" x

        testCase "parse data" <| fun _ -> 
            let rawXml = @"<tns:getDataResponse xmlns:tns='https://urchin.com/api/urchin/v1/'>
  <record>
    <recordId>1</recordId>
    <dimensions>
      <dimension name='u:robot_agent'>Mozilla Compatible Agent</dimension>
      <dimension name='u:cs_useragent'>Mozilla/5.0+(compatible;+YandexBot/3.0;++http://yandex.com/bots)</dimension>
    </dimensions>
    <metrics>
      <u:validhits xmlns:u='https://urchin.com/api/urchin/v1/'>407966</u:validhits>
    </metrics>
  </record>
  <record>
    <recordId>2</recordId>
    <dimensions>
      <dimension name='u:robot_agent'>Googlebot</dimension>
      <dimension name='u:cs_useragent'>Mozilla/5.0+(compatible;+Googlebot/2.1;++http://www.google.com/bot.html)</dimension>
    </dimensions>
    <metrics>
      <u:validhits xmlns:u='https://urchin.com/api/urchin/v1/'>4126139</u:validhits>
    </metrics>
  </record>
</tns:getDataResponse>"
            let xml = XDocument.Parse rawXml
            let result = parseData xml |> Seq.toList
            Assert.Equal("record count", 2, result.Length)
            Assert.Equal("first record dimensions",
                [ Dimension.Robot_agent, "Mozilla Compatible Agent"
                  Dimension.Cs_useragent, "Mozilla/5.0+(compatible;+YandexBot/3.0;++http://yandex.com/bots)" ],
                result.[0].Dimensions)
            Assert.Equal("first record metrics", [Metric.ValidHits, 407966], result.[0].Metrics)
            Assert.Equal("second record dimensions",
                [ Dimension.Robot_agent, "Googlebot"
                  Dimension.Cs_useragent, "Mozilla/5.0+(compatible;+Googlebot/2.1;++http://www.google.com/bot.html)" ],
                result.[1].Dimensions)
            Assert.Equal("second record metrics", [Metric.ValidHits, 4126139], result.[1].Metrics)
            ()
    ]

[<EntryPoint>]
let main _ = 
    run tests 
    //+ run integrationTests