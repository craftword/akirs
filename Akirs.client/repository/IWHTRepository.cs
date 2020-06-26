using Akirs.client.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.repository
{
    public interface IWHTRepository : IRepository<WHTUPLOAD>
    {
        IEnumerable<WHTUPLOAD> GetPendingWhtUpload(string EnrollId);
        IEnumerable<WHTUPLOAD> GetWhtUpload(string EnrollId);
        WHTUPLOAD GetWhtUploadSingle(string EnrollId);
        WHTUPLOAD GetWhtUploadById(int Itbid);
        decimal? calculateWitholdTaxPayment(string EnrollID);
    }
}
