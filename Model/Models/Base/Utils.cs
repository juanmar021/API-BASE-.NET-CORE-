using Models.Constants;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Models.Base
{
    public  class Utils
    {
        public static CustomResponse evaluateResponseDelete(string msg)
        {
            if (msg == null) return CustomResponse.notFound(null);

            if (msg.Equals(ResponseMsg.CONFLIT_REFERENCE.Value)) return CustomResponse.badRequest(ResponseMsg.CONFLIT_REFERENCE.Value);



            return CustomResponse.ok(null, msg);


        }


        public static string evaluateException(Exception ex)
        {

            var sqlException = ex.InnerException as SqlException;

            if (sqlException != null)
            {
                switch (sqlException.Number)
                {
                    case 2601:
                        return "SQL error duplicate key";
                    case 547:
                        return "SQL conflicted with the FOREIGN KEY constraint";
                    case 8152:
                        return "One or more values are longer than the column length";
                }
            }

          return null;


        }
        public static string valuesToString(string[] values)
        {
            if (values.Length == 0) return null;

            string vals = "";

            int i = 0;
            foreach (var item in values)
            {

                vals += item;

                if (i != values.Length - 1)
                    vals += ";;;";

                i++;

            }

            return vals;


        }


        public static string[] stringToValues(string values)
        {
            if (values == null) return new string[0];

            return values.Split(";;;");

        }


         


    }
}
