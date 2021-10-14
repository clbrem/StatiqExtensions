namespace Statiq.Giraffe.Sample.Template
open Giraffe.ViewEngine
open Statiq.Common

module Archive =
    let getLead (doc: IDocument) =
        doc.Get<string>("Lead")
    let getTitle (doc: IDocument) =
        doc.Get<string>("Title")
    let blurb (doc: IDocument) =
        div [_class "blurb"] [
            h3 [] [
                getTitle doc |> str
            ]
            p [] [
                getLead doc |> str
            ]
        ]
    let create (doc: IDocument, ctx: IExecutionContext) =
        let children =
            doc.GetChildren()
            |> Seq.map blurb
            |> List.ofSeq
        let myDoc = doc.GetContentTextReader().ReadToEnd()
        div [] [
            str "Hi, I'm an Archive!"
            rawText myDoc
            div [] children
        ]
        |> Layout.layout ctx


