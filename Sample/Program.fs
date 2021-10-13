 
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
       .CreateDefault(argv)
       .AddHostingCommands()
       .BuildPipeline(
           "Inputs",
           fun builder ->
               builder
                   .WithInputReadFiles("*.md")
                   .WithProcessModules(                       
                       ExecuteIf(
                           Config.FromDocument(fun doc -> doc.ContainsKey(WebKeys.MediaType)),
                           SetMediaType(Config.FromDocument(fun doc -> doc.GetString(WebKeys.MediaType)))
                           ),
                       ExtractFrontMatter(ParseYaml()),
                       SetContent(ContentType.Content)
                       )                   
                   |> ignore
           )
       .BuildPipeline(
         "GiraffeContent",
         fun builder ->
             builder
                 .WithDependencies("Inputs")
                 .WithProcessModules(
                     GetPipelineDocuments(ContentType.Content),
                     FilterDocuments(
                         Config.FromDocument(fun doc -> not (Archives.IsArchive(doc)))
                         ),
                     new CacheDocuments(
                         AddTitle(),
                         SetDestination(),
                         OptimizeFileName(),
                         RenderMarkdown().UseExtensions(),
                         ExecuteIf(
                             Config.FromDocument(
                                 fun doc -> doc.MediaTypeEquals(MediaTypes.Html)
                                 ),
                             GenerateExcerpt(),
                             GatherHeadings(Config.FromDocument<int>(WebKeys.GatherHeadingsLevel,1))
                             )
                             
                         ),
                         OrderDocuments()
                         )
                     
                 .WithPostProcessModules(
                     RenderHandlebars(),
                     RenderTemplate mainView                     
                     )
                 .WithOutputWriteFiles(".html")
                 |> ignore
             
       )
       .RunAsync()
       .GetAwaiter()
       .GetResult()
       
       
     