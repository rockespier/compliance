using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance.Application.UseCases
{
    public record EvidenciaDto(
     string NombreDocumento,
     string UrlReferencia,
     string Comentario
    );
}
