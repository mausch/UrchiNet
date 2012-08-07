namespace UrchiNet

open System
open System.Runtime.InteropServices

type Account = {
    Id: int
    Name: string
    Contact: string option
    Email: string option
}

type Profile = {
    Id: int
    Name: string
    Account: Account
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
    with 
        override x.ToString() =
            (sprintf "%A" x).ToLowerInvariant()

/// https://secure.urchin.com/helpwiki/en/Metrics_and_Units_v1.html
[<RequireQualifiedAccess>]
type Metric =
    | Hits
    | ValidHits
    | ErrorHits
    | Bytes
    | Pages
    | NonPages
    | EntrancePages
    | ExitPages
    | BouncePages
    | PageTime
    | Visits
    | Visitors
    | NewVisitors
    | PriorVisitors
    | Transactions
    | Customers
    | NewCustomers
    | PriorCustomers
    | Revenue
    | Tax
    | Shipping
    | Items
    | ItemRevenue
    | Responses
    | Impressions
    | Clicks
    | Cost
    | Goals1
    | Goals2
    | Goals3
    | Goals4
    | GoalStarts1
    | GoalStarts2
    | GoalStarts3
    | GoalStarts4
    | Score
    | AvgSessionTime
    with
        override x.ToString() =
            (sprintf "%A" x).ToLowerInvariant()

type DataParameters = {
    ProfileId: int
    StartIndex: int option
    MaxResults: int option
    StartDate: DateTime
    EndDate: DateTime
    Dimensions: Dimension NonEmptyList
    Metrics: Metric list
    // Sort
    // Filters
    Table: Table option
} with
    static member Create(profileId, startDate, endDate, dimensions, [<Optional; DefaultParameterValueAttribute(null)>] ?startIndex, [<Optional; DefaultParameterValueAttribute(null)>] ?maxResults, [<Optional; DefaultParameterValueAttribute(null)>] ?metrics, [<Optional; DefaultParameterValueAttribute(null)>] ?table) =
        { DataParameters.ProfileId = profileId
          StartIndex = startIndex
          MaxResults = maxResults
          StartDate = startDate
          EndDate = endDate
          Dimensions = dimensions
          Metrics = defaultArg metrics []
          Table = table }

[<RequireQualifiedAccess>]
type Command =
    | AccountList
    | ProfileList of int
    | TableList of int
    | Data of DataParameters

type DataRecord = {
    Dimensions: (Dimension * string) list
    Metrics: (Metric * int) list
}

type TableDefinition = {
    Id: int
    Dimensions: Dimension list
    Metrics: Metric list
}

type Config = {
    Host: string
    Login: string
    Password: string
}