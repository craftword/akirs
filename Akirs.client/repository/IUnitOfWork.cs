using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRentPriceRepository RentPrices { get; }
        IWhtHistoryRepository WhtHistory { get; }
        ICompanyDataRepository CompanyData { get; }
        INotificationUpdateRepository Notification { get; }
        ISalaryHistoryRepository SalaryHistory { get; }
        INexEnrollNumberRepository NextEnroll { get; }
        INotificationRepository  NotificationAlert { get;}
        IAddNotification AddNotification { get; }
        ICompanyNameRepository CompanyName { get; }
        IFamilyRepository Family { get; }
        IFamilyDetailsRepository FamilyDetails { get; }
        IRelationshipTypeRepository RelationshipType { get; }
        IAssessmentRepository Assessment { get; }
        ISalaryUploadRepository SalaryUpload { get; }
        IWHTRepository WhtUpload { get; }
        IEnrollRepository EnrollLog { get; }
        IRolesRepository Roles { get; }
        ITaxTypeRepository TaxType { get; }
        ICityRepository City { get; }
        IMaritalStatusRepository MaritalStatus { get; }
        IRevenueHeadRepository RevenueHead { get; }
        IStateRepository State { get; }
        IMinistriesRepository Ministries { get; }
        IAssessmentRecordRepository AssessmentRecord { get; }
        IIncomeDeclarationRepository IncomeDeclarartion { get; }
        IIncomeSourceTypeRepository IncomeSourceType { get; }
        IIncomeSourceModelRepository IncomeModel { get; }

        ITransactionLog TransactionLog { get; }
        int Complete();
    }
}