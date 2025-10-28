using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Controller.Private.Caja
{
    public class CajaControllerCreateOneDto
    {
        public required short EstadoId { get; set; }
        public required string Codigo { get; set; }
        public required string Nombre { get; set; }
        public bool Activo { get; set; } = false;
    }
}