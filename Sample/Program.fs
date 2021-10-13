 
 open Statiq.App
 open Statiq.Common
 open Statiq.Core
 open Statiq.Giraffe
 open Giraffe.ViewEngine
 open Statiq.Handlebars
 open Statiq.Html
 open Statiq.Markdown
 open Statiq.Web
 open Statiq.Web.Modules
 open Statiq.Web.Pipelines
 open Statiq.Yaml
 
 
 let mainView =
     fun (doc: IDocument) ->
         use stream = doc.GetContentTextReader()
         div []
             [ rawText "Chief"
               stream.ReadToEnd()
               |> rawText
             ]
 
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
                 RenderTemplate(
                     mainView
                     ),
                 ExecuteIf(
                     Config.FromDocument(                     
                       fun doc -> doc.MediaTypeEquals(MediaTypes.HtmlFragment)
                     ),
                      
                     )
                 )                      
       )
       .RunAsync()
       .GetAwaiter()
       .GetResult()
       
       
     