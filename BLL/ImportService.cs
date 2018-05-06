using BLL_API;
using System;
using System.Collections.Generic;
using BOL.Objects;
using Bytescout.Spreadsheet;
using System.Drawing;
using System.Threading.Tasks;

namespace BLL
{
    //TODO: find proper name
    public class ImportService : IImportService
    {
        //TODO: can it be class variables?
        private Spreadsheet _document;
        private Worksheet _worksheet;

        public void SetDocument(string path)
        {
            _document = new Spreadsheet();
            try
            {
                _document.LoadFromFile(path);
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("File not found"))
                {
                    throw new Exception("File <" + path + "> is not found");
                }
                if (ex.Message.Contains("The process cannot access the file"))
                {
                    throw new Exception("Cannot import from file <" + path + "> as it is open somewhere else");
                }
                throw new Exception("Something went wrong, try again");
            }

            _worksheet = _document.Workbook.Worksheets["Items"];

            if (_worksheet == null)
            {
                throw new Exception("Worksheet <Items> is missing");
            }
        }

        public Task<List<Item>> ImportItemsFromFile()
        {
            return Task.Run(() =>
            {
               int i = 1;
               string name, description, imageUrl;
               int price;
               int? categoryId;
               var items = new List<Item>();

               while (IsValidRow(name = _worksheet.Cell(i, 0).ValueAsString,
                                price = _worksheet.Cell(i, 1).ValueAsInteger))
               {
                   description = _worksheet.Cell(i, 2).ValueAsString;
                   categoryId = _worksheet.Cell(i, 3).ValueAsInteger;
                   imageUrl = _worksheet.Cell(i, 4).ValueAsString;

                   CorrectValues(ref description, ref imageUrl, ref categoryId);

                   items.Add(new Item
                   {
                       Name = name,
                       Price = price,
                       Description = description,
                       CategoryId = categoryId,
                       ImageUrl = imageUrl
                   });
                   i++;
               }

                _document.Close();
               return items;
            });
        }

        private bool IsValidRow(string name, int price)
        {
            return !(string.IsNullOrEmpty(name) && string.IsNullOrWhiteSpace(name)) &&
                    price != 0;
        }

        private void CorrectValues(ref string description, ref string imageUrl, 
            ref int? categoryId)
        {
            if (string.IsNullOrEmpty(description) && string.IsNullOrWhiteSpace(description))
            {
                description = null;
            }
            if (string.IsNullOrEmpty(imageUrl) && string.IsNullOrWhiteSpace(imageUrl))
            {
                imageUrl = null;
            }
            if (categoryId == 0)
            {
                categoryId = null;
            }
        }

        public Task ExportItemsToFile(IEnumerable<Item> items, string path)
        {
            var document = new Spreadsheet();

            Worksheet worksheet = document.Workbook.Worksheets.Add("Items");

            return Task.Run(() =>
            {
                var columnNames = new List<string> { "Name", "Price", "Description", "CategoryId", "ImageUrl" };
                for (int i = 0; i < columnNames.Count; i++)
                {
                    worksheet.Columns[i].Width = 90;
                    var cell = worksheet.Cell(0, i);
                    cell.Value = columnNames[i];
                    cell.Font = new Font("Calibri", 12, FontStyle.Bold);
                }

                int cellIndex = 1;
                foreach (var item in items)
                {
                    worksheet.Cell(cellIndex, 0).Value = item.Name;
                    worksheet.Cell(cellIndex, 1).Value = item.Price;
                    worksheet.Cell(cellIndex, 2).Value = item.Description;
                    worksheet.Cell(cellIndex, 3).Value = item.CategoryId;
                    worksheet.Cell(cellIndex, 4).Value = item.ImageUrl;

                    cellIndex++;
                }

                //TODO: remove additionaly created worksheet. 
                document.SaveAs(path);
                document.Close();
            });
        }
    }
}
