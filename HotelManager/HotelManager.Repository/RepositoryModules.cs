﻿using Autofac;
using HotelManager.Repository.Common;

namespace HotelManager.Repository
{
    public class RepositoryModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        { 
            builder.RegisterType<ProfileRepository>().As<IProfileRepository>();
            builder.RegisterType<RoleTypeRepository>().As<IRoleTypeRepository>();
        }
    }
}
