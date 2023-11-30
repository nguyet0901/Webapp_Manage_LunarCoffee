using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;


namespace MLunarCoffee.Comon
{  
    public static class DigestAuthFixer
    { 
        private static string _realm;
        private static string _nonce;
        private static string _qop;
        private static string _cnonce;
        private static DateTime _cnonceDate;
        private static int _nc;

        private static string CalculateMd5Hash(
            string input)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = MD5.Create().ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        private static string GrabHeaderVar(
            string varName,
            string header)
        {
            var regHeader = new Regex(string.Format(@"{0}=""([^""]*)""", varName));
            var matchHeader = regHeader.Match(header);
            if (matchHeader.Success)
                return matchHeader.Groups[1].Value;
            throw new ApplicationException(string.Format("Header {0} not found", varName));
        }


        private static string GetDigestHeader(string dir, string user, string password)
        {
            _nc = _nc + 1;

            var ha1 = CalculateMd5Hash(string.Format("{0}:{1}:{2}", user, _realm, password));
            var ha2 = CalculateMd5Hash(string.Format("{0}:{1}", "GET", dir));
            var digestResponse =
                CalculateMd5Hash(string.Format("{0}:{1}:{2:00000000}:{3}:{4}:{5}", ha1, _nonce, _nc, _cnonce, _qop, ha2));

            return string.Format("Digest username=\"{0}\", realm=\"{1}\", nonce=\"{2}\", uri=\"{3}\", " +
                "algorithm=MD5, response=\"{4}\", qop={5}, nc={6:00000000}, cnonce=\"{7}\"",
                user, _realm, _nonce, dir, digestResponse, _qop, _nc, _cnonce);
        }

        public static string GrabResponse(string dir, string host, string user, string password)
        {
            var url = host + dir;
            var uri = new Uri(url);

            var request = (HttpWebRequest)WebRequest.Create(uri);

            // If we've got a recent Auth header, re-use it!
            if (!string.IsNullOrEmpty(_cnonce) &&
                DateTime.Now.Subtract(_cnonceDate).TotalHours < 1.0)
            {
                request.Headers.Add("Authorization", GetDigestHeader(dir,user,password));
            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                // Try to fix a 401 exception by adding a Authorization header
                if (ex.Response == null || ((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.Unauthorized)
                    throw;

                var wwwAuthenticateHeader = ex.Response.Headers["WWW-Authenticate"];
                _realm = GrabHeaderVar("realm", wwwAuthenticateHeader);
                _nonce = GrabHeaderVar("nonce", wwwAuthenticateHeader);
                _qop = GrabHeaderVar("qop", wwwAuthenticateHeader);

                _nc = 0;
                _cnonce = new Random().Next(123400, 9999999).ToString();
                _cnonceDate = DateTime.Now;

                var request2 = (HttpWebRequest)WebRequest.Create(uri);
                request2.Headers.Add("Authorization", GetDigestHeader(dir, user, password));
                response = (HttpWebResponse)request2.GetResponse();
            }
            var reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd();
        }
    }
}
