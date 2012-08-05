namespace UrchiNet

type AsyncService(config: Config) =
    member x.AsyncGetAccountList() = getAccountList config
    member x.AsyncGetProfileList accountId = getProfileList config accountId
    member x.AsyncGetTableList profileId = getTableList config profileId
    member x.AsyncGetData query = getData config query

type Service(config: Config) =
    let s = AsyncService(config)
    member x.GetAccountList() = s.AsyncGetAccountList() |> Async.RunSynchronously
    member x.GetProfileList accountId = s.AsyncGetProfileList accountId |> Async.RunSynchronously
    member x.GetTableList profileId = s.AsyncGetTableList profileId |> Async.RunSynchronously
    member x.GetData query = s.AsyncGetData query |> Async.RunSynchronously