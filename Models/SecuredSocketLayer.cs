using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAILI.Models
{
    public class SecuredSocketLayer
    {
        public SecuredSocketLayer() { }

        public string IsNotAuthenticated()
        {
            string returnUrl = null;

            var uri = new UriBuilder()
            {
                Scheme = Uri.UriSchemeHttp,
                Port = 4529
            };
            returnUrl = uri.ToString() + "Home/Index";
           // returnUrl = uri.Scheme.ToString() + "://saili.azurewebsites.net/Home/Index";

            return returnUrl;
        }

        public string ReturnToHTTP(string returnUrl)
        {
            string url = null;

            var uri = new UriBuilder()
            {
                Scheme = Uri.UriSchemeHttp,
                Port = 4529
            };
            url = uri.Scheme.ToString() + returnUrl;

            return url;
        }

        public string SecuredLayerLogin()
        {
            string returnUrl = null;

            var uri = new UriBuilder()
            {
                Scheme = Uri.UriSchemeHttps,
                Port = 44347
            };

            returnUrl = uri.ToString() + "Account/Login";
            //returnUrl = uri.Scheme.ToString() + "://saili.azurewebsites.net/Account/Login";
            return returnUrl;
        }

        public string SecuredLayerRegister()
        {
            string returnUrl = null;

            var uri = new UriBuilder()
            {
                Scheme = Uri.UriSchemeHttps,
                Port = 44347
            };

            returnUrl = uri.ToString() + "Account/Register";
            //returnUrl = uri.Scheme.ToString() + "://saili.azurewebsites.net/Account/Register";
            return returnUrl;
        }
    }
}