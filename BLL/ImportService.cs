using BLL_API;
using System;
using System.Collections.Generic;
using BOL.Objects;
using System.IO;
using OfficeOpenXml;
using BOL;
using BOL.Property;

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
                var worksheet = excelPackage.Workbook.Worksheets[1];

                if (worksheet == null)
                {
                    throw new Exception("Worksheet is missing");
                }

                int row = 2;

                string name = null, title = null, imageUrl = null, skuCode = null, description = null,
                    categoryName = null, propertyName = null, propertyValue = null;
                int price = 0;

                int lastItemRow = worksheet.Cells[row, 1].GetLastItemRow();
                bool firstRow = true;
                Item item = null;
                HashSet<ItemProperty> properties = null;

                ReadValues(ref name, ref title, ref price, ref imageUrl, ref skuCode, 
                    ref description, ref categoryName, ref propertyName, ref propertyValue, worksheet, row);

                while (!IsRowEmpty(name, title, price, imageUrl, skuCode, description, categoryName, propertyName, propertyValue))
                {
                    if (firstRow)
                    {
                        item = new Item
                        {
                            SKUCode = skuCode,
                            Name = name,
                            Title = title,
                            Price = price,
                            Description = description,
                            Category = new Category() { Name = categoryName },
                            ImageUrl = imageUrl,
                        };

                        properties = new HashSet<ItemProperty>();

                        firstRow = false;
                    }


                    if (row == lastItemRow)
                    {
                        item.ItemProperties = properties;
                        CorrectValues(item);
                        items.Add(item);


                        firstRow = true;
                        lastItemRow = worksheet.Cells[row, 1].GetLastItemRow();

                        ReadValues(ref name, ref title, ref price, ref imageUrl, ref skuCode,
                            ref description, ref categoryName, ref propertyName, ref propertyValue, worksheet, row);
                    }
                    else
                    {
                        properties.Add(new ItemProperty() { Property = new Property() { Name = propertyName }, Value = propertyValue } );
                        row++;
                        ReadValues(ref propertyName, ref propertyValue, worksheet, row);
                    }

                }

            }
            return items;
        }

        private void ReadValues(ref string name, ref string title, ref int price, ref string imageUrl,
            ref string skuCode, ref string description, ref string categoryName, ref string propertyName,
            ref string propertyValue, ExcelWorksheet worksheet, int row)
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

            ReadValues(ref propertyName, ref propertyValue, worksheet, row);
        }

        private void ReadValues(ref string propertyName, ref string propertyValue, ExcelWorksheet worksheet, int row)
        {
            propertyName = ConvertTostring(worksheet.Cells[row, 8].Value);
            propertyValue = ConvertTostring(worksheet.Cells[row, 9].Value);
        }


        private bool IsRowEmpty(string name, string title, int price, string imageUrl,
            string skuCode, string description, string categoryName, string propertyName,
            string propertyValue)
        {
            return IsEmpty(name) &&
                   IsEmpty(title) &&
                   price == 0 &&
                   IsEmpty(imageUrl) &&
                   IsEmpty(skuCode) &&
                   IsEmpty(description) &&
                   IsEmpty(categoryName) &&
                   IsEmpty(propertyName) &&
                   IsEmpty(propertyValue);
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

                var columnNames = new List<string> { "Product Name", "Title", "Price", "Image", "SKU code",
                    "Description", "Category", "Properties" };
                for (int i = 1; i < columnNames.Count + 1; i++)
                {
                    worksheet.Column(i).Width = 20;
                    var cell = worksheet.Cells[1, i];
                    cell.Value = columnNames[i - 1];
                    cell.Style.Font.Bold = true;
                    //= new Font("Calibri", 12, FontStyle.Bold);
                }

                int rowIndex = 2;
                foreach (var item in items)
                {
                    worksheet.Cells[rowIndex, 1].Value = item.Name;
                    worksheet.Cells[rowIndex, 2].Value = item.Title;
                    worksheet.Cells[rowIndex, 3].Value = item.Price;
                    worksheet.Cells[rowIndex, 4].Value = item.ImageUrl;
                    worksheet.Cells[rowIndex, 5].Value = item.SKUCode;
                    worksheet.Cells[rowIndex, 6].Value = item.Description;
                    worksheet.Cells[rowIndex, 7].Value = (item.Category != null) ? 
                        item.Category.Name : "";

                    foreach (var itemProperty in item.ItemProperties)
                    {
                        worksheet.Cells[rowIndex, 8].Value = itemProperty.Property.Name;
                        worksheet.Cells[rowIndex, 9].Value = itemProperty.Value;
                        rowIndex++;
                    }

                    if (item.ItemProperties.Count == 0)
                    {
                    rowIndex++;
                    }
                }             
                excelPackage.Save();
            }

            return attacmentPath;
        }
    }

    public static class Extensions
    {
        public static int GetLastItemRow(this ExcelRange @this)
        {
            int startRow = @this.Start.Row;
            if (@this.Merge)
            {
                var idx = @this.Worksheet.GetMergeCellId(startRow, @this.Start.Column);
                return @this.Worksheet.Cells[@this.Worksheet.MergedCells[idx - 1]].Rows + startRow;
            }
            else
            {
                return startRow;
            }
        }
    }

}
