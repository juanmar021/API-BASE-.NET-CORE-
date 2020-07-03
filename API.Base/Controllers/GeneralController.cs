using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Service.Generals;
using Models.Base;
using Models;
using Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace API.Base.Controllers
{ 
    /// <summary>
    ///General
    /// </summary>
   // [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class GeneralController : Controller
    {

        private readonly IGeneralService _service;
        readonly ILogger<BaseController> _logger;

        /// <summary>
        /// Constructor del controlador
        /// </summary>
        /// <param name="service">Inyección del servicio</param>
        /// <param name="logger">Inyección del log</param>
        /// <param name="configuration">Inyección de la configuración global</param>
        public GeneralController(IGeneralService service, ILogger<BaseController> logger, IConfiguration configuration)
        {
            _service = service;
            _logger = logger;

        }



        /// <summary>
        /// Save a general model
        /// </summary>
        /// <returns>id  General model created</returns>
        [HttpPost, Route("Save")]
        [ProducesResponseType(typeof(CustomResponse), 200)]
        public IActionResult Save([FromBody]GeneralModel model)
        {
            try
            {
                int result = _service.Add(model);


                return Ok(CustomResponse.ok(result, ResponseMsg.RECORD_SAVED.Value));
            }
            catch (Exception ex)
            {
                if (Utils.evaluateException(ex) != null) return Ok(CustomResponse.badRequest(Utils.evaluateException(ex)));

                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Method allows GET ALL   General models
        /// </summary>
        ///<param name="group">Group required</param>
        /// <returns></returns>
        [HttpGet, Route("GetAll")]
        [ProducesResponseType(typeof(List<CustomResponse>), 200)]
        public IActionResult GetAll([Required]string group)
        {
            try
            {
                return Ok(CustomResponse.ok(_service.GetAll(group)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }


    

        /// <summary>
        /// Method allows update a General model
        /// </summary>
        /// <returns>code General model updated</returns>
        [HttpPut, Route("Update")]
        [ProducesResponseType(typeof(CustomResponse), 200)]
        public IActionResult Update([FromBody]GeneralModel model)
        {
            try
            {
                int result = _service.Update(model);


                return Ok(CustomResponse.ok(result, ResponseMsg.RECORD_UPDATED.Value));
            }
            catch (Exception ex)
            {
                if (Utils.evaluateException(ex) != null) return Ok(CustomResponse.badRequest(Utils.evaluateException(ex)));

                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Method allows delete a General model
        /// </summary>
        /// <param name="id">Id of the General model</param>
        /// <returns>result message</returns>
        [HttpDelete, Route("Delete")]
        [ProducesResponseType(typeof(CustomResponse), 200)]
        public IActionResult Delete(int id)
        {
            try
            {
                string result = _service.Delete(id);

                return Ok(Utils.evaluateResponseDelete(result));


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}
