using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_sorted_name.Models;
using System.IO;
using System.Web.Helpers;

namespace Web_sorted_name.Controllers
{
    public class UploadsController : Controller
    {
        

        public class NamaLengkap
        {
            public string _GivenName { get; set; }

            public string _LastName { get; set; }
            public NamaLengkap(string GivenName, string LastName)
            {
                _GivenName = GivenName;
                _LastName = LastName;
            }

        }

        public ActionResult Index()
        {
            return View("Upload");
        }


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            List<NamaLengkap> FullName = new List<NamaLengkap>();
            List<NamaLengkap> FullNameSorted = new List<NamaLengkap>();

            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    file.SaveAs(path);
                    string[] lines = System.IO.File.ReadAllLines(path);
                    string[] splitnames = new string[8];
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string givenName = "";
                        string lastName = "";
                        splitnames = lines[i].Split(' ');
                        for (int j = splitnames.Length - 1; j == splitnames.Length - 1; j--)
                        {
                            lastName = splitnames[j];
                        }
                        for (int x = 0; x < splitnames.Length - 1; x++)
                        {
                            givenName = givenName + " " + splitnames[x];
                        }
                        FullName.Add(new NamaLengkap(givenName, lastName));
                    }



                    FullNameSorted = (from s in FullName
                                      orderby s._LastName ascending, s._GivenName
                                      select s).ToList();
                    string[] splitfilename = new string[3];
                    splitfilename = fileName.Split('.');
                    fileName = Path.GetFileName(file.FileName);
                    var pathresult = Path.Combine(Server.MapPath("~/App_Data/result"), splitfilename[0] + "_sorted.txt");

                    using (StreamWriter outputFile = new StreamWriter(pathresult))
                    {
                        foreach (var line in FullNameSorted)
                            outputFile.WriteLine(line._GivenName + " " + line._LastName);
                    }
                }
                ViewBag.test = FullNameSorted;
            }
            catch (Exception)
            {

                //throw;
                return View("Upload");
            }
            

            return View("Upload");
        }

    }
}
