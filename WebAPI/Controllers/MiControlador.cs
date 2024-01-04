using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MiControlador : ControllerBase
    {        
        [HttpGet(Name = "getEstados")]
        public async Task<ActionResult> GetEstado()
        {
            //Creo lista donde guardo las urls
            List<string> URLS = new List<string> 
            { 
                "https://mercadopago.bistrosoft.com/api/check", 
                "https://modo.bistrosoft.com/api/v1/check",
                "https://multidelivery.bistrosoft.com/api/check",
                "https://mx-clip.bistrosoft.com/api/v1.0/check"
            };

            //Creo lista de nombres
            List<string> nombres = new List<string> 
            { 
                "Mercado Pago", 
                "Modo",
                "Multidelivery",
                "Clip"
            };

            // Crea un nuevo cliente HTTP
            HttpClient client = new HttpClient();

            //Creo una lista dinamica de objetos de la clase Statuses
            List<Statuses> statuses = new List<Statuses>();

            //Defino variables
            int i = 0;
            string estado = "";

            //Bucle que recorre cada URL
            foreach (string url in URLS)
            {
                //Realiza una solicitud HTTP a la URL
                HttpResponseMessage respuesta_http = await client.GetAsync(url);

                //Almaceno en un string la respuesta HTTP (de manera asincronica)
                var contenido = await respuesta_http.Content.ReadAsStringAsync();

                //Guardo CONTENIDO en un objeto llamado producto de la clase Producto.
                StatusesDTO? producto = JsonSerializer.Deserialize<StatusesDTO>(contenido);

                //Le asigno segun corresponda un valor a estado.                
                if(producto?.responseCode == 0)
                    estado = "OK";
                else if (producto?.responseCode == -1)
                    estado = "ERROR";

                //Creo un objeto donde guardo los datos leidos para despues añadirlo a mi lista.
                Statuses status = new Statuses
                {
                    PlataformName = nombres[i],
                    version = producto?.version,
                    Status = estado,
                };

                //Añado el producto leido a mi lista: STATUSES
                statuses.Add(status);

                //Sumo el contador para pasar al siguiente nombre
                i++;
            }

            //Devuelvo un json con mi lista statuses
            return Ok(new { statuses = statuses });
        }
    }
}