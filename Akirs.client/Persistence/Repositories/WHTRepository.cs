using Akirs.client.DL;
using Akirs.client.Enums;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Akirs.client.Persistence.Repositories
{
    public class WHTRepository : Repository<WHTUPLOAD>, IWHTRepository
    {
        public WHTRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        public IEnumerable<WHTUPLOAD> GetPendingWhtUpload(string EnrollId)
        {
            var ret = PlutoContext.WHTUPLOADs.Where(p => p.EnrollmentID == EnrollId && p.Status == "P").ToList();
            return ret;
        }
        public IEnumerable<WHTUPLOAD> GetWhtUpload(string EnrollId)
        {
            var ret = PlutoContext.WHTUPLOADs.Where(p => p.EnrollmentID == EnrollId && p.Status =="A").ToList();
            return ret;
        }
        public WHTUPLOAD GetWhtUploadSingle(string EnrollId)
        {
            
            var ret = PlutoContext.WHTUPLOADs.Where(p => p.EnrollmentID == EnrollId).FirstOrDefault();
            return ret;
        }
        public WHTUPLOAD GetWhtUploadById(int Itbid)
        {

            var ret = PlutoContext.WHTUPLOADs.Where(p => p.ItbID == Itbid).FirstOrDefault();
            return ret;
        }

        public decimal? calculateWitholdTaxPayment(string EnrollID)
        {

            decimal? amount = 0;
            var ret = PlutoContext.WHTUPLOADs.Where(p => p.EnrollmentID == EnrollID && p.Status == "P").ToList();

            if(ret.Count != 0)
            {
                foreach (var witholdTax in ret)
                {
                    amount += witholdTax.TaxAmount;
                }
            }
            return amount;
        }
        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}