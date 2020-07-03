using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Base;

namespace API.Base.Controllers
{
    [Produces("application/json")]
    [Route("api/Base")]
    public class BaseController : Controller
    {
       

        /// <summary>
        /// Informa sobre el estado del servicio
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // GET: Authentication

        [Route("Health")]
        [ProducesResponseType(typeof(HealthModel), 200)]
        public  IActionResult Gethealth()
        {
            HealthModel health = new HealthModel { date = DateTime.Now, status = "OK", GUID = Guid.NewGuid().ToString() };

            return Ok(health);
        }

        protected CustomResponse GetResponse(object data,string type, string status)
        {
            CustomResponse r = new CustomResponse();
            //r.okay = status;
           // r.type = type;
            r.data = data;

            return r;
            

        }


    }
}