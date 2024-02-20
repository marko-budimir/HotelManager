using Autofac;
using HotelManager.Service.Common;


namespace HotelManager.Service
{
    public class ServiceModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProfileService>().As<IProfileService>();
            builder.RegisterType<RoomService>().As<IRoomService>();
            builder.RegisterType<HotelServiceService>().As<IHotelServiceService>();
            builder.RegisterType<RoomTypeService>().As<IRoomTypeService>();
            builder.RegisterType<ReceiptService>().As<IReceiptService>();
            builder.RegisterType<ReservationService>().As<IReservationService>();
        }
    }
}
