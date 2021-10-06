namespace DocumentService

open System.Collections.Generic
open Microsoft.AspNetCore.Http
open StackExchange.Redis.Extensions.Core.Abstractions
open StackExchange.Redis.Extensions.Core.Configuration
open StackExchange.Redis.Extensions.Core.Implementations
open StackExchange.Redis.Extensions.MsgPack

type RedisService() =

    let config = RedisConfiguration(ConnectionString = "localhost",Database = 2)
    let poolManager = new RedisCacheConnectionPoolManager(config)
    let client = RedisCacheClient(poolManager, MsgPackObjectSerializer(), config)
    let db = client.GetDbFromConfiguration()

    member this.CreateName(file: IFormFile) =
            async {
                let name = file.FileName
                db.AddAsync(name, name, tags = HashSet(["file"])) |> Async.AwaitTask |> ignore
                ()
            }

    member this.DeleteName(name: string) =
            async {
                db.RemoveAsync(name) |> Async.AwaitTask |> ignore
                ()
            }
			
	member this.GetAllNames() =
            async {

                let! getAsync = db.GetByTagAsync<string>("file") |> Async.AwaitTask
                return getAsync
            }


