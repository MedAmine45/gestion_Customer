using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using ProjectBusinessCustomer.Models;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Configuration;
using System.Threading.Tasks;
using ProjectBusinessCustomer.TokenStorage;
using System.IdentityModel.Tokens;
using System.IdentityModel.Claims;
using Microsoft.Owin.Security;
using Microsoft.Identity.Client;
using System.Web;

namespace ProjectBusinessCustomer
{
    public partial class Startup
    {

        // The appId is used by the application to uniquely identify itself to Azure AD.
        // The appSecret is the application's password.
        // The redirectUri is where users are redirected after sign in and consent.
        // The graphScopes are the Microsoft Graph permission scopes that are used by this sample: User.Read Mail.Send
        private static string appId = ConfigurationManager.AppSettings["ida:AppId"];
        private static string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static string graphScopes = ConfigurationManager.AppSettings["ida:GraphScopes"];

        // Pour plus d'informations sur la configuration de l'authentification, visitez https://go.microsoft.com/fwlink/?LinkId=301864
        //public void ConfigureAuth(IAppBuilder app)
        //{
        //    app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

        //    app.UseCookieAuthentication(new CookieAuthenticationOptions());

        //    app.UseOpenIdConnectAuthentication(
        //        new OpenIdConnectAuthenticationOptions
        //        {

        //            // The `Authority` represents the Microsoft v2.0 authentication and authorization service.
        //            // The `Scope` describes the permissions that your app will need. See https://azure.microsoft.com/documentation/articles/active-directory-v2-scopes/                    
        //            ClientId = appId,
        //            //https://login.microsoftonline.com/common/v2.0
        //            Authority = "https://login.microsoftonline.com/common/v2.0",
        //            PostLogoutRedirectUri = redirectUri,
        //            RedirectUri = redirectUri,
        //            Scope = "openid email profile offline_access " + graphScopes,
        //            TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ValidateIssuer = false,
        //                // In a real application you would use IssuerValidator for additional checks, 
        //                // like making sure the user's organization has signed up for your app.
        //                //IssuerValidator = (issuer, token, tvp) =>
        //                //{
        //                //    if (MyCustomTenantValidation(issuer))
        //                //        return issuer;
        //                //    else
        //                //        throw new SecurityTokenInvalidIssuerException("Invalid issuer");
        //                //},
        //            },
        //            Notifications = new OpenIdConnectAuthenticationNotifications
        //            {
        //                AuthorizationCodeReceived = async (context) =>
        //                {
        //                    var code = context.Code;
        //                    string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;

        //                    TokenCache userTokenCache = new SessionTokenCache(signedInUserID,
        //                        context.OwinContext.Environment["System.Web.HttpContextBase"] as HttpContextBase).GetMsalCacheInstance();
        //                    ConfidentialClientApplication cca = new ConfidentialClientApplication(
        //                        appId,
        //                        redirectUri,
        //                        new ClientCredential(appSecret),
        //                        userTokenCache,
        //                        null);
        //                    string[] scopes = graphScopes.Split(new char[] { ' ' });

        //                    AuthenticationResult result = await cca.AcquireTokenByAuthorizationCodeAsync(code, scopes);
        //                },
        //                AuthenticationFailed = (context) =>
        //                {
        //                    context.HandleResponse();
        //                    context.Response.Redirect("/Error?message=" + context.Exception.Message);
        //                    return Task.FromResult(0);
        //                }
        //            }
        //        });
        //}

        //private bool MyCustomTenantValidation(string issuer)
        //{
        //    throw new NotImplementedException();
        //}

        public void ConfigureAuth(IAppBuilder app)
        {
            // Configurer le contexte de base de données, le gestionnaire des utilisateurs et le gestionnaire des connexions pour utiliser une instance unique par demande
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Autoriser l’application à utiliser un cookie pour stocker des informations pour l’utilisateur connecté
            // et pour utiliser un cookie à des fins de stockage temporaire des informations sur la connexion utilisateur avec un fournisseur de connexion tiers
            // Configurer le cookie de connexion
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                ExpireTimeSpan = TimeSpan.FromMinutes(7),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Permet à l'application de valider le timbre de sécurité quand l'utilisateur se connecte.
                    // Cette fonction de sécurité est utilisée quand vous changez un mot de passe ou ajoutez une connexion externe à votre compte.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Permet à l'application de stocker temporairement les informations utilisateur lors de la vérification du second facteur dans le processus d'authentification à 2 facteurs.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Permet à l'application de mémoriser le second facteur de vérification de la connexion, un numéro de téléphone ou un e-mail par exemple.
            // Lorsque vous activez cette option, votre seconde étape de vérification pendant le processus de connexion est mémorisée sur le poste à partir duquel vous vous êtes connecté.
            // Ceci est similaire à l'option RememberMe quand vous vous connectez.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Supprimer les commentaires des lignes suivantes pour autoriser la connexion avec des fournisseurs de connexions tiers
            app.UseMicrosoftAccountAuthentication(
                clientId: appId,
                clientSecret: appSecret);

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}