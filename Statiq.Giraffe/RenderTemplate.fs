namespace Statiq.Giraffe
open Giraffe.ViewEngine
open Statiq.Common
open FSharp.Control.Tasks.V2
open System.Threading.Tasks

type RenderTemplate (template: IDocument * IExecutionContext -> XmlNode ) =  
  inherit SyncModule() with
    new(template: IDocument -> XmlNode) =
      RenderTemplate(fun (doc, _) -> template doc)
    new(template: IExecutionContext-> XmlNode) =
      RenderTemplate(fun (_, context) -> template context)
    override _.ExecuteInput(input: IDocument, context: IExecutionContext) =
      use doc = input.GetContentTextReader()
      let rendered =
        template (input, context) |> RenderView.AsString.htmlNode      
      input.Clone(
          context.GetContentProvider(rendered, MediaTypes.Html)
      ).Yield()

type RenderTemplateAsync private (template: IDocument * IExecutionContext -> Task<XmlNode> ) =  
  inherit ParallelModule() with
    new(template: IDocument -> Task<XmlNode>) =
      RenderTemplateAsync(fun (doc, _) -> template doc)
    new(template: IExecutionContext-> Task<XmlNode>) =
      RenderTemplateAsync(fun (_, context) -> template context)
    override _.ExecuteInputAsync(input: IDocument, context: IExecutionContext) =
      task {
        let! rendered = template (input, context)
        let view =
          rendered |> RenderView.AsString.htmlNode      
        return input.Clone(
                  context.GetContentProvider(view, MediaTypes.Html)
                ).Yield()     
      }
       