using SGS.Models.ShipRequest;
using System;
using System.Collections.Generic;
using System.Text;

namespace SGS.Service.ShipRequest
{
    public interface IShipRequestService
    {
        ShipRequestModel Get(string OriginCode, string NumberApplication);
        List<ShipRequestModel> GetAll(string OriginCode, string NumberApplication, string CustomerCode, short? Status);
        string AddRequest(ShipRequestModel model);
        void UpdateRequest(string NumberApplication, ShipRequestModel model);
    }
}
