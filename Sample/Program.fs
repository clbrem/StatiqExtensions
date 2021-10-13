 open Statiq.App
 open Statiq.Common
 open Statiq.Giraffe
 open Giraffe.ViewEngine
 open Statiq.Markdown
 
 let mainView =
     fun (input: IDocument) ->
         use text = input.GetContentTextReader()                  
         div []
             [ div [] [rawText "Whasssup"]
               text.ReadToEnd() |> rawText
             ]
 
 [<EntryPoint>]
 let main argv =
     Bootstrapper
       .Factory       
       .CreateDefault(argv)
       .BuildPipeline(
           "Render Markdown",
           fun builder ->
               builder.WithInputReadFiles("*.md")
                      .WithProcessModules(RenderMarkdown())
                      .WithOutputWriteFiles(".html")
                      |> ignore                      
           )
       .BuildPipeline(
           "render",
           fun builder ->
               builder.WithInputReadFiles("*.md")
                      .WithProcessModules(
                          RenderTemplate(
                              mainView
                          )
                          )
                      .WithOutputWriteFiles(".html")
                      |> ignore
           )
       .RunAsync()
       .GetAwaiter()
       .GetResult()
       
       
     