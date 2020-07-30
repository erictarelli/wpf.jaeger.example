using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Alumnado.Services
{
    public interface IAlumnadoService
    {
         Task<string> getAlumnos();
    }
}
