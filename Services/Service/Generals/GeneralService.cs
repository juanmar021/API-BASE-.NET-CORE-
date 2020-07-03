using DataContext.Context;
using DataContext.Model;
using Microsoft.Extensions.Options;
using Models;
using Models.Base;
using Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Models.Constants.StatusModels;

namespace Service.Generals
{
    public class GeneralService : IGeneralService
    {


        private readonly DBContext contextDB;
        private readonly IOptions<AppSettings> appSetings;
        public GeneralService(DBContext context, IOptions<AppSettings> options)
        {
            this.contextDB = context;
            this.appSetings = options;
        }



        public int Add(GeneralModel model)
        {
            try
            {
                var modelInsert = new General()
                {
                    name = model.name,
                    group = model.group,
                    status = (int) Status.ENABLED
                };

                contextDB.Generals.Add(modelInsert);

                contextDB.SaveChanges();

                return modelInsert.id;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public List<GeneralModel> GetAll(string group)
        {
            try
            {
                return contextDB.Generals.Where(x => x.group == group && x.status == (int)Status.ENABLED)
                    .Select(x => new GeneralModel()
                    {
                    id = x.id,
                    name = x.name,
                    group = x.group,
                    status = x.status
                    }).ToList();

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

 

        public int Update(GeneralModel model)
        {
            try
            {
                var modelUpdate = new General()
                {
                    id = model.id,
                    name = model.name,
                    group = model.group,
                    status = model.status
                };


                contextDB.Entry(modelUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                contextDB.SaveChanges();


                return modelUpdate.id;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        public string Delete(int id)
        {
            try
            {

                var model = contextDB.Generals.FirstOrDefault(x => x.id == id);

                if (model == null)
                {
                    return null;
                }

                model.status =(int) Status.DISABLED;

                contextDB.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                contextDB.SaveChanges();

                return ResponseMsg.RECORD_DELETED.Value;

            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
    }
}
