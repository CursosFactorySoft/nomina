using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nomina.Models.dbNomina
{
    [Table("cargos", Schema = "public")]
    public partial class Cargo
    {
        [Key]
        [Required]
        public int car_id { get; set; }

        [Required]
        public string car_ccodigo { get; set; }

        [Required]
        public string car_nombre { get; set; }

        [Required]
        public decimal car_salrio { get; set; }

    }
}