﻿using BOL.Objects;
using System;
using System.Collections.Generic;

namespace DAL_API
{
    public interface IItemRepository
    {
        Item Find(int? id);
        List<Item> GetAll();
        void Remove(Item item);
        void Add(Item item);
        void Modify(Item item);
    }
}
