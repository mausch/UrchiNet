namespace UrchiNet

[<AutoOpen>]
module Functions =
    open System
    open System.Globalization
    open System.Xml
    open System.Xml.Linq
    open System.IO
    open System.Net
    open Microsoft.FSharp.Control.WebExtensions
    open UrchiNet.Helpers

    let dimensionId =
        function
        | Dimension.Sc_status -> 10
        | Dimension.Cs_useragent -> 13
        | Dimension.Referral_host -> 77
        | Dimension.Referral_stem -> 82
        | Dimension.Request_stem -> 98
        | Dimension.Request_query -> 99
        | Dimension.Request_origfilepath -> 104
        | Dimension.Request_origmime -> 105
        | Dimension.Browser_base -> 108
        | Dimension.Browser_version -> 109
        | Dimension.Platform_base -> 110
        | Dimension.Platform_version -> 111
        | Dimension.Domain_primary -> 112
        | Dimension.Domain_complete -> 113
        | Dimension.Utm_campaign -> 137
        | Dimension.Utm_medium -> 138
        | Dimension.Utm_source -> 139
        | Dimension.Utm_term -> 140
        | Dimension.Utm_content -> 141
        | Dimension.Utm_screen_resolution -> 126
        | Dimension.Utm_screen_colors -> 129
        | Dimension.Utm_language -> 130
        | Dimension.Utm_java_enabled -> 131
        | Dimension.Utm_js_version -> 134
        | Dimension.Utm_campaign_hour -> 150
        | Dimension.Utm_campaign_goal -> 151
        | Dimension.Log_source_name -> 152
        | Dimension.Robot_agent -> 156
        | Dimension.Client_ipaddress -> 157
        | Dimension.Username -> 159
        | Dimension.Geo_country -> 160
        | Dimension.Geo_region -> 161
        | Dimension.Geo_city -> 162
        | Dimension.Geo_latitude -> 163
        | Dimension.Geo_longitude -> 164
        | Dimension.Geo_organization -> 166
        | Dimension.Geo_connection_speed -> 167
        | Dimension.Utm_flash_version -> 168
        | Dimension.Utm_page_title -> 169
        | Dimension.Visitor_type -> 170
        | Dimension.User_defined_variable -> 172
        | Dimension.Ecommerce_affiliation -> 174
        | Dimension.Ecommerce_bill_city -> 178
        | Dimension.Ecommerce_bill_region -> 179
        | Dimension.Ecommerce_bill_country -> 180
        | Dimension.Ecommerce_product_code -> 181
        | Dimension.Ecommerce_product_name -> 182
        | Dimension.Ecommerce_variation -> 183
        | Dimension.Utm_request_hostname -> 186
        | Dimension.Request_download -> 187
        | Dimension.Request_form -> 188
        | Dimension.Transaction_id -> 318
        | Dimension.Visitor_id -> 317
        | Dimension.Totals -> 312
        | Dimension.Month -> 314
        | Dimension.Day -> 315
        | Dimension.Hour -> 316
        | Dimension.Clickthru_page_from -> 301
        | Dimension.Clickthru_page_to -> 302
        | Dimension.Initial_path_page1 -> 305
        | Dimension.Initial_path_page2 -> 306
        | Dimension.Initial_path_page3 -> 307
        | Dimension.Goal_clickthru_entrance_page -> 303
        | Dimension.Goal_clickthru_exit_page -> 304
        | Dimension.Rev_goal_path -> 308
        | Dimension.Rev_goal_path_page1 -> 309
        | Dimension.Rev_goal_path_page2 -> 310
        | Dimension.Rev_goal_path_page3 -> 311

    let tableId = 
        function
        | Table.MarketingCampaignResults -> 1
        | Table.MarketingKeywordConversionAndAdVersionInformation -> 2
        | Table.VisitorGeoLocation -> 3
        | Table.VisitorOriganizationInformation -> 4
        | Table.ISPMetrics -> 5
        | Table.BrowserLanguage -> 6
        | Table.BrowserPlatformConnectionSpeed1 -> 7
        | Table.ScreenResolutionColorDepthJavascriptFlash -> 8
        | Table.Username -> 9
        | Table.UserDefined -> 10
        | Table.VisitorType -> 11
        | Table.PagesFiles1 -> 12
        | Table.PagesFiles2 -> 13
        | Table.GoalsFunnel1 -> 14
        | Table.GoalsFunnel2 -> 15
        | Table.FunnelNavigation -> 16
        | Table.ReverseGoalPath -> 17
        | Table.GoalTracking -> 18
        | Table.GoalConversion -> 19
        | Table.Billing -> 20
        | Table.AffiliateMarketing -> 21
        | Table.Product -> 22
        | Table.ProductCityRegionCountry -> 23
        | Table.ProductKeywordSourceMedium -> 24
        | Table.HostName -> 25
        | Table.HitsBytes -> 26
        | Table.BrowserPlatformConnectionSpeed2 -> 27
        | Table.UsernameHitsBytes -> 28
        | Table.PagesFilesHitsBytes -> 29
        | Table.DownloadsHitsBytes -> 30
        | Table.FormHits -> 31
        | Table.StatusCodeHits -> 32
        | Table.ReferralErrors -> 33
        | Table.Robot -> 34
        | Table.RobotContent -> 35
        | Table.LogHitsBytes -> 36
        | Table.AdvertiserCampaign -> 37
        | Table.AdvertiserTerm -> 38
        | Table.ROI -> 39
        | Table.VisitorId -> 256
        | Table.TransactionId -> 257
        | Table.Aggregates -> 258

    let commandService =
        function
        | Command.AccountList -> "adminservice/accounts"
        | Command.ProfileList _ -> "adminservice/profiles"
        | Command.TableList _ -> "reportservice/tables"
        | Command.Data _ -> "reportservice/data"

    let newHttpRequest (uri: string) =
        WebRequest.Create uri :?> HttpWebRequest

    let parseAccount (x: XElement) = 
        let elementValue n = x |> Xml.element n |> Xml.value
        let tryElementValue n = x |> Xml.tryElement n |> Option.map Xml.value
        { Account.Id = elementValue "accountId" |> int
          Name = elementValue "accountName" 
          Contact = tryElementValue "contactName"
          Email = tryElementValue "emailAddress" }

    let parseAccounts (x: XDocument) =
        x.Root.Elements() |> Seq.map parseAccount

    let parseProfile (x: XElement) =
        let elementValue n = x |> Xml.element n |> Xml.value
        { Profile.Id = elementValue "profileId" |> int
          Name = elementValue "profileName"
          Account = parseAccount x }

    let parseProfiles (x: XDocument) =
        x.Root.Elements() |> Seq.map parseProfile

    let serializeDate (t: DateTime) =
        t.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)

    let dimensionToString (d: Dimension) =
        (sprintf "%A" d).ToLowerInvariant()

    let dimensionFromString =
        function
        | "sc_status" -> Some Dimension.Sc_status
        | "cs_useragent" -> Some Dimension.Cs_useragent 
        | "referral_host" -> Some Dimension.Referral_host 
        | "referral_stem" -> Some Dimension.Referral_stem 
        | "request_stem" -> Some Dimension.Request_stem 
        | "request_query" -> Some Dimension.Request_query 
        | "request_origfilepath" -> Some Dimension.Request_origfilepath 
        | "request_origmime" -> Some Dimension.Request_origmime 
        | "browser_base" -> Some Dimension.Browser_base 
        | "browser_version" -> Some Dimension.Browser_version 
        | "platform_base" -> Some Dimension.Platform_base 
        | "platform_version" -> Some Dimension.Platform_version 
        | "domain_primary" -> Some Dimension.Domain_primary 
        | "domain_complete" -> Some Dimension.Domain_complete 
        | "utm_campaign" -> Some Dimension.Utm_campaign 
        | "utm_medium" -> Some Dimension.Utm_medium 
        | "utm_source" -> Some Dimension.Utm_source 
        | "utm_term" -> Some Dimension.Utm_term 
        | "utm_content" -> Some Dimension.Utm_content 
        | "utm_screen_resolution" -> Some Dimension.Utm_screen_resolution 
        | "utm_screen_colors" -> Some Dimension.Utm_screen_colors 
        | "utm_language" -> Some Dimension.Utm_language 
        | "utm_java_enabled" -> Some Dimension.Utm_java_enabled 
        | "utm_js_version" -> Some Dimension.Utm_js_version 
        | "utm_campaign_hour" -> Some Dimension.Utm_campaign_hour 
        | "utm_campaign_goal" -> Some Dimension.Utm_campaign_goal 
        | "log_source_name" -> Some Dimension.Log_source_name 
        | "robot_agent" -> Some Dimension.Robot_agent 
        | "client_ipaddress" -> Some Dimension.Client_ipaddress 
        | "username" -> Some Dimension.Username 
        | "geo_country" -> Some Dimension.Geo_country 
        | "geo_region" -> Some Dimension.Geo_region 
        | "geo_city" -> Some Dimension.Geo_city 
        | "geo_latitude" -> Some Dimension.Geo_latitude 
        | "geo_longitude" -> Some Dimension.Geo_longitude 
        | "geo_organization" -> Some Dimension.Geo_organization 
        | "geo_connection_speed" -> Some Dimension.Geo_connection_speed 
        | "utm_flash_version" -> Some Dimension.Utm_flash_version 
        | "utm_page_title" -> Some Dimension.Utm_page_title 
        | "visitor_type" -> Some Dimension.Visitor_type 
        | "user_defined_variable" -> Some Dimension.User_defined_variable 
        | "ecommerce_affiliation" -> Some Dimension.Ecommerce_affiliation 
        | "ecommerce_bill_city" -> Some Dimension.Ecommerce_bill_city 
        | "ecommerce_bill_region" -> Some Dimension.Ecommerce_bill_region 
        | "ecommerce_bill_country" -> Some Dimension.Ecommerce_bill_country 
        | "ecommerce_product_code" -> Some Dimension.Ecommerce_product_code 
        | "ecommerce_product_name" -> Some Dimension.Ecommerce_product_name 
        | "ecommerce_product_variation" -> Some Dimension.Ecommerce_variation 
        | "utm_request_hostname" -> Some Dimension.Utm_request_hostname 
        | "request_download" -> Some Dimension.Request_download 
        | "request_form" -> Some Dimension.Request_form 
        | "transaction_id" -> Some Dimension.Transaction_id 
        | "visitor_id" -> Some Dimension.Visitor_id 
        | "totals" -> Some Dimension.Totals 
        | "month" -> Some Dimension.Month 
        | "day" -> Some Dimension.Day 
        | "hour" -> Some Dimension.Hour 
        | "clickthru_page_from" -> Some Dimension.Clickthru_page_from
        | "clickthru_page_to" -> Some Dimension.Clickthru_page_to
        | "initial_path_page1" -> Some Dimension.Initial_path_page1
        | "initial_path_page2" -> Some Dimension.Initial_path_page2 
        | "initial_path_page3" -> Some Dimension.Initial_path_page3 
        | "goal_clickthru_entrance_page" -> Some Dimension.Goal_clickthru_entrance_page
        | "goal_clickthru_exit_page" -> Some Dimension.Goal_clickthru_exit_page
        | "rev_goal_path" -> Some Dimension.Rev_goal_path
        | "rev_goal_path_page1" -> Some Dimension.Rev_goal_path_page1
        | "rev_goal_path_page2" -> Some Dimension.Rev_goal_path_page2
        | "rev_goal_path_page3" -> Some Dimension.Rev_goal_path_page3
        | _ -> None

    let metricToString (m: Metric) =
        (sprintf "%A" m).ToLowerInvariant()

    let metricFromString =
        function
        | "hits" -> Some Metric.Hits
        | "validhits" -> Some Metric.ValidHits
        | "errorhits" -> Some Metric.ErrorHits
        | "bytes" -> Some Metric.Bytes
        | "pages" -> Some Metric.Pages
        | "nonpages" -> Some Metric.NonPages
        | "entrancepages" -> Some Metric.EntrancePages
        | "exitpages" -> Some Metric.ExitPages
        | "bouncepages" -> Some Metric.BouncePages
        | "pagetime" -> Some Metric.PageTime
        | "visits" -> Some Metric.Visits
        | "visitors" -> Some Metric.Visitors
        | "newvisitors" -> Some Metric.NewVisitors
        | "priorvisitors" -> Some Metric.PriorVisitors
        | "transactions" -> Some Metric.Transactions
        | "customers" -> Some Metric.Customers
        | "newcustomers" -> Some Metric.NewCustomers
        | "priorcustomers" -> Some Metric.PriorCustomers
        | "revenue" -> Some Metric.Revenue
        | "tax" -> Some Metric.Tax
        | "shipping" -> Some Metric.Shipping
        | "items" -> Some Metric.Items
        | "itemrevenue" -> Some Metric.ItemRevenue
        | "responses" -> Some Metric.Responses
        | "impressions" -> Some Metric.Impressions
        | "clicks" -> Some Metric.Clicks
        | "cost" -> Some Metric.Cost
        | "goals1" -> Some Metric.Goals1
        | "goals2" -> Some Metric.Goals2
        | "goals3" -> Some Metric.Goals3
        | "goals4" -> Some Metric.Goals4
        | "goalstarts1" -> Some Metric.GoalStarts1
        | "goalstarts2" -> Some Metric.GoalStarts2
        | "goalstarts3" -> Some Metric.GoalStarts3
        | "goalstarts4" -> Some Metric.GoalStarts4
        | "score" -> Some Metric.Score
        | "avgsessiontime" -> Some Metric.AvgSessionTime
        | _ -> None

    let parseDimension e =
        let name = Xml.getAttr e |> List.find (fst >> (=) "name") |> snd
        let name = name.Split(':').[1]
        dimensionFromString name
        |> Option.map (fun d -> d,e.Value)

    let parseDimensions x =
        Xml.elements "dimension" x
        |> Seq.choose parseDimension

    let parseMetric (e: XElement) = 
        metricFromString e.Name.LocalName
        |> Option.bind (fun m -> 
                        Int32.parse e.Value
                        |> Option.map (fun v -> m,v))

    let parseMetrics (x: XElement) =
        x.Elements() |> Seq.choose parseMetric

    let parseDatum (x: XElement) =
        let tryElement n = Xml.tryElement n x
        let parseSubElement name parser = 
            tryElement name |> Option.map parser |> Option.toList |> List.concat
        { DataRecord.Dimensions = parseSubElement "dimensions" (parseDimensions >> Seq.toList)
          Metrics = parseSubElement "metrics" (parseMetrics >> Seq.toList) }

    let parseData (x: XDocument) =
        x.Root.Elements() |> Seq.map parseDatum

    let serializeDataParameters (x: DataParameters) =
        [ [ "ids", x.ProfileId.ToString() ]
          x.StartIndex |> Option.map (fun a -> "start-index",a.ToString()) |> Option.toList
          x.MaxResults |> Option.map (fun a -> "max-results",a.ToString()) |> Option.toList
          [ "start-date", serializeDate x.StartDate ]
          [ "end-date", serializeDate x.EndDate ]
          [ "dimensions", NonEmptyList.toSeq x.Dimensions |> Seq.map dimensionToString |> String.concat "," ]
          [ "metrics", x.Metrics |> Seq.map metricToString |> String.concat "," ]
          // sort
          // filters
          x.Table |> Option.map (fun t -> "table", (tableId t).ToString()) |> Option.toList
        ] |> List.concat
          
    let serializeParameters x = 
        x 
        |> Seq.map (fun (k,v) -> sprintf "%s=%s" (Uri.EscapeDataString k) (Uri.EscapeDataString v))
        |> String.concat "&"

    let serializeCommand x =
        match x with
        | Command.AccountList -> commandService x, []
        | Command.ProfileList accountId -> commandService x, ["accountId", accountId.ToString()]
        | Command.TableList profileId -> commandService x, ["profileId", profileId.ToString()]
        | Command.Data data -> commandService x, serializeDataParameters data

    let dorequestAsync urchinHost login password service parameters =
        let url = sprintf "http://%s/services/v1/%s/?login=%s&password=%s&%s" urchinHost service login password (serializeParameters parameters)
        let request = newHttpRequest url
        async {
            use! response = request.AsyncGetResponse()
            use responseStream = response.GetResponseStream()
            use xmlReader = new XmlTextReader(responseStream)
            let xdoc = XDocument.Load xmlReader
            return xdoc.ToString()
        }
