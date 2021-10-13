namespace Statiq.Giraffe
open Giraffe.ViewEngine
open Statiq.Common
type RenderTemplate private (template: IDocument * IExecutionContext -> XmlNode ) =  
  inherit SyncModule() with
    new(template: IDocument -> XmlNode) =
      RenderTemplate(fun (doc, _) -> template doc)
    new(template: IExecutionContext-> XmlNode) =
      RenderTemplate(fun (_, context) -> template context)
    override _.ExecuteInput(input: IDocument, context: IExecutionContext) =
      use doc = input.GetContentTextReader()
      let read = doc.ReadToEnd()
      let title = input.Get("Title")
      let markdown = input.Get("MarkdownDocument")
      
      let keys = input.Keys
      let rendered =
        template (input, context) |> RenderView.AsString.htmlNode
        
        
      input.Clone(
          context.GetContentProvider(rendered, MediaTypes.Html)
      ).Yield()
  
  
      
