using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Function;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

using Microsoft.Data.SqlClient;
//using EFSample.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        public readonly APIDBContext _context;

        public HomeController(APIDBContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}


        [HttpGet]
        [Route("UserCheck/{username}/{fullname}/{company}/{email}")]
        public List<VwPeriode> Get(string username, string fullname, string company, string email)
        {
            
            string sql = "EXEC [SP_CekUser] @UserName,@FullName,@Company, @Email";

       
            var parameters = new[] {
            new SqlParameter("@UserName", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = username },
            new SqlParameter("@FullName", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = fullname },
            new SqlParameter("@Company", SqlDbType.VarChar) { Direction = ParameterDirection.InputOutput, Value = company },
            new SqlParameter("@Email", SqlDbType.VarChar) { Direction = ParameterDirection.InputOutput, Value = email }
        };

          var vwPeriode = _context.VwPeriode.FromSqlRaw(sql, parameters);//_context.Database.ExecuteSqlRaw(sql, parms.ToArray());

            return vwPeriode.ToList();
            //return List<VwPeriode>;
        }

        [HttpPost]
        [Route("UserCheckV2")]
        public List<TblTUser> Post(TblTUser DataUser)
        {

            string sql = "EXEC [SP_CekUser_V2] @UserName,@FullName,@Company, @Email, @Regional, @Zona, @Field";


            var parameters = new[] {
            new SqlParameter("@UserName", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = DataUser.UserName },
            new SqlParameter("@FullName", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = DataUser.Name },
            new SqlParameter("@Company", SqlDbType.VarChar) { Direction = ParameterDirection.InputOutput, Value = DataUser.Company },
            new SqlParameter("@Email", SqlDbType.VarChar) { Direction = ParameterDirection.InputOutput, Value = DataUser.Email },
            new SqlParameter("@Regional", SqlDbType.VarChar) { Direction = ParameterDirection.InputOutput, Value = DataUser.Regional },
            new SqlParameter("@Zona", SqlDbType.VarChar) { Direction = ParameterDirection.InputOutput, Value = DataUser.Zona },
            new SqlParameter("@Field", SqlDbType.VarChar) { Direction = ParameterDirection.InputOutput, Value = DataUser.Field }
        };

            var Result = _context.TblTUser.FromSqlRaw(sql, parameters);//_context.Database.ExecuteSqlRaw(sql, parms.ToArray());

            return Result.ToList();
            //return List<VwPeriode>;
        }
    }
}
