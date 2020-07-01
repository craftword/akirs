using Akirs.client.DL;
using Akirs.client.Persistence.Repositories;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AKIRSTAXEntities _context;
        //internal object IncomePaymentModel;

        public UnitOfWork(AKIRSTAXEntities context)
        {
            _context = context;
            Notification = new NotificationUpdateRepository(_context);
            IncomePayment = new IncomePaymentRepository(_context);
            PayrollData = new PayrollDataRepository(_context);
            CompanyData = new CompanyDataRepository(_context);
            NextEnroll = new NexEnrollNumberRepository(_context);
            Family = new FamilyRepository(_context);
            RelationshipType = new RelationshipTypeRepository(_context);
            RelationshipType = new RelationshipTypeRepository(_context);
            FamilyDetails = new FamilyDetailsRepository(_context);
            Assessment = new AssessmentRepository(_context);
            AssessmentRecord = new AssessmentRecordRepository(_context);
            SalaryUpload = new SalaryUploadRepository(_context);
            WhtUpload = new WHTRepository(_context);
            EnrollLog = new EnrollRepository(_context);
            Roles = new RolesRepository(_context);
            Users = new UserRepository(_context);
            IncomeSourceType = new IncomeSourceTypeRepository(_context);
            IncomeDeclarartion = new IncomeDeclarationRepository(_context);
            IncomeModel = new IncomeSourceModelRepository(_context);
            IncomePaymentModel = new IncomePaymentModelRepository(_context);
            TaxType = new TaxTypeRepository(_context);
            City = new CityRepository(_context);
            State = new StateRepository(_context);
            Ministries = new MinistriesRepository(_context);
            RevenueHead = new RevenueHeadRepository(_context);
            MaritalStatus = new MaritalStatusRepository(_context);
            CompanyName = new CompanyNameRepository(_context);
            NotificationAlert = new NotificationRepository(_context);
            AddNotification = new AddNotification(_context);
            SalaryHistory = new SalaryHistoryRepository(_context);
            WhtHistory = new WhtHistoryRepository(_context); 
            RentPrices = new RentPriceRepository(_context);
            TransactionLog = new TransactionLogRepository(_context);
            UserRoleRepository = new UserRoleRepository(_context);
            SalaryUploadSecondary = new SalaryUploadSecondaryRepository(_context);
        }
        public ISalaryUploadSecondaryRepository SalaryUploadSecondary { get; private set; }
        public IRentPriceRepository RentPrices { get; private set; }
        public IWhtHistoryRepository WhtHistory { get; private set; } 
        public INotificationUpdateRepository Notification { get; private set; }
        public ISalaryHistoryRepository SalaryHistory { get; private set; }
        public IPayrollDataRepository PayrollData { get; private set; }
        public ICompanyDataRepository CompanyData { get; private set; }
        public INexEnrollNumberRepository NextEnroll { get; private set; }
        public ICompanyNameRepository CompanyName { get; private set; }
        public IMaritalStatusRepository MaritalStatus { get; private set; }
        public ICityRepository City { get; private set; }
        public IStateRepository State { get; private set; }
        public IMinistriesRepository Ministries { get; private set; }
        public IRevenueHeadRepository RevenueHead { get; private set; }
        public ITaxTypeRepository TaxType { get; private set; }
        public IIncomeSourceModelRepository IncomeModel { get; private set; }
        //public IIncomePaymentRepository IncomePaymentModel { get; private set; }
        public IUserRepository Users { get; private set; }
        public IIncomeDeclarationRepository IncomeDeclarartion { get; private set; }
        public IIncomeSourceTypeRepository IncomeSourceType { get; private set; }
        public IRolesRepository Roles { get; private set; }
        public IEnrollRepository EnrollLog { get; private set; }
        public ISalaryUploadRepository SalaryUpload { get; private set; }
        public IIncomePaymentRepository IncomePayment { get; private set; }
        public IWHTRepository WhtUpload { get; private set; }
        public IFamilyRepository Family { get; private set; }
        public IAssessmentRepository Assessment { get; private set; }
        public IAssessmentRecordRepository AssessmentRecord { get; private set; }
        public IFamilyDetailsRepository FamilyDetails { get; private set; }
        public IRelationshipTypeRepository RelationshipType { get; private set; }
        public INotificationRepository  NotificationAlert { get; private set; }
        public IAddNotification AddNotification { get; private set; }
        public INotificationLogRepository NotificationLog { get; private set; }
        public IIncomePaymentModelRepository IncomePaymentModel { get; private set; }
        public ITransactionLog TransactionLog { get; private set; }

        public IUserRoleRepository UserRoleRepository { get; private set;  }

        public int Complete()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch(DbEntityValidationException /*Exception*/ ex)
            {
                Exception raise = ex;
                foreach(var validationErrors in ex.EntityValidationErrors)
                {
                    foreach(var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(),
                                                        validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw ex;
            }
            return 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}