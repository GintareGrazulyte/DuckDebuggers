﻿using AutoMapper;

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