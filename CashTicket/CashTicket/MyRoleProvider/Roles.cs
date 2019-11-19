using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CashTicket.Models;

namespace CashTicket.MyRoleProvider
{
    public class Roles : RoleProvider
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

        public override string[] GetRolesForUser(string username)
        {
            string[] roles = new string[] { };

            using (CashDeskEntities db = new CashDeskEntities())
            {
                Client client = db.Clients.FirstOrDefault(u => u.login == username);
                if (client != null)
                {
                    Role clientRole = db.Roles.Find(client.role_id);
                    if (clientRole != null)
                        roles = new string[] { clientRole.name_role };
                }
            }
            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;

            using (CashDeskEntities db = new CashDeskEntities())
            {
                Client client = db.Clients.FirstOrDefault(u => u.login == username);
                if (username != null)
                {
                    Role clientRole = db.Roles.Find(client.role_id);
                    if (clientRole != null && clientRole.name_role == roleName)
                        outputResult = true;
                }
            }
            return outputResult;
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