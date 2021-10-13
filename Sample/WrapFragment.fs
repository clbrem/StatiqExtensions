namespace Statiq.Giraffe
open System.IO
open Statiq.Common
open FSharp.Control.Tasks.V2

type WrapFragment() =
    inherit ParallelModule() with
     override _.ExecuteInputAsync(input: IDocument, _: IExecutionContext) =
         failwith "not implemented"
         
         
         
         
