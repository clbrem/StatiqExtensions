namespace Statiq.Giraffe
open Statiq.Common
open Statiq.Web

type SetContent(contentType: ContentType) =
    inherit SyncModule() with
      override _.ExecuteInput(input: IDocument, _: IExecutionContext) =
          let metaData = MetadataItems()
          metaData.Add(WebKeys.ContentType,contentType)
          input.Clone(metaData).Yield()