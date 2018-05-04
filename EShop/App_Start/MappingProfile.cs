using AutoMapper;
using BOL.Accounts;
using BOL.Carts;
using EShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShop.App_Start
{
    public static class MappingProfile
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                //TODO: create mappings
            });

            return config;
        }
    }
}