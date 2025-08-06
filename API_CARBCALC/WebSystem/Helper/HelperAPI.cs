using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebSystem.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Data;

namespace WebSystem.Helper
{
    public class HelperAPI
    {

        public  string userId { get; } = "_UserId";
        public  string userName { get; } = "_UserName";
        public  string displayname { get; } = "_DisplayName";
        public  string email { get; } = "_Email";
        public  string company { get; } = "_Company";
        public  string regional { get; } = "_Regional";
        public  string zona { get; } = "_Zona";
        public  string field { get; } = "Field";
        public string periodsession { get; } = "_Periode";
        public string periodeIdsession { get; } = "_PeriodeId";
        public string yearValuesession { get; } = "_YearValue";
        public string MonthValuesession { get; } = "_MonthValue";
        public string IsAdminsession { get; } = "_IsAdmin";

        public  string PeriodeStatic { set; get; } = "";
        public  Int32 UserID { set; get; } = 0;
        public  Int32 PeriodeID { set; get; } = 0;
        public  Int32 MonthValue { set; get; } = 0;
        public  Int32 YearValue { set; get; } = 0;
        public  string UserName { set; get; } = "";
        public  string Email { set; get; } = "";
        public  string Name { set; get; } = "";
        public string Company { set; get; } = "";
        public string IsAdmin { set; get; } = "";

        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51974/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public string SetPeriodeDetail(string PeriodeDetail)
        {
            this.PeriodeStatic = PeriodeDetail;
            string result = this.PeriodeStatic;

            return result;
        }

        //public TblTUser GetSession()
        //{
        //    TblTUser tblTUser_Session = new TblTUser();

        //    tblTUser_Session.userID = Convert.ToInt32(HttpContext.Session.GetString(userId));
        //    tblTUser_Session.UserName = HttpContext.Session.GetString(userName);
        //    tblTUser_Session.Name = HttpContext.Session.GetString(displayname);
        //    tblTUser_Session.Company = HttpContext.Session.GetString(company);

        //    return tblTUser_Session;

        //}

        public void GetValueSession(HttpContext SessionData)
        {
            if(SessionData != null)
            {
                UserID = Convert.ToInt32(SessionData.Session.GetString(userId));
                UserName = SessionData.Session.GetString(userName);
                Name = SessionData.Session.GetString(displayname);
                PeriodeStatic = SessionData.Session.GetString(periodsession);
                PeriodeID = Convert.ToInt32(SessionData.Session.GetString(periodeIdsession));
                YearValue = Convert.ToInt32(SessionData.Session.GetString(yearValuesession));
                MonthValue = Convert.ToInt32(SessionData.Session.GetString(MonthValuesession));               
                Company = SessionData.Session.GetString(company);
                IsAdmin = SessionData.Session.GetString(IsAdminsession); 

            }
        }
        public DataTable UBDCC()
        {
            DataTable DT = new DataTable();

            DataColumn col1 = new DataColumn("UserName", typeof(string));
            DataColumn col2 = new DataColumn("Pass", typeof(string));
            DT.Columns.Add(col1);
            DT.Columns.Add(col2);

            DataRow row1 = DT.NewRow();
            row1["UserName"] = "MK.RENDY.RAMADHAN1";
            row1["Pass"] = "pass_ubdcc2023";
            DT.Rows.Add(row1);

            DataRow row2 = DT.NewRow();
            row1["UserName"] = "mk.jody.ananda";
            row1["Pass"] = "pass_ubdcc2023";
            DT.Rows.Add(row2);
            

            DataRow row3 = DT.NewRow();
            row1["UserName"] = "mk.nur.wijaya";
            row1["Pass"] = "pass_ubdcc2023";
            DT.Rows.Add(row3);

            return DT;
        }

        public enum UBDCC_pass
        {
            pass_ubdcc2023
        }


        //public static MvcHtmlString EncodedActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, string iconclass)
        //{

        //    string queryString = string.Empty;
        //    string htmlAttributesString = string.Empty;
        //    if (routeValues != null)
        //    {
        //        RouteValueDictionary d = new RouteValueDictionary(routeValues);
        //        for (int i = 0; i < d.Keys.Count; i++)
        //        {
        //            if (i > 0)
        //            {
        //                queryString += "?";
        //            }
        //            queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
        //        }
        //    }

        //    if (htmlAttributes != null)
        //    {
        //        RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
        //        for (int i = 0; i < d.Keys.Count; i++)
        //        {
        //            htmlAttributesString += " " + d.Keys.ElementAt(i) + "=" + "'" + d.Values.ElementAt(i) + "'";
        //        }
        //    }


        //    StringBuilder ancor = new StringBuilder();
        //    ancor.Append("<a ");
        //    if (htmlAttributesString != string.Empty)
        //    {
        //        ancor.Append(htmlAttributesString);
        //    }
        //    ancor.Append(" href='");
        //    if (controllerName != string.Empty)
        //    {
        //        ancor.Append("/" + controllerName);
        //    }

        //    if (actionName != "Index")
        //    {
        //        ancor.Append("/" + actionName);
        //    }
        //    if (queryString != string.Empty)
        //    {
        //        ancor.Append("?q=" + EncryptURL(queryString));
        //    }
        //    ancor.Append("'");
        //    ancor.Append(">");
        //    if (!string.IsNullOrEmpty(iconclass))
        //        ancor.Append("<i class='" + iconclass + "'></i> ");
        //    ancor.Append(linkText);
        //    ancor.Append("</a>");
        //    return new MvcHtmlString(ancor.ToString());
        //}
        //public static string EncryptURL(string clearText)
        //{
        //    string EncryptionKey = "LUPHLAKMAKJ2SPBNI99212"; //Carbon Calculator phrase code
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return clearText;
        //}

        //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        //public class EncryptedActionParameterAttribute : ActionFilterAttribute
        //{
        //    public override void OnActionExecuting(ActionExecutingContext filterContext)
        //    {

        //        Dictionary<string, object> decryptedParameters = new Dictionary<string, object>();                
        //        if (HttpContext.Current.Request.QueryString.Get("q") != null)
        //        {
        //            string encryptedQueryString = HttpContext.Current.Request.QueryString.Get("q");
        //            string decrptedString = Decrypt(encryptedQueryString.ToString());
        //            string[] paramsArrs = decrptedString.Split('?');

        //            for (int i = 0; i < paramsArrs.Length; i++)
        //            {
        //                string[] paramArr = paramsArrs[i].Split('=');
        //                //if(paramArr[1].GetType().Name == "String")
        //                //    decryptedParameters.Add(paramArr[0], paramArr[1]);
        //                //else
        //                int val = 0;
        //                if (Int32.TryParse(paramArr[1], out val))
        //                    decryptedParameters.Add(paramArr[0], Convert.ToInt32(paramArr[1]));
        //                else
        //                    decryptedParameters.Add(paramArr[0], paramArr[1]);
        //            }
        //        }
        //        for (int i = 0; i < decryptedParameters.Count; i++)
        //        {
        //            filterContext.ActionParameters[decryptedParameters.Keys.ElementAt(i)] = decryptedParameters.Values.ElementAt(i);
        //        }
        //        base.OnActionExecuting(filterContext);

        //    }

        //    private string Decrypt(string cipherText)
        //    {
        //        string EncryptionKey = "JUDHAAUMAKV2SPBNI99212"; //Carbon Calculator phrase code
        //        cipherText = cipherText.Replace(" ", "+");
        //        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //        using (Aes encryptor = Aes.Create())
        //        {
        //            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //            encryptor.Key = pdb.GetBytes(32);
        //            encryptor.IV = pdb.GetBytes(16);
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //                {
        //                    cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                    cs.Close();
        //                }
        //                cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //            }
        //        }
        //        return cipherText;
        //    }

        //}




    }
}
