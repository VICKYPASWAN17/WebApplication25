using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication25.Models;
using System.Linq.Expressions;
using System.Data;
using System.ComponentModel;
using System.Web.Security;
namespace WebApplication25.Controllers
{
    [Authorize] 
    public class HomeController : Controller
    {
        SqlConnection connection = new SqlConnection("Data Source=LAPTOP-A4LCE48T;Initial Catalog=app24;Integrated Security=True;Encrypt=False");
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
     
        public ActionResult Index(Class1 s)
        {
            SqlCommand a=new SqlCommand("data1",connection);
            a.CommandType = System.Data.CommandType.StoredProcedure;
            a.Parameters.AddWithValue("@action",1);
            a.Parameters.AddWithValue("@name",s.name);
            a.Parameters.AddWithValue("@email",s.email);
            a.Parameters.AddWithValue("@password",s.password);
            a.Parameters.AddWithValue("@number",s.number);
            a.Parameters.AddWithValue("@DOB",s.date);
            a.Parameters.AddWithValue("@gender",s.gen);
            a.Parameters.AddWithValue("@address",s.address);
            a.Parameters.AddWithValue("@city",s.city);
            a.Parameters.AddWithValue("@postalcode",s.postal);
            connection.Open();
            a.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Dashboard");
        }

        public ActionResult About(int? id)
        {
            SqlCommand n = new SqlCommand("data1", connection);
            n.CommandType= System.Data.CommandType.StoredProcedure;
            n.Parameters.AddWithValue("@action",4);
           n.Parameters.AddWithValue("@sr",id);
            DataTable dt = new DataTable();
            SqlDataAdapter sd = new SqlDataAdapter(n);
            sd.Fill(dt);
            if(dt.Rows.Count>0)
            {
                return View(dt.Rows[0]);
            }
            else
            {
                return Content("<script>alert('try again');"+"location.href='/home/Index'</script>");
            }
            
        }
        [HttpPost]
        public ActionResult About(Class1 b)
        {
            SqlCommand n = new SqlCommand("data1", connection);
            n.CommandType = System.Data.CommandType.StoredProcedure;
            n.Parameters.AddWithValue("@action", 5);
            n.Parameters.AddWithValue("@sr", b.sr);
            n.Parameters.AddWithValue("@name", b.name);
            n.Parameters.AddWithValue("@email", b.email);
            n.Parameters.AddWithValue("@password", b.password);
            n.Parameters.AddWithValue("@number", b.number);
            n.Parameters.AddWithValue("@DOB", b.date);
            n.Parameters.AddWithValue("@gender", b.gen);
            n.Parameters.AddWithValue("@address", b.address);
            n.Parameters.AddWithValue("@city", b.city);
            n.Parameters.AddWithValue("@postalcode", b.postal);
            connection.Open();
            n.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Dashboard");

           
        }
        public ActionResult Dashboard(int? id)
        {
            SqlCommand d = new SqlCommand("data1", connection);
            d.CommandType = System.Data.CommandType.StoredProcedure;
            if (id.HasValue)
            {
                SqlCommand del = new SqlCommand("data1", connection);
                del.CommandType = System.Data.CommandType.StoredProcedure;
                del.Parameters.AddWithValue("@action", 3);
                del.Parameters.AddWithValue("@sr", id);
                connection.Open();
                int res = del.ExecuteNonQuery();
                connection.Close();
                if (res > 0)
                {

                    return Content("<script>alert('data  deleted');" + "location.href='/home/Dashboard'</script>");
                }
                else
                {
                    return Content("<script>alert('data not deleted');</script>");
                }

            }
            d.Parameters.AddWithValue("@action", 2);
            DataTable dt = new DataTable();
            SqlDataAdapter sd = new SqlDataAdapter(d);
            sd.Fill(dt);
            return View(dt);
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost,AllowAnonymous]
        public ActionResult Contact(string email, string password)
        {
            SqlCommand nn = new SqlCommand("data1", connection);
            nn.CommandType = System.Data.CommandType.StoredProcedure;
            nn.Parameters.AddWithValue("@email", email);
            nn.Parameters.AddWithValue("@password", password);
            nn.Parameters.AddWithValue("@action", 6);
            DataTable dt = new DataTable();
            SqlDataAdapter sdd = new SqlDataAdapter(nn);
            sdd.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                FormsAuthentication.SetAuthCookie(email, false);
                Session["name"] = dt.Rows[0]["name"];
                return Content("<script>alert('loggined');location.href='/Home/Dashboard';</script>");
            }
            else
            {
                return Content("<script>alert('Incorrect email id & password');location.href='/Home/Contact';</script>");
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Remove("name");
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}