using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Generals
{
    public interface IGeneralService
    {

        int Add(GeneralModel model);


        List<GeneralModel> GetAll(string group);


        int Update(GeneralModel model);


        string Delete(int id);

    }
}
