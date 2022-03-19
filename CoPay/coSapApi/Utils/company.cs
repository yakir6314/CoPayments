using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace coSapApi.Utils
{

    public  class company
    {
        public static SAPbobsCOM.Company SapContext { get; set; }
        public static void Connect(bool isTest)
        {
            if (SapContext != null && SapContext.Connected)
            {
                SapContext.Disconnect();
            }
            SapContext = new SAPbobsCOM.Company();
            SapContext.Server = Environment.GetEnvironmentVariable("Server").ToString();
            SapContext.DbUserName = Environment.GetEnvironmentVariable("DBUser").ToString();
            SapContext.DbPassword = Environment.GetEnvironmentVariable("DBPassword").ToString();
            SapContext.UserName = Environment.GetEnvironmentVariable("SapUser").ToString();
            SapContext.Password = Environment.GetEnvironmentVariable("SapPassword").ToString();
            SapContext.CompanyDB = isTest ? Environment.GetEnvironmentVariable("DBTestName").ToString() : Environment.GetEnvironmentVariable("DBName").ToString();
            

            string serverType = Environment.GetEnvironmentVariable("ServerType").ToString();
            switch (serverType)
            {
                case "2017":
                    SapContext.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017;
                    break;
                case "2016":
                    SapContext.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
                    break;
                case "2014":
                    SapContext.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
                    break;
                case "2012":
                    SapContext.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                    break;
                case "2008":
                    SapContext.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008;
                    break;
                case "2005":
                    SapContext.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005;
                    break;
                default:
                    SapContext.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                    break;
            }

            int responseCode = SapContext.Connect();
            if (responseCode != 0)
            {
                string errorMsg = $"Error {responseCode}: sap connection failure: {SapContext.GetLastErrorDescription()}";
                SapContext = null;
                throw new SapConnectionFailureException(errorMsg);
            }
        }
    }
    [Serializable]
    internal class SapConnectionFailureException : Exception
    {
        public SapConnectionFailureException()
        {
            string errMsg = "Error: sap connection failure";
            throw new Exception(errMsg);
        }

        public SapConnectionFailureException(string message) : base(message)
        {

        }

        public SapConnectionFailureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SapConnectionFailureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
