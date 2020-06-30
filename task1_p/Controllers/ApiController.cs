using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using NPoco;
using NPoco.fastJSON;
using Microsoft.AspNetCore.Http;

namespace task1_p.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ApiController : Controller
    {
        [HttpGet]
        public async Task<JsonResult> Get(string Number)
        {
            DBConn dbConn = new DBConn();


            //List<User> Users = null;
            User user = null;
            using (IDatabase db = dbConn.Connect())
            {
                user = (await db.FetchAsync<User>()).Find(x => x.PhoneNumber.Equals(Number));
                if (null != user)
                {
                    user.Appointments = (await db.FetchAsync<Appointment>()).FindAll(x => x.UserId == user.Id);
                }
                db.Dispose();
            }

            dbConn.Dispose(); dbConn = null;
            return Json(user);
        }

        // POST api
        [HttpPost]
        public async Task<ActionResult> Post([FromForm]string json)
        {
            DBConn dbConn = new DBConn();

            JsonAppointment info;
            try
            {
                info = JsonSerializer.Deserialize<JsonAppointment>(json);
                using (IDatabase db = dbConn.Connect())
                {
                    if (-1 == (await db.FetchAsync<User>()).FindIndex(x => x.PhoneNumber.Equals(info.PhoneNumber)))
                    {
                        User user = new User()
                        {
                            Firstname = info.Firstname,
                            Lastname = info.Lastname,
                            Patronymic = info.Patronymic,
                            PhoneNumber = info.PhoneNumber
                        };
                        await db.InsertAsync<User>(user);
                    }
                    Appointment appointment = new Appointment()
                    {
                        Date = DateTime.Now,
                        Comment = info.Comment,
                        UserId = db.Fetch<User>().Find(x => x.PhoneNumber == info.PhoneNumber).Id
                    };
                    await db.InsertAsync<Appointment>(appointment);
                    db.Dispose();
                }
            } catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, Json(new ErrorContainer(ex.Message)));
            }
            dbConn.Dispose(); dbConn = null;
            return Redirect("~/Api?Number=" + info.PhoneNumber);
        }
    }
}
