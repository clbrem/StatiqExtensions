 open Statiq.App
 open Statiq.Common 
 open Statiq.Giraffe
 open Giraffe.ViewEngine
 open Statiq.Giraffe.Sample.Template
 open Statiq.Markdown
 open Statiq.Web
 
 let mainView =
     fun (doc: IDocument, ctx: IExecutionContext) ->         
         let content =
             doc.GetContentTextReader().ReadToEnd()
             |> rawText         
         body [] [ rawText "Chief"
                   content               
                 ]
         |> Layout.layout ctx
 
 [<EntryPoint>]
 let main argv =
     Bootstrapper
       .Factory
       .CreateWeb(argv)
       .AddHostingCommands()
       .ModifyPipeline(
         "Content",
         fun content ->
             content.PostProcessModules.Add(                                  
                 RenderTemplate( mainView )
                 )
             )
       .ModifyPipeline(
           "Archives",
           fun archive ->               
               archive.ProcessModules.Add(RenderMarkdown())
               archive.PostProcessModules.Add(                   
                   RenderTemplate(Archive.create)
                   )
               
           )
       .RunAsync()
       .GetAwaiter()
       .GetResult()
       
       
     