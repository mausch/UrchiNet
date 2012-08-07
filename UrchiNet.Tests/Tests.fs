open System
open System.Configuration
open System.Xml
open System.Xml.Linq
open Fuchu
open UrchiNet
open UrchiNet.Impl

let pintegrationTests (config: Config) = 
    testList "integration" [
        testCase "accounts request" <| fun _ ->
            let q = doRequest ("adminservice/accounts/", []) config |> Async.RunSynchronously
            printfn "%s" (q.ToString())

        testCase "accounts" <| fun _ ->
            let results = getAccountList config |> Async.RunSynchronously |> Seq.toList
            Seq.iter (printfn "%A") results

        testCase "profiles" <| fun _ ->
            let results = getProfileList config 1 |> Async.RunSynchronously |> Seq.toList
            Seq.iter (printfn "%A") results

        testCase "data 1" <| fun _ ->
            let query = 
                Query.Create(profileId = 1, 
                                      startDate = DateTime.Now.AddDays(-7.0),
                                      endDate = DateTime.Now,
                                      dimensions = NonEmptyList.create Dimension.Browser_base [],
                                      table = Table.BrowserPlatformConnectionSpeed1)
            
            let url = buildUrl (serializeCommand (Command.Data query)) config
            printfn "%s" url
            let results = getData config query |> Async.RunSynchronously |> Seq.toList
            Seq.iter (printfn "%A") results
    ]

let integrationTests = 
    let getOrThrow (key: string) = 
        let r = ConfigurationManager.AppSettings.[key]
        if String.IsNullOrEmpty r
            then failwithf "Configure %s in your app.config" key
            else r
    let config = 
        { Config.Host = getOrThrow "urchin.hostport"
          Login = getOrThrow "urchin.login"
          Password = getOrThrow "urchin.password" }
    pintegrationTests config

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
            let metricToTuple (x: MetricValue) = x.Metric, x.Value
            let dimensionToTuple (x: DimensionValue) = x.Dimension, x.Value
            Assert.Equal("record count", 2, result.Length)
            Assert.Equal("first record dimensions",
                [ Dimension.Robot_agent, "Mozilla Compatible Agent"
                  Dimension.Cs_useragent, "Mozilla/5.0+(compatible;+YandexBot/3.0;++http://yandex.com/bots)" ],
                List.map dimensionToTuple result.[0].Dimensions)
            Assert.Equal("first record metrics", [Metric.ValidHits, 407966], List.map metricToTuple result.[0].Metrics)
            Assert.Equal("second record dimensions",
                [ Dimension.Robot_agent, "Googlebot"
                  Dimension.Cs_useragent, "Mozilla/5.0+(compatible;+Googlebot/2.1;++http://www.google.com/bot.html)" ],
                List.map dimensionToTuple result.[1].Dimensions)
            Assert.Equal("second record metrics", [Metric.ValidHits, 4126139], List.map metricToTuple result.[1].Metrics)
            
        testCase "parse tables" <| fun _ ->
            let rawXml = @"<tns:getTableListResponse xmlns:tns='https://urchin.com/api/urchin/v1/'>
    <table>
        <tableId>1</tableId>
        <dimensions>
            <dimension>u:utm_source</dimension>
            <dimension>u:utm_medium</dimension>
            <dimension>u:utm_campaign</dimension>
        </dimensions>
        <metrics>
            <metric>u:pages</metric>
            <metric>u:visits</metric>
            <metric>u:transactions</metric>
            <metric>u:revenue</metric>
            <metric>u:responses</metric>
            <metric>u:impressions</metric>
            <metric>u:clicks</metric>
            <metric>u:cost</metric>
            <metric>u:goals1</metric>
            <metric>u:goals2</metric>
            <metric>u:goals3</metric>
            <metric>u:goals4</metric>
        </metrics>
    </table>
</tns:getTableListResponse>"
            let xml = XDocument.Parse rawXml
            let tables = parseTables xml |> Seq.toList
            Assert.Equal("table count", 1, tables.Length)
            let table = tables.[0]
            Assert.Equal("table id", 1, table.Id)
            Assert.Equal("dimension count", 3, table.Dimensions.Length)
            Assert.Equal("metric count", 12, table.Metrics.Length)
            Assert.Equal("dimensions", 
                [ Dimension.Utm_source; Dimension.Utm_medium; Dimension.Utm_campaign ],
                table.Dimensions)
            Assert.Equal("metrics",
                [ Metric.Pages; Metric.Visits; Metric.Transactions; Metric.Revenue;
                  Metric.Responses; Metric.Impressions; Metric.Clicks; Metric.Cost; 
                  Metric.Goals1; Metric.Goals2; Metric.Goals3; Metric.Goals4; ],
                table.Metrics)
    ]

[<EntryPoint>]
let main _ = 
    //run tests 
    run integrationTests