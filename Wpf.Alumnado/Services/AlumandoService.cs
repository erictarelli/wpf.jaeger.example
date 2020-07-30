using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Alumnado.Services
{
    public class AlumandoService : IAlumnadoService
    {
        public async Task<string> getAlumnos()
        {
            try
            {
                HttpClient _client = new HttpClient();

                var response = await _client.GetAsync(Constant.BaseAdressServices.URI_ALUMNADO);

                return "";
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
