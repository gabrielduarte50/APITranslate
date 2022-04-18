using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using ApiTranslate.Domain.Interfaces.Apis;
using ApiTranslate.Domain.Entities;
using ApiTranslate.Domain.Entities.Response;
using System.Web;
using System;

namespace ApiTranslate.Infra.CrossCutting.Apis
{
    public class HuamiApi : IHuamiApi
    {
        public async Task<CredentialResponse> GetHuamiCredentials(string deviceId) // "FE:22:50:4B:49:D2",
        {

            try
            {
                var options = new RestClientOptions("https://account.huami.com/v2/client/")
                {
                    Timeout = -1,
                    FollowRedirects= false
                };
                var client = new RestClient(options);

                RestRequest request = new RestRequest("login", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                //o campo code vem da autorização do google que vai ser o empecilho do projeto
                //no momento apenas consegui fazer rodar aqui passando ele depois de logar no app no celular
                //obter pela planilha e colocar o token gerado nessa hora aqui
                request.AddParameter("code", HttpUtility.HtmlEncode("ya29.A0ARrdaM8Is4ponWn15Vg2y-nyPRDsyDad9cQ3ouplW-SIuZf4f-Iiim3xg-2hDjuOuGdvsvu8yM_uhbvQU3iERuEE7CH7Fi3l6_mezGlB_8bxF0PGU_WQWM7Wb_orVQ0Yff6BEZbYXs9ankz-r-iQROiiOI5H"));
                request.AddParameter("grant_type", HttpUtility.HtmlEncode("access_token"));
                request.AddParameter("allow_registration", HttpUtility.HtmlEncode("false"));
                request.AddParameter("country_code", HttpUtility.HtmlEncode("BR"));
                request.AddParameter("app_name", HttpUtility.HtmlEncode("com.xiaomi.hm.health"));
                request.AddParameter("device_id", HttpUtility.HtmlEncode($"{deviceId}"));
                request.AddParameter("third_name", HttpUtility.HtmlEncode("google"));
                request.AddParameter("app_version", HttpUtility.HtmlEncode("4.8.1"));
                request.AddParameter("device_model", HttpUtility.HtmlEncode("android_phone"));

                var response = await client.ExecuteAsync(request); //recebendo erro_code: 0106 -> devido ao campo code

                CredentialResponse responseCredential = JsonConvert.DeserializeObject<CredentialResponse>(response.Content); //ver a melhor forma de mapear da resposta para o que quero


                return responseCredential;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<DataMiBandResponse> GetHuamiBandData(DataMiBandRequest data, TokenInfo credential)
        {
            try
            {
                var options = new RestClientOptions("https://api-mifit.huami.com/v1/data/band_data.json")
                {
                    Timeout = -1,
                    FollowRedirects = false
                };
                var client = new RestClient(options);

                RestRequest request = new RestRequest("/", Method.Get);
                request.AddParameter("query_type", HttpUtility.HtmlEncode("summary"));
                request.AddParameter("device_type", HttpUtility.HtmlEncode("android_phone"));
                request.AddParameter("userId", HttpUtility.HtmlEncode($"{credential.user_id}"));
                request.AddParameter("from_date", HttpUtility.HtmlEncode( $"{data.startDate:yyyy-MM-dd}"));
                request.AddParameter("to_date", HttpUtility.HtmlEncode($"{data.endDate:yyyy-MM-dd}"));
                client.AddDefaultHeader("apptoken", string.Format("{0}", HttpUtility.HtmlEncode(credential.app_token)));
                
                var response = await client.ExecuteAsync(request);

                DataMiBandResponse responseData = System.Text.Json.JsonSerializer.Deserialize<DataMiBandResponse>(response.Content);

                return responseData;
            }
            catch
            {
                return null;
            }
        }

        public async Task<DataMiBandResponse> GetHuamiBandDataSport(DataMiBandRequest data, TokenInfo credential)
        {
            try
            {
                var options = new RestClientOptions("https://api-mifit-us2.huami.com/v1/sport/run/history.json")
                {
                    Timeout = -1,
                    FollowRedirects = false
                };
                var client = new RestClient(options);

                RestRequest request = new RestRequest("/", Method.Get);
               
                request.AddParameter("userId", HttpUtility.HtmlEncode($"{credential.user_id}"));
                request.AddParameter("from", HttpUtility.HtmlEncode($"{data.startDate:yyyy-MM-dd}"));
                request.AddParameter("to", HttpUtility.HtmlEncode($"{data.endDate:yyyy-MM-dd}"));
                request.AddParameter("source", HttpUtility.HtmlEncode("huami"));
                client.AddDefaultHeader("apptoken", string.Format("{0}", HttpUtility.HtmlEncode(credential.app_token)));

                var response = await client.ExecuteAsync(request);

                DataMiBandResponse responseData = System.Text.Json.JsonSerializer.Deserialize<DataMiBandResponse>(response.Content);

                return responseData;
            }
            catch
            {
                return null;
            }
        }
    }
}

