using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Utils
{
    public class ExcelUtils
    {
        private DateTime _lastUtilDay;

        private readonly byte[] FlowFour = { 195, 210, 214 };
        private readonly byte[] White = { 255, 255, 255 };
        private readonly byte[] Black = { 0, 0, 0 };
        private XSSFFont fontBlack;



        private XSSFCellStyle StyleWhite;
        private XSSFCellStyle StyleFlow;
        private XSSFCellStyle RightWhite;
        private XSSFCellStyle RightFlow;
        private XSSFColor ColorWhite;
        private XSSFColor ColorFlow;

        public DocumentUpload ReadExcelFile(IFormFile file, BankAccount bankAccount, DateTime lastUtilDay)
        {
            _lastUtilDay = lastUtilDay;
            DocumentUpload documentUpload = new DocumentUpload
            {
                Filename = file.FileName,
                AccountId = bankAccount.Id,
                UploadedAt = DateTime.Now,
                BankAccount = bankAccount,
                Transactions = new List<Transaction>()
            };
            switch (bankAccount.Bank.bankCode)
            {
                case "001":
                    ReadXLSXBancoBrasil(file, documentUpload);
                    break;
                case "237":
                    ReadXLSBradesco(file, documentUpload);
                    break;
                case "341":
                    ReadXLSItau(file, documentUpload);
                    break;
                case "033":
                    ReadXLSSantander(file, documentUpload);
                    break;
                default:
                    throw new ArgumentException($"Código do banco não encontrado: {bankAccount.Bank.bankCode}");
            }
            return documentUpload;
        }

        private void ReadXLSItau(IFormFile file, DocumentUpload du)
        {
            //This XLS is actually a HTML file inside
            StreamReader reader = new StreamReader(file.OpenReadStream());
            string text = reader.ReadToEnd();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);
            var rows = doc.DocumentNode.SelectNodes("//tr").ToList();
            rows.ForEach(row =>
            {
                var cells = row.SelectNodes("td").ToArray();
                if (cells.Length >= 6)
                {
                    string sDate = cells[1].InnerText;
                    string fsDate = Regex.Match(sDate, "\\d{2}/\\d{2}").Value + "/" + DateTime.Now.Year;
                    string sAmt = cells[5].InnerText.Trim().Replace(",", "").Replace(".", ",");
                    string sBalance = cells[6].InnerText.Trim().Replace(",", "").Replace(".", ",");
                    string sDesc = cells[4].InnerText;
                    if (!string.IsNullOrEmpty(fsDate)
                    && !string.IsNullOrEmpty(sAmt)
                    && !sDesc.ToUpper().Contains("SALDO"))
                    {
                        try
                        {
                            //Get Transaction
                            DateTime gotDate = DateTime.ParseExact(fsDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                            if (gotDate.Date.Equals(_lastUtilDay.Date))
                            {
                                Transaction t = new Transaction
                                {
                                    Date = gotDate,
                                    Amount = Convert.ToDecimal(sAmt, CultureInfo.GetCultureInfo("pt-BR")),
                                    Description = sDesc
                                };
                                t.InOut = t.Amount > 0 ? "IN" : "OUT";
                                t.OperationId = t.Amount > 0 ? 1 : 2; //1: receive, 2: pay
                                t.BankAccountId = du.BankAccount.Id;
                                t.RowNumber = rows.IndexOf(row);
                                du.Transactions.Add(t);
                            }
                        }
                        catch { }
                    }
                    else if (!string.IsNullOrEmpty(fsDate)
                      && !string.IsNullOrEmpty(sBalance)
                      && sDesc.Equals("S A L D O"))
                    {
                        DateTime gotDate = DateTime.ParseExact(fsDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                        if (gotDate.Date.Equals(_lastUtilDay))
                        {
                            //Get Account Balance
                            AccountBalance ab = new AccountBalance
                            {
                                BankAccountId = du.BankAccount.Id,
                                Date = gotDate,
                                Balance = Convert.ToDecimal(sBalance, CultureInfo.GetCultureInfo("pt-BR"))
                            };
                            du.AccountBalance = ab;
                        }
                    }
                }
            });
        }

        private void ReadXLSXBancoBrasil(IFormFile file, DocumentUpload du)
        {
            ISheet sheet;
            using (var stream = file.OpenReadStream())
            {
                stream.Position = 0;
                IWorkbook xssWorkbook;
                if (file.FileName.ToLower().EndsWith("xls"))
                {
                    xssWorkbook = new HSSFWorkbook(stream);
                }
                else
                {
                    xssWorkbook = new XSSFWorkbook(stream);
                }
                sheet = xssWorkbook.GetSheetAt(0);

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    try
                    {
                        string sAmt = row.GetCell(8).StringCellValue; //amount
                        string sPosNeg = row.GetCell(9).StringCellValue; //C or D -> + or -
                        string sDescr = row.GetCell(7).StringCellValue; //description
                        string sDate = GetFormattedCellValue(row.GetCell(0));
                        if (sDescr.ToUpper().Contains("S A L D O"))
                        {
                            DateTime gotDate = DateTime.ParseExact(sDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                            if (gotDate.Date.Equals(_lastUtilDay))
                            {
                                AccountBalance ab = new AccountBalance
                                {
                                    Balance = Convert.ToDecimal(sAmt, CultureInfo.GetCultureInfo("pt-BR")),
                                    Date = gotDate,
                                    BankAccountId = du.BankAccount.Id
                                };
                                if (sPosNeg.Equals("D"))
                                    ab.Balance *= -1;
                                du.AccountBalance = ab;
                                return; //in case that the balance is found, stops reading the file
                            }
                        }
                        else if (!sDescr.ToUpper().Contains("SALDO") && !string.IsNullOrEmpty(sDescr))
                        {
                            DateTime gotDate = DateTime.ParseExact(sDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                            if (gotDate.Date.Equals(_lastUtilDay))
                            {
                                Transaction t = new Transaction();
                                if (sPosNeg.Equals("D"))
                                    t.Amount = Convert.ToDecimal(sAmt, CultureInfo.GetCultureInfo("pt-BR")) * -1;
                                else if (sPosNeg.Equals("C"))
                                    t.Amount = Convert.ToDecimal(sAmt, CultureInfo.GetCultureInfo("pt-BR"));
                                else
                                    continue; //ignore anything that is not "D" or "C"
                                t.BankAccountId = du.BankAccount.Id;
                                t.Description = sDescr.Trim();
                                t.Date = gotDate;
                                t.InOut = t.Amount > 0 ? "IN" : "OUT";
                                t.OperationId = t.Amount > 0 ? 1 : 2; //1: receive, 2: pay
                                t.RowNumber = row.RowNum;
                                du.Transactions.Add(t);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                if (du.AccountBalance == null) //document doesn't contain any transactions or balance, then read differently
                {
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        try
                        {
                            string sDescr = row.GetCell(0).StringCellValue; //description
                            if (sDescr.ToUpper().Contains("SALDO ATUAL"))
                            {
                                string sDate = Regex.Match(file.FileName, "\\d{4}").Value.Insert(2, "/");
                                DateTime gotDate = DateTime.ParseExact(sDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                                if (gotDate.Date.Equals(_lastUtilDay))
                                {
                                    AccountBalance ab = new AccountBalance();
                                    du.AccountBalance = ab;
                                    string sAmt = row.GetCell(1).StringCellValue; //amount
                                    string sPosNeg = row.GetCell(2).StringCellValue; //C or D -> + or -
                                    ab.Balance = Convert.ToDecimal(sAmt, CultureInfo.GetCultureInfo("pt-BR"));
                                    if (sPosNeg.Equals("D"))
                                        ab.Balance *= -1;
                                    ab.Date = gotDate;
                                    ab.BankAccountId = du.BankAccount.Id;
                                    return;
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        private void ReadXLSBradesco(IFormFile file, DocumentUpload du)
        {
            ISheet sheet;
            using (var stream = file.OpenReadStream())
            {
                stream.Position = 0;
                IWorkbook xssWorkbook;
                if (file.FileName.ToLower().EndsWith("xls"))
                {
                    xssWorkbook = new HSSFWorkbook(stream);
                }
                else
                {
                    xssWorkbook = new XSSFWorkbook(stream);
                }
                sheet = xssWorkbook.GetSheetAt(0);
                AccountBalance ab = new AccountBalance();
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    try
                    {
                        string cell1 = row.GetCell(1).StringCellValue;
                        string sDate = GetFormattedCellValue(row.GetCell(0));
                        string sBalance = row.GetCell(5).StringCellValue;
                        DateTime gotDate = DateTime.ParseExact(sDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;

                        if (gotDate.Date.Equals(_lastUtilDay))
                        {
                            ab.Balance = Convert.ToDecimal(sBalance, CultureInfo.GetCultureInfo("pt-BR"));
                            ab.Date = _lastUtilDay;
                            ab.BankAccountId = du.BankAccount.Id;
                            du.AccountBalance = ab;
                        }
                        if (sDate.ToUpper().Equals("TOTAL"))
                        {
                            ab.Balance = Convert.ToDecimal(sBalance, CultureInfo.GetCultureInfo("pt-BR"));
                            ab.Date = _lastUtilDay;
                            ab.BankAccountId = du.BankAccount.Id;
                            du.AccountBalance = ab;
                            return; //in case that the balance is found, stops reading the file
                        }

                        else if (!cell1.Contains("SALDO") && !string.IsNullOrEmpty(cell1))
                        {
                            if (gotDate.Date.Equals(_lastUtilDay))
                            {
                                Transaction t = new Transaction
                                {
                                    BankAccountId = du.BankAccount.Id,
                                    Description = cell1,
                                    Date = gotDate
                                };
                                string sAmount1 = row.GetCell(3).StringCellValue;
                                if (string.IsNullOrEmpty(sAmount1))
                                {
                                    string sAmount2 = row.GetCell(4).StringCellValue;
                                    t.Amount = Convert.ToDecimal(sAmount2, CultureInfo.GetCultureInfo("pt-BR"));
                                }
                                else
                                {
                                    t.Amount = Convert.ToDecimal(sAmount1, CultureInfo.GetCultureInfo("pt-BR"));
                                }
                                t.InOut = t.Amount > 0 ? "IN" : "OUT";
                                t.OperationId = t.Amount > 0 ? 1 : 2; //1: receive, 2: pay
                                t.RowNumber = row.RowNum;
                                du.Transactions.Add(t);
                            }
                        }
                        else if (sDate.Contains("Não há lançamentos"))
                        {
                            return;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal List<CashFlow> ReadReceivables(IFormFile file, List<DomainModel> domains)
        {
            List<CashFlow> flows = new List<CashFlow>();
            ISheet sheet;
            using (var stream = file.OpenReadStream())
            {
                stream.Position = 0;
                XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
                int nSheets = xssWorkbook.NumberOfSheets;

                for (int sh = 0; sh < nSheets; sh++)
                {
                    sheet = xssWorkbook.GetSheetAt(sh);
                    DomainModel dm = domains.FirstOrDefault(e => e.Category.Description.Replace("/", " ") == sheet.SheetName);
                    if (sheet.SheetName.ToLower().Contains("dealers"))
                    {
                        for (int i = (sheet.FirstRowNum); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            try
                            {
                                string v1 = GetFormattedCellValue(row.GetCell(1));
                                string v2 = GetFormattedCellValue(row.GetCell(2));
                                string vT = GetFormattedCellValue(row.GetCell(3));
                                if (!string.IsNullOrEmpty(v1) || !string.IsNullOrEmpty(v2) || !string.IsNullOrEmpty(vT))
                                {
                                    string description = GetFormattedCellValue(row.GetCell(0));
                                    CashFlow cashFlow = new CashFlow();
                                    var domId = GetFormattedCellValue(row.GetCell(4));
                                    if (!string.IsNullOrEmpty(domId))
                                    {
                                        cashFlow.DomainId = int.Parse(domId);
                                    }
                                    else
                                    {
                                        cashFlow.DomainId = dm.Id;
                                    }
                                    if (!string.IsNullOrEmpty(v1) || !string.IsNullOrEmpty(v2))
                                    {
                                        if (v1.Equals("-") || v1.Equals(""))
                                        {
                                            v1 = "0";
                                        }
                                        if (v2.Equals("-") || v2.Equals(""))
                                        {
                                            v2 = "0";
                                        }
                                        decimal d1, d2;
                                        d1 = Convert.ToDecimal(v1.Trim().Replace(",", "").Replace(".", ","), CultureInfo.GetCultureInfo("pt-BR"));
                                        d2 = Convert.ToDecimal(v2.Trim().Replace(",", "").Replace(".", ","), CultureInfo.GetCultureInfo("pt-BR"));
                                        cashFlow.Amount = d1 + d2;
                                    }
                                    else
                                    {
                                        cashFlow.Amount = Convert.ToDecimal(vT.Trim().Replace(",", "").Replace(".", ","), CultureInfo.GetCultureInfo("pt-BR"));
                                    }
                                    cashFlow.Id = 0;
                                    cashFlow.Description = description;

                                    flows.Add(cashFlow);
                                }

                            }
                            catch { }
                        }
                    }
                    else
                    {
                        for (int i = (sheet.FirstRowNum); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            try
                            {
                                CashFlow cashFlow = new CashFlow();
                                string description = GetFormattedCellValue(row.GetCell(0));
                                string domId = GetFormattedCellValue(row.GetCell(2));
                                if (!string.IsNullOrEmpty(domId))
                                {
                                    cashFlow.DomainId = int.Parse(domId);
                                }
                                else
                                {
                                    cashFlow.DomainId = dm.Id;
                                }
                                string ammt = GetFormattedCellValue(row.GetCell(1)).Trim().Replace(",", "").Replace(".", ",");
                                cashFlow.Amount = Convert.ToDecimal(ammt, CultureInfo.GetCultureInfo("pt-BR"));
                                cashFlow.Id = 0;
                                cashFlow.Description = description;

                                flows.Add(cashFlow);
                            }
                            catch { }
                        }
                    }

                }
            }
            return flows;
        }

        private void ReadXLSSantander(IFormFile file, DocumentUpload du)
        {
            ISheet sheet;
            using (var stream = file.OpenReadStream())
            {
                stream.Position = 0;
                IWorkbook xssWorkbook;
                if (file.FileName.ToLower().EndsWith("xls"))
                {
                    xssWorkbook = new HSSFWorkbook(stream);
                }
                else
                {
                    xssWorkbook = new XSSFWorkbook(stream);
                }
                sheet = xssWorkbook.GetSheetAt(0);

                for (int i = (sheet.FirstRowNum); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    try
                    {
                        string cell1 = row.GetCell(2).StringCellValue;
                        if (!cell1.Contains("SALDO") && !string.IsNullOrEmpty(cell1))
                        {
                            string cell0 = GetFormattedCellValue(row.GetCell(0));
                            DateTime gotDate = DateTime.ParseExact(cell0, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                            if (gotDate.Date.Equals(_lastUtilDay))
                            {
                                Transaction t = new Transaction
                                {
                                    BankAccountId = du.BankAccount.Id,
                                    Description = cell1,
                                    Date = gotDate
                                };
                                string sAmount1 = row.GetCell(4).NumericCellValue.ToString().Replace(".", ",");
                                t.Amount = Convert.ToDecimal(sAmount1, CultureInfo.GetCultureInfo("pt-BR"));
                                t.InOut = t.Amount > 0 ? "IN" : "OUT";
                                t.OperationId = t.Amount > 0 ? 1 : 2; //1: receive, 2: pay
                                t.RowNumber = row.RowNum;
                                du.Transactions.Add(t);


                                string balance = GetFormattedCellValue(row.GetCell(5));
                                if (!string.IsNullOrEmpty(balance))
                                {
                                    //Get AccountBalance
                                    string sBalance = row.GetCell(5).NumericCellValue.ToString().Replace(".", ",");
                                    AccountBalance ab = new AccountBalance();
                                    du.AccountBalance = ab;
                                    ab.Balance = Convert.ToDecimal(sBalance, CultureInfo.GetCultureInfo("pt-BR"));
                                    ab.Date = gotDate;
                                    ab.BankAccountId = du.BankAccount.Id;
                                    return; //no account balance in the file
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public string GetFormattedCellValue(ICell cell)
        {
            if (cell != null)
            {
                switch (cell.CellType)
                {
                    case CellType.String:
                        return cell.StringCellValue;

                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            DateTime date = cell.DateCellValue;
                            return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            return cell.NumericCellValue.ToString();
                        }

                    case CellType.Boolean:
                        return cell.BooleanCellValue ? "TRUE" : "FALSE";

                    case CellType.Formula:
                        return cell.CellFormula;

                    case CellType.Error:
                        return FormulaError.ForInt(cell.ErrorCellValue).String;
                }
            }
            return string.Empty;
        }

        public byte[] GenerateExportCashFlow(ExportCashFlowModel exportModel)
        {
            XSSFWorkbook workbook = CreateBaseWorkbook();

            CreateReportSheet(workbook, exportModel);
            CreateDetailSheet(workbook, exportModel);
            CreateTotalizationSheet(workbook, exportModel);

            //generate the array for download
            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            try
            {
                workbook.Write(bos);
            }
            finally
            {
                bos.Close();
            }
            return bos.ToByteArray();
        }

        private void CreateDetailSheet(XSSFWorkbook workbook, ExportCashFlowModel exportModel)
        {
            int RowIndex = 0;
            ISheet Sheet = workbook.CreateSheet("Detail");
            //Create the first line

            IRow Row = Sheet.CreateRow(++RowIndex);
            CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 10), $"Volvo do Brasil Veículos - CashFlow - gerado em {DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"))}", Sheet, 1);

            Row = Sheet.CreateRow(++RowIndex);
            CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 10), $"Movimentações do dia: {exportModel.Date.Date}", Sheet, 1);

            Row = Sheet.CreateRow(++RowIndex);
            CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 10), "Detalhamento", Sheet, 1);

            Row = Sheet.CreateRow(++RowIndex);
            CreateCell(Row, 0, "Banco", StyleFlow);
            CreateCell(Row, 1, "Conta Contábil", StyleFlow);
            CreateCell(Row, 2, "Categoria", StyleFlow);
            CreateCell(Row, 3, "Descrição", StyleFlow);
            CreateCell(Row, 4, "IN/OUT", StyleFlow);
            CreateCell(Row, 5, "Operação", StyleFlow);
            CreateCell(Row, 6, "Aprovação", StyleFlow);
            CreateCell(Row, 7, "Valor Total", StyleFlow);
            CreateCell(Row, 8, "DOC", StyleFlow);
            CreateCell(Row, 9, "Valor DOC", StyleFlow);
            CreateCell(Row, 10, "Observações", StyleFlow);

            int colorIndex = 0; //Even => Flow, UnEven => White
            foreach (ExportCashFlowBank bank in exportModel.ExportCashFlowBanks.OrderBy(b => b.BankAccount.ReportOrder))
            {
                colorIndex++;
                XSSFCellStyle style = colorIndex % 2 == 0 ? StyleFlow : StyleWhite;
                XSSFCellStyle numberStyle = colorIndex % 2 == 0 ? RightFlow : RightWhite;
                int mergeIndexColor = bank.BankAccount.ReportOrder % 2 == 0 ? 1 : 2;
                bank.CashFlows.ForEach(flow =>
                {
                    if (flow.Domain.IsDetailedTransaction ?? false)
                    {
                        if (flow.CashFlowDetaileds.Any())
                        {
                            flow.CashFlowDetaileds.ForEach(detail =>
                            {
                                IRow DetailedRow = Sheet.CreateRow(++RowIndex);
                                CreateCell(DetailedRow, 0, bank.BankAccount.Nickname, style);
                                CreateCell(DetailedRow, 1, bank.BankAccount.AccountingDescription, style);
                                CreateCell(DetailedRow, 2, flow.Domain.Category.Description, style);
                                CreateCell(DetailedRow, 3, flow.Domain.Description, style);
                                CreateCell(DetailedRow, 4, flow.Domain.InOut, style);
                                CreateCell(DetailedRow, 5, flow.Domain.Operation.Code, style);
                                CreateCell(DetailedRow, 6, BoolToText(flow.Approval, flow.Domain.ApprovationNeeded), style);
                                CreateCell(DetailedRow, 7, flow.Amount, numberStyle);
                                CreateCell(DetailedRow, 8, detail.DocumentName, style);
                                CreateCell(DetailedRow, 9, detail.Amount, numberStyle);
                                CreateCell(DetailedRow, 10, detail.DetailedDescription, style);
                            });
                        }
                    }
                });
            }

            Sheet.MergedRegions.ForEach(range =>
            {
                RegionUtil.SetBorderTop(1, range, Sheet);
                RegionUtil.SetBorderBottom(1, range, Sheet);
                RegionUtil.SetBorderLeft(1, range, Sheet);
                RegionUtil.SetBorderRight(1, range, Sheet);
            });

            //AutoSizeColumns(RowIndex, Sheet);
        }

        private void CreateTotalizationSheet(XSSFWorkbook workbook, ExportCashFlowModel exportModel)
        {
            int RowIndex = 0;
            ISheet Sheet = workbook.CreateSheet("CF Balance");
            //Create the first line
            IRow Row = Sheet.CreateRow(++RowIndex);
            CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 6), $"Volvo do Brasil Veículos - CashFlow - gerado em {DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"))}", Sheet, 1);
            Row = Sheet.CreateRow(++RowIndex);
            CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 6), $"Movimentações do dia: {exportModel.Date.Date}", Sheet, 1);

            Row = Sheet.GetRow(RowIndex);

            foreach (ExportCashFlowBank bank in exportModel.ExportCashFlowBanks.OrderBy(b => b.BankAccount.ReportOrder))
            {
                XSSFCellStyle style = StyleWhite;
                XSSFCellStyle numberStyle = RightWhite;
                //Create Header
                //Headers
                Row = Sheet.CreateRow(++RowIndex);
                CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 6), "Totalizadores - " + bank.BankAccount.Nickname, Sheet, 1);

                //Headers
                Row = Sheet.CreateRow(++RowIndex);
                CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 1), "Entradas", Sheet, 1);
                CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 2, 6), "Saidas", Sheet, 1);

                Row = Sheet.CreateRow(++RowIndex);
                CreateCell(Row, 0, "Categoria", StyleFlow);
                CreateCell(Row, 1, "Total", StyleFlow);
                CreateCell(Row, 2, "Categoria", StyleFlow);
                CreateCell(Row, 3, "Aprovado", StyleFlow);
                CreateCell(Row, 4, "A Aprovar", StyleFlow);
                CreateCell(Row, 5, "Pré-Aprovado", StyleFlow);
                CreateCell(Row, 6, "Total", StyleFlow);

                int elementToGet = 0;

                while (bank.TotalizationReport.ItemsIn.ElementAtOrDefault(elementToGet) != null ||
                        bank.TotalizationReport.ItemsOut.ElementAtOrDefault(elementToGet) != null)
                {
                    Row = Sheet.CreateRow(++RowIndex);

                    if (bank.TotalizationReport.ItemsIn.ElementAtOrDefault(elementToGet) != null)
                    {
                        CreateCell(Row, 0, bank.TotalizationReport.ItemsIn.ElementAtOrDefault(elementToGet).CategoryName, style);
                        CreateCell(Row, 1, bank.TotalizationReport.ItemsIn.ElementAtOrDefault(elementToGet).SubTotal, numberStyle);
                    }

                    if (bank.TotalizationReport.ItemsOut.ElementAtOrDefault(elementToGet) != null)
                    {
                        CreateCell(Row, 2, bank.TotalizationReport.ItemsOut.ElementAtOrDefault(elementToGet).CategoryName, style);
                        CreateCell(Row, 3, bank.TotalizationReport.ItemsOut.ElementAtOrDefault(elementToGet).SubTotalApproved, numberStyle);
                        CreateCell(Row, 4, bank.TotalizationReport.ItemsOut.ElementAtOrDefault(elementToGet).SubTotalNotApproved, numberStyle);
                        CreateCell(Row, 5, bank.TotalizationReport.ItemsOut.ElementAtOrDefault(elementToGet).SubTotalPreApproved, numberStyle);
                        CreateCell(Row, 6, bank.TotalizationReport.ItemsOut.ElementAtOrDefault(elementToGet).SubTotal, numberStyle);
                    }
                    elementToGet++;
                }

                //Add SubTotals
                Row = Sheet.CreateRow(++RowIndex);
                CreateCell(Row, 0, "Total IN", StyleFlow);
                CreateCell(Row, 1, bank.TotalizationReport.SubtotalIn, numberStyle);
                CreateCell(Row, 2, "Total OUT", StyleFlow);
                CreateCell(Row, 3, bank.TotalizationReport.SubtotalApproved, numberStyle);
                CreateCell(Row, 4, bank.TotalizationReport.SubtotalNotApproved, numberStyle);
                CreateCell(Row, 5, bank.TotalizationReport.SubtotalPreApproved, numberStyle);
                CreateCell(Row, 6, bank.TotalizationReport.SubtotalOut, numberStyle);

                //Add Totals
                Row = Sheet.CreateRow(++RowIndex);
                CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 5), "TOTAL NET", Sheet, 1);
                CreateCell(Row, 6, bank.TotalizationReport.Total, numberStyle);

                //Line Jump (2x)
                Sheet.CreateRow(++RowIndex);
                Sheet.CreateRow(++RowIndex);
            }

            Sheet.MergedRegions.ForEach(range =>
            {
                RegionUtil.SetBorderTop(1, range, Sheet);
                RegionUtil.SetBorderBottom(1, range, Sheet);
                RegionUtil.SetBorderLeft(1, range, Sheet);
                RegionUtil.SetBorderRight(1, range, Sheet);
            });

            //AutoSizeColumns(RowIndex, Sheet);
        }

        private void CreateReportSheet(XSSFWorkbook workbook, ExportCashFlowModel exportModel)
        {
            int RowIndex = 0;
            ISheet Sheet = workbook.CreateSheet("Statement");

            IRow Row = Sheet.CreateRow(++RowIndex);
            CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 8), $"Volvo do Brasil Veículos - CashFlow - gerado em {DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"))}", Sheet, 1);

            Row = Sheet.CreateRow(++RowIndex);
            CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 8), $"Movimentações do dia: {exportModel.Date.Date.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"))}", Sheet, 1);

            Row = Sheet.CreateRow(++RowIndex);
            CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 8), "Movimentações", Sheet, 1);

            IRow HeaderRow = Sheet.CreateRow(++RowIndex);

            //Create The Actual Cells
            CreateCell(HeaderRow, 0, "Banco", StyleFlow);
            CreateCell(HeaderRow, 1, "Conta Contábil", StyleFlow);
            CreateCell(HeaderRow, 2, "Categoria", StyleFlow);
            CreateCell(HeaderRow, 3, "Descrição", StyleFlow);
            CreateCell(HeaderRow, 4, "IN/OUT", StyleFlow);
            CreateCell(HeaderRow, 5, "Operação", StyleFlow);
            CreateCell(HeaderRow, 6, "Aprovação", StyleFlow);
            CreateCell(HeaderRow, 7, "Detalhado", StyleFlow);
            CreateCell(HeaderRow, 8, "Valor", StyleFlow);


            int colorIndex = 0; //Even => Flow, UnEven => White

            foreach (ExportCashFlowBank bank in exportModel.ExportCashFlowBanks.OrderBy(b => b.BankAccount.ReportOrder))
            {
                colorIndex++;
                XSSFCellStyle style = colorIndex % 2 == 0 ? StyleFlow : StyleWhite;
                XSSFCellStyle numberStyle = colorIndex % 2 == 0 ? RightFlow : RightWhite;

                //Iteration through cashFlows
                foreach (CashFlow flow in bank.CashFlows)
                {
                    IRow CurrentRow = Sheet.CreateRow(++RowIndex);

                    CreateCell(CurrentRow, 0, bank.BankAccount.Nickname, style);
                    CreateCell(CurrentRow, 1, bank.BankAccount.AccountingDescription, style);
                    CreateCell(CurrentRow, 2, flow.Domain.Category.Description, style);
                    CreateCell(CurrentRow, 3, flow.Domain.Description, style);
                    CreateCell(CurrentRow, 4, flow.Domain.InOut, style);
                    CreateCell(CurrentRow, 5, flow.Domain.Operation.Code, style);
                    CreateCell(CurrentRow, 6, BoolToText(flow.Approval, flow.Domain.ApprovationNeeded), style);
                    CreateCell(CurrentRow, 7, flow.Domain.IsDetailedTransaction ?? false ? "X" : "", style);
                    CreateCell(CurrentRow, 8, flow.Amount, numberStyle);
                }
            }

            Sheet.MergedRegions.ForEach(range =>
            {
                RegionUtil.SetBorderTop(1, range, Sheet);
                RegionUtil.SetBorderBottom(1, range, Sheet);
                RegionUtil.SetBorderLeft(1, range, Sheet);
                RegionUtil.SetBorderRight(1, range, Sheet);
            });

            //AutoSizeColumns();
        }

        private void CreateMergedCell(CellRangeAddress region, string text, ISheet sheet, int color)
        {
            XSSFCell cell = (XSSFCell)sheet.GetRow(region.FirstRow).CreateCell(region.FirstColumn);
            cell.SetCellValue(text);
            cell.CellStyle = color == 1 ? StyleFlow : StyleWhite;
            sheet.AddMergedRegion(region);
        }

        private void CreateCell(IRow CurrentRow, int CellIndex, string Value, XSSFCellStyle Style)
        {
            ICell Cell = CurrentRow.CreateCell(CellIndex);
            Cell.SetCellValue(Value);
            Cell.CellStyle = Style;
        }

        private void CreateCell(IRow CurrentRow, int CellIndex, decimal Value, XSSFCellStyle Style)
        {
            ICell Cell = CurrentRow.CreateCell(CellIndex);
            Style.SetDataFormat(HSSFDataFormat.GetBuiltinFormat("#,##0.00"));
            Cell.SetCellValue((double)Value);
            Cell.CellStyle = Style;
        }

        private string BoolToText(bool approval, bool isApprovalNeeded)
        {
            return isApprovalNeeded ? approval ? "Aprovado" : "A aprovar" : "Pré aprovado";
        }

        private XSSFCellStyle GetCellStyle(XSSFWorkbook workbook, XSSFFont font, XSSFColor color, BorderStyle borderStyle = BorderStyle.Thin)
        {
            XSSFCellStyle style = CreateBaseStyle(workbook, font, color, borderStyle, HorizontalAlignment.Left);
            return style;
        }
        private XSSFCellStyle GetCellStyleRight(XSSFWorkbook workbook, XSSFFont font, XSSFColor color, BorderStyle borderStyle = BorderStyle.Thin)
        {
            XSSFCellStyle style = CreateBaseStyle(workbook, font, color, borderStyle, HorizontalAlignment.Right);
            return style;
        }

        private static XSSFCellStyle CreateBaseStyle(XSSFWorkbook workbook, XSSFFont font, XSSFColor color, BorderStyle borderStyle, HorizontalAlignment horizontalAlignment)
        {
            XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
            style.SetFont(font);
            style.Alignment = horizontalAlignment;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.BorderLeft = borderStyle;
            style.BorderTop = borderStyle;
            style.BorderRight = borderStyle;
            style.BorderBottom = borderStyle;
            style.SetFillForegroundColor(color);
            style.FillPattern = FillPattern.SolidForeground;
            return style;
        }

        internal byte[] GenerateExportKPI(KPIReport report)
        {
            XSSFWorkbook workbook = CreateBaseWorkbook();
            CreateKPISheet(workbook, report);

            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            try
            {
                workbook.Write(bos);
            }
            finally
            {
                bos.Close();
            }
            return bos.ToByteArray();
        }

        private void CreateKPISheet(XSSFWorkbook workbook, KPIReport report)
        {
            int nHeaderRows = 3;
            int RowIndex = 0;
            ISheet Sheet = workbook.CreateSheet("Bank Account Balance");

            IRow Row = Sheet.CreateRow(RowIndex);
            CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, report.Accounts.Count), $"Volvo do Brasil Veículos - Bank Account Balance - gerado em {DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"))}", Sheet, 1);

            IRow HeaderRow = Sheet.CreateRow(++RowIndex); //Skip 1 line

            HeaderRow = Sheet.CreateRow(++RowIndex);

            //Create The Actual Cells
            CreateCell(HeaderRow, 0, "Data", StyleFlow);
            foreach (BankAccount acc in report.Accounts.OrderBy(b => b.KpiOrder))
            {
                CreateCell(HeaderRow, acc.KpiOrder, acc.Nickname, StyleFlow);
            }

            DateTime actualDate = report.StartDate;

            var pairs = new Dictionary<DateTime, int>();

            while (actualDate <= report.EndDate)
            {
                if (actualDate.DayOfWeek != DayOfWeek.Saturday && actualDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    IRow DateRow = Sheet.CreateRow(++RowIndex);
                    CreateCell(DateRow, 0, actualDate.ToString("dd/MM/yyyy"), StyleFlow);
                    pairs.Add(actualDate, DateRow.RowNum);
                }
                actualDate = actualDate.AddDays(1);
            }

            foreach (BankAccount acc in report.Accounts.OrderBy(b => b.KpiOrder))
            {
                List<AccountBalance> balances = report.Balances.Where(b => b.BankAccountId == acc.Id).ToList();
                balances.ForEach(b =>
                {
                    if (pairs.ContainsKey(b.Date.Date))
                    {
                        pairs.TryGetValue(b.Date.Date, out int row);
                        IRow r = Sheet.GetRow(row);
                        CreateCell(r, acc.KpiOrder, b.Balance, RightWhite);
                    }
                });
            }

            for (int i = nHeaderRows; i <= Sheet.LastRowNum; i++)
            {
                IRow row = Sheet.GetRow(i);
                for (int j = 1; j < report.Accounts.Count + 1; j++)
                {
                    if (row.GetCell(j) == null)
                    {
                        CreateCell(row, j, 0.00M, RightWhite);
                    }
                }
            }

            Sheet.MergedRegions.ForEach(range =>
            {
                RegionUtil.SetBorderTop(1, range, Sheet);
                RegionUtil.SetBorderBottom(1, range, Sheet);
                RegionUtil.SetBorderLeft(1, range, Sheet);
                RegionUtil.SetBorderRight(1, range, Sheet);
            });

            //AutoSizeColumns();
        }

        internal byte[] GenerateExportConciliation(ExportConciliationModel report)
        {
            XSSFWorkbook workbook = CreateBaseWorkbook();
            CreateConciliationSheet(workbook, report);

            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            try
            {
                workbook.Write(bos);
            }
            finally
            {
                bos.Close();
            }
            return bos.ToByteArray();
        }

        private XSSFWorkbook CreateBaseWorkbook()
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            fontBlack = (XSSFFont)workbook.CreateFont();
            fontBlack.FontHeightInPoints = 11;
            fontBlack.FontName = "Tahoma";
            fontBlack.SetColor(new XSSFColor(Black));

            ColorWhite = new XSSFColor(White);
            ColorFlow = new XSSFColor(FlowFour);

            //Create Cell Styles
            StyleFlow = GetCellStyle(workbook, fontBlack, ColorFlow);
            StyleWhite = GetCellStyle(workbook, fontBlack, ColorWhite);

            //numbers
            RightFlow = GetCellStyleRight(workbook, fontBlack, ColorFlow);
            RightWhite = GetCellStyleRight(workbook, fontBlack, ColorWhite);
            return workbook;
        }

        private void CreateConciliationSheet(XSSFWorkbook workbook, ExportConciliationModel exportModel)
        {
            int RowIndex = 0;
            ISheet Sheet = workbook.CreateSheet("Conciliação");
            //Create the first line
            IRow Row = Sheet.CreateRow(++RowIndex);
            CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 7), $"Volvo do Brasil Veículos - CashFlow - gerado em {DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"))}", Sheet, 1);
            Row = Sheet.CreateRow(++RowIndex);
            CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 7), $"Movimentações do dia: {exportModel.Date.Date.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"))}", Sheet, 1);

            //Headers
            Row = Sheet.CreateRow(++RowIndex);
            CreateCell(Row, 0, "Código", StyleFlow);
            CreateCell(Row, 1, "Banco", StyleFlow);
            CreateCell(Row, 2, "Agência", StyleFlow);
            CreateCell(Row, 3, "Conta", StyleFlow);
            CreateCell(Row, 4, "Conta Contábil", StyleFlow);
            CreateCell(Row, 5, "eCash", StyleFlow);
            CreateCell(Row, 6, "Extrato", StyleFlow);
            CreateCell(Row, 7, "Distorções", StyleFlow);

            exportModel.TotalCashFlow = 0.00M;
            exportModel.TotalBalance = 0.00M;
            exportModel.TotalDistortion = 0.00M;
            foreach (BankAccount bank in exportModel.Accounts.OrderBy(b => b.ReportOrder))
            {
                //Add Data
                Row = Sheet.CreateRow(++RowIndex);
                CreateCell(Row, 0, bank.Bank.bankCode, StyleWhite);
                CreateCell(Row, 1, bank.Nickname, StyleWhite);
                CreateCell(Row, 2, bank.Agency, StyleWhite);
                CreateCell(Row, 3, bank.Account, StyleWhite);
                CreateCell(Row, 4, bank.AccountingDescription, StyleWhite);
                CashConsolidationItem item = exportModel.CashReport.CashConsolidationItems.FirstOrDefault(i => i.BankAccount.Id == bank.Id);
                decimal calculated = item == null ? 0.00M : item.ReserveAmount;
                CreateCell(Row, 5, calculated, RightWhite);
                AccountBalance ab = exportModel.AccountBalances.FirstOrDefault(e => e.BankAccountId == bank.Id);
                decimal balance = ab == null ? 0.00M : ab.Balance;
                CreateCell(Row, 6, balance, RightWhite);
                BankAccountDistortion distortion = exportModel.Distortions.FirstOrDefault(i => i.BankAccountId == bank.Id);
                decimal dist = distortion == null ? 0.00M : distortion.DistortionAmount;
                CreateCell(Row, 7, dist, RightWhite);

                exportModel.TotalCashFlow += calculated;
                exportModel.TotalBalance += balance;
                exportModel.TotalDistortion += dist;
            }

            Row = Sheet.CreateRow(++RowIndex);
            CreateMergedCell(new CellRangeAddress(Row.RowNum, Row.RowNum, 0, 4), $"Total", Sheet, 1);
            CreateCell(Row, 5, exportModel.TotalCashFlow, RightFlow);
            CreateCell(Row, 6, exportModel.TotalBalance, RightFlow);
            CreateCell(Row, 7, exportModel.TotalDistortion, RightFlow);

            Sheet.MergedRegions.ForEach(range =>
            {
                RegionUtil.SetBorderTop(1, range, Sheet);
                RegionUtil.SetBorderBottom(1, range, Sheet);
                RegionUtil.SetBorderLeft(1, range, Sheet);
                RegionUtil.SetBorderRight(1, range, Sheet);
            });

            //AutoSizeColumns();
        }

        internal byte[] GenerateOperationalReport(List<ExportCashFlowBank> exports)
        {
            XSSFWorkbook workbook = CreateBaseWorkbook();
            CreateOperationalSheet(workbook, exports);

            //generate the array for download
            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            try
            {
                workbook.Write(bos);
            }
            finally
            {
                bos.Close();
            }
            return bos.ToByteArray();
        }

        private void CreateOperationalSheet(XSSFWorkbook workbook, List<ExportCashFlowBank> exports)
        {
            int RowIndex = 0;
            ISheet Sheet = workbook.CreateSheet("Operational Daily Cash Flow");
            //Create the first line
            IRow Row = Sheet.CreateRow(++RowIndex);
            CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 11), $"Volvo do Brasil Veículos - Operational Daily Cash Flow - gerado em {DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"))}", Sheet, 1);

            //Skip line
            Sheet.CreateRow(++RowIndex);

            //Headers
            Row = Sheet.CreateRow(++RowIndex);
            CreateCell(Row, 0, "Data", StyleFlow);
            CreateCell(Row, 1, "Banco", StyleFlow);
            CreateCell(Row, 2, "Agência", StyleFlow);
            CreateCell(Row, 3, "Conta", StyleFlow);
            CreateCell(Row, 4, "Valor", StyleFlow);
            CreateCell(Row, 5, "Descrição", StyleFlow);
            CreateCell(Row, 6, "Operação", StyleFlow);
            CreateCell(Row, 7, "Descrição da Operação", StyleFlow);
            CreateCell(Row, 8, "Categoria", StyleFlow);
            CreateCell(Row, 9, "Aprovação", StyleFlow);
            CreateCell(Row, 10, "Ajuste", StyleFlow);
            CreateCell(Row, 11, "IN/OUT", StyleFlow);

            foreach (ExportCashFlowBank item in exports)
            {
                foreach (var element in item.CashFlows.OrderBy(c => c.Date.Date))
                {
                    //Add Data
                    Row = Sheet.CreateRow(++RowIndex);
                    CreateCell(Row, 0, element.Date.ToString("dd/MM/yyyy"), StyleWhite);
                    CreateCell(Row, 1, item.BankAccount.Nickname, StyleWhite);
                    CreateCell(Row, 2, item.BankAccount.Agency, StyleWhite);
                    CreateCell(Row, 3, item.BankAccount.Account, StyleWhite);
                    CreateCell(Row, 4, element.Amount, RightWhite);
                    CreateCell(Row, 5, element.Description, StyleWhite);
                    CreateCell(Row, 6, element.Domain.Operation.Code, StyleWhite);
                    CreateCell(Row, 7, element.Domain.Operation.Description, StyleWhite);
                    CreateCell(Row, 8, element.Domain.Category.Description, StyleWhite);
                    string typeOfApproval = element.Domain.ApprovationNeeded ? element.Approval ? "Aprovado" : "A Aprovar" : "Pré Aprovado";
                    CreateCell(Row, 9, typeOfApproval, StyleWhite);
                    element.IsDistortion ??= false;
                    string isAdjustment = element.IsDistortion.Value ? "S" : "";
                    CreateCell(Row, 10, isAdjustment, StyleWhite);
                    CreateCell(Row, 11, element.Domain.InOut, StyleWhite);
                }
            }

            Sheet.MergedRegions.ForEach(range =>
            {
                RegionUtil.SetBorderTop(1, range, Sheet);
                RegionUtil.SetBorderBottom(1, range, Sheet);
                RegionUtil.SetBorderLeft(1, range, Sheet);
                RegionUtil.SetBorderRight(1, range, Sheet);
            });

            //AutoSizeColumns();
        }

        internal byte[] GenerateReceivablesTemplate(ReceivablesTemplate templateModel)
        {
            XSSFWorkbook workbook = CreateBaseWorkbook();
            CreateTemplate(workbook, templateModel);

            //generate the array for download
            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            try
            {
                workbook.Write(bos);
            }
            finally
            {
                bos.Close();
            }
            return bos.ToByteArray();
        }

        private void CreateTemplate(XSSFWorkbook workbook, ReceivablesTemplate templateModel)
        {
            templateModel.Categories.OrderBy(c => c.ReportOrder).ToList().ForEach(category =>
            {
                int RowIndex = 0;
                ISheet Sheet = workbook.CreateSheet(category.Description.Replace("/", " "));
                IRow Row = Sheet.CreateRow(RowIndex);

                if (category.Description.ToLower().Contains("dealers"))
                {
                    CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 3), $"Categoria: {category.Description}", Sheet, 1);
                    Row = Sheet.CreateRow(++RowIndex);
                    CreateCell(Row, 0, category.Description, StyleFlow);
                    CreateCell(Row, 1, "Chassi", StyleFlow);
                    CreateCell(Row, 2, "Peças", StyleFlow);
                    CreateCell(Row, 3, "Total", StyleFlow);
                    CreateCell(Row, 4, "Dados de Sistema, não modificar", StyleFlow);

                    templateModel.Domains.OrderBy(r => r.ReportOrder).ToList().ForEach(d =>
                    {
                        if (d.CategoryId == category.Id)
                        {
                            Row = Sheet.CreateRow(++RowIndex);
                            CreateCell(Row, 0, d.Description, StyleWhite);
                            CreateCell(Row, 1, "", StyleWhite);
                            CreateCell(Row, 2, "", StyleWhite);
                            CreateCell(Row, 3, "", StyleWhite);
                            CreateCell(Row, 4, d.Id, StyleWhite);
                        }
                    });

                    //hide column with ID's
                    Sheet.SetColumnHidden(4, true);
                }
                else
                {
                    CreateMergedCell(new CellRangeAddress(RowIndex, RowIndex, 0, 1), $"Categoria: {category.Description}", Sheet, 1);
                    Row = Sheet.CreateRow(++RowIndex);
                    CreateCell(Row, 0, category.Description, StyleFlow);
                    CreateCell(Row, 1, "Valor", StyleFlow);
                    CreateCell(Row, 2, "Dados de Sistema, não modificar", StyleFlow);

                    templateModel.Domains.OrderBy(r => r.ReportOrder).ToList().ForEach(d =>
                    {
                        if (d.CategoryId == category.Id)
                        {
                            Row = Sheet.CreateRow(++RowIndex);
                            CreateCell(Row, 0, d.Description, StyleWhite);
                            CreateCell(Row, 1, "", StyleWhite);
                            CreateCell(Row, 2, d.Id, StyleWhite);
                        }
                    });

                    //hide column with ID's
                    Sheet.SetColumnHidden(2, true);
                }

                Sheet.MergedRegions.ForEach(range =>
                {
                    RegionUtil.SetBorderTop(1, range, Sheet);
                    RegionUtil.SetBorderBottom(1, range, Sheet);
                    RegionUtil.SetBorderLeft(1, range, Sheet);
                    RegionUtil.SetBorderRight(1, range, Sheet);
                });
            });
        }
    }
}