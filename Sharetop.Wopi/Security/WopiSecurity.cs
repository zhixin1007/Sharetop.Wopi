using Sharetop.Wopi.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;

using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Sharetop.Wopi.Models;
using System.Security.Cryptography;
using Sharetop.Wopi.Models.Api;

namespace Sharetop.Wopi.Security
{
    public class WopiSecurity
    {
        public static string MD5Encrypt(string value)
        {
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
            StringBuilder builder = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                builder.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return builder.ToString();
        }

        public static string Encrypt(string value, string key = "")
        {
            if (key == "") key = ServerUtil.Config().SecureKey;

            byte[] buffer = Encoding.UTF8.GetBytes(value);
            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(ms, tdes.CreateEncryptor(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(ConfigBag.initKey())), CryptoStreamMode.Write);
            encStream.Write(buffer, 0, buffer.Length);
            encStream.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray()).Replace("+", "%");
        }

        public static string Decrypt(string SecureString, string key = "")
        {
            if (key == "") key = ServerUtil.Config().SecureKey;

            SecureString = SecureString.Replace("%", "+");
            byte[] buffer = Convert.FromBase64String(SecureString);
            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(ms, tdes.CreateDecryptor(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(ConfigBag.initKey())), CryptoStreamMode.Write);
            encStream.Write(buffer, 0, buffer.Length);
            encStream.FlushFinalBlock();

            return Encoding.UTF8.GetString(ms.ToArray());
        }
        /// <summary>
        /// Generates an access token specific to a user and file
        /// </summary>
        public static bool ValidateToken(Token token, string docId)
        {
            try
            {
                if (token.Document == docId && token.Expires>=DateTime.UtcNow)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ServerUtil.LogException(ex);
                return false;
            }

        }

        /// <summary>
        /// Extracts the user information from a provided access token
        /// </summary>
        public static string GetUserFromToken(string tokenString)
        {
            try
            {
                var token = Token.Get(tokenString);
                return token.User;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Private token handler used in instance classes
        /// </summary>
        //private JwtSecurityTokenHandler tokenHandler;

        /// <summary>
        /// Generates an access token for the user and file
        /// </summary>
        public Token GenerateToken( string docId,string user,string userDisplayName = null)
        {
            if (String.IsNullOrEmpty(userDisplayName))
            {
                userDisplayName = user;
            }

            var token = Token.Create();
            token.User = user;
            token.UserDisplayName = userDisplayName;
            token.Document = docId;
            token.Save();

            return token;
        }

        /// <summary>
        /// Converts the JwtSecurityToken to a Base64 string that can be used by the Host
        /// </summary>
        //public string WriteToken(Token token)
        //{
        //    //return tokenHandler.WriteToken(token);
        //    return token.Id;
        //}

        ///// <summary>
        ///// Gets the self-signed certificate used to sign the access tokens
        ///// </summary>
        //private static X509Certificate2 getCert()
        //{
        //    var certPath = Path.Combine(System.Web.ServerUtil.MapPath("~"), "OfficeWopi.pfx");
        //    var certfile = System.IO.File.OpenRead(certPath);
        //    var certificateBytes = new byte[certfile.Length];
        //    certfile.Read(certificateBytes, 0, (int)certfile.Length);
        //    var cert = new X509Certificate2(
        //        certificateBytes,
        //        ConfigurationManager.AppSettings["CertPassword"],
        //        X509KeyStorageFlags.Exportable |
        //        X509KeyStorageFlags.MachineKeySet |
        //        X509KeyStorageFlags.PersistKeySet);

        //    return cert;
        //}
    }
}