using BLL_API;
using System;
using System.Collections.Generic;
using BOL.Objects;
using System.IO;
using OfficeOpenXml;
using BOL;

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
                var worksheet = excelPackage.Workbook.Worksheets[1];

                if (worksheet == null)
                {
                    throw new Exception("Worksheet is missing");
                }
                int row = 2;

                ReadValues(out string name, out string title, out int price, out string imageUrl, out string skuCode, 
                    out string description, out string categoryName, worksheet, row);
                row++;

                while (!IsRowEmpty(name, title, price, imageUrl, skuCode, description, categoryName))
                {
                    ReadValues(out name, out title, out price, out imageUrl, out skuCode, 
                        out description, out categoryName, worksheet, row);

                    var item = new Item
                    {
                        SKUCode = skuCode,
                        Name = name,
                        Title = title,
                        Price = price,
                        Description = description,
                        Category = new Category() { Name = categoryName },
                        ImageUrl = imageUrl,
                    };
                    CorrectValues(item);

                    items.Add(item);
                    row++;
                }

            }
            return items;
        }

        private void ReadValues(out string name, out string title, out int price, out string imageUrl,
            out string skuCode, out string description, out string categoryName,
            ExcelWorksheet worksheet, int row)
        {
            name = ConvertTostring(worksheet.Cells[row, 1].Value);
            title = ConvertTostring(worksheet.Cells[row, 2].Value);
            string priceStr = ConvertTostring(worksheet.Cells[row, 3].Value);
            if (priceStr != null)
            {
                decimal.TryParse(priceStr.Replace(',', '.'), out decimal priceDec);
                price = (int)(priceDec * 100);
            }
            else
            {
                price = 0;
            }
            imageUrl = ConvertTostring(worksheet.Cells[row, 4].Value);
            skuCode = ConvertTostring(worksheet.Cells[row, 5].Value);
            description = ConvertTostring(worksheet.Cells[row, 6].Value);
            var categoriesStr = ConvertTostring(worksheet.Cells[row, 7].Value);
            if (categoriesStr != null)
            {
                var categories = categoriesStr.Split('/');
                //NOTE : we don't have subcategories so the most spetific subcategory is used
                categoryName = categories[categories.Length - 1];
            }
            else
            {
                categoryName = null;
            }
        }

        private bool IsRowEmpty(string name, string title, int price, string imageUrl,
            string skuCode, string description, string categoryName)
        {
            return IsEmpty(name) &&
                   IsEmpty(title) &&
                   price == 0 &&
                   IsEmpty(imageUrl) &&
                   IsEmpty(skuCode) &&
                   IsEmpty(description) &&
                   IsEmpty(categoryName);
        }

        private bool IsEmpty(string str)
        {
            return (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str));
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
            if (IsEmpty(item.Description))
            {
                item.Description = null;
            }
            if (IsEmpty(item.ImageUrl))
            {
                item.ImageUrl = null;
            }
        }

        public string ExportItemsToFile(IEnumerable<Item> items, string folderToSave)
        {
            string attacmentPath = folderToSave + "\\ExportedItems" + DateTime.Now.ToString("yyyy-MM-dd HH mm ss") + ".xlsx";

            var newFile = new FileInfo(attacmentPath);
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

            return attacmentPath;
        }
    }
}
