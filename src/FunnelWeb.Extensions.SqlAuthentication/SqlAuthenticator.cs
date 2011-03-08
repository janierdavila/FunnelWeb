﻿using System.Web.Mvc;
using System.Web.Security;
using FunnelWeb.Authentication;
using FunnelWeb.Authentication.Internal;
using FunnelWeb.Extensions.SqlAuthentication.Model;
using FunnelWeb.Settings;
using NHibernate;

namespace FunnelWeb.Extensions.SqlAuthentication
{
    public class SqlAuthenticator : IAuthenticator
    {
        private readonly FormsAuthenticator _formsAuthenticator;

        public SqlAuthenticator()
        {
            _formsAuthenticator = new FormsAuthenticator();
        }

        public bool AuthenticateAndLogin(string username, string password)
        {
            return UseFormsAuthentication ?
                _formsAuthenticator.AuthenticateAndLogin(username, password) :
                SqlAuthenticateAndLogin(username, password);
        }

        private static bool UseFormsAuthentication
        {
            get
            {
                return DependencyResolver.Current.GetService<IDatabaseUpgradeDetector>().UpdateNeeded() ||
                    !DependencyResolver.Current.GetService<ISettingsProvider>().GetSettings<SqlAuthSettings>().SqlAuthenticationEnabled;
            }
        }

        private static bool SqlAuthenticateAndLogin(string username, string password)
        {
            var session = DependencyResolver.Current.GetService<ISession>();
            var user = session.QueryOver<User>()
                .Where(u => u.Username == username && u.Password == FunnelWebSqlMembership.HashPassword(password))
                .SingleOrDefault();

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(username, true);
                return true;
            }

            return false;
        }

        public void Logout()
        {
            _formsAuthenticator.Logout();
        }
    }
}
