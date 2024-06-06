using System;
using SAPbobsCOM;

namespace SAPWebApi24.Utils // Assurez-vous d'utiliser le bon espace de noms
{
    public class SAPConnection
    {
        public Company OpenConnection()
        {
            Company oCompany = new Company();

            try
            {
                oCompany.Server = "DESKTOP-9OCVRC9";
                oCompany.CompanyDB = "SBODemoFR";
                oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2019;
                oCompany.DbUserName = "sa";
                oCompany.DbPassword = "HQ?WQ2chqgqne2024";
                oCompany.UserName = "manager";
                oCompany.Password = "HAMZA@chagane2024";
                oCompany.UseTrusted = false; // Use false for SQL Server authentication
                oCompany.language = BoSuppLangs.ln_English;

                // Attempt to connect
                int connectionResult = oCompany.Connect();
                if (connectionResult != 0)
                {
                    throw new Exception($"SAP connection failed. Error: {oCompany.GetLastErrorDescription()}");
                }

                return oCompany;
            }
            catch (Exception ex)
            {
                // Log the exception (implement a logging mechanism as per your requirement)
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }
    }
}