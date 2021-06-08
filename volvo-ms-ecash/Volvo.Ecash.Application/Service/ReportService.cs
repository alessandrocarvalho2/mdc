using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Application.Utils;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;
using static Volvo.Ecash.Dto.Enum.EnumCommon;

namespace Volvo.Ecash.Application.Service
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IDomainRepository _domainRepository;
        private readonly ICashFlowRepository _cashFlowRepository;

        public ReportService(IReportRepository reportRepository,
            IDomainRepository domainRepository,
            ICashFlowRepository cashFlowRepository)
        {
            _reportRepository = reportRepository;
            _domainRepository = domainRepository;
            _cashFlowRepository = cashFlowRepository;
        }

        public async Task<CashConsolidationReport> GetCashConsolidationReport(DateTime date, DateTime dayBefore)
        {
            CashConsolidationReport report = new CashConsolidationReport
            {
                CashConsolidationItems = await _reportRepository.GetListCCIAsync(dayBefore)
            };
            report.CashConsolidationItems.ForEach(i =>
            {
                i.BankAccount.AccountBalance = i.BankAccount.AccountBalance ?? new AccountBalance();
                decimal amount = _reportRepository.GetSumDbtCrdt(i.BankAccount.Id, date);
                i.ReserveAmount = i.BankAccount.AccountBalance.Balance + amount;
                if (amount > 0)
                {
                    i.Credits = amount;
                }
                else
                {
                    i.Debits = amount;
                }
                CalculateCashNeeds(i);
                report.TotalMinimum += i.MinimumBalance;
                report.TotalReserve += i.ReserveAmount;
                report.TotalCredits += i.Credits;
                report.TotalDebits += i.Debits;
                //Total Amount = Total Credits - Total Debits
                report.TotalAmount += i.BankAccount.AccountBalance.Balance;
            });
            CalculateTotals(report);
            return report;
        }

        private void CalculateTotals(CashConsolidationReport report)
        {
            //Aplication Amount must be a multiple of 1000
            var intermediario = (report.TotalReserve - report.TotalMinimum) % 1000;
            report.ApplicationAmount = report.TotalReserve - report.TotalMinimum - intermediario;

            // Application Amount > 0  = 'Aplicação'
            // Application Amount = 0  = '-'
            // Application Amount < 0  = 'Resgate'
            report.Message = report.ApplicationAmount != 0 ? report.ApplicationAmount > 0 ? "Aplicação" : "Resgate" : "-";
        }

        private void CalculateCashNeeds(CashConsolidationItem cci)
        {
            if (!cci.BankAccount.IsMainAccount)
            {
                var min = cci.BankAccount.MinimumBalance - cci.BankAccount.BalanceTolerance;
                var max = cci.BankAccount.MinimumBalance + cci.BankAccount.BalanceTolerance;
                if (min < cci.ReserveAmount && cci.ReserveAmount <= max)
                {
                    cci.MinimumBalance = cci.ReserveAmount;
                }
                else
                {
                    var rest = cci.ReserveAmount % 1000;
                    cci.MinimumBalance = cci.BankAccount.MinimumBalance + rest;
                }
            }
            else
            {
                cci.MinimumBalance = cci.BankAccount.MinimumBalance;
            }
        }

        public async Task<List<CashTransferReport>> GetListCashTransferReport(DateTime date, DateTime dayBefore)
        {
            CashConsolidationReport cashConsolidationReport = await GetCashConsolidationReport(date, dayBefore);
            List<CashTransferReport> reports = new List<CashTransferReport>();
            CashConsolidationItem mainAccount = cashConsolidationReport.CashConsolidationItems.Where(r => r.BankAccount.IsMainAccount).First();
            if (mainAccount == null)
            {
                throw new ArgumentException("Conta principal não encontrada");
            }
            cashConsolidationReport.CashConsolidationItems.ForEach(account =>
            {
                if (!account.BankAccount.IsMainAccount)
                {
                    CashTransferReport ctr = new CashTransferReport();
                    ctr.AmountToTransfer = account.ReserveAmount - account.MinimumBalance;
                    if (ctr.AmountToTransfer > 0)
                    {
                        ctr.DestinyAccount = mainAccount.BankAccount;
                        ctr.OriginAccount = account.BankAccount;
                    }
                    else
                    {
                        ctr.OriginAccount = mainAccount.BankAccount;
                        ctr.DestinyAccount = account.BankAccount;
                    }
                    ctr.AmountToTransfer = Math.Abs(ctr.AmountToTransfer);
                    reports.Add(ctr);
                }
            });
            return reports;
        }

        public async Task<List<CashFlow>> GenerateCashTransferReport(DateTime date, int userId, DateTime dayBefore)
        {
            List<CashTransferReport> reports = await GetListCashTransferReport(date, dayBefore);
            try
            {
                List<CashFlow> cashFlows = new List<CashFlow>();
                reports.ForEach(transfer =>
                {
                    decimal amount = Math.Abs(transfer.AmountToTransfer);

                    //money leaves the account
                    CashFlow outFlow = new CashFlow();
                    outFlow.Domain = _domainRepository.GetTransferDomainOut(transfer);
                    outFlow.DomainId = outFlow.Domain.Id; //get the domainID for the destiny account
                    outFlow.Amount = amount * -1;
                    outFlow.Date = date;
                    outFlow.Approval = false;
                    outFlow.CreateBy = userId;
                    outFlow.CreateAt = DateTime.Now;

                    cashFlows.Add(outFlow);

                    //money comes to the account
                    CashFlow inFlow = new CashFlow();
                    inFlow.Domain = _domainRepository.GetTransferDomainIn(transfer); //get the domainID for the origin account
                    inFlow.DomainId = inFlow.Domain.Id; //get the domainID for the origin account
                    inFlow.Amount = amount;
                    inFlow.Date = date;
                    inFlow.Approval = false;
                    inFlow.CreateBy = userId;
                    inFlow.CreateAt = DateTime.Now;

                    cashFlows.Add(inFlow);
                });
                return cashFlows;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<TotalizationReport> GetTotalizationReport(CashFlowFilters filters)
        {
            TotalizationReport totalization = new TotalizationReport
            {
                ItemsIn = new List<TotalizationReportItemIn>(),
                ItemsOut = new List<TotalizationReportItemOut>()
            };

            var cashFlows = await _cashFlowRepository.GetListCashFlowAsync(filters);

            cashFlows.ForEach(cf =>
            {
                if (cf.Amount != 0)
                {
                    if (cf.Amount > 0)
                    {
                        //Check if category exists
                        if (totalization.ItemsIn.Any(i => cf.Domain.Category.Description == i.CategoryName))
                        {
                            //Get the category item
                            var tri = totalization.ItemsIn.First(e => e.CategoryName == cf.Domain.Category.Description);
                            //then add value
                            tri.SubTotal += cf.Amount;
                        }
                        else
                        {
                            //if not exists, Add to the list and to the initial value
                            totalization.ItemsIn.Add(new TotalizationReportItemIn()
                            {
                                CategoryName = cf.Domain.Category.Description,
                                SubTotal = cf.Amount
                            });
                        }
                        //sumup the totals in this category
                        totalization.SubtotalIn += cf.Amount;
                    }
                    else
                    {
                        totalization.SubtotalOut += cf.Amount;
                        if (totalization.ItemsOut.Any(i => cf.Domain.Category.Description == i.CategoryName))
                        {
                            var tro = totalization.ItemsOut.First(e => e.CategoryName == cf.Domain.Category.Description);
                            if (cf.Domain.ApprovationNeeded)
                            {
                                if (cf.Approval)
                                {
                                    tro.SubTotalApproved += cf.Amount;
                                    totalization.SubtotalApproved += cf.Amount;
                                }
                                else
                                {
                                    tro.SubTotalNotApproved += cf.Amount;
                                    totalization.SubtotalNotApproved += cf.Amount;
                                }
                            }
                            else
                            {
                                tro.SubTotalPreApproved += cf.Amount;
                                totalization.SubtotalPreApproved += cf.Amount;
                            }
                            tro.SubTotal += cf.Amount;
                        }
                        else
                        {
                            var tri = new TotalizationReportItemOut()
                            {
                                CategoryName = cf.Domain.Category.Description,
                            };
                            if (cf.Domain.ApprovationNeeded)
                            {
                                if (cf.Approval)
                                {
                                    tri.SubTotalApproved = cf.Amount;
                                    totalization.SubtotalApproved += cf.Amount;
                                }
                                else
                                {
                                    tri.SubTotalNotApproved = cf.Amount;
                                    totalization.SubtotalNotApproved += cf.Amount;
                                }
                            }
                            else
                            {
                                tri.SubTotalPreApproved = cf.Amount;
                                totalization.SubtotalPreApproved += cf.Amount;
                            }
                            tri.SubTotal += cf.Amount;
                            totalization.ItemsOut.Add(tri);
                        }
                    }
                }
                else
                {
                    if (cf.Domain.InOut == "OUT")
                    {
                        if (!totalization.ItemsOut.Any(i => cf.Domain.Category.Description == i.CategoryName))
                        {
                            totalization.ItemsOut.Add(new TotalizationReportItemOut()
                            {
                                CategoryName = cf.Domain.Category.Description
                            });

                        }
                    }
                    else
                    {
                        if (!totalization.ItemsIn.Any(i => cf.Domain.Category.Description == i.CategoryName))
                        {
                            totalization.ItemsIn.Add(new TotalizationReportItemIn()
                            {
                                CategoryName = cf.Domain.Category.Description
                            });

                        }
                    }
                }
            });

            totalization.Total = totalization.SubtotalIn + totalization.SubtotalApproved + totalization.SubtotalNotApproved + totalization.SubtotalPreApproved;
            return totalization;
        }
    }
}
