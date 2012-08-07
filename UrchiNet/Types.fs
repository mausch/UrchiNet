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
    /// Marketing Campaign results 
    | MarketingCampaignResults
    /// Marketing Keyword Conversion and Ad Version information 
    | MarketingKeywordConversionAndAdVersionInformation
    /// Visitor Geo Location information such as Country, Region, City, Latitude and Longitude 
    | VisitorGeoLocation
    /// Visitor Organization information. 
    | VisitorOriganizationInformation
    /// metrics by ISP domain determined by user IP address 
    | ISPMetrics
    /// metrics by browser language code setting. 
    | BrowserLanguage
    /// Browser, Platform and Visitor Connection Speed information. 
    | BrowserPlatformConnectionSpeed1
    /// Screen resolution and color depth, Javascript and Flash version for visitor. 
    | ScreenResolutionColorDepthJavascriptFlash
    ///  metrics by username (if any) 
    | Username
    /// metrics by arbitrary segment variable value 
    | UserDefined
    ///  New and Returning visitors. 
    | VisitorType
    /// Pages & Files 
    | PagesFiles1
    /// Pages & Files 
    | PagesFiles2
    /// Goals & Funnel Process information 
    | GoalsFunnel1
    /// Goals & Funnel Process information 
    | GoalsFunnel2
    /// Defined Funnel Navigation information 
    | FunnelNavigation
    /// Reverse Goal Path information 
    | ReverseGoalPath
    /// Goal Tracking information 
    | GoalTracking
    /// Goal Conversion information 
    | GoalConversion
    /// Billing information for e-commerce transactions 
    | Billing
    /// affiliate marketing information for e-commerce transactions 
    | AffiliateMarketing
    /// Product information for e-commerce transactions 
    | Product
    /// Product City,Region,Country Correlation information for e-commerce transactions 
    | ProductCityRegionCountry
    /// Product Keyword, Source, Medium Correlation and Referring Sources informations for e-commerce transactions. 
    | ProductKeywordSourceMedium
    /// Host names of the compaign hit. 
    | HostName
    /// ISP domain information determined by user IP address; number of hits and bytes transferred. 
    | HitsBytes
    ///  Browser, Platform, Visitor Connection Speed, hits and bytes transferred. 
    | BrowserPlatformConnectionSpeed2
    /// visitors by username (if any), number of successful hits and bytes transferred. 
    | UsernameHitsBytes
    ///  Pages & Files; number of successful hits, and number of bytes transferred. 
    | PagesFilesHitsBytes
    /// list of download requests 
    | DownloadsHitsBytes
    /// list of requested "posted form". 
    | FormHits
    /// return status codes from server 
    | StatusCodeHits
    /// Referral Errors information. 
    | ReferralErrors
    /// robot information. 
    | Robot
    /// robot Content information 
    | RobotContent
    /// list of log source assigned to profile and their information 
    | LogHitsBytes
    ///  Advertiser information for UTM campaign 
    | AdvertiserCampaign
    ///  Advertiser information for UTM term (keyword) 
    | AdvertiserTerm
    ///  ROI reports information 
    | ROI
    /// metrics by visitor id 
    | VisitorId
    /// metrics by transaction id 
    | TransactionId
    /// aggregates by period (for example monthly, daily, hourly totals). 
    | Aggregates

/// https://secure.urchin.com/helpwiki/en/Dimensions_v1.html
[<RequireQualifiedAccess>]
type Dimension =
    /// Return status code from server. 
    | Sc_status
    /// Browser user-agent information. 
    | Cs_useragent
    /// Referral complete hostname. 
    | Referral_host
    /// Referral URI stem without query info. 
    | Referral_stem
    /// Request URI without query. 
    | Request_stem
    /// Request query information (e.g., after ?) 
    | Request_query
    /// Request original uri stem if UTM. 
    | Request_origfilepath
    /// Request original mime type if UTM. 
    | Request_origmime
    /// Browser name (e.g., Netscape) 
    | Browser_base
    /// Browser version 
    | Browser_version
    /// Platform (e.g., Windows). 
    | Platform_base
    /// Platform version. 
    | Platform_version
    /// First level domain. (e.g. com). 
    | Domain_primary
    /// Complete domain. (e.g. urchin.com). 
    | Domain_complete
    /// UTM campaign name. 
    | Utm_campaign
    /// The utm_medium helps to qualify the source (cpc|cpm|link|email|organic) 
    | Utm_medium
    /// Every referral to a web site has an origin, or source. Examples of sources are the Google search engine, the AOL search engine, the name of a newsletter, or the name of a referring web site. 
    | Utm_source
    /// The utm_term or keyword is the word or phrase that a user types into a search engine 
    | Utm_term
    /// The utm_content dimension describes the version of an advertisement on which a visitor clicked. 
    | Utm_content
    /// Screen resolution (e.g., 800x600). 
    | Utm_screen_resolution
    /// Screen color bit depth. 
    | Utm_screen_colors
    /// Browser language code setting. 
    | Utm_language
    /// yes|no if java is enabled. 
    | Utm_java_enabled
    /// Javascript version info. 
    | Utm_js_version
    /// Hour of the day when that compaign hit occured. 
    | Utm_campaign_hour
    /// UTM compaign goals met. 
    | Utm_campaign_goal
    /// Name of the log source 
    | Log_source_name
    /// Robot (crawlers) requests. 
    | Robot_agent
    /// Client IP Address. 
    | Client_ipaddress
    /// Client username (if any). 
    | Username
    /// Visitor Country 
    | Geo_country
    /// Visitor region
    | Geo_region
    /// Visitor city
    | Geo_city
    /// Visitor latitude
    | Geo_latitude
    /// Visitor longitude
    | Geo_longitude
    /// Visitor organization
    | Geo_organization
    /// Visitor connection speed
    | Geo_connection_speed
    /// Flash version info 
    | Utm_flash_version
    /// UTM page title 
    | Utm_page_title
    /// New vs Returning visitors 
    | Visitor_type
    /// User defined var extracted from utm_cookiev variable. 
    | User_defined_variable
    /// Affiliate marketing for e-commerce transaction. 
    | Ecommerce_affiliation
    /// Billing city for e-commernce transaction. 
    | Ecommerce_bill_city
    /// Billing region (state/province) for e-commernce transaction. 
    | Ecommerce_bill_region
    /// Billing country for e-commernce transaction. 
    | Ecommerce_bill_country
    /// E-commerce product code. 
    | Ecommerce_product_code
    /// E-commerce product name. 
    | Ecommerce_product_name
    /// E-commerce product variation. 
    | Ecommerce_variation
    /// Host name of the compaign hit. 
    | Utm_request_hostname
    /// Download requests. 
    | Request_download
    /// Requested "posted form". 
    | Request_form
    /// E-commerce transaction id. 
    | Transaction_id
    /// Visitor ID 
    | Visitor_id
    /// Totals aggregation of "total" table 
    | Totals
    /// Monthly aggregation of "total" table 
    | Month
    /// Daily aggregation of "total" table 
    | Day
    /// Hourly aggregation of "total" table 
    | Hour
    /// Click through (PA->PB): PA - page from (clickthru_page_from) PB - page to (clickthru_page_to) 
    | Clickthru_page_from
    /// Click through (PA->PB): PA - page from (clickthru_page_from) PB - page to (clickthru_page_to) 
    | Clickthru_page_to
    /// Initial path (P1->P2->P3): P1 - initial_path_page1, P2 - initial_path_page2, P3 - initial_path_page3 
    | Initial_path_page1
    /// Initial path (P1->P2->P3): P1 - initial_path_page1, P2 - initial_path_page2, P3 - initial_path_page3 
    | Initial_path_page2
    /// /// Initial path (P1->P2->P3): P1 - initial_path_page1, P2 - initial_path_page2, P3 - initial_path_page3 
    | Initial_path_page3
    /// Goal Click through : PG - Entrance Page (goal_clickthru_entrance_page), CD - Abandonment Page (goal_clickthru_exit_page) 
    | Goal_clickthru_entrance_page
    /// Goal Click through : PG - Entrance Page (goal_clickthru_entrance_page), CD - Abandonment Page (goal_clickthru_exit_page) 
    | Goal_clickthru_exit_page
    /// Reverse Goal Path (R3->R2->R1->GX): GX - Goal path
    | Rev_goal_path
    /// Reverse Goal Path (R3->R2->R1->GX): R1 - page 1
    | Rev_goal_path_page1
    /// Reverse Goal Path (R3->R2->R1->GX): R2 - page 2
    | Rev_goal_path_page2
    /// Reverse Goal Path (R3->R2->R1->GX): R3 - page 3
    | Rev_goal_path_page3
    with 
        override x.ToString() =
            (sprintf "%A" x).ToLowerInvariant()

/// https://secure.urchin.com/helpwiki/en/Metrics_and_Units_v1.html
[<RequireQualifiedAccess>]
type Metric =
    ///  Number of hits 
    | Hits
    ///  Number of valid hits, server status=[200-300], 302, 304 
    | ValidHits
    ///  Number of error hits, server status not equal to valid (see validhits) 
    | ErrorHits
    ///  Number of bytes 
    | Bytes
    ///  Number of pages 
    | Pages
    ///  Number of pages, that don't have a visitor key, session id or complete visitor cookie sets for UTM 
    | NonPages
    | EntrancePages
    | ExitPages
    | BouncePages
    | PageTime
    ///  Number of vistors (new + returning) 
    | Visits
    | Visitors
    | NewVisitors
    | PriorVisitors
    | Transactions
    | Customers
    | NewCustomers
    | PriorCustomers
    ///  Revenue value based on transaction parameter "ecommerce_total" (for "E-Commerce Website"=yes) or goal conversion (for "E-Commerce Website"=no) 
    | Revenue
    ///  Tax value based on transaction parameter "ecommerce_tax" 
    | Tax
    ///  Shipping value based on transaction parameter "ecommerce_shipping" 
    | Shipping
    ///  Number of items based on transaction parameter "ecommerce_quantity" 
    | Items
    ///  Revenue value based on transaction parameter "ecommerce_unit_price" 
    | ItemRevenue
    ///  Number of UTM campaign responses where utm_new_campaign is equal to "1" 
    | Responses
    ///  Number of UTM campaign impressions based on utm_campaign_impressions or where utm_type is equal to "imp" 
    | Impressions
    /// Number of UTM campaign clicks based on utm_campaign_clicks 
    | Clicks
    ///  Cost of UTM campaign based on utm_campaign_cost 
    | Cost
    | Goals1
    | Goals2
    | Goals3
    | Goals4
    | GoalStarts1
    | GoalStarts2
    | GoalStarts3
    | GoalStarts4
    ///  Number of goals multiplied by goal values 
    | Score
    ///  Average session duration in seconds 
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

type DimensionValue = { 
    Dimension: Dimension
    Value: string 
}

type MetricValue = {
    Metric: Metric
    Value: int
}

type DataRecord = {
    Dimensions: DimensionValue list
    Metrics: MetricValue list
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