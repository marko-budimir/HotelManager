using Autofac;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;


namespace HotelManager.Repository
{
    public class RepositoryModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ReceiptRepository>().As<IReceiptRepository>();
            builder.RegisterType<ServiceInvoiceRepository>().As<IServiceInvoiceRepository>();
        }     
    }
}
