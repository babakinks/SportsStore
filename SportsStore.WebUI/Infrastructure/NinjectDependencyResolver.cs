﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Ninject;
//using System.Web.Mvc;
//using Moq;
//using SportsStore.Domain.Entities;
//using SportsStore.Domain.Abstract;
//using System.Configuration;
//using SportsStore.Domain.Concrete;

//namespace SportsStore.WebUI.Infrastructure
//{
//    public class NinjectDependencyResolver: IDependencyResolver
//    {
//        private IKernel kernel;

//        public NinjectDependencyResolver(IKernel kernelParam)
//        {
//            kernel = kernelParam;
//            AddBindings();
//        }

//        private void AddBindings()
//        {
//            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
//            //mock.Setup(m => m.Products).Returns(new List<Product>
//            //{
//            //    new Product {Name = "Football", Price = 25},
//            //    new Product {Name = "Surf Board", Price = 179},
//            //    new Product {Name = "Shoes", Price = 96}
//            //});

//            //kernel.Bind<IProductRepository>().ToConstant(mock.Object);

//            kernel.Bind<IProductRepository>().To<EFProductRepository>();
//        }
//        public object GetService(Type serviceType)
//        {
//            return kernel.TryGet(serviceType);
//        }

//        public IEnumerable<object> GetServices(Type serviceType)
//        {
//            return kernel.GetAll(serviceType);
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Moq;
using Ninject;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Infrastructure.Concrete;

namespace SportsStore.WebUI.Infrastructure
{

    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IProductRepository>().To<EFProductRepository>();

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager
                    .AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);

            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}
