using System.Security.Claims;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TMS.Infrastructure.Auth;

 public record MyRolesRecord
        {
            public MyRolesRecord(string token)
            {
                Token = token;
                var parts = token.Split('.');
                if (parts.Length < 2)
                    throw new ArgumentException("Invalid token format");

                string partToConvert = parts[1];
                partToConvert = partToConvert.Replace('-', '+');
                partToConvert = partToConvert.Replace('_', '/');
                switch (partToConvert.Length % 4)
                {
                    case 0:
                        break;
                    case 2:
                        partToConvert += "==";
                        break;
                    case 3:
                        partToConvert += "=";
                        break;
                    default:
                        throw new ArgumentException("Encountered unexpected node, i.e. `partToConvert.Length % 4`");
                }
                var partAsBytes = Convert.FromBase64String(partToConvert);
                var partAsUtf8String = Encoding.UTF8.GetString(partAsBytes, 0, partAsBytes.Length);

                var jwt = JObject.Parse(partAsUtf8String);
                Data =
                    jwt["data"]?.ToObject<Dictionary<string, object>>()
                    ?? new Dictionary<string, object>();
                Phone = jwt[ClaimTypes.MobilePhone]?.ToObject<string>() ?? "";
                Name = jwt[ClaimTypes.Name]?.ToObject<string>() ?? "";
                var rolesArray = jwt[ClaimTypes.Role];
                if (rolesArray != null)
                {
                    if (rolesArray is JArray)
                    {
                        Roles = rolesArray.Select(r => r.ToString()).ToList();
                    }
                    else if (rolesArray is JValue)
                    {
                        Roles = new List<string> { rolesArray.ToString() };
                    }
                }
                else
                {
                    Roles = new List<string>();
                }
            }

            public Dictionary<string, object> Data { get; }
            public List<string> Roles { get; }
            public string Token { get; }
            public string Name { get; set; }
            public string Phone { get; set; }
        }