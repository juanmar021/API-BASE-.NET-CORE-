using Microsoft.AspNetCore.Http;
using Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Base
{
    public class CustomResponse
    {


        public bool okay { get; set; }

        public object data { get; set; }

        public string msg { get; set; }

        public ErrorResponse error { get; set; }


        public static CustomResponse notFound(string msg)
        {

            return new CustomResponse()
            {
                okay = false,
                error = new ErrorResponse()
                {
                    codError = StatusCodes.Status404NotFound,
                    msgError = msg ?? ResponseMsg.DATA_NOT_FOUND.Value

                }
            };
        }

        public static CustomResponse badRequest(string msg)
        {

            return new CustomResponse()
            {
                okay = false,
                error = new ErrorResponse()
                {
                    codError = StatusCodes.Status400BadRequest,
                    msgError = msg ?? ResponseMsg.BAD_REQUEST.Value

                }
            };
        }


        public static CustomResponse ok(object data, string msg = "")
        {

            return new CustomResponse()
            {
                okay = true,
                data = data,
                msg = msg
            };
        }


        public static CustomResponse DuplicatedId()
        {

            return new CustomResponse()
            {
                okay = false,
                data = null,
                error = new ErrorResponse()
                {
                    codError = StatusCodes.Status400BadRequest,
                    msgError = "El ID esta duplicado"

                }
            };
        }



    }
}
