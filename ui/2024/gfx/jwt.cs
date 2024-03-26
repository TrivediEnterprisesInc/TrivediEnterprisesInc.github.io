using System;
using System.Text;
using System.IdentityModel;
using System.Security;
using System.IdentityModel.Tokens.Jwt;

//nds nuget pkg System.IdentityModel.Tokens.Jwt
/*  output:

tokenString:
eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzb21lICI6ImhlbGxvICIsInNjb3BlIjoiaHR0cDovL2R1bW15LmNvbS8ifQ.FPkHESpldjwEsdE_ii8936gFq4pfptl3b6ao13BTLZk
Consume Token:
{"some ":"hello ","scope":"http://dummy.com/"}

*/
public class Program
{
static void Main(string[] args)
       {
           Console.WriteLine("");

           // Define const Key this should be private secret key  stored in some safe place
           string key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

           // note that the dll uses Microsoft namespace instead of System
           var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

           // Also note that securityKey length should be >256b
           // so you have to make sure that your private key has a proper length
           //
           var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

           //  Finally create a Token
           var header = new JwtHeader(credentials);

           //Some PayLoad that contain information about the  customer
           var payload = new JwtPayload
           {
               { "some ", "hello "},
               { "scope", "http://dummy.com/"},
           };

           var secToken = new JwtSecurityToken(header, payload);
           var handler = new JwtSecurityTokenHandler();
           var tokenString = handler.WriteToken(secToken);

		   Console.WriteLine("tokenString:");
           Console.WriteLine(tokenString);

           // And finally when  you received token from client
           // you can  either validate it or try to  read
           var token = handler.ReadJwtToken(tokenString);
           Console.WriteLine("Consume Token:");
		   Console.WriteLine(token.Payload.SerializeToJson());
           Console.ReadLine();
       }
}
/*
To send cookies and set hdr manually, see:
https://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage
var baseAddress = new Uri("http://example.com");
using (var handler = new HttpClientHandler { UseCookies = false })
using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
{
    var message = new HttpRequestMessage(HttpMethod.Get, "/test");
    message.Headers.Add("Cookie", "cookie1=value1; cookie2=value2");
    var result = await client.SendAsync(message);
    result.EnsureSuccessStatusCode();
}
*/
