using ApiTranslate.Domain.Interfaces.Apis;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util;
using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTranslate.Infra.CrossCutting
{
    public class GoogleApi : IGoogleApi 
    {
        UserCredential userCredential ;
        public UserCredential GetAccessTokenAccount()
        {
            //ISOLAR EM ALGUM LUGAR
            string OAUTH_CLIENT_ID = "412602187402-2kmnlf3e71ift7re24sgid4eom25kjrt.apps.googleusercontent.com";
            string OAUTH_CLIENT_SECRET = "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";


            string[] scopes =
            {   
                "https://www.googleapis.com/auth/userinfo.email",
                "email",
                "openid",
                "https://www.googleapis.com/auth/userinfo.profile",
                "profile"

            };

            userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = OAUTH_CLIENT_ID,
                    ClientSecret = OAUTH_CLIENT_SECRET,
                },
                scopes, "user", CancellationToken.None).Result;


            if (userCredential.Token.IsExpired(SystemClock.Default))
            {
                userCredential.RefreshTokenAsync(CancellationToken.None).Wait();
            }

            return userCredential;
        }

        public UserCredential RevokeTokenAccount()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken taskCancellationToken = source.Token;
            
            userCredential.RevokeTokenAsync(taskCancellationToken);

            return userCredential;
        }
    }
}
