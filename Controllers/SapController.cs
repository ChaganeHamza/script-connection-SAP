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
using System.Linq;

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
        public IHttpActionResult getItems()
        {
            try
            {
                DataSet ds = new DataSet();
                using (OdbcConnection conn = new OdbcConnection(@"Driver={SQL Server};Server=DESKTOP-9OCVRC9;Database=SBODemoFR;uid=sa;pwd=HQ?WQ2chqgqne2024"))
                {
                    conn.Open();
                    string query = "SELECT ItemName, ItemCode FROM OITM";
                    OdbcCommand cmd = new OdbcCommand(query, conn);
                    cmd.CommandTimeout = 120; // Set timeout to 120 seconds


                    OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                    da.Fill(ds, "Items");
                }

                var itemsList = ds.Tables["Items"].AsEnumerable().Select(dataRow => new
                {
                    ItemName = dataRow.Field<string>("ItemName"),
                    ItemCode = dataRow.Field<string>("ItemCode"),
                    CustomPer = dataRow.Field<double>("CustomPer")
                }).ToList();

                return Ok(itemsList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [Route("api/UpdateItem")]
        public IHttpActionResult UpdateItem(ItemDetails itemDetails)
        {
            CallResponse callResponse = new CallResponse();

            //initialize connection to SAP
            SAPConnection connection = new SAPConnection();
            SAPbobsCOM.Company company = connection.OpenConnection();

            SAPbobsCOM.Items oItems;
            oItems = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);

            if (oItems.GetByKey(itemDetails.ItemCode))
            {
                oItems.ItemCode = itemDetails.ItemCode;
                oItems.ItemName = itemDetails.ItemName;
                int status = oItems.Update();

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
            }
            else
            {
                callResponse.RespCode    = "90";
                callResponse.Description = "Item Not Found In SAP";
            }


                return Json(callResponse); // Utilisez Json pour retourner un objet JSON
             
        }

        [HttpGet]
        [Route("api/getSalesOrders")]
        public IHttpActionResult GetSalesOrders()
        {
            try
            {
                DataSet ds = new DataSet();
                using (OdbcConnection conn = new OdbcConnection(@"Driver={SQL Server};Server=DESKTOP-9OCVRC9;Database=SBODemoFR;uid=sa;pwd=HQ?WQ2chqgqne2024"))
                {
                    conn.Open();
                    string query = "SELECT DocEntry, DocNum, CardCode, CardName, DocDate, DocTotal FROM ORDR";
                    OdbcCommand cmd = new OdbcCommand(query, conn);
                    cmd.CommandTimeout = 120; // Set timeout to 120 seconds

                    OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                    da.Fill(ds, "SalesOrders");
                }

                var salesOrdersList = ds.Tables["SalesOrders"].AsEnumerable().Select(dataRow => new
                {
                    DocNum = dataRow.Field<int>("DocNum"),
                    CardCode = dataRow.Field<string>("CardCode"),
                    CardName = dataRow.Field<string>("CardName"),
                    DocDate = dataRow.Field<DateTime>("DocDate"),
                    DocTotal = dataRow.Field<decimal>("DocTotal"),
                }).ToList();

                return Ok(salesOrdersList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



        [HttpGet]
        [Route("api/getPurchaseOrders")]
        public IHttpActionResult GetPurchaseOrders()
        {
            try
            {
                DataSet ds = new DataSet();
                using (OdbcConnection conn = new OdbcConnection(@"Driver={SQL Server};Server=DESKTOP-9OCVRC9;Database=SBODemoFR;uid=sa;pwd=HQ?WQ2chqgqne2024"))
                {
                    conn.Open();
                    string query = "SELECT DocEntry, DocNum, CardCode, CardName, DocDate, DocTotal FROM OPOR";
                    OdbcCommand cmd = new OdbcCommand(query, conn);
                    cmd.CommandTimeout = 120; // Set timeout to 120 seconds

                    OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                    da.Fill(ds, "PurchaseOrders");
                }

                var purchaseOrdersList = ds.Tables["PurchaseOrders"].AsEnumerable().Select(dataRow => new
                {
                    DocNum = dataRow.Field<int>("DocNum"),
                    CardCode = dataRow.Field<string>("CardCode"),
                    CardName = dataRow.Field<string>("CardName"),
                    DocDate = dataRow.Field<DateTime>("DocDate"),
                    DocTotal = dataRow.Field<decimal>("DocTotal")
                }).ToList();

                return Ok(purchaseOrdersList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/getSalesReturns")]
        public IHttpActionResult GetSalesReturns()
        {
            try
            {
                DataSet ds = new DataSet();
                using (OdbcConnection conn = new OdbcConnection(@"Driver={SQL Server};Server=DESKTOP-9OCVRC9;Database=SBODemoFR;uid=sa;pwd=HQ?WQ2chqgqne2024"))
                {
                    conn.Open();
                    string query = "SELECT DocEntry, DocNum, CardCode, CardName, DocDate, DocTotal FROM ORIN";
                    OdbcCommand cmd = new OdbcCommand(query, conn);
                    cmd.CommandTimeout = 120; // Set timeout to 120 seconds

                    OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                    da.Fill(ds, "SalesReturns");
                }

                var salesReturnsList = ds.Tables["SalesReturns"].AsEnumerable().Select(dataRow => new
                {
                    DocNum = dataRow.Field<int>("DocNum"),
                    CardCode = dataRow.Field<string>("CardCode"),
                    CardName = dataRow.Field<string>("CardName"),
                    DocDate = dataRow.Field<DateTime>("DocDate"),
                    DocTotal = dataRow.Field<decimal>("DocTotal")
                }).ToList();

                return Ok(salesReturnsList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/getDeliveries")]
        public IHttpActionResult GetDeliveries()
        {
            try
            {
                DataSet ds = new DataSet();
                using (OdbcConnection conn = new OdbcConnection(@"Driver={SQL Server};Server=DESKTOP-9OCVRC9;Database=SBODemoFR;uid=sa;pwd=HQ?WQ2chqgqne2024"))
                {
                    conn.Open();
                    string query = "SELECT DocEntry, DocNum, CardCode, CardName, DocDate, DocTotal FROM ODLN";
                    OdbcCommand cmd = new OdbcCommand(query, conn);
                    cmd.CommandTimeout = 120; // Set timeout to 120 seconds

                    OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                    da.Fill(ds, "Deliveries");
                }

                var deliveriesList = ds.Tables["Deliveries"].AsEnumerable().Select(dataRow => new
                {
                    DocNum = dataRow.Field<int>("DocNum"),
                    CardCode = dataRow.Field<string>("CardCode"),
                    CardName = dataRow.Field<string>("CardName"),
                    DocDate = dataRow.Field<DateTime>("DocDate"),
                    DocTotal = dataRow.Field<decimal>("DocTotal")
                }).ToList();

                return Ok(deliveriesList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
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