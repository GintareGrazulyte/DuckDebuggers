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
            //TODO: delete mock
            path = "C:\\Users\\Laura\\Desktop\\Test2.xlsx";

            var items = new List<Item>();

            var document = new Spreadsheet();
            
            document.LoadFromFile(path);

            Worksheet worksheet = document.Workbook.Worksheets["Items"];

            
            for (int i = 1; i < 2; i++)
            {
                items.Add(new Item
                {
                    Name = worksheet.Cell(i, 0).ValueAsString,
                    Price = worksheet.Cell(i, 1).ValueAsInteger,
                    Description = worksheet.Cell(i, 2).ValueAsString,
                    CategoryId = worksheet.Cell(i, 3).ValueAsInteger,
                    ImageUrl = worksheet.Cell(i, 4).ValueAsString
                });
            }


            return items;
        }
    }
}
