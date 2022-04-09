using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using ApiTranslate.Domain.Interfaces.Apis;
using ApiTranslate.Domain.Entities;
using ApiTranslate.Domain.Entities.Response;

namespace ApiTranslate.Infra.CrossCutting.Apis
{
    public class HuamiApi : IHuamiApi
    {
        public async Task<CredentialResponse> GetHuamiCredentials(string deviceId)
        {

            try
            {
                var login_payload = new
                {
                    code = "access_token", // vem de outra chamada
                    grant_type = "access_token",
                    allow_registration = "false",
                    country_code = "BR",
                    app_name = "com.xiaomi.hm.health",
                    device_id = "FE:22:50:4B:49:D2", // "FE:22:50:4B:49:D2",
                    third_name = "google",
                    app_version = "4.8.1",
                    device_model = "android_phone"
                };
                var options = new RestClientOptions("https://account.huami.com/v2/client/login")
                {
                    Timeout = 100,
                    FollowRedirects= false
                };
                var client = new RestClient(options);

                RestRequest request = new RestRequest("/", Method.Post); // entender pq agr pede essa "/"
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                var json = JsonConvert.SerializeObject(login_payload);
                request.AddParameter("application/json", json, ParameterType.RequestBody);

                var response = await client.ExecuteAsync(request);

                var responseCredential = System.Text.Json.JsonSerializer.Deserialize<CredentialResponse>(response.Content);

                return responseCredential;
            }
            catch
            {
                return null;
            }
        }

        public async Task<DataMiBandResponse> GetHuamiBandData(DataMiBandRequest data)
        {
            try
            {
                // CredentialResponse credential = await GetHuamiCredentials(data.DeviceId);
                //user_id = 3082192403
                //apptoken = "UQVBQFJyQktGHlp6QkpbRl5LRl5qek4uXAQABAAAAAD49OnYivqtWLh9k2urSZWlj_FFDiRtnm-c8Bh62DN5O8VEFtGSHO7ZOVmvGo6LttOuQN_BNB1dPdgvVocBv_5jv_mUUt9DyJO3bV4lYknZ65CI_ukFrrxVNmsVzUXtGXK-A-BS9_cy8bzmnSfHEX-btve_Szc…"
                CredentialResponse credential = new CredentialResponse
                {
                    UserId = "3082192403",
                    Token = "UQVBQFJyQktGHlp6QkpbRl5LRl5qek4uXAQABAAAAAD49OnYivqtWLh9k2urSZWlj_FFDiRtnm-c8Bh62DN5O8VEFtGSHO7ZOVmvGo6LttOuQN_BNB1dPdgvVocBv_5jv_mUUt9DyJO3bV4lYknZ65CI_ukFrrxVNmsVzUXtGXK-A-BS9_cy8bzmnSfHEX-btve_Szc…"

                };

                var options = new RestClientOptions("https://api-mifit.huami.com/v1/data/band_data.json")
                {
                    Timeout = -1,
                    FollowRedirects = false
                };
                var client = new RestClient(options);

                RestRequest request = new RestRequest("/", Method.Get);
                request.AddParameter("query_type","summary");
                request.AddParameter("device_type","android_phone");
                request.AddParameter("userId", $"{credential.UserId}");
                request.AddParameter("from_date", $"{data.startDate:yyyy-MM-dd}");
                request.AddParameter("to_date", $"{data.endDate:yyyy-MM-dd}");
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", credential.Token));
                
                var response = await client.ExecuteAsync(request);

                var responseData = System.Text.Json.JsonSerializer.Deserialize<DataMiBandResponse>(response.Content); //entedenr aqui pois provavlemente é um array

                return responseData;
            }
            catch
            {
                return null;
            }
        }
    }
}

