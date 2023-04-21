using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using GoogleApiTest.Mappers;
using GoogleApiTest.Models;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;


namespace GoogleApiTest.Helpers
{
    internal class SpreadsheetsServiceHelper
    {
        private readonly SheetsService sheetsService;
        private string? spreadsheetId = null;
        private readonly string range = "A:E";

        public SpreadsheetsServiceHelper(UserCredential credential)
        {
            sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        }

        public void CreateList(List<FileModel> files)
        {
            if (files != null || files.Count != 0)
            {
                if (string.IsNullOrEmpty(spreadsheetId))
                {
                    Spreadsheet emptySpreadsheet = new Spreadsheet();
                    SpreadsheetsResource.CreateRequest createRequest = sheetsService.Spreadsheets.Create(emptySpreadsheet);
                    Spreadsheet createResponse = createRequest.Execute();

                    spreadsheetId = createResponse.SpreadsheetId;
                }

                AddColumnsTitles();
                AddColumnsValues(files, range);
            }
            
        }
        public void  UpdateList(List<FileModel> files)
        {
            ClearSheet();
            AddColumnsTitles();
            AddColumnsValues(files, range);
        }

        public void AddNewFilesToList(List<FileModel> files)
        {
            string appendRange = GetAppendRange();
            AddColumnsValues(files, appendRange);
        }

        private void ClearSheet()
        {
            ClearValuesRequest clearValuesRequest = new ClearValuesRequest();

            ClearRequest clearRequest = sheetsService.Spreadsheets.Values.Clear(clearValuesRequest, spreadsheetId, range);
            clearRequest.Execute();
        }

        private void AddColumnsTitles()
        {
            ValueRange addedData = new ValueRange()
            {
                Values = new List<IList<object>>() { new List<object>() { "ID", "Name", "Size", "Upload Date", "File Type" } }
            };

            AppendRequest getSpreadsheetsValueRequest = sheetsService.Spreadsheets.Values.Append(addedData, spreadsheetId, range);
            getSpreadsheetsValueRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
            getSpreadsheetsValueRequest.Execute();
        }

        private void AddColumnsValues(List<FileModel> files, string range)
        {
            ValueRange addedData = new ValueRange()
            {
                Values = files.ToSheetsValue()
            };

            AppendRequest getSpreadsheetsValueRequest = sheetsService.Spreadsheets.Values.Append(addedData, spreadsheetId, range);
            getSpreadsheetsValueRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
            getSpreadsheetsValueRequest.Execute();
        }

        private string GetAppendRange()
        {
            GetRequest getRequest = sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = getRequest.Execute();
            IList<IList<object>> values = response.Values;
            return $"A" + (values.Count+1) + ":E" + (values.Count+1);
        }
    }
}
