namespace Statiq.Giraffe.Sample.Template
open Giraffe.ViewEngine
open Statiq.Common
    
module Layout =

    let linkStyle (styleLink: string) (context: IExecutionContext) =
        let myLink = context.GetLink(styleLink)        
        link [ _rel "stylesheet"; _href myLink ]
    let linkScript (scriptLink:string) (context: IExecutionContext) =
        let myLink = context.GetLink(scriptLink)
        script [ _src myLink ]
    let defaultHead (context: IExecutionContext) =        
        head [] [
            linkStyle "/assets/styles.css" context            
        ]
    let layout (context: IExecutionContext) =
        fun _body ->
            html [] [
                defaultHead context
                _body
            ]