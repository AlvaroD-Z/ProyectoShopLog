﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProyectoShopLog.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
//using ProyectoShopLog.DAL.Implementacion;
//using ProyectoShopLog.DAL.Interfaces;
//using ProyectoShopLog.BLL.Implementacion;
//using ProyectoShopLog.BLL.Interfaces;

namespace ProyectoShopLog.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<DbShoplogContext>( options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CadenaSQL"));
            });
        }
    }
}