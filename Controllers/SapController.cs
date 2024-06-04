using System;
using System.Data;
using System.Data.Odbc;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using SAPWebApi24.Utils;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace WebApplication3.Controllers
{

    public class SapController : ApiController
    {
        [HttpPost]
        [Route("api/CreateItem")]
        public IHttpActionResult CreateItem(ItemDetails itemDetails)
        {
            CallResponse callResponse = new CallResponse();

            //initialize connection to SAP
            SAPConnection connection = new SAPConnection();
            SAPbobsCOM.Company company = connection.OpenConnection();

            SAPbobsCOM.Items oItems;
            oItems = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
            oItems.ItemCode = itemDetails.ItemCode;
            oItems.ItemName = itemDetails.ItemName;
            int status = oItems.Add();

            //check whether saving was successful
            if (status == 0)
            {
                callResponse.RespCode = "00";
                callResponse.Description = "Saving Successful";
            }
            else
            {
                callResponse.RespCode = "99";
                callResponse.Description = company.GetLastErrorDescription().ToString();
            }

            return Json(callResponse); // Utilisez Json pour retourner un objet JSON
        }

        [HttpGet]
        [Route("api/getItems")]
        public HttpResponseMessage getItems()
        {
            DataSet ds = new DataSet();
            OdbcCommand cmd;

            using (OdbcConnection conn = new OdbcConnection("Driver={SQL Server};Server=DESKTOP-9OCVRC9;Database=SBODemoFR;uid=sa;pwd=HQ?WQ2chqgqne2024"))
            {
                string query = "Select ItemName, ItemCode From OITM";
                cmd = new OdbcCommand(query, conn);
                OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                da.Fill(ds, "Items");
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new ObjectContent<DataSet>(ds, new System.Net.Http.Formatting.XmlMediaTypeFormatter())
            };
        }

        public class ItemDetails
        {
            public string ItemName { get; set; }
            public string ItemCode { get; set; }
            public string BasePrice { get; set; }
        }

        public class CallResponse
        {
            public string RespCode { get; set; }
            public string Description { get; set; }
        }
    }
}
