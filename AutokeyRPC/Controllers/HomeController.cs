using AutokeyRPC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.Entity;
using System.Net;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using MoreLinq;

namespace AutokeyRPC.Controllers
{
    //[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class HomeController : Controller
    {
        private AutokeyEntities db = new AutokeyEntities();

        private bool local = false;


        //public ActionResult Index(string SearchString, string SearchLocation, string SearchLotto, string usr)
        //{
        //    if (usr != null)
        //        Session["User"] = usr;
        //    if (usr == null)
        //        usr = Session["User"].ToString();

        //    Session["Scelta1"] = "";

        //    bool isAuth = false;

        //    if (usr != String.Empty)
        //    {
        //        string UserName = "";

        //        string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
        //        HttpCookie cookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
        //        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value); //Decrypt it
        //        UserName = ticket.Name; //You have the UserName!


        //        if (usr == UserName)
        //        {
        //            ViewBag.Messaggio = "BENE il cookie corrisponde!";
        //            //ViewBag.Messaggio = personaggio;
        //            isAuth = true;
        //        }
        //        else
        //        {
        //            ViewBag.Messaggio = "il cookie contenente lo 'username' non corrisponde allo User della queryString!";
        //            isAuth = false;
        //            return View("IncorrectLogin");
        //        }

        //    }



        //    if (isAuth)
        //    {

        //        // Carico DropDownList
        //        using (AutokeyEntities val = new AutokeyEntities())
        //        {
        //            Session["Scelta1"] = "";
        //            var fromDatabaseEF = new SelectList(val.RPC_Cantieri_vw.ToList(), "IDCantiere", "ragioneSociale");
        //            ViewData["RPC_Cantieri_vw"] = fromDatabaseEF;

        //            var fromDatabaseEF1 = new SelectList(val.RPC_Lotti.ToList(), "ID", "Descrizione");
        //            ViewData["RPC_Lotti"] = fromDatabaseEF1;

        //        }

        //        TempData["mySearch"] = SearchLocation;
        //        TempData["myIDOperatore"] = SearchString;
        //        TempData["myLotto"] = SearchLotto;

        //        // Verifico esista il perito...
        //        using (AutokeyEntities db = new AutokeyEntities())
        //        {
        //            if (String.IsNullOrEmpty(SearchString))
        //            {
        //                return View();
        //            }
        //            else
        //            {
        //                if (SearchString.Length == 4)
        //                {
        //                    try
        //                    {
        //                        var periti = (from s in db.AUK_tecnici
        //                                      where s.Codice == SearchString
        //                                      select s.ID).First();
        //                        if (periti.ToString() != null)
        //                        {
        //                            var model = new Models.HomeModel();

        //                            //var telai = from s in db.RPC_Telai
        //                            //            where s.IDCantiere.ToString() == SearchLocation
        //                            //            && s.IDOperatore.ToString() == SearchString
        //                            //            select s;
        //                            var telai = from s in db.RPC_telai_vw
        //                                        where s.IDCantiere.ToString() == SearchLocation //&& s.IDLotto.ToString() == SearchLotto
        //                                        orderby s.Telaio
        //                                        select s;
        //                            model.RPC_Telai_vw = telai.ToList();
        //                            return View("VinList", model);
        //                            return RedirectToAction("DoRefresh", "Home");
        //                        }
        //                        else
        //                        {
        //                            return View();
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        ViewBag.Messaggio = "Codice non riconosciuto.";
        //                        return View("IncorrectLogin");
        //                    }

        //                }
        //                else
        //                {
        //                    ViewBag.Messaggio = "Lunghezza codice errata.";
        //                    return View("IncorrectLogin");
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return Redirect("http://192.168.20.1/Utente/Login");
        //        return Redirect("https://webservices.interconsult.it/applogin/");
        //    }
        //}

        public ActionResult Index(string SearchString, string SearchLocation, string SearchLotto, string usr)
        {



            // Carico DropDownList
            using (AutokeyEntities val = new AutokeyEntities())
            {
                Session["Scelta1"] = "";
                Session["SearchTelaio"] = "";
                var fromDatabaseEF = new SelectList(val.RPC_Cantieri_vw.ToList(), "IDCantiere", "ragioneSociale");
                ViewData["RPC_Cantieri_vw"] = fromDatabaseEF;

                var fromDatabaseEF1 = new SelectList(val.RPC_Lotti.ToList(), "ID", "Descrizione");
                ViewData["RPC_Lotti"] = fromDatabaseEF1;

            }

            TempData["mySearch"] = SearchLocation;
            TempData["myIDOperatore"] = SearchString;
            TempData["myLotto"] = SearchLotto;

            // Verifico esista il perito...
            using (AutokeyEntities db = new AutokeyEntities())
            {
                if (String.IsNullOrEmpty(SearchString))
                {
                    return View();
                }
                else
                {
                    if (SearchString.Length == 4)
                    {
                        try
                        {
                            var periti = (from s in db.AUK_tecnici
                                          where s.Codice == SearchString
                                          select s.ID).First();
                            if (periti.ToString() != null)
                            {
                                var model = new Models.HomeModel();

                                //var telai = from s in db.RPC_Telai
                                //            where s.IDCantiere.ToString() == SearchLocation
                                //            && s.IDOperatore.ToString() == SearchString
                                //            select s;
                                var telai = from s in db.RPC_telai_vw
                                            where s.IDCantiere.ToString() == SearchLocation //&& s.IDLotto.ToString() == SearchLotto
                                            orderby s.LastUpdateTime descending, s.Telaio
                                            select s;
                                model.RPC_Telai_vw = telai.ToList();
                                return View("VinList", model);
                                //return RedirectToAction("DoRefresh", "Home");
                            }
                            else
                            {
                                return View();
                            }
                        }
                        catch (Exception exc)
                        {
                            ViewBag.Messaggio = "Codice non riconosciuto.";
                            return View("IncorrectLogin");
                        }

                    }
                    else
                    {
                        ViewBag.Messaggio = "Lunghezza codice errata.";
                        return View("IncorrectLogin");
                    }
                }
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Home";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contatti.";

            return View();
        }

        //public ActionResult Delete()
        //{
        //    ViewBag.Message = "Delete.";

        //    return View();
        //}

       

        public ActionResult Edit(int ID)
        {
            RPC_Telai telai = db.RPC_Telai.Find(ID);
            ViewBag.IDTelaio = ID;

            return View(telai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = " ID, IDCantiere, IDOperatore, IDLotto, IsFinished, InsertDate, Telaio, Descr,LastUpdateTime,Note")] RPC_Telai rPC_Telai,string SearchResult)
        {
            if (ModelState.IsValid)
            {
                string myIDOperatore = TempData["myIDOperatore"] as string;
                if (rPC_Telai.IsFinished == true)
                    rPC_Telai.IDOperatore = myIDOperatore.ToUpper();
                else
                    rPC_Telai.IDOperatore = null;

                rPC_Telai.LastUpdateTime = DateTime.Now;

                db.Entry(rPC_Telai).State = EntityState.Modified;
                db.SaveChanges();

                var model = new Models.HomeModel();

                
                string mySearch = TempData["mySearch"] as string;
                string myLotto = TempData["myLotto"] as string;

                var telai = from s in db.RPC_Telai
                            where s.IDCantiere.ToString() == mySearch //&& s.IDLotto.ToString() == myLotto
                            select s;
               
                model.RPC_Telai = telai.ToList();

                TempData["mySearch"] = mySearch;
                TempData["myLotto"] = myLotto;


                //return View("VinList", model);
                return RedirectToAction("DoRefresh", "Home");


            }
            else
            {
                return View(rPC_Telai);
            }
        }

        public ActionResult DoRefresh(string Scelta1, string SearchTelaio)
        {
            var model = new Models.HomeModel();

            bool CercaSuTuttiLotti = true;

            if (!String.IsNullOrEmpty(Scelta1))
                Session["Scelta1"] = Scelta1;
            else
                Scelta1 = Session["Scelta1"].ToString();

            if (SearchTelaio != null)
                Session["SearchTelaio"] = SearchTelaio;
            else
                SearchTelaio = Session["SearchTelaio"].ToString();



            string mySearch = TempData["mySearch"] as string;
            string myLotto = TempData["myLotto"] as string;
            

            //string strName = Request["Scelta1"].ToString();

            if (SearchTelaio != null && SearchTelaio != "")
            {

                if (!CercaSuTuttiLotti)
                {
                    var telai = from s in db.RPC_telai_vw
                                where s.IDCantiere.ToString() == mySearch //&& s.IDLotto.ToString() == myLotto
                                && s.Telaio.Contains(SearchTelaio)
                                orderby s.LastUpdateTime descending, s.Telaio
                                select s;
                    model.RPC_Telai_vw = telai.ToList();
                }
                else
                {
                    var telai = from s in db.RPC_telai_vw
                                where s.Telaio.Contains(SearchTelaio)
                                orderby s.LastUpdateTime descending, s.Telaio
                                select s;
                    model.RPC_Telai_vw = telai.ToList();
                }
               //model.RPC_Telai = telai.ToList();

            }

            else if (Scelta1 == "TUTTE")
            {
                var telai = from s in db.RPC_telai_vw
                            where s.IDCantiere.ToString() == mySearch //&& s.IDLotto.ToString() == myLotto
                            orderby s.LastUpdateTime descending, s.Telaio
                            select s;
                model.RPC_Telai_vw = telai.ToList();
            }
            else if (Scelta1 == "CHIUSE")
            {
                var telai = from s in db.RPC_telai_vw
                            where s.IDCantiere.ToString() == mySearch //&& s.IDLotto.ToString() == myLotto
                            && s.IsFinished == true
                            orderby s.LastUpdateTime descending, s.Telaio
                            select s;
                model.RPC_Telai_vw = telai.ToList();
            }
            else if (Scelta1 == "APERTE")
            {
                var telai = from s in db.RPC_telai_vw
                            where s.IDCantiere.ToString() == mySearch //&& s.IDLotto.ToString() == myLotto
                            && s.IsFinished == false
                            orderby s.LastUpdateTime descending, s.Telaio
                            select s;
                model.RPC_Telai_vw = telai.ToList();
            }
            else 
            {
                var telai = from s in db.RPC_telai_vw
                            where s.IDCantiere.ToString() == mySearch // && s.IDLotto.ToString() == myLotto
                            orderby s.LastUpdateTime descending, s.Telaio
                            select s;
                model.RPC_Telai_vw = telai.ToList();
            }


            TempData["mySearch"] = mySearch;
            TempData["myLotto"] = myLotto;
            


            return View("VinList", model);
           
        }

        [NonAction]
        public SelectList ToSelectList(DataTable table, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }

        public ActionResult Create()
        {
            ViewBag.IDCantiere = new SelectList(db.RPC_Cantieri_vw, "IDCantiere", "ragioneSociale");
            ViewBag.IDLotto = new SelectList(db.RPC_Lotti, "ID", "Descrizione");
            ViewBag.ID = new SelectList(db.RPC_Telai, "ID", "IDOperatore");
            
            ViewBag.IDOperatore = new SelectList(db.PKT_Operatori, "ID", "ID");

            ViewBag.Testing = new SelectList(GetLotti(), "Text", "Value");

            return View();
        }


        public IEnumerable<SelectListItem> GetLotti()
        {
            IEnumerable<SelectListItem> list = null;

           
            var query = (from ca in db.RPC_Lotti
                         where 0 == 0
                         select new SelectListItem { Text = ca.ID.ToString(), Value = ca.Descrizione }).Distinct();
            list = query.ToList();


            return list;
        }



        // POST: Telai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDCantiere,IDOperatore,IDLotto,IsFinished,InsertDate,Telaio,Descr")] RPC_Telai rPC_Telai)
        {
            if (ModelState.IsValid)
            {
                db.RPC_Telai.Add(rPC_Telai);
                db.SaveChanges();
                return RedirectToAction("DoRefresh", "Home");
            }

            ViewBag.IDCantiere = new SelectList(db.RPC_Cantieri_vw, "IDCantiere", "Descr", rPC_Telai.IDCantiere);
            ViewBag.IDLotto = new SelectList(db.RPC_Lotti, "ID", "Descrizione", rPC_Telai.IDLotto);
            ViewBag.ID = new SelectList(db.RPC_Telai, "ID", "IDOperatore", rPC_Telai.ID);
            ViewBag.ID = new SelectList(db.RPC_Telai, "ID", "IDOperatore", rPC_Telai.ID);
            ViewBag.IDOperatore = new SelectList(db.PKT_Operatori, "ID", "ID", rPC_Telai.IDOperatore);
            return View(rPC_Telai);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RPC_Telai rPC_Telai = db.RPC_Telai.Find(id);
            if (rPC_Telai == null)
            {
                return HttpNotFound();
            }
            return View(rPC_Telai);
        }

        // POST: Telai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RPC_Telai rPC_Telai = db.RPC_Telai.Find(id);
            db.RPC_Telai.Remove(rPC_Telai);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("DoRefresh", "Home");
        }

        public ActionResult PrintList()
        {
            var model = new Models.HomeModel();

            string mySearch = TempData["mySearch"] as string;
            string myLotto = TempData["myLotto"] as string;

            string Scelta1 = Session["Scelta1"].ToString();
            
            if (Scelta1 == "TUTTE")
            {
                var telai = from s in db.RPC_Telai
                            where s.IDCantiere.ToString() == mySearch //&& s.IDLotto.ToString() == myLotto
                            orderby s.Telaio
                            select s;
                model.RPC_Telai = telai.ToList();
            }
            else if (Scelta1 == "CHIUSE")
            {
                var telai = from s in db.RPC_Telai
                            where s.IDCantiere.ToString() == mySearch //&& s.IDLotto.ToString() == myLotto
                            && s.IsFinished == true
                            orderby s.Telaio
                            select s;
                model.RPC_Telai = telai.ToList();
            }
            else if (Scelta1 == "APERTE")
            {
                var telai = from s in db.RPC_Telai
                            where s.IDCantiere.ToString() == mySearch //&& s.IDLotto.ToString() == myLotto
                            && s.IsFinished == false
                            orderby s.Telaio
                            select s;
                model.RPC_Telai = telai.ToList();
            }
            else
            {
                var telai = from s in db.RPC_Telai
                            where s.IDCantiere.ToString() == mySearch //&& s.IDLotto.ToString() == myLotto
                            orderby s.Telaio
                            select s;
                model.RPC_Telai = telai.ToList();
            }

            TempData["mySearch"] = mySearch;
            TempData["myLotto"] = myLotto;
            

            return View("PrintList", model);
        }

        public ActionResult Image(int? ID)
        {

            if (ID == null)
                ID = (int)TempData["myIDTelaio"];
            ViewBag.Message = "Images";
            ViewBag.IDTelaio = ID;
            RPC_Telai telaio = db.RPC_Telai.Find(ID);

            //RPC_FotoXTelaio foto = db.RPC_FotoXTelaio.Find(ID);

            TempData["myIDTelaio"] = ID;

            return View(telaio);
        }

        
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files,int? ID)
        {
            string pic = "";
            string path = "";
            int myIDTelaio = 0;

            
            string mySearch = TempData["mySearch"] as string;
            string myLotto = TempData["myLotto"] as string;
            myIDTelaio = (int)TempData["myIDTelaio"];

            foreach (var file in files)
            {
                pic = DateTime.Now.ToString("yyyyMMddHHmmssfff") + System.IO.Path.GetFileName(file.FileName);
                //pic = "Test.jpg";
                path = System.IO.Path.Combine(Server.MapPath("~/UploadedFiles"), myIDTelaio.ToString() + "_"+ pic);
                //path = System.IO.Path.Combine(Server.MapPath(@"E:\AutOK"), pic);


                if (file != null)
                {
                    file.SaveAs(path);
                }

                var sql = @"Insert Into RPC_FotoXTelaio (IDTelaio, NomeFile) Values (@IDTelaio, @NomeFile)";
                int noOfRowInserted = db.Database.ExecuteSqlCommand(sql,
                    new SqlParameter("@IDTelaio", myIDTelaio),
                    new SqlParameter("@NomeFile", myIDTelaio.ToString() + "_" + pic));

            }


            TempData["mySearch"] = mySearch;
            TempData["myLotto"] = myLotto;
            TempData["myIDTelaio"] = myIDTelaio;

            return RedirectToAction("Image", "Home", myIDTelaio);
        }

        public static void ResizeJpg(string path, int nWidth, int nHeight)
        {
            using (var result = new Bitmap(nWidth, nHeight))
            {
                using (var input = new Bitmap(path))
                {
                    using (Graphics g = Graphics.FromImage((System.Drawing.Image)result))
                    {
                        g.DrawImage(input, 0, 0, nWidth, nHeight);
                    }
                }

                var ici = ImageCodecInfo.GetImageEncoders().FirstOrDefault(ie => ie.MimeType == "image/jpeg");
                var eps = new EncoderParameters(1);
                eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                result.Save(path, ici, eps);
            }
        }

        
        public ActionResult Images(int? ID)
        {
            var model = new Models.HomeModel();
            int myIDTelaio = 0;
            if (TempData["myIDTelaio"] != null)
                myIDTelaio = (int)TempData["myIDTelaio"];
            else
                myIDTelaio = (int)ID;


            var foto = from s in db.RPC_FotoXTelaio
                        where s.IDTelaio.ToString() == ID.ToString()
                        select s;
            model.RPC_FotoXTelaio = foto.ToList();

            TempData["myIDTelaio"] = myIDTelaio;

            return View(foto);
        }

        public ActionResult RitornaPannelloUtente()
        {
            string myParams = "Username=" + User.Identity.Name + "&param=0&from=1";

            return Redirect("https://webservices.interconsult.it/applogin/Utente/UtentePanel?" + myParams);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return Redirect("https://webservices.interconsult.it/applogin");

        }


        public ActionResult DeleteSingleFoto(int id,int idTelaio,string picName )
        {

            var sql = @"DELETE FROM RPC_FotoXTelaio WHERE ID = @IDFoto";
            int myRecordCounter = db.Database.ExecuteSqlCommand(sql, new SqlParameter("@IDFoto", id));

            string fullPath = Request.MapPath("~/UploadedFiles/" + picName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            var model = new Models.HomeModel();
            int myIDTelaio = 0;
            if (TempData["myIDTelaio"] != null)
                myIDTelaio = (int)TempData["myIDTelaio"];
            else
                myIDTelaio = idTelaio;


            var foto = from s in db.RPC_FotoXTelaio
                       where s.IDTelaio.ToString() == myIDTelaio.ToString()
                       select s;
            model.RPC_FotoXTelaio = foto.ToList();

            TempData["myIDTelaio"] = myIDTelaio;

            return View("Images", foto);
        }

        public ActionResult ExportToExcel()
        {
            var dati = new System.Data.DataTable();
            DataTable s = db.RPC_Telai.ToDataTable();


            var grid = new GridView();
            grid.DataSource = dati;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("DoRefresh", "Home");
        }

    }
}