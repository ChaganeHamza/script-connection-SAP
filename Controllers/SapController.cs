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
                callResponse.RespCode = "90";
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
                    string query = "SELECT DocEntry, DocNum, CardCode, CardName, DocDate, DocTotal, DocStatus FROM ORDR";
                    OdbcCommand cmd = new OdbcCommand(query, conn);
                    cmd.CommandTimeout = 120; // Set timeout to 120 seconds

                    OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                    da.Fill(ds, "SalesOrders");
                }

                var salesOrdersList = ds.Tables["SalesOrders"].AsEnumerable().Select(dataRow => new
                {
                    Id = dataRow.Field<int>("DocEntry"), // Utilisation de DocEntry comme id
                    DocNum = dataRow.Field<int>("DocNum"),
                    CardCode = dataRow.Field<string>("CardCode"),
                    CardName = dataRow.Field<string>("CardName"),
                    DocDate = dataRow.Field<DateTime>("DocDate"),
                    DocTotal = dataRow.Field<decimal>("DocTotal"),
                    Status = dataRow.Field<string>("DocStatus") // Ajout du statut  
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
                    string query = "SELECT DocEntry, DocNum, CardCode, CardName, DocDate, DocTotal, DocStatus FROM OPOR";
                    OdbcCommand cmd = new OdbcCommand(query, conn);
                    cmd.CommandTimeout = 120; // Set timeout to 120 seconds

                    OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                    da.Fill(ds, "PurchaseOrders");
                }

                var purchaseOrdersList = ds.Tables["PurchaseOrders"].AsEnumerable().Select(dataRow => new
                {
                    Id = dataRow.Field<int>("DocEntry"), // Utilisation de DocEntry comme id
                    DocNum = dataRow.Field<int>("DocNum"),
                    CardCode = dataRow.Field<string>("CardCode"),
                    CardName = dataRow.Field<string>("CardName"),
                    DocDate = dataRow.Field<DateTime>("DocDate"),
                    DocTotal = dataRow.Field<decimal>("DocTotal"),
                    Status = dataRow.Field<string>("DocStatus") // Ajout du statut
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
                    string query = "SELECT DocEntry, DocNum, CardCode, CardName, DocDate, DocTotal, DocStatus FROM ORIN";
                    OdbcCommand cmd = new OdbcCommand(query, conn);
                    cmd.CommandTimeout = 120; // Set timeout to 120 seconds

                    OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                    da.Fill(ds, "SalesReturns");
                }

                var salesReturnsList = ds.Tables["SalesReturns"].AsEnumerable().Select(dataRow => new
                {
                    Id = dataRow.Field<int>("DocEntry"), // Utilisation de DocEntry comme id
                    DocNum = dataRow.Field<int>("DocNum"),
                    CardCode = dataRow.Field<string>("CardCode"),
                    CardName = dataRow.Field<string>("CardName"),
                    DocDate = dataRow.Field<DateTime>("DocDate"),
                    DocTotal = dataRow.Field<decimal>("DocTotal"),
                    Status = dataRow.Field<string>("DocStatus") // Ajout du statut
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
                    string query = "SELECT DocEntry, DocNum, CardCode, CardName, DocDate, DocTotal, DocStatus FROM ODLN";
                    OdbcCommand cmd = new OdbcCommand(query, conn);
                    cmd.CommandTimeout = 120; // Set timeout to 120 seconds

                    OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                    da.Fill(ds, "Deliveries");
                }

                var deliveriesList = ds.Tables["Deliveries"].AsEnumerable().Select(dataRow => new
                {
                    Id = dataRow.Field<int>("DocEntry"), // Utilisation de DocEntry comme id
                    DocNum = dataRow.Field<int>("DocNum"),
                    CardCode = dataRow.Field<string>("CardCode"),
                    CardName = dataRow.Field<string>("CardName"),
                    DocDate = dataRow.Field<DateTime>("DocDate"),
                    DocTotal = dataRow.Field<decimal>("DocTotal"),
                    Status = dataRow.Field<string>("DocStatus") // Ajout du statut
                }).ToList();

                return Ok(deliveriesList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Route("api/getDocumentById")]
        public IHttpActionResult GetDocumentById(string id)
        {
            try
            {
                DataSet ds = new DataSet();
                string tableName = string.Empty;
                string sqlQuery = string.Empty;

                if (id.StartsWith("ORDR")) // Commande de vente
                {
                    tableName = "ORDR";
                    sqlQuery = $"SELECT * FROM {tableName} WHERE DocEntry = ?";
                }
                else if (id.StartsWith("OPOR")) // Commande d'achat
                {
                    tableName = "OPOR";
                    sqlQuery = $"SELECT * FROM {tableName} WHERE DocEntry = ?";
                }
                else if (id.StartsWith("ODLN")) // Livraison
                {
                    tableName = "ODLN";
                    sqlQuery = $"SELECT * FROM {tableName} WHERE DocEntry = ?";
                }
                else if (id.StartsWith("ORIN")) // Retour de vente
                {
                    tableName = "ORIN";
                    sqlQuery = $"SELECT * FROM {tableName} WHERE DocEntry = ?";
                }
                else
                {
                    return NotFound();
                }

                if (!int.TryParse(id.Substring(4), out int docEntry))
                {
                    return BadRequest("Invalid document ID format.");
                }

                using (OdbcConnection conn = new OdbcConnection(@"Driver={SQL Server};Server=DESKTOP-9OCVRC9;Database=SBODemoFR;uid=sa;pwd=HQ?WQ2chqgqne2024"))
                {
                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(sqlQuery, conn))
                    {
                        cmd.CommandTimeout = 120;
                        cmd.Parameters.Add(new OdbcParameter("DocEntry", OdbcType.Int)).Value = docEntry;

                        OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                        da.Fill(ds, tableName);
                    }
                }

                var documentData = ds.Tables[tableName].AsEnumerable().Select(dataRow => new
                {
                    DocEntry = dataRow.Field<int?>("DocEntry"),
                    DocNum = dataRow.Field<int?>("DocNum"),
                    DocType = dataRow.Field<string>("DocType"),
                    CANCELED = dataRow.Field<string>("CANCELED"),
                    Handwrtten = dataRow.Field<string>("Handwrtten"),
                    Printed = dataRow.Field<string>("Printed"),
                    DocStatus = dataRow.Field<string>("DocStatus"),
                    InvntSttus = dataRow.Field<string>("InvntSttus"),
                    Transfered = dataRow.Field<string>("Transfered"),
                    ObjType = dataRow.Field<string>("ObjType"),
                    DocDate = dataRow.Field<DateTime?>("DocDate"),
                    DocDueDate = dataRow.Field<DateTime?>("DocDueDate"),
                    CardCode = dataRow.Field<string>("CardCode"),
                    CardName = dataRow.Field<string>("CardName"),
                    Address = dataRow.Field<string>("Address"),
                    NumAtCard = dataRow.Field<string>("NumAtCard"),
                    VatPercent = dataRow.Field<decimal?>("VatPercent"),
                    VatSum = dataRow.Field<decimal?>("VatSum"),
                    VatSumFC = dataRow.Field<decimal?>("VatSumFC"),
                    DiscPrcnt = dataRow.Field<decimal?>("DiscPrcnt"),
                    DiscSum = dataRow.Field<decimal?>("DiscSum"),
                    DiscSumFC = dataRow.Field<decimal?>("DiscSumFC"),
                    DocCur = dataRow.Field<string>("DocCur"),
                    DocRate = dataRow.Field<decimal?>("DocRate"),
                    DocTotal = dataRow.Field<decimal?>("DocTotal"),
                    DocTotalFC = dataRow.Field<decimal?>("DocTotalFC"),
                    PaidToDate = dataRow.Field<decimal?>("PaidToDate"),
                    PaidFC = dataRow.Field<decimal?>("PaidFC"),
                    GrosProfit = dataRow.Field<decimal?>("GrosProfit"),
                    GrosProfFC = dataRow.Field<decimal?>("GrosProfFC"),
                    Ref1 = dataRow.Field<string>("Ref1"),
                    Ref2 = dataRow.Field<string>("Ref2"),
                    Comments = dataRow.Field<string>("Comments"),
                    JrnlMemo = dataRow.Field<string>("JrnlMemo"),
                    TransId = dataRow.Field<int?>("TransId"),
                    ReceiptNum = dataRow.Field<int?>("ReceiptNum"),
                    GroupNum = dataRow.Field<short?>("GroupNum"),
                    DocTime = dataRow.Field<short?>("DocTime"),
                    SlpCode = dataRow.Field<int?>("SlpCode"),
                    TrnspCode = dataRow.Field<short?>("TrnspCode"),
                    PartSupply = dataRow.Field<string>("PartSupply"),
                    Confirmed = dataRow.Field<string>("Confirmed"),
                    GrossBase = dataRow.Field<short?>("GrossBase"),
                    ImportEnt = dataRow.Field<int?>("ImportEnt"),
                    CreateTran = dataRow.Field<string>("CreateTran"),
                    SummryType = dataRow.Field<string>("SummryType"),
                    UpdInvnt = dataRow.Field<string>("UpdInvnt"),
                    UpdCardBal = dataRow.Field<string>("UpdCardBal"),
                    Instance = dataRow.Field<short?>("Instance"),
                    Flags = dataRow.Field<int?>("Flags"),
                    InvntDirec = dataRow.Field<string>("InvntDirec"),
                    CntctCode = dataRow.Field<int?>("CntctCode"),
                    ShowSCN = dataRow.Field<string>("ShowSCN"),
                    FatherCard = dataRow.Field<string>("FatherCard"),
                    SysRate = dataRow.Field<decimal?>("SysRate"),
                    CurSource = dataRow.Field<string>("CurSource"),
                    VatSumSy = dataRow.Field<decimal?>("VatSumSy"),
                    DiscSumSy = dataRow.Field<decimal?>("DiscSumSy"),
                    DocTotalSy = dataRow.Field<decimal?>("DocTotalSy"),
                    PaidSys = dataRow.Field<decimal?>("PaidSys"),
                    FatherType = dataRow.Field<string>("FatherType"),
                    GrosProfSy = dataRow.Field<decimal?>("GrosProfSy"),
                    UpdateDate = dataRow.Field<DateTime?>("UpdateDate"),
                    IsICT = dataRow.Field<string>("IsICT"),
                    CreateDate = dataRow.Field<DateTime?>("CreateDate"),
                    Volume = dataRow.Field<decimal?>("Volume"),
                    VolUnit = dataRow.Field<short?>("VolUnit"),
                    Weight = dataRow.Field<decimal?>("Weight"),
                    WeightUnit = dataRow.Field<short?>("WeightUnit"),
                    Series = dataRow.Field<int?>("Series"),
                    TaxDate = dataRow.Field<DateTime?>("TaxDate"),
                    Filler = dataRow.Field<string>("Filler"),
                    DataSource = dataRow.Field<string>("DataSource"),
                    StampNum = dataRow.Field<string>("StampNum"),
                    isCrin = dataRow.Field<string>("isCrin"),
                    FinncPriod = dataRow.Field<int?>("FinncPriod"),
                    UserSign = dataRow.Field<short?>("UserSign"),
                    selfInv = dataRow.Field<string>("selfInv"),
                    VatPaid = dataRow.Field<decimal?>("VatPaid"),
                    VatPaidFC = dataRow.Field<decimal?>("VatPaidFC"),
                    VatPaidSys = dataRow.Field<decimal?>("VatPaidSys"),
                    UserSign2 = dataRow.Field<short?>("UserSign2"),
                    WddStatus = dataRow.Field<string>("WddStatus"),
                    draftKey = dataRow.Field<int?>("draftKey"),
                    TotalExpns = dataRow.Field<decimal?>("TotalExpns"),
                    TotalExpFC = dataRow.Field<decimal?>("TotalExpFC"),
                    TotalExpSC = dataRow.Field<decimal?>("TotalExpSC"),
                    DunnLevel = dataRow.Field<int?>("DunnLevel"),
                    Address2 = dataRow.Field<string>("Address2"),
                    LogInstanc = dataRow.Field<int?>("LogInstanc"),
                    Exported = dataRow.Field<string>("Exported"),
                    StationID = dataRow.Field<int?>("StationID"),
                    Indicator = dataRow.Field<string>("Indicator"),
                    NetProc = dataRow.Field<string>("NetProc"),
                    AqcsTax = dataRow.Field<decimal?>("AqcsTax"),
                    AqcsTaxFC = dataRow.Field<decimal?>("AqcsTaxFC"),
                    AqcsTaxSC = dataRow.Field<decimal?>("AqcsTaxSC"),
                    CashDiscPr = dataRow.Field<decimal?>("CashDiscPr"),
                    CashDiscnt = dataRow.Field<decimal?>("CashDiscnt"),
                    CashDiscFC = dataRow.Field<decimal?>("CashDiscFC"),
                    CashDiscSC = dataRow.Field<decimal?>("CashDiscSC"),
                    ShipToCode = dataRow.Field<string>("ShipToCode"),
                    LicTradNum = dataRow.Field<string>("LicTradNum"),
                    PaymentRef = dataRow.Field<string>("PaymentRef"),
                    WTSum = dataRow.Field<decimal?>("WTSum"),
                    WTSumFC = dataRow.Field<decimal?>("WTSumFC"),
                    WTSumSC = dataRow.Field<decimal?>("WTSumSC"),
                    RoundDif = dataRow.Field<decimal?>("RoundDif"),
                    RoundDifFC = dataRow.Field<decimal?>("RoundDifFC"),
                    //RoundDifSC = dataRow.Field<decimal?>("RoundDifSC"),
                    CheckDigit = dataRow.Field<string>("CheckDigit"),
                    Form1099 = dataRow.Field<int?>("Form1099"),
                    Box1099 = dataRow.Field<string>("Box1099"),
                    Submitted = dataRow.Field<string>("Submitted"),
                    //DocTotal = dataRow.Field<decimal?>("DocTotal"), // Première occurrence
                    TotalDocument = dataRow.Field<decimal?>("DocTotal") // Deuxième occurrence renommée
                }).FirstOrDefault();

                if (documentData == null)
                {
                    return NotFound();
                }

                return Ok(documentData);
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