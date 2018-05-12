using BLL_API;
using System;
using System.Collections.Generic;
using BOL.Objects;
using System.Drawing;
using System.IO;
using OfficeOpenXml;

namespace BLL
{
    //TODO: find proper name
    public class ImportService : IImportService
    {
        public List<Item> ImportItemsFromFile(string path)
        {
            var items = new List<Item>();
        
            var file = new FileInfo(path);
            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                //todo rename
                var worksheet = excelPackage.Workbook.Worksheets["Items"];

                if (worksheet == null)
                {
                    throw new Exception("Worksheet <Items> is missing");
                }
                int i = 2;
                string name, description, imageUrl;

                while (IsValidName(worksheet.Cells[i, 1].Value, out name))
                {
                    Int32.TryParse(ConvertTostring(worksheet.Cells[i, 2].Value), out int price);
                    description = ConvertTostring(worksheet.Cells[i, 3].Value);
                    Int32.TryParse(ConvertTostring(worksheet.Cells[i, 4].Value), out int categoryId);
                    imageUrl = ConvertTostring(worksheet.Cells[i, 5].Value);

                    var item = new Item
                    {
                        Name = name,
                        Price = price,
                        Description = description,
                        CategoryId = categoryId,
                        ImageUrl = imageUrl
                    };
                    CorrectValues(item);

                    items.Add(item);
                    i++;
                }

            }
            return items;
        }

        private bool IsValidName(object stringObj, out string name)
        {
            name = ConvertTostring(stringObj);
            return !(string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name));
        }

        private string ConvertTostring(object stringObj)
        {
            string name = null;
            if (stringObj == null)
            {
                return name;
            }
            return name = stringObj.ToString();
        }

        private void CorrectValues(Item item)
        {
            if (string.IsNullOrEmpty(item.Description) && string.IsNullOrWhiteSpace(item.Description))
            {
                item.Description = null;
            }
            if (string.IsNullOrEmpty(item.ImageUrl) && string.IsNullOrWhiteSpace(item.ImageUrl))
            {
                item.ImageUrl = null;
            }
            if (item.CategoryId == 0)
            {
                item.CategoryId = null;
            }
        }

        public string ExportItemsToFile(IEnumerable<Item> items)
        {
            string temproraryAttachmentPath = Path.GetTempPath() + "ExportedItems" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            var newFile = new FileInfo(temproraryAttachmentPath);
            using (ExcelPackage excelPackage = new ExcelPackage(newFile))
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Items");

                var columnNames = new List<string> { "Name", "Price", "Description", "CategoryId", "ImageUrl" };
                for (int i = 1; i < columnNames.Count + 1; i++)
                {
                    worksheet.Column(i).Width = 20;
                    var cell = worksheet.Cells[1, i];
                    cell.Value = columnNames[i - 1];
                    cell.Style.Font.Bold = true;
                    //= new Font("Calibri", 12, FontStyle.Bold);
                }

                int cellIndex = 2;
                foreach (var item in items)
                {
                    worksheet.Cells[cellIndex, 1].Value = item.Name;
                    worksheet.Cells[cellIndex, 2].Value = item.Price;
                    worksheet.Cells[cellIndex, 3].Value = item.Description;
                    worksheet.Cells[cellIndex, 4].Value = item.CategoryId;
                    worksheet.Cells[cellIndex, 5].Value = item.ImageUrl;

                    cellIndex++;
                }             
                excelPackage.Save();
            }

            return temproraryAttachmentPath;
        }
    }
}
