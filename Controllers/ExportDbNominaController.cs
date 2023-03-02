using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Nomina.Data;

namespace Nomina.Controllers
{
    public partial class ExportdbNominaController : ExportController
    {
        private readonly dbNominaContext context;
        private readonly dbNominaService service;

        public ExportdbNominaController(dbNominaContext context, dbNominaService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/dbNomina/empleados/csv")]
        [HttpGet("/export/dbNomina/empleados/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmpleadosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEmpleados(), Request.Query), fileName);
        }

        [HttpGet("/export/dbNomina/empleados/excel")]
        [HttpGet("/export/dbNomina/empleados/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmpleadosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEmpleados(), Request.Query), fileName);
        }

        [HttpGet("/export/dbNomina/cargos/csv")]
        [HttpGet("/export/dbNomina/cargos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCargosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCargos(), Request.Query), fileName);
        }

        [HttpGet("/export/dbNomina/cargos/excel")]
        [HttpGet("/export/dbNomina/cargos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCargosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCargos(), Request.Query), fileName);
        }
    }
}
