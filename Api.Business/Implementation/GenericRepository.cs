using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Api.Business.Interface;
using Api.Data;
using Api.Data.ServiceModels;

namespace Api.Business.Implementation
{
    public class GenericRepository : IGenericRepository
    {
        private readonly IConfigurationRoot _config;

        private readonly GenContext _context;

        public GenericRepository(GenContext context, IConfigurationRoot config) : base()
        {
            _context = context;

            _config = config;
        }

        public object GetForex(GetForexRequest request)
        {
            using (var sqlCmd = _context.Database.GetDbConnection().CreateCommand())
            {
                
                string query = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\SQL\\Forex.sql");
                
                try
                {
                    SqlParameter[] sqlParam =
                   {
                            new SqlParameter("@CurrencyCode", SqlDbType.VarChar, 20) { Value = request.CurrencyCode },
                            new SqlParameter("@ReferenceDate", SqlDbType.DateTime) { Value = request.ReferenceDate },
                            new SqlParameter("@ReferenceType", SqlDbType.VarChar,20) { Value = request.ReferenceType }
                    };

                    sqlCmd.Parameters.AddRange(sqlParam);
                    sqlCmd.CommandText = query;
                    sqlCmd.CommandType = CommandType.Text;

                    if (sqlCmd.Connection.State != ConnectionState.Open)
                        sqlCmd.Connection.Open();

                    var reader = sqlCmd.ExecuteReader();

                    if (reader != null && reader.HasRows)
                    {
                        List<Forex> list = SqlMapToForexList(reader);

                        if(list != null && list.Any())
                        {
                            var returnData = JObject.FromObject(new { data = list.FirstOrDefault() });
                        
                            return returnData;
                        }
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            return null;
        }

        

        private String SqlDatoToJson(DbDataReader dataReader)
        {
            var dataTable = new DataTable();
            dataTable.Load(dataReader);
            string JSONString = JsonConvert.SerializeObject(dataTable);
            return JSONString;
        }

        private Forex SqlMapToForex(DataRow dr)
        {

            Forex t = new Forex();
            t.CurrencyCode = dr["CurrencyCode"].ToString();
            t.ForexRate = float.Parse(dr["ForexRate"].ToString());
            t.ForexUSDtoCAD = float.Parse(dr["ForexUSDtoCAD"].ToString());
            t.ForexUSDFluctuation = float.Parse(dr["ForexUSDFluctuation"].ToString());

            return t;
        }

        private List<Forex> SqlMapToForexList(DbDataReader dataReader)
        {
            var dataTable = new DataTable();
            dataTable.Load(dataReader);
            return SqlMapToForexList(dataTable);
        }

            private List<Forex> SqlMapToForexList(DataTable dt)
        {
            List<Forex> list = new List<Forex>();
            foreach (DataRow dr in dt.Rows)
            {  
                list.Add(SqlMapToForex(dr));
            }
            return list;
        }

    }



}
