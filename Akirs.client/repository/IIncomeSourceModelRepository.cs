﻿using Akirs.client.DL;
using Akirs.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.repository
{
    public interface IIncomeSourceModelRepository : IRepository<IncomeSourceModel>
    {
        IEnumerable<IncomeSourceModel> GetIncomeSourceTypeDetails(string EnrollId);
        IncomeSourceModel GetIncomeSourceTypeDetailsById(int Itbid);
    }
}
