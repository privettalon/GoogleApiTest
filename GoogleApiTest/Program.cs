using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
using Google.Apis.Util;
using GoogleApiTest.Helpers;
using GoogleApiTest.Services;
using System.Text;

namespace GoogleApiTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GoogleApisService googleApis = new GoogleApisService();
            googleApis.UpdateFilesListAsync().Wait();
        }
    }
}