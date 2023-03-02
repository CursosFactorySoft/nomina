using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Nomina.Models.dbNomina;

namespace Nomina.Data
{
    public partial class dbNominaContext : DbContext
    {
        public dbNominaContext()
        {
        }

        public dbNominaContext(DbContextOptions<dbNominaContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.OnModelBuilding(builder);
        }

        public DbSet<Nomina.Models.dbNomina.Empleado> Empleados { get; set; }

        public DbSet<Nomina.Models.dbNomina.Cargo> Cargos { get; set; }
    }
}