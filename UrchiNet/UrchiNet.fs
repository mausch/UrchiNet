namespace UrchiNet

type Account = {
    Id: int
    Name: string
    Contact: string option
    Email: string option
}

/// https://secure.urchin.com/helpwiki/en/Tables_v1.html
[<RequireQualifiedAccess>]
type Table =
    | MarketingCampaignResults
    | MarketingKeywordConversionAndAdVersionInformation
    | VisitorGeoLocation
    | VisitorOriganizationInformation
    | ISPMetrics
    | BrowserLanguage
    | BrowserPlatformConnectionSpeed1
    | ScreenResolutionColorDepthJavascriptFlash
    | Username
    | UserDefined
    | VisitorType
    | PagesFiles1
    | PagesFiles2
    | GoalsFunnel1
    | GoalsFunnel2
    | FunnelNavigation
    | ReverseGoalPath
    | GoalTracking
    | GoalConversion
    | Billing
    | AffiliateMarketing
    | Product
    | ProductCityRegionCountry
    | ProductKeywordSourceMedium
    | HostName
    | HitsBytes
    | BrowserPlatformConnectionSpeed2
    | UsernameHitsBytes
    | PagesFilesHitsBytes
    | DownloadsHitsBytes
    | FormHits
    | StatusCodeHits
    | ReferralErrors
    | Robot
    | RobotContent
    | LogHitsBytes
    | AdvertiserCampaign
    | AdvertiserTerm
    | ROI
    | VisitorId
    | TransactionId
    | Aggregates

/// https://secure.urchin.com/helpwiki/en/Dimensions_v1.html
[<RequireQualifiedAccess>]
type Dimension =
    | Sc_status
    | Cs_useragent
    | Referral_host
    | Referral_stem
    | Request_stem
    | Request_query
    | Request_origfilepath
    | Request_origmime
    | Browser_base
    | Browser_version
    | Platform_base
    | Platform_version
    | Domain_primary
    | Domain_complete
    | Utm_campaign
    | Utm_medium
    | Utm_source
    | Utm_term
    | Utm_content
    | Utm_screen_resolution
    | Utm_screen_colors
    | Utm_language
    | Utm_java_enabled
    | Utm_js_version
    | Utm_campaign_hour
    | Utm_campaign_goal
    | Log_source_name
    | Robot_agent
    | Client_ipaddress
    | Username
    | Geo_country
    | Geo_region
    | Geo_city
    | Geo_latitude
    | Geo_longitude
    | Geo_organization
    | Geo_connection_speed
    | Utm_flash_version
    | Utm_page_title
    | Visitor_type
    | User_defined_variable
    | Ecommerce_affiliation
    | Ecommerce_bill_city
    | Ecommerce_bill_region
    | Ecommerce_bill_country
    | Ecommerce_product_code
    | Ecommerce_product_name
    | Ecommerce_variation
    | Utm_request_hostname
    | Request_download
    | Request_form
    | Transaction_id
    | Visitor_id
    | Totals
    | Month
    | Day
    | Hour
    | Clickthru_page_from
    | Clickthru_page_to
    | Initial_path_page1
    | Initial_path_page2
    | Initial_path_page3
    | Goal_clickthru_entrance_page
    | Goal_clickthru_exit_page
    | Rev_goal_path
    | Rev_goal_path_page1
    | Rev_goal_path_page2
    | Rev_goal_path_page3
     

[<AutoOpen>]
module Functions =
    open System
    open System.Xml
    open System.Xml.Linq
    open System.IO
    open System.Net
    open Microsoft.FSharp.Control.WebExtensions
    open UrchiNet

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
