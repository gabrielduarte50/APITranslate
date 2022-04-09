using ApiTranslate.Domain.Interfaces.Apis;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace ApiTranslate.Infra.CrossCutting
{
    public class GoogleApi : IGoogleApi // aparentemente nao é tao simples assim
    {
        public async Task<string> GetAccessTokenAccount()
        {
            string OAUTH_CLIENT_ID = "412602187402-606djaeg6im0ng2a4bg9t846v8la6t9c.apps.googleusercontent.com";
            string OAUTH_CLIENT_SECRET = "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";

            try
            {
                var options = new RestClientOptions("https://accounts.google.com/o/oauth2/auth")
                {
                    Timeout = 100
                };
                var client = new RestClient(options);
                var request = new RestRequest("", Method.Post);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&scope=all&client_id=" + OAUTH_CLIENT_ID + "&client_secret=" + OAUTH_CLIENT_SECRET, ParameterType.RequestBody);

                var response = await client.PostAsync(request);
                return System.Text.Json.JsonSerializer.Deserialize<string>(response.Content);
            }
            catch
            {
                return null;
            }
        }
    }
}

//function getOpenIdService()
//{

//    return OAuth2.createService('openid')
//        .setAuthorizationBaseUrl('https://accounts.google.com/o/oauth2/auth')
//        .setTokenUrl('https://accounts.google.com/o/oauth2/token')
//        .setClientId(OAUTH_CLIENT_ID)
//        .setClientSecret(OAUTH_CLIENT_SECRET)
//        .setCallbackFunction('authCallback')
//        .setPropertyStore(PropertiesService.getUserProperties())
//        .setScope('https://www.googleapis.com/auth/userinfo.email email openid https://www.googleapis.com/auth/userinfo.profile profile')
//        .setParam('access_type', 'offline')
//        .setParam('prompt', 'consent')
//}
//
// var googleToken = getOpenIdService().getAccessToken();
//https://www.c-sharpcorner.com/article/asp-net-mvc-oauth-2-0-rest-web-api-authorization-using-database-first-approach/
// https://developers.google.com/api-client-library/dotnet/guide/aaa_oauth
//https://developers.google.com/api-client-library/dotnet/get_started
// a chave deve ser salva em client_secrets.json
//https://developers.google.com/api-client-library/dotnet/guide/aaa_oauth -> esse aqui tem mais cara de que mostra as configuraçoes
//https://www.youtube.com/watch?v=Cwwd9iiMeQo entender melhor sobre o OaUTH DO GOOGLE
//https://www.youtube.com/watch?v=6nQf76HF20A o cara faz algo ligado a logar a conta abrindo o console de permissao para isso, nao sei se é viável para o projeto, ams talvez seja o necessario

