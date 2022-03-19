using CoPayApi.Data.Entities;
using coSapApi.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coSapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class paymentController : ControllerBase
    {
        [Route("addPayment"),HttpPost]
        public string addPayment(bool isTest,[FromBody] Payment payment)
        {
            company.Connect(isTest);
            JournalEntries journalEntries = (JournalEntries)company.SapContext.GetBusinessObject(BoObjectTypes.oJournalEntries);
            journalEntries.Memo =payment.comment;
            journalEntries.Reference = payment.CraditCard.Mask.ToString();
            journalEntries.Reference2 = payment.user.FirstName+payment.user.LastName;
            journalEntries.Reference3 = payment.business;
            journalEntries.ReferenceDate = payment.dateAdded;
            journalEntries.DueDate = payment.dateAdded;
            journalEntries.TaxDate = payment.dateAdded;

            journalEntries.Lines.SetCurrentLine(0);
            journalEntries.Lines.ShortName = payment.user.company.customerCode;
            journalEntries.Lines.Debit = (double)payment.sum;
            journalEntries.Lines.Add();

            int responseCode = journalEntries.Add();
            if (responseCode != 0)
            {
                string errMsg = $"Error: Failed to add voucher: {company.SapContext.GetLastErrorDescription()}";

               return $"errMsg:{errMsg} sapErrorCode:{responseCode}";
            }
            return "success";

        }
    }
}
