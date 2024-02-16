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
        }
    }
}
