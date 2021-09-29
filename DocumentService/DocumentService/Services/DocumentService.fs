namespace DocumentService

open Microsoft.AspNetCore.StaticFiles
open System.IO
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http

type DocumentService() =
   let dir = "./data/files"
   interface IDocumentService with
      member this.Get(name: string) = 
          let filePath = Path.Combine(dir, name)
      
          let provider = FileExtensionContentTypeProvider();
      
          let content = provider.TryGetContentType(filePath)
          let mutable gotContentType, contentType = content
          if not gotContentType
              then contentType = "application/octet-stream" |> ignore
      
          let bytes = File.ReadAllBytes(filePath);
          FileContentResult(bytes, contentType);

      member this.Create(file: IFormFile) =
          let name = file.FileName
          let filePath = Path.Combine(dir, name)
          
          let pathExists () = Directory.Exists(dir)
          let createDir () =
              if not (pathExists ()) then
                  Directory.CreateDirectory(dir) |> ignore

          let saveFile (fs: FileStream) = file.CopyTo(fs)

          createDir ()
          using (new FileStream(filePath, FileMode.Create)) saveFile
       
      member this.Delete(name: string) =
          let filePath = Path.Combine(dir, name)
          File.Delete(filePath)

      member this.GetAllFileNames() =
          Directory.GetFiles(dir) |> Array.map Path.GetFileName