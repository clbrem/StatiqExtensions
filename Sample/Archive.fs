namespace Statiq.Giraffe.Sample.Template
open Giraffe.ViewEngine
open Statiq.Common

type Styles(ctx: IExecutionContext) =
    let inner =
        ctx.Outputs
           .FromPipeline("Data")
           .["styles/theme.yaml"]
           .FirstOrDefaultDestination()
    member _.lookup(name: string) =
        match inner.Get<string>(name) with
        | null -> []
        | cls -> [_class cls]

module Archive =
    
         
    let getLead (doc: IDocument) =
        doc.Get<string>("Lead")
    
    let getTitle (doc: IDocument) =
        doc.Get<string>("Title")
    let blurb (styles: Styles) (doc: IDocument) =        
        div (styles.lookup("blurb")) [
            h2 (styles.lookup("blurbTitle")) [
                getTitle doc |> str
            ]
            p (styles.lookup("blurbContent")) [
                getLead doc |> str
            ]
        ]
    let create (doc: IDocument, ctx: IExecutionContext) =        
        let styles = Styles(ctx);
        let children =
            doc.GetChildren()
            |> Seq.map (blurb styles)
            |> List.ofSeq
        let myDoc = doc.GetContentTextReader().ReadToEnd()
        body []
            [
              header (styles.lookup("main")) [
                h1 (styles.lookup("mainTitle")) [getTitle doc |> str]
                span (styles.lookup("mainSub")) [rawText myDoc]            
              ]
              div (styles.lookup("blurbContainer")) children
        ]
        |> Layout.layout ctx


