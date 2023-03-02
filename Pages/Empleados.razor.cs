using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Nomina.Pages
{
    public partial class Empleados
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public dbNominaService dbNominaService { get; set; }

        protected IEnumerable<Nomina.Models.dbNomina.Empleado> empleados;

        protected RadzenDataGrid<Nomina.Models.dbNomina.Empleado> grid0;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            empleados = await dbNominaService.GetEmpleados(new Query { Filter = $@"i => i.emp_apellidos.Contains(@0) || i.emp_nombres.Contains(@0) || i.emp_ccorreo.Contains(@0) || i.emp_cdireccion.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            empleados = await dbNominaService.GetEmpleados(new Query { Filter = $@"i => i.emp_apellidos.Contains(@0) || i.emp_nombres.Contains(@0) || i.emp_ccorreo.Contains(@0) || i.emp_cdireccion.Contains(@0)", FilterParameters = new object[] { search } });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddEmpleado>("Add Empleado", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<Nomina.Models.dbNomina.Empleado> args)
        {
            await DialogService.OpenAsync<EditEmpleado>("Edit Empleado", new Dictionary<string, object> { {"emp_id", args.Data.emp_id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, Nomina.Models.dbNomina.Empleado empleado)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await dbNominaService.DeleteEmpleado(empleado.emp_id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete Empleado" 
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await dbNominaService.ExportEmpleadosToCSV(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible()).Select(c => c.Property))
}, "Empleados");
            }

            if (args == null || args.Value == "xlsx")
            {
                await dbNominaService.ExportEmpleadosToExcel(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible()).Select(c => c.Property))
}, "Empleados");
            }
        }
    }
}