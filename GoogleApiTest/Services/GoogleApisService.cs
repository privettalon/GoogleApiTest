using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
using Google.Apis.Util;
using GoogleApiTest.Helpers;
using GoogleApiTest.Models;

namespace GoogleApiTest.Services
{
    internal class GoogleApisService
    {
        private readonly string[] scopes = { SheetsService.Scope.Spreadsheets, DriveService.Scope.Drive };
        private UserCredential userCredential;
        public GoogleApisService()
        {
            userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = Const.clientId,
                    ClientSecret = Const.clientSecret,
                }
                , scopes, Const.userName, CancellationToken.None).Result;
        }

        public async Task UpdateFilesListAsync()
        {
            if (userCredential.Token.IsExpired(SystemClock.Default))
            {
                userCredential.RefreshTokenAsync(CancellationToken.None).Wait();
            };

            DriveServiceHelper driveService = new DriveServiceHelper(userCredential);
            SpreadsheetsServiceHelper spreadsheetsService = new SpreadsheetsServiceHelper(userCredential);

            List<FileModel> filesList = driveService.GetFileModels();
            spreadsheetsService.WriteFilesList(filesList);

            PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMinutes(15));
            while (await timer.WaitForNextTickAsync())
            {
                filesList = driveService.GetFileModels();
                spreadsheetsService.WriteFilesList(filesList);
            }
        }
    }
}
