namespace UrchiNet

open FSharpx

/// API methods
type Service(config: Config) =

    /// Retrieves the list of accounts for an authenticated user. 
    member x.AsyncGetAccountList() = getAccountList config

    /// Retrieves a list of profiles for an authenticated user and account.
    member x.AsyncGetProfileList accountId = getProfileList config accountId

    /// Retrieves list of tables for a specified profile. 
    member x.AsyncGetTableList profileId = getTableList config profileId

    /// Retrieves data for the specified dimensions/metrics
    member x.AsyncGetData query = getData config query

    /// Retrieves the list of accounts for an authenticated user. 
    member x.GetAccountList() = x.AsyncGetAccountList() |> Async.RunSynchronously

    /// Retrieves the list of accounts for an authenticated user. 
    member x.GetAccountListOrThrow() = x.AsyncGetAccountList() |> Async.RunSynchronously |> Choice.get

    /// Retrieves a list of profiles for an authenticated user and account.
    member x.GetProfileList accountId = x.AsyncGetProfileList accountId |> Async.RunSynchronously

    /// Retrieves a list of profiles for an authenticated user and account.
    member x.GetProfileListOrThrow accountId = x.AsyncGetProfileList accountId |> Async.RunSynchronously |> Choice.get

    /// Retrieves list of tables for a specified profile. 
    member x.GetTableList profileId = x.AsyncGetTableList profileId |> Async.RunSynchronously

    /// Retrieves list of tables for a specified profile. 
    member x.GetTableListOrThrow profileId = x.AsyncGetTableList profileId |> Async.RunSynchronously |> Choice.get

    /// Retrieves data for the specified dimensions/metrics
    member x.GetData query = x.AsyncGetData query |> Async.RunSynchronously

    /// Retrieves data for the specified dimensions/metrics
    member x.GetDataOrThrow query = x.AsyncGetData query |> Async.RunSynchronously |> Choice.get
