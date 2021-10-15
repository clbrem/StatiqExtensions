namespace Statiq.Web.MetaGenerator
open System
open Statiq.Common
type GenerateDateCreated() =
    inherit SyncModule() with
    override _.ExecuteInput(doc: IDocument, ctx: IExecutionContext) =
        let date = Config.FromContext(fun ctx -> ctx.Get<DateTime>("Date"))
        let meta = MetadataItems()
        do meta.Add(MetadataItem("Date", date))
        if doc.ContainsKey("Date") |> not then                        
            doc.Clone(meta).Yield()
        else
            doc.Yield()
            
            
        

