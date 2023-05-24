namespace Services
{
    using ClosedXML.Excel;
    using Common;
    using Microsoft.AspNetCore.Http;
    using Models;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ExcelService : IExcelService
    {
        public List<T> Import<T>(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var workbook = new XLWorkbook(stream);

            var worksheet = workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
            {
                throw new Exception("Worksheet not found");
            }

            var rows = worksheet.Rows();

            var headerRow = worksheet.Rows().Where(x => x.RowNumber() == 1).FirstOrDefault();

            if (headerRow == null)
            {
                throw new Exception("Template does not have header row");
            }

            var columns = headerRow.Cells().Select(x => Convert.ToString(x.Value)).ToList();

            var items = new List<JObject>();
            foreach (var row in rows.Where(x => x.RowNumber() != 1))
            {
                var item = new JObject();
                foreach (var range in Enumerable.Range(0, columns.Count))
                {
                    item[columns[range]] = JToken.FromObject(row.Cell(range + 1).Value);
                }

                if (!item.IsEmpty())
                {
                    items.Add(item);
                }
            }

            return items.Deserialize<List<T>>();
        }

        public DownloadFile Export<T>(List<T> tList)
        {
            /*
                var workbook = new XLWorkbook($@"{Directory.GetCurrentDirectory()}\templates\Export_Product.xlsx");

                var referencRow = workbook.Worksheets.FirstOrDefault(x => x.Name == "Mapping")?.Row(1);

                var worksheet = workbook.Worksheets.FirstOrDefault(x => x.Name == typeof(T).Name);

                if (worksheet == null)
                {
                    throw new Exception($"Worksheet {typeof(T).Name} not found");
                }
            */

            var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add(typeof(T).Name);

            var list = tList.ToJson().DeserializeJson<List<JObject>>();

            var properties = tList.FirstOrDefault()?.GetType().GetProperties().Select(x => x.Name).ToList();

            foreach (var cell in worksheet.Row(1).Cells($"1:{properties.Count}"))
            {
                cell.Value = properties[cell.WorksheetColumn().ColumnNumber() - 1];
                worksheet.Row(1).Style.Fill.BackgroundColor = XLColor.BlueGray;
                worksheet.Row(1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            foreach (var num in Enumerable.Range(0, tList.Count))
            {
                var t = tList[num];
                var values = t.GetType().GetProperties().Select(x => x.GetValue(t)).ToList();

                foreach (var cell in worksheet.Row(num + 2).Cells($"1:{values.Count}"))
                {
                    // cell.Value = values[cell.WorksheetColumn().ColumnNumber() - 1];
                }

                // not working
                // worksheet.Row(num + 1).Value = new List<object> { "1", "two" } as IEnumerable;
                // worksheet.Row(num + 1).SetValue(tList[num]);
            }

            return ToApiFile<T>(workbook);
        }

        private DownloadFile ToApiFile<T>(XLWorkbook workbook)
        {
            var file = new DownloadFile
            {
                Type = FileType.Excel,
                FileName = $"{typeof(T).Name}.xlsx"
            };

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                file.Base64 = Convert.ToBase64String(stream.ToArray());
            }

            return file;
        }

    }
}
