using AutoMapper;
using BLL;
using BLL_API;
using DAL;
using DAL_API;
using EShop.App_Start;
using Mehdime.Entity;
using System;

using Unity;

namespace EShop
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            
            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<IAdminRepository, AdminRepository>();
            container.RegisterType<IFileLoader, FileLoader>();
            container.RegisterType<IPaymentService, PaymentService>();
            container.RegisterType<IImportService, ImportService>();
            container.RegisterType<ICustomerAccountService, CustomerAccountService>();
            container.RegisterType<IAdminService, AdminService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IItemQueryService, ItemQueryService>();
            container.RegisterType<IItemManagementService, ItemManagementService>();
            container.RegisterType<ICustomerPaymentService, CustomerPaymentService>();
            container.RegisterType<ICartService, CartService>();
            container.RegisterType<IOrderRatingRepository, OrderRatingRepository>();
            container.RegisterType<IOrderRepository, OrderRepository>();
            container.RegisterType<IDiscountRepository, DiscountRepository>();
            container.RegisterType<IOrderRatingService, OrderRatingService>();
            container.RegisterType<IDiscountManagementService, DiscountManagementService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IPropertyService, PropertyService>();
            container.RegisterType<IPropertyRepository, PropertyRepository>();

            container.RegisterType<IEmailService, EmailService>();

            container.RegisterInstance<IMapper>(MappingProfile.InitializeAutoMapper().CreateMapper());
            container.RegisterInstance<IDbContextScopeFactory>(new DbContextScopeFactory());
            container.RegisterInstance<IAmbientDbContextLocator>(new AmbientDbContextLocator());
        }
    }
}