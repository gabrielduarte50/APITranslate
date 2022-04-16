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
                request.AddParameter("code", HttpUtility.HtmlEncode("ya29.A0ARrdaM_a2gF0khAbA6ZWtuxRjeluiTl30ousud5tTBPAbKd0Lvqjz9MV4vGMsJYXL2DHPUE_6wws82RTZfoLj7oX-68LZtdVUaEQ1o0DXS0s_oUCOVR8w6XBM8i7nzPogjznupLza9Q5yHHSfTOXxRVaSHv7"));
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

        public async Task<DataMiBandResponse> GetHuamiBandData(DataMiBandRequest data)
        {
            try
            {
                CredentialResponse credential = await GetHuamiCredentials(data.DeviceId);

                var options = new RestClientOptions("https://api-mifit.huami.com/v1/data/band_data.json")
                {
                    Timeout = -1,
                    FollowRedirects = false
                };
                var client = new RestClient(options);

                RestRequest request = new RestRequest("/", Method.Get);
                request.AddParameter("query_type", HttpUtility.HtmlEncode("summary"));
                request.AddParameter("device_type", HttpUtility.HtmlEncode("android_phone"));
                request.AddParameter("userId", HttpUtility.HtmlEncode($"{credential.token_info.user_id}"));
                request.AddParameter("from_date", HttpUtility.HtmlEncode( $"{data.startDate:yyyy-MM-dd}"));
                request.AddParameter("to_date", HttpUtility.HtmlEncode($"{data.endDate:yyyy-MM-dd}"));
                client.AddDefaultHeader("apptoken", string.Format("{0}", HttpUtility.HtmlEncode(credential.token_info.app_token)));
                
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

