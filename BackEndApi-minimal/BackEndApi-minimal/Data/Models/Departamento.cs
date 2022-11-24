using System;
using System.Collections.Generic;

namespace BackEndApi_minimal.Data.Models
{
    public partial class Departamento
    {
        public Departamento()
        {
            Empleados = new HashSet<Empleado>();
        }

        public int IdDepartamento { get; set; }
        public string? Nombre { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}
