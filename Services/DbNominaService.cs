using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using Nomina.Data;

namespace Nomina
{
    public partial class dbNominaService
    {
        dbNominaContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly dbNominaContext context;
        private readonly NavigationManager navigationManager;

        public dbNominaService(dbNominaContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportEmpleadosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbnomina/empleados/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbnomina/empleados/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEmpleadosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbnomina/empleados/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbnomina/empleados/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEmpleadosRead(ref IQueryable<Nomina.Models.dbNomina.Empleado> items);

        public async Task<IQueryable<Nomina.Models.dbNomina.Empleado>> GetEmpleados(Query query = null)
        {
            var items = Context.Empleados.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnEmpleadosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEmpleadoGet(Nomina.Models.dbNomina.Empleado item);

        public async Task<Nomina.Models.dbNomina.Empleado> GetEmpleadoByEmpId(int empid)
        {
            var items = Context.Empleados
                              .AsNoTracking()
                              .Where(i => i.emp_id == empid);

  
            var itemToReturn = items.FirstOrDefault();

            OnEmpleadoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEmpleadoCreated(Nomina.Models.dbNomina.Empleado item);
        partial void OnAfterEmpleadoCreated(Nomina.Models.dbNomina.Empleado item);

        public async Task<Nomina.Models.dbNomina.Empleado> CreateEmpleado(Nomina.Models.dbNomina.Empleado empleado)
        {
            OnEmpleadoCreated(empleado);

            var existingItem = Context.Empleados
                              .Where(i => i.emp_id == empleado.emp_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Empleados.Add(empleado);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(empleado).State = EntityState.Detached;
                throw;
            }

            OnAfterEmpleadoCreated(empleado);

            return empleado;
        }

        public async Task<Nomina.Models.dbNomina.Empleado> CancelEmpleadoChanges(Nomina.Models.dbNomina.Empleado item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEmpleadoUpdated(Nomina.Models.dbNomina.Empleado item);
        partial void OnAfterEmpleadoUpdated(Nomina.Models.dbNomina.Empleado item);

        public async Task<Nomina.Models.dbNomina.Empleado> UpdateEmpleado(int empid, Nomina.Models.dbNomina.Empleado empleado)
        {
            OnEmpleadoUpdated(empleado);

            var itemToUpdate = Context.Empleados
                              .Where(i => i.emp_id == empleado.emp_id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(empleado);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEmpleadoUpdated(empleado);

            return empleado;
        }

        partial void OnEmpleadoDeleted(Nomina.Models.dbNomina.Empleado item);
        partial void OnAfterEmpleadoDeleted(Nomina.Models.dbNomina.Empleado item);

        public async Task<Nomina.Models.dbNomina.Empleado> DeleteEmpleado(int empid)
        {
            var itemToDelete = Context.Empleados
                              .Where(i => i.emp_id == empid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEmpleadoDeleted(itemToDelete);


            Context.Empleados.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEmpleadoDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCargosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbnomina/cargos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbnomina/cargos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCargosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbnomina/cargos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbnomina/cargos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCargosRead(ref IQueryable<Nomina.Models.dbNomina.Cargo> items);

        public async Task<IQueryable<Nomina.Models.dbNomina.Cargo>> GetCargos(Query query = null)
        {
            var items = Context.Cargos.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnCargosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCargoGet(Nomina.Models.dbNomina.Cargo item);

        public async Task<Nomina.Models.dbNomina.Cargo> GetCargoByCarId(int carid)
        {
            var items = Context.Cargos
                              .AsNoTracking()
                              .Where(i => i.car_id == carid);

  
            var itemToReturn = items.FirstOrDefault();

            OnCargoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCargoCreated(Nomina.Models.dbNomina.Cargo item);
        partial void OnAfterCargoCreated(Nomina.Models.dbNomina.Cargo item);

        public async Task<Nomina.Models.dbNomina.Cargo> CreateCargo(Nomina.Models.dbNomina.Cargo cargo)
        {
            OnCargoCreated(cargo);

            var existingItem = Context.Cargos
                              .Where(i => i.car_id == cargo.car_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Cargos.Add(cargo);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(cargo).State = EntityState.Detached;
                throw;
            }

            OnAfterCargoCreated(cargo);

            return cargo;
        }

        public async Task<Nomina.Models.dbNomina.Cargo> CancelCargoChanges(Nomina.Models.dbNomina.Cargo item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCargoUpdated(Nomina.Models.dbNomina.Cargo item);
        partial void OnAfterCargoUpdated(Nomina.Models.dbNomina.Cargo item);

        public async Task<Nomina.Models.dbNomina.Cargo> UpdateCargo(int carid, Nomina.Models.dbNomina.Cargo cargo)
        {
            OnCargoUpdated(cargo);

            var itemToUpdate = Context.Cargos
                              .Where(i => i.car_id == cargo.car_id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(cargo);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCargoUpdated(cargo);

            return cargo;
        }

        partial void OnCargoDeleted(Nomina.Models.dbNomina.Cargo item);
        partial void OnAfterCargoDeleted(Nomina.Models.dbNomina.Cargo item);

        public async Task<Nomina.Models.dbNomina.Cargo> DeleteCargo(int carid)
        {
            var itemToDelete = Context.Cargos
                              .Where(i => i.car_id == carid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCargoDeleted(itemToDelete);


            Context.Cargos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCargoDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}