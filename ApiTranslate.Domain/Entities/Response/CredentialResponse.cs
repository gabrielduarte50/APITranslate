using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTranslate.Domain.Entities 
{
    public class TokenInfo
    {
        public string login_token { get; set; }
        public string app_token { get; set; }
        public string user_id { get; set; }
        public int ttl { get; set; }
        public int app_ttl { get; set; }
    }

    public class RegistInfo
    {
        public int is_new_user { get; set; }
        public long regist_date { get; set; }
        public string region { get; set; }
        public string country_code { get; set; }
    }

    public class ThirdpartyInfo
    {
        public string nickname { get; set; }
        public string icon { get; set; }
        public string third_id { get; set; }
        public string email { get; set; }
    }

    public class Domain
    {
        [JsonProperty("id-dns")]
        public string IdDns { get; set; }
    }

    public class CredentialResponse
    {
        public TokenInfo token_info { get; set; }
        public RegistInfo regist_info { get; set; }
        public ThirdpartyInfo thirdparty_info { get; set; }
        public string result { get; set; }
        public Domain domain { get; set; }
    }
}
