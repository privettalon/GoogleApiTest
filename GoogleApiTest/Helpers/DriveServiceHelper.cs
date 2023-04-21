using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Requests;
using Google.Apis.Services;
using GoogleApiTest.Mappers;
using GoogleApiTest.Models;
using Data = Google.Apis.Drive.v3.Data;

namespace GoogleApiTest.Helpers
{
    internal class DriveServiceHelper
    {
        private readonly DriveService driveService;

        public DriveServiceHelper(UserCredential credential)
        {
            driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        }

        public List<FileModel> GetFileModels()
        {
            var request = driveService.Files.List();
            request.Fields = "*";
            var page = new PageStreamer<Data.File, FilesResource.ListRequest, Data.FileList, string>(
                                                   (req, token) => req.PageToken = token,
                                                   response => response.NextPageToken,
                                                   response => response.Files);

            return page.Fetch(request)
                 .Select(x => x.ToModel())
                 .ToList();
        }
    }
}
