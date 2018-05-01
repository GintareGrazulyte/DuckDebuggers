using BLL_API;
using System;
using System.Collections.Generic;
using DOL.Objects;
using Bytescout.Spreadsheet;

namespace BLL
{
    public class ImportService : IImportService
    {

        public ICollection<Item> GetItemsFromFile(string path)
        {
            var document = new Spreadsheet();

            try
            {
                document.LoadFromFile(path);
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

            
            Worksheet worksheet = document.Workbook.Worksheets["Items"];

            if (worksheet == null)
            {
                throw new Exception("Worksheet <Items> is missing");
            }

            int i = 1;
            string name, description, imageUrl;
            int price;
            int? categoryId;
            var items = new List<Item>();

            //TODO get column order name
            while (IsValidRow(name = worksheet.Cell(i, 0).ValueAsString, 
                             price = worksheet.Cell(i, 1).ValueAsInteger))
            {
                description = worksheet.Cell(i, 2).ValueAsString;
                categoryId = worksheet.Cell(i, 3).ValueAsInteger;
                imageUrl = worksheet.Cell(i, 4).ValueAsString;

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
            return items;
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
    }
}
