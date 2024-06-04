using System;

namespace SAPWebApi24.Utils // Assurez-vous d'utiliser le bon espace de noms
{
    public class SAPConnection
    {
        public SAPbobsCOM.Company OpenConnection()
        {
            SAPbobsCOM.Company oCompany = new SAPbobsCOM.Company();

            oCompany.Server = "DESKTOP-9OCVRC9";
            oCompany.CompanyDB = "SBODemoFR";
            oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019;
            oCompany.DbUserName = "sa";
            oCompany.DbPassword = "HQ?WQ2chqgqne2024";
            oCompany.UserName = "manager";
            oCompany.Password = "HAMZA@chagane2024";
            oCompany.UseTrusted = true; // Utilisez false pour l'authentification SQL Server
            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English;

            //create the connection
            oCompany.Connect();

            return oCompany;
        }
    }
}
