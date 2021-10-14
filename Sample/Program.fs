 open Statiq.App
 open Statiq.Common
 open Statiq.Core
 open Statiq.Giraffe
 open Statiq.Giraffe.Sample
 open Giraffe.ViewEngine
 open Statiq.Giraffe.Sample.Template
 open Statiq.Markdown
 open Statiq.Web
 
 let mainView =
     fun (doc: IDocument, ctx: IExecutionContext) ->
         let children = doc.GetChildren()
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
       
       
     