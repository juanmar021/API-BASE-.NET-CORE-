using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SGS.DataContext.Context;
using SGS.Models.ShipRequest;

namespace SGS.Service.ShipRequest
{
    public class ShipRequestService : IShipRequestService
    {
        private DBContextSGS _context;

        public ShipRequestService(DBContextSGS contextSGS)
        {
            _context = contextSGS;
        }

        public ShipRequestModel Get(string OriginCode, string NumberApplication)
        {
            try
            {
                using (var conn = (SqlConnection)_context.Database.GetDbConnection())
                {
                    conn.Open();
                    ShipRequestModel shipRequestModel = null;

                    SqlCommand cmd = new SqlCommand("UP_WEB_QCL_OBTENER_SOLICITUDES", conn);
                    cmd.Parameters.AddWithValue("@OriCod", OriginCode);
                    if (string.IsNullOrEmpty(NumberApplication))
                    {
                        cmd.Parameters.AddWithValue("@NumSol", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NumSol", NumberApplication);
                    }
                    cmd.Parameters.AddWithValue("@CiaCod", DBNull.Value);
                    cmd.Parameters.AddWithValue("@SolEst", DBNull.Value);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            shipRequestModel = new ShipRequestModel()
                            {
                                OriginCode = reader.GetString(0),
                                NumberApplication = reader.GetString(1),
                                CustomerCode = reader.GetString(2),
                                Vessel = reader.GetString(3),
                                PuertLoad = reader.GetString(4),
                                PortDownload = reader.GetString(5),
                                CityLoad = reader.GetString(6),
                                DestinationCity = reader.GetString(8),
                                DestinationCountry = reader.GetString(7),
                                Quality = reader.GetString(9),
                                ShipReference = reader.GetString(10),
                                DispatchCode = !reader.IsDBNull(12) ? reader.GetString(12) : "",
                                EstimatedDate = reader.GetDateTime(11),
                                Status = reader.GetInt16(13),
                                Tonnage = reader.GetDecimal(14),
                                Observations = reader.GetString(15),
                                User = reader.GetString(16),
                                DateRequest = reader.GetDateTime(17),
                                AnalysisSpecification = new List<AnalysisShipRequest>(),
                                Associations = new List<ShipRequestAssociationModel>()
                            };
                        }
                    }

                    if (shipRequestModel != null)
                    {
                        SqlCommand cmd2 = new SqlCommand("UP_WEB_QCL_OBTENER_DETALLE_SOLICITUD", conn);
                        cmd2.Parameters.AddWithValue("@OriCod", OriginCode);
                        if (string.IsNullOrEmpty(NumberApplication))
                        {
                            cmd2.Parameters.AddWithValue("@NumSol", DBNull.Value);
                        }
                        else
                        {
                            cmd2.Parameters.AddWithValue("@NumSol", NumberApplication);
                        }
                        cmd2.Parameters.AddWithValue("@ConDetSol", DBNull.Value);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        using (var reader = cmd2.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var model = new AnalysisShipRequest()
                                    {
                                        CodeAnalysisShipRequest = reader.GetString(3),
                                        AnalysisCode = reader.GetString(4),
                                        Base = reader.GetString(5)
                                    };
                                    if (!reader.IsDBNull(6))
                                    {
                                        model.GL = reader.GetDecimal(6);
                                    }
                                    if (!reader.IsDBNull(7))
                                    {
                                        model.RL = reader.GetDecimal(7);
                                    }
                                    if (!reader.IsDBNull(8))
                                    {
                                        model.PL = reader.GetDecimal(8);
                                    }
                                    shipRequestModel.AnalysisSpecification.Add(model);
                                }
                            }
                        }

                        SqlCommand cmd3 = new SqlCommand("up_web_qcl_dev_EncCgfBuqResAso", conn);
                        cmd3.Parameters.AddWithValue("@OriCod", OriginCode);
                        if (string.IsNullOrEmpty(NumberApplication))
                        {
                            cmd3.Parameters.AddWithValue("@NumSol", DBNull.Value);
                        }
                        else
                        {
                            cmd3.Parameters.AddWithValue("@NumSol", NumberApplication);
                        }
                        cmd3.Parameters.AddWithValue("@CiaCod", shipRequestModel.CustomerCode);
                        cmd3.Parameters.AddWithValue("@ConAso", DBNull.Value);
                        cmd3.CommandType = CommandType.StoredProcedure;

                        using (var reader = cmd3.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    shipRequestModel.Associations.Add(new ShipRequestAssociationModel()
                                    {
                                        CodigoAssociation = reader.GetInt32(3),
                                        ComercialOrder = reader.GetInt32(4),
                                        InspectionOrder = reader.GetInt32(5)
                                    });
                                }
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                    return shipRequestModel;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ShipRequestModel> GetAll(string OriginCode, string NumberApplication, string CustomerCode, short? Status)
        {
            try
            {

                using (var conn = (SqlConnection)_context.Database.GetDbConnection())
                {
                    conn.Open();
                    List<ShipRequestModel> shipRequests = new List<ShipRequestModel>();

                    SqlCommand cmd = new SqlCommand("UP_WEB_QCL_OBTENER_SOLICITUDES", conn);
                    cmd.Parameters.AddWithValue("@OriCod", OriginCode);
                    if (string.IsNullOrEmpty(NumberApplication))
                    {
                        cmd.Parameters.AddWithValue("@NumSol", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NumSol", NumberApplication);
                    }
                    if (string.IsNullOrEmpty(NumberApplication))
                    {
                        cmd.Parameters.AddWithValue("@CiaCod", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CiaCod", NumberApplication);
                    }
                    if (Status == null)
                    {
                        cmd.Parameters.AddWithValue("@SolEst", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SolEst", Status);
                    }

                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var shipRequestModel = new ShipRequestModel()
                                {
                                    OriginCode = reader.GetString(0),
                                    NumberApplication = reader.GetString(1),
                                    CustomerCode = reader.GetString(2),
                                    Vessel = reader.GetString(3),
                                    PuertLoad = reader.GetString(4),
                                    PortDownload = reader.GetString(5),
                                    CityLoad = reader.GetString(6),
                                    DestinationCity = reader.GetString(8),
                                    DestinationCountry = reader.GetString(7),
                                    Quality = reader.GetString(9),
                                    ShipReference = reader.GetString(10),
                                    DispatchCode = !reader.IsDBNull(12) ? reader.GetString(12) : "",
                                    EstimatedDate = reader.GetDateTime(11),
                                    Status = reader.GetInt16(13),
                                    Tonnage = reader.GetDecimal(14),
                                    Observations = reader.GetString(15),
                                    User = reader.GetString(16),
                                    DateRequest = reader.GetDateTime(17),
                                    AnalysisSpecification = new List<AnalysisShipRequest>(),
                                    Associations = new List<ShipRequestAssociationModel>()
                                };
                                shipRequests.Add(shipRequestModel);
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                    return shipRequests;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AddRequest(ShipRequestModel model)
        {
            try
            {
                string numberSolicitud = "";
                using (var cnn = _context.Database.GetDbConnection())
                {
                    cnn.Open();
                    using (var cmd = cnn.CreateCommand())
                    {

                        cmd.CommandText = "UP_WEB_QCL_INS_SOLICITUD_ENCPARA_BUQUES";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@OriCod", SqlDbType.VarChar) { Value = model.OriginCode });
                        cmd.Parameters.Add(new SqlParameter("@NumSol", SqlDbType.VarChar) { Value = "0" });
                        cmd.Parameters.Add(new SqlParameter("@CiaCod", SqlDbType.VarChar) { Value = model.CustomerCode });
                        cmd.Parameters.Add(new SqlParameter("@DesBuq", SqlDbType.VarChar) { Value = model.Vessel });
                        cmd.Parameters.Add(new SqlParameter("@PueCar", SqlDbType.VarChar) { Value = model.PuertLoad });
                        cmd.Parameters.Add(new SqlParameter("@PueDes", SqlDbType.VarChar) { Value = model.PortDownload });
                        cmd.Parameters.Add(new SqlParameter("@CiuCar", SqlDbType.VarChar) { Value = model.CityLoad });
                        cmd.Parameters.Add(new SqlParameter("@PaiDes", SqlDbType.VarChar) { Value = model.DestinationCountry });
                        cmd.Parameters.Add(new SqlParameter("@CiuDes", SqlDbType.VarChar) { Value = model.DestinationCity });
                        cmd.Parameters.Add(new SqlParameter("@CalPro", SqlDbType.VarChar) { Value = model.Quality });
                        cmd.Parameters.Add(new SqlParameter("@RefBuqCod", SqlDbType.VarChar) { Value = model.ShipReference });
                        cmd.Parameters.Add(new SqlParameter("@DesCod", SqlDbType.VarChar) { Value = model.DispatchCode });
                        cmd.Parameters.Add(new SqlParameter("@FecEta", SqlDbType.SmallDateTime) { Value = Convert.ToDateTime(model.EstimatedDate) });
                        cmd.Parameters.Add(new SqlParameter("@SolEst", SqlDbType.Int) { Value = model.Status });
                        cmd.Parameters.Add(new SqlParameter("@TonSol", SqlDbType.Decimal) { Value = Convert.ToDecimal(model.Tonnage) });
                        cmd.Parameters.Add(new SqlParameter("@ObsSol", SqlDbType.VarChar) { Value = model.Observations });
                        cmd.Parameters.Add(new SqlParameter("@UsuSol", SqlDbType.VarChar) { Value = model.User });
                        cmd.Parameters.Add(new SqlParameter("@TipOer", SqlDbType.VarChar) { Value = "I" });

                        numberSolicitud = cmd.ExecuteScalar().ToString();
                        if (!string.IsNullOrEmpty(numberSolicitud))
                        {
                            foreach (var analysis in model.AnalysisSpecification)
                            {
                                using (var cmd2 = cnn.CreateCommand())
                                {
                                    cmd2.CommandText = "UP_WEB_QCL_INS_SOLICITUD_DETPARA_BUQUES";
                                    cmd2.CommandType = CommandType.StoredProcedure;

                                    cmd2.Parameters.Add(new SqlParameter("@OriCod", SqlDbType.VarChar) { Value = model.OriginCode });
                                    cmd2.Parameters.Add(new SqlParameter("@NumSol", SqlDbType.VarChar) { Value = numberSolicitud });
                                    cmd2.Parameters.Add(new SqlParameter("@CiaCod", SqlDbType.VarChar) { Value = model.CustomerCode });
                                    cmd2.Parameters.Add(new SqlParameter("@ConDetSol", SqlDbType.VarChar) { Value = "0" });
                                    cmd2.Parameters.Add(new SqlParameter("@SchCod", SqlDbType.VarChar) { Value = analysis.AnalysisCode });
                                    cmd2.Parameters.Add(new SqlParameter("@AniRepCod", SqlDbType.VarChar) { Value = analysis.Base });
                                    cmd2.Parameters.Add(new SqlParameter("@RslVal", SqlDbType.VarChar) { Value = (analysis.GL != null) ? analysis.GL.Value.ToString(".##") : string.Empty });
                                    cmd2.Parameters.Add(new SqlParameter("@RslValPen", SqlDbType.VarChar) { Value = (analysis.RL != null) ? analysis.RL.Value.ToString(".##") : string.Empty });
                                    cmd2.Parameters.Add(new SqlParameter("@RslValPrm", SqlDbType.VarChar) { Value = (analysis.PL != null) ? analysis.PL.Value.ToString(".##") : string.Empty });
                                    cmd2.Parameters.Add(new SqlParameter("@TipOpe", SqlDbType.VarChar) { Value = "I" });
                                    cmd2.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Error al crear la solicitud");
                        }

                    }
                }
                return numberSolicitud;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRequest(string NumberApplication, ShipRequestModel model)
        {
            try
            {
                using (var cnn = _context.Database.GetDbConnection())
                {
                    cnn.Open();
                    using (var cmd = cnn.CreateCommand())
                    {
                        cmd.CommandText = "UP_WEB_QCL_INS_SOLICITUD_ENCPARA_BUQUES";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@OriCod", SqlDbType.VarChar) { Value = model.OriginCode });
                        cmd.Parameters.Add(new SqlParameter("@NumSol", SqlDbType.VarChar) { Value = NumberApplication });
                        cmd.Parameters.Add(new SqlParameter("@CiaCod", SqlDbType.VarChar) { Value = model.CustomerCode });
                        cmd.Parameters.Add(new SqlParameter("@DesBuq", SqlDbType.VarChar) { Value = model.Vessel });
                        cmd.Parameters.Add(new SqlParameter("@PueCar", SqlDbType.VarChar) { Value = model.PuertLoad });
                        cmd.Parameters.Add(new SqlParameter("@PueDes", SqlDbType.VarChar) { Value = model.PortDownload });
                        cmd.Parameters.Add(new SqlParameter("@CiuCar", SqlDbType.VarChar) { Value = model.CityLoad });
                        cmd.Parameters.Add(new SqlParameter("@PaiDes", SqlDbType.VarChar) { Value = model.DestinationCountry });
                        cmd.Parameters.Add(new SqlParameter("@CiuDes", SqlDbType.VarChar) { Value = model.DestinationCity });
                        cmd.Parameters.Add(new SqlParameter("@CalPro", SqlDbType.VarChar) { Value = model.Quality });
                        cmd.Parameters.Add(new SqlParameter("@RefBuqCod", SqlDbType.VarChar) { Value = model.ShipReference });
                        cmd.Parameters.Add(new SqlParameter("@DesCod", SqlDbType.VarChar) { Value = model.DispatchCode });
                        cmd.Parameters.Add(new SqlParameter("@FecEta", SqlDbType.SmallDateTime) { Value = Convert.ToDateTime(model.EstimatedDate) });
                        cmd.Parameters.Add(new SqlParameter("@SolEst", SqlDbType.Int) { Value = model.Status });
                        cmd.Parameters.Add(new SqlParameter("@TonSol", SqlDbType.Decimal) { Value = Convert.ToDecimal(model.Tonnage) });
                        cmd.Parameters.Add(new SqlParameter("@ObsSol", SqlDbType.VarChar) { Value = model.Observations });
                        cmd.Parameters.Add(new SqlParameter("@UsuSol", SqlDbType.VarChar) { Value = model.User });
                        cmd.Parameters.Add(new SqlParameter("@TipOer", SqlDbType.VarChar) { Value = "A" });

                        cmd.ExecuteNonQuery();

                        if (model.AnalysisSpecification != null)
                        {
                            model.AnalysisSpecification.ForEach(x => x.CodeAnalysisShipRequest = x.CodeAnalysisShipRequest.Trim());
                            List<string> analysisConList = model.AnalysisSpecification.Where(x => !string.IsNullOrEmpty(x.CodeAnalysisShipRequest)).Select(x => x.CodeAnalysisShipRequest).ToList();
                            using (var cmd0 = cnn.CreateCommand())
                            {
                                cmd0.CommandText = "UP_WEB_QCL_VERIFY_REQUEST_ANALYSIS_REMOVAL";
                                cmd0.CommandType = CommandType.StoredProcedure;

                                cmd0.Parameters.Add(new SqlParameter("@NumSol", SqlDbType.NVarChar) { Value = NumberApplication });
                                cmd0.Parameters.Add(new SqlParameter("@ConDetSolList", SqlDbType.NVarChar) { Value = string.Join(',', analysisConList) });
                                cmd0.ExecuteNonQuery();
                            }

                            foreach (var analysis in model.AnalysisSpecification)
                            {
                                using (var cmd2 = cnn.CreateCommand())
                                {
                                    cmd2.CommandText = "UP_WEB_QCL_INS_SOLICITUD_DETPARA_BUQUES";
                                    cmd2.CommandType = CommandType.StoredProcedure;

                                    cmd2.Parameters.Add(new SqlParameter("@OriCod", SqlDbType.VarChar) { Value = model.OriginCode });
                                    cmd2.Parameters.Add(new SqlParameter("@NumSol", SqlDbType.VarChar) { Value = NumberApplication });
                                    cmd2.Parameters.Add(new SqlParameter("@CiaCod", SqlDbType.VarChar) { Value = model.CustomerCode });
                                    cmd2.Parameters.Add(new SqlParameter("@ConDetSol", SqlDbType.VarChar) { Value = (string.IsNullOrEmpty(analysis.CodeAnalysisShipRequest)) ? "0" : analysis.CodeAnalysisShipRequest });
                                    cmd2.Parameters.Add(new SqlParameter("@SchCod", SqlDbType.VarChar) { Value = analysis.AnalysisCode });
                                    cmd2.Parameters.Add(new SqlParameter("@AniRepCod", SqlDbType.VarChar) { Value = analysis.Base });
                                    cmd2.Parameters.Add(new SqlParameter("@RslVal", SqlDbType.VarChar) { Value = (analysis.GL != null) ? analysis.GL.Value.ToString(".##") : string.Empty });
                                    cmd2.Parameters.Add(new SqlParameter("@RslValPen", SqlDbType.VarChar) { Value = (analysis.RL != null) ? analysis.RL.Value.ToString(".##") : string.Empty });
                                    cmd2.Parameters.Add(new SqlParameter("@RslValPrm", SqlDbType.VarChar) { Value = (analysis.PL != null) ? analysis.PL.Value.ToString(".##") : string.Empty });
                                    cmd2.Parameters.Add(new SqlParameter("@TipOpe", SqlDbType.VarChar) { Value = (string.IsNullOrEmpty(analysis.CodeAnalysisShipRequest)) ? "I" : "A" });
                                    cmd2.ExecuteNonQuery();
                                }
                            }
                        }

                        if (model.Associations != null && model.Associations.Count > 0)
                        {
                            List<string> associationConList = model.Associations.Where(x => x.CodigoAssociation != 0).Select(x => x.CodigoAssociation.ToString()).ToList();
                            using (var cmd0 = cnn.CreateCommand())
                            {
                                cmd0.CommandText = "UP_WEB_QCL_VERIFY_REQUEST_OL_REMOVAL";
                                cmd0.CommandType = CommandType.StoredProcedure;

                                cmd0.Parameters.Add(new SqlParameter("@NumSol", SqlDbType.NVarChar) { Value = NumberApplication });
                                cmd0.Parameters.Add(new SqlParameter("@ConAsoList", SqlDbType.NVarChar) { Value = string.Join(',', associationConList) });
                                cmd0.ExecuteNonQuery();
                            }

                            foreach (var association in model.Associations)
                            {
                                using (var cmd3 = cnn.CreateCommand())
                                {
                                    cmd3.CommandText = "up_web_qcl_reg_EncCgfBuqResAso";
                                    cmd3.CommandType = CommandType.StoredProcedure;

                                    cmd3.Parameters.Add(new SqlParameter("@OriCod", SqlDbType.VarChar) { Value = model.OriginCode });
                                    cmd3.Parameters.Add(new SqlParameter("@NumSol", SqlDbType.VarChar) { Value = NumberApplication });
                                    cmd3.Parameters.Add(new SqlParameter("@CiaCod", SqlDbType.VarChar) { Value = model.CustomerCode });
                                    cmd3.Parameters.Add(new SqlParameter("@OcmNum", SqlDbType.Int) { Value = Convert.ToInt32(association.ComercialOrder) });
                                    cmd3.Parameters.Add(new SqlParameter("@OenNum", SqlDbType.Int) { Value = Convert.ToInt32(association.InspectionOrder) });
                                    cmd3.Parameters.Add(new SqlParameter("@LogUsu", SqlDbType.VarChar) { Value = model.User });
                                    if (association.CodigoAssociation != 0)
                                    {
                                        cmd3.Parameters.Add(new SqlParameter("@ConAso", SqlDbType.Int) { Value = association.CodigoAssociation });
                                        cmd3.Parameters.Add(new SqlParameter("@TipOer", SqlDbType.VarChar) { Value = "A" });
                                    }
                                    else
                                    {
                                        cmd3.Parameters.Add(new SqlParameter("@ConAso", SqlDbType.Int) { Value = 0 });
                                        cmd3.Parameters.Add(new SqlParameter("@TipOer", SqlDbType.VarChar) { Value = "I" });
                                    }
                                    cmd3.ExecuteNonQuery();
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
