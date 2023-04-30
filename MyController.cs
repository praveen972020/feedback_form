using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Remoting.Messaging;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;
using System.Drawing;
using Org.BouncyCastle.Crypto.Tls;
using System.Web.Services.Description;
using System.Windows.Forms;

namespace StudentPortal.Controllers
{
    public class MyController : Controller
    {
        // GET: My
        public ActionResult student_reg_form()
        {
            return View();
        }
        //[HttpPost]
        public ActionResult Upload(string myCroppedImage)
        {
            byte[] bytes = Convert.FromBase64String(myCroppedImage);


            /*  if (file != null && file.ContentLength > 0)
              {
                  string path = Server.MapPath("~/images");
                  string filename = Path.GetFileName(file.FileName);
                  string fullpath = Path.Combine(path, filename);
                  file.SaveAs(fullpath);
                  return Content("success");
              }*/
            return RedirectToAction("student_reg_form");
        }

        //       [HttpPost]
        //public ActionResult insertRecord(string myCroppedImage, string name,DateTime dob,string fathername,string mothername,string course,string email,string contactno,string address)
        //{




        //    byte[] imageBytes = Convert.FromBase64String(myCroppedImage.Split(',')[1]);
        //    string path = Server.MapPath("~/images");
        //    MemoryStream ms = new MemoryStream(imageBytes);
        //    Image image = Image.FromStream(ms);
        //    string fileName = Path.GetFileName(Request.Url.AbsolutePath);
        //    string extension = Path.GetExtension(fileName);
        //    string fname = Guid.NewGuid().ToString() + (".jpeg");

        //    //string fname = fileName + extension;


        //    //byte[] bytes = Convert.FromBase64String(myCroppedImage);
        //    var c = ExecuteMe.Select("select count(*) from student_reg_record where name='"+name+"' and father_name='"+fathername+"'" , "Demo");

        //    if(c != null)
        //    {
        //        return Content("sorry you are already register");
        //    }
        //    string fullpath = Path.Combine(path, fname);
        //    image.Save(fullpath);

        //    var date1 = dob.ToString("dd/MM/yyyy");
        //    ExecuteMe.DeleteInsertUpdate("insert into student_reg_record (name,email,contact,father_name,mother_name,address,course,dob,filename) values('" + name + "','" + email + "','" + contactno + "','" + fathername + "','" + mothername + "','" + address + "','" + course + "','" + date1 + "','"+ fname + "')", "Demo");
        //            Content("insert record successfully");



        //    return RedirectToAction("student_reg_form");


        //}

        static List<Demo> list1 = new List<Demo>();

        public ActionResult FetchRecord()
        {
            list1.Clear();  
          // public List<Demo> list1 = new List<Demo>();
            string myquery = "SELECT id,name,father_name,mother_name,email,address,course,contact,dob,filename FROM `student_reg_record` where Status='Active'";
            DataTable dt = ExecuteMe.Select(myquery, "Demo");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list1.Add(new Demo
                {
                    id = dt.Rows[i]["id"].ToString(),
                    name = dt.Rows[i]["name"].ToString(),
                    fathername = dt.Rows[i]["father_name"].ToString(),
                    mothername = dt.Rows[i]["mother_name"].ToString(),
                    address = dt.Rows[i]["address"].ToString(),
                    course = dt.Rows[i]["course"].ToString(),
                    contactno = dt.Rows[i]["contact"].ToString(),
                    email = dt.Rows[i]["email"].ToString(),
                    fileName = dt.Rows[i]["filename"].ToString(),
                    //dob = dt.Rows[i]["dob"].GetType().t
                    
                    

                });
            }
            return Json(new { list1 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult insertRecord(string myCroppedImage, string name, DateTime dob, string fathername, string mothername, string course, string email, string contactno, string address)
        {

            for(int i=0;i<list1.Count;i++)
            {
                if (list1[i].name==name && list1[i].fathername==fathername && list1[i].mothername==mothername && list1[i].course==course && list1[i].contactno==contactno && list1[i].email==email && list1[i].address==address)
                {
                    //  return Content("<script language='javascript' type=text/javascript>alert('sorry you are already registered');</script>"); ;
                    // return Content("sorry you are alresdy registred");
                    MessageBox.Show("sorry you are already registered");


                    return RedirectToAction("student_reg_form");

                }
                


            }
           
                byte[] imageBytes = Convert.FromBase64String(myCroppedImage.Split(',')[1]);
                string path = Server.MapPath("~/images");
                MemoryStream ms = new MemoryStream(imageBytes);
                Image image = Image.FromStream(ms);
                string fileName = Path.GetFileName(Request.Url.AbsolutePath);
                string extension = Path.GetExtension(fileName);
                string fname = Guid.NewGuid().ToString() + (".jpeg");

                //string fname = fileName + extension;


                //byte[] bytes = Convert.FromBase64String(myCroppedImage);

                string fullpath = Path.Combine(path, fname);
                image.Save(fullpath);

                var date1 = dob.ToString("dd/MM/yyyy");
                ExecuteMe.DeleteInsertUpdate("insert into student_reg_record (name,email,contact,father_name,mother_name,address,course,dob,filename) values('" + name + "','" + email + "','" + contactno + "','" + fathername + "','" + mothername + "','" + address + "','" + course + "','" + date1 + "','" + fname + "')", "Demo");
                //return Content("<script language='javascript' type=text/javascript>alert('record insert successfully');</script>");


                MessageBox.Show("insert record successfully");
            

            return RedirectToAction("student_reg_form");


        }



        public ActionResult deleteRecord(Demo user)
        {
            string myquery = "update `student_reg_record` set Status='In-Active' where id='"+user.id+"'";
            DataTable dt = ExecuteMe.Select(myquery, "Demo");
            return Content("delete record successfully");

        }

        public ActionResult updateRecord(Demo user)
        {
          //  string myquery = "update `student_reg_record` set name='" + user.name+ "' ,father_name='" + user.fathername+ "',mother_name='" + user.mothername+ "',email='" + user.email+ "',address='" + user.address+ "',course='" + user.course+ "',contact='" + user.contactno+ "' where id='" + user.id + "'";
            ExecuteMe.DeleteInsertUpdate("update `student_reg_record` set name='" + user.name+ "' ,father_name='" + user.fathername+ "',mother_name='" + user.mothername+ "',email='" + user.email+ "',address='" + user.address+ "',course='" + user.course+ "',contact='" + user.contact+ "' where id='" + user.id + "'", "Demo");
           return Content("record updated successfully");
           // return RedirectToAction("reg_form", "Home");
            // DataTable dt = ExecuteMe.Select(myquery, "Demo");
            // return Content("update record successfully");

        }

        public ActionResult fetchidrecordCtrl(Demo user)
        {
            List<Demo> list2 = new List<Demo>();
            string myquery = "SELECT id,name,father_name,mother_name,email,address,course,contact FROM `student_reg_record` where id='"+user.id+"'";
            DataTable dt = ExecuteMe.Select(myquery, "Demo");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list2.Add(new Demo
                {
                    id = dt.Rows[i]["id"].ToString(),
                    name = dt.Rows[i]["name"].ToString(),
                    fathername = dt.Rows[i]["father_name"].ToString(),
                    mothername = dt.Rows[i]["mother_name"].ToString(),
                    address = dt.Rows[i]["address"].ToString(),
                    course = dt.Rows[i]["course"].ToString(),
                    contactno = dt.Rows[i]["contact"].ToString(),
                    email = dt.Rows[i]["email"].ToString(),

                });
            }
            return Json(new { list2=list2 }, JsonRequestBehavior.AllowGet);
        }

    }
   
}
public class Demo
{
    public string imageData { get; set; }

    public DateTime dob { get; set; }
    public string StuName { get; set; }
    public string fileName { get; set; }


    [Required(ErrorMessage = "Please enter your name.")]
    public string name { get; set; }
    public string img { get; set; }
    public string email { get; set; }
    public string contact { get; set; }
    public string id { get; set; }
    public string contactno { get; set; }
    public string course { get; set; }
    public string fathername { get; set; }
    public string mothername { get; set; }
    public string address { get; set; }
}
