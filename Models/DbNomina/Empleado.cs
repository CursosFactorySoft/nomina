using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nomina.Models.dbNomina
{
    [Table("empleados", Schema = "public")]
    public partial class Empleado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_id { get; set; }

        [Required]
        public string emp_apellidos { get; set; }

        [Required]
        public string emp_nombres { get; set; }

        public string emp_ccorreo { get; set; }

        public string emp_cdireccion { get; set; }

    }
}