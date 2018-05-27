using System;
using System.Web.Security;
using BLL_API;

namespace EShop.RoleProviders
{
    public class SiteRole : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)   //TODO: IMPORTANT
        {
            string role = "";
            if(username[0] == 'a')
            {
                var adminService = UnityConfig.Container.Resolve(typeof(IAdminService), "") as IAdminService;
                var admin = adminService.GetAdmin(username.Substring(1, username.Length - 1));
                role = admin != null ? "Admin" : "";
            }
            else if(username[0] == 'c')
            {
                var customerService = UnityConfig.Container.Resolve(typeof(ICustomerAccountService), "") as ICustomerAccountService;
                var customer = customerService.GetCustomer(username.Substring(1, username.Length - 1));
                role = customer != null ? "Customer" : "";
            }
            string[] result = { role };
            return result;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}