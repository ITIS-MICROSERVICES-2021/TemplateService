namespace DocumentService

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http

type IDocumentService =
   abstract member Get : string -> FileContentResult
   abstract member Create : IFormFile -> unit
   abstract member Delete : string -> unit
   abstract member GetAllFileNames : unit -> string[]
