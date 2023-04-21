using GoogleApiTest.Models;

namespace GoogleApiTest.Mappers
{
    internal static class FilesMapper
    {
        internal static FileModel ToModel(this Google.Apis.Drive.v3.Data.File file)
        {
            return new FileModel()
            {
                Id = file.Id,
                Name = file.Name,
                UploadDate = file.CreatedTime,
                Size = file.Size,
                FileType = file.MimeType
            };
        }

        internal static object[][] ToSheetsValue(this List<FileModel> models)
        {
            return models.Select(model =>
            {
                return new object[] { model.Id, model.Name, model.Size, model.UploadDate, model.FileType };
            })
            .ToArray();
        }
    }
}
