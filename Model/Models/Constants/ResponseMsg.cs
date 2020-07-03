using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Constants
{
    public class ResponseMsg
    {
        private ResponseMsg(string value) { Value = value; }

        public string Value { get; set; }

        public static ResponseMsg RECORD_DELETED { get { return new ResponseMsg("RECORD_DELETED"); } }
        public static ResponseMsg CONFLIT_REFERENCE { get { return new ResponseMsg("CONFLIT_REFERENCE"); } }
        public static ResponseMsg RECORD_SAVED { get { return new ResponseMsg("RECORD_SAVED"); } }
        public static ResponseMsg RECORD_UPDATED { get { return new ResponseMsg("RECORD_UPDATED"); } }
        public static ResponseMsg MODEL_INVALID { get { return new ResponseMsg("MODEL_INVALID"); } }
        public static ResponseMsg DATA_NOT_FOUND { get { return new ResponseMsg("DATA_NOT_FOUND"); } }
        public static ResponseMsg BAD_REQUEST { get { return new ResponseMsg("BAD_REQUEST"); } }
    }
}
