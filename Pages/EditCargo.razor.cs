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
    public partial class EditCargo
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

        [Parameter]
        public int car_id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            cargo = await dbNominaService.GetCargoByCarId(car_id);
        }
        protected bool errorVisible;
        protected Nomina.Models.dbNomina.Cargo cargo;

        protected async Task FormSubmit()
        {
            try
            {
                await dbNominaService.UpdateCargo(car_id, cargo);
                DialogService.Close(cargo);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
           dbNominaService.Reset();
            hasChanges = false;
            canEdit = true;

            cargo = await dbNominaService.GetCargoByCarId(car_id);
        }
    }
}