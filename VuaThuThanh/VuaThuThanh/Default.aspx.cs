using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Ex = Ext.Net;
using VuaThuThanh.DB;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace VuaThuThanh
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["tuong"]) && !Ex.X.IsAjaxRequest)
            {
                string tuong = Request["tuong"].Trim();
                bool bBigSize = false;
                if (!string.IsNullOrEmpty(Request["big"]))
                {
                    bBigSize = true;
                }
                if (!bBigSize)
                {
                    if (!File.Exists(Server.MapPath("~/Images/Tuong150x225/" + tuong + ".png")))
                    {
                        DB.DBDataContext db = new DB.DBDataContext();
                        var off = (from t
                                        in db.M_Tuongs
                                   where t.MaTuong == tuong
                                   select new
                                   {
                                       t.PhaiXid,
                                       t.MaTuong,
                                       t.TenTuong,
                                       t.M_BoTuong.BoTuong,
                                       t.SoSao,
                                       t.NuocXid
                                   }).FirstOrDefault();

                        Bitmap bm = new Bitmap(150, 225);

                        Graphics g = Graphics.FromImage(bm);
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //Hinh Tuong
                        g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Tuong/" + off.MaTuong + ".jpg")), 0, 0, 150, 225);
                        //Phai
                        g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Phai/" + off.PhaiXid + ".png")), 5, 5, 25, 25);
                        //So sao
                        for (int iSao = 1; iSao <= off.SoSao; iSao++)
                        {
                            g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Misc/star.png")), 150 - (iSao * 20), 205, 16, 16);
                        }
                        //Nuoc
                        g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Nuoc/" + off.NuocXid + ".png")), 117, 2, 30, 30);

                        //Luu vao o cung
                        bm.Save(Server.MapPath("~/Images/Tuong150x225/" + Request["tuong"] + ".png"), ImageFormat.Png);
                        //Tra ve
                        //Response.ContentType = "image/png";
                        //bm.Save(Response.OutputStream, ImageFormat.Png);

                        bm.Dispose();
                        g.Dispose();
                    }
                    Response.Redirect("/Images/Tuong150x225/" + Request["tuong"] + ".png");
                }
                else
                { //Big size
                    DB.DBDataContext db = new DB.DBDataContext();
                    var off = (from t
                                    in db.M_Tuongs
                               where t.MaTuong == tuong
                               select new
                               {
                                   t.PhaiXid,
                                   t.MaTuong,
                                   t.TenTuong,
                                   t.M_BoTuong.BoTuong,
                                   t.SoSao,
                                   t.NuocXid,
                                   t.LucDanh,
                                   t.TamDanh,
                                   t.TocDoDanh,
                                   t.Mau,
                                   t.TriLuc,
                                   t.TocDoDiChuyen,
                                   t.M_Nuoc.Nuoc,
                                   t.TuyetKy1Xid,
                                   t.TuyetKy2Xid,
                                   t.M_TuyetKy,
                                   t.M_TuyetKy1
                               }).FirstOrDefault();

                    Bitmap bm = new Bitmap(480, 320);

                    Graphics g = Graphics.FromImage(bm);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //bgkhung
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Misc/bgkhung.jpg")), 0, 0, 480, 320);
                    //khung
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Misc/khung.png")), 0, 0, 480, 320);
                    //tuong
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Tuong/" + off.MaTuong + ".jpg")), 35, 25, 180, 270);
                    //Border
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Misc/" + off.SoSao + "sao.png")), 30, 20, 190, 280);
                    //phai
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Phai/" + off.PhaiXid + ".png")), 35, 25, 31, 31);
                    //sao
                    for (int iSao = 1; iSao <= off.SoSao; iSao++)
                    {
                        g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Misc/star.png")), 48 + (iSao * 22), 30, 18, 18);
                    }
                    //Ten Tuong
                    g.DrawString(off.TenTuong, new Font("Tahoma", 13, FontStyle.Bold), new SolidBrush(Color.White), 240, 20);
                    //bg Thong so
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Misc/bgthongso.png")), 240, 40, 210, 80);
                    //Thong so
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Misc/lucdanh.png")), 245, 45, 16, 16);
                    g.DrawString(off.LucDanh.ToString(), new Font("Tahoma", 11, FontStyle.Bold), new SolidBrush(Color.FromArgb(245, 190, 0)), 262, 44);
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Misc/phamvi.png")), 315, 45, 16, 16);
                    g.DrawString(off.TamDanh.ToString(), new Font("Tahoma", 11, FontStyle.Bold), new SolidBrush(Color.FromArgb(245, 190, 0)), 332, 44);
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Misc/tocdanh.png")), 375, 45, 16, 16);
                    g.DrawString(off.TocDoDanh.ToString(), new Font("Tahoma", 11, FontStyle.Bold), new SolidBrush(Color.FromArgb(245, 190, 0)), 392, 44);
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Misc/mau.png")), 245, 71, 16, 16);
                    g.DrawString(off.Mau.ToString(), new Font("Tahoma", 11, FontStyle.Bold), new SolidBrush(Color.FromArgb(245, 190, 0)), 262, 70);
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Misc/triluc.png")), 315, 71, 16, 16);
                    g.DrawString(off.TriLuc.ToString(), new Font("Tahoma", 11, FontStyle.Bold), new SolidBrush(Color.FromArgb(245, 190, 0)), 332, 70);
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Misc/dichuyen.png")), 375, 71, 16, 16);
                    g.DrawString(off.TocDoDiChuyen.ToString(), new Font("Tahoma", 11, FontStyle.Bold), new SolidBrush(Color.FromArgb(245, 190, 0)), 392, 70);
                    ////Nuoc
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/Nuoc/" + off.NuocXid + ".png")), 243, 95, 20, 20);
                    g.DrawString(off.Nuoc, new Font("Tahoma", 8, FontStyle.Bold), new SolidBrush(Color.White), 263, 99);
                    //g.DrawString(off.Nuoc, new Font("Tahoma", 8, FontStyle.Bold), new SolidBrush(Color.White), 300, 98);
                    StringFormat sf = new StringFormat();

                    if (!string.IsNullOrEmpty(off.BoTuong))
                    {
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Alignment = StringAlignment.Far;
                        g.DrawString("Bộ " + off.BoTuong, new Font("Tahoma", 7, FontStyle.Bold), new SolidBrush(Color.FromArgb(0, 255, 0)), new Rectangle(300, 98, 145, 14), sf);
                    }
                    //Tuyet Ki
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Near;
                    g.DrawString("Tuyệt Kỹ", new Font("Tahoma", 11, FontStyle.Bold), new SolidBrush(Color.White), 240, 140);
                    g.DrawImage(Image.FromFile(Server.MapPath("~/Images/TuyetKy/" + off.TuyetKy1Xid + ".png")), 240, 170, 45, 48);
                    g.DrawString(off.M_TuyetKy.TuyetKy, new Font("Tahoma", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(245, 190, 0)), 295, 167);
                    //g.DrawRectangle(new Pen(new SolidBrush(Color.Green)), new Rectangle(295, 180, 165, 35));
                    g.DrawString(off.M_TuyetKy.MoTa, new Font("Tahoma", 7, FontStyle.Bold), new SolidBrush(Color.FromArgb(245, 190, 200)), new Rectangle(295, 180, 165, 35), sf);
                    if (off.TuyetKy2Xid.HasValue)
                    {
                        g.DrawImage(Image.FromFile(Server.MapPath("~/Images/TuyetKy/" + off.TuyetKy2Xid + ".png")), 240, 230, 45, 48);
                        g.DrawString(off.M_TuyetKy1.TuyetKy, new Font("Tahoma", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(245, 190, 0)), 295, 227);
                        //g.DrawRectangle(new Pen(new SolidBrush(Color.Green)), new Rectangle(295, 180, 165, 35));
                        g.DrawString(off.M_TuyetKy1.MoTa, new Font("Tahoma", 7, FontStyle.Bold), new SolidBrush(Color.FromArgb(245, 190, 200)), new Rectangle(295, 240, 145, 35), sf);
                    }
                    //Dong Moc
                    g.DrawString("vuathuthanh.net", new Font("Tahoma", 7, FontStyle.Regular), new SolidBrush(Color.DarkRed), 395, 295);
                    //Luu vao o cung
                    //bm.Save(Server.MapPath("~/Images/Tuong150x225/" + Request["tuong"] + ".png"), ImageFormat.Png);
                    //Tra ve
                    Response.ContentType = "image/png";
                    bm.Save(Response.OutputStream, ImageFormat.Png);

                    bm.Dispose();
                    g.Dispose();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ex.X.IsAjaxRequest && string.IsNullOrEmpty(Request["tuong"]))
            {
                DB.DBDataContext db = new DB.DBDataContext();
                var listTuong = from tuong
                                in db.M_Tuongs
                                orderby tuong.SoSao descending
                                select new
                                {
                                    tuong.Pid,
                                    tuong.MaTuong,
                                    tuong.TenTuong,
                                    tuong.SoSao,
                                    tuong.NuocXid
                                };

                this.storeTuong.DataSource = listTuong;
                this.storeTuong.DataBind();
            }
        }
        protected void viewTuongItem_Click(object sender, Ex.DirectEventArgs e)
        {
            if (sender is Ex.DataView)
            {
                string selectedID = (sender as Ex.DataView).SelectedRecordID;
                DB.DBDataContext db = new DB.DBDataContext();
                var off = (from t
                            in db.M_Tuongs
                           where t.Pid.ToString() == selectedID
                           select t).FirstOrDefault();

                Ex.Window win = new Ex.Window
                {
                    ID = "wShowDetail" + off.MaTuong,
                    Title = off.TenTuong,
                    Height = 353,
                    Width = 493,
                    //BodyPadding = 5,
                    Resizable = false,
                    Modal = false,
                    CloseAction = Ex.CloseAction.Destroy,
                    Html = "<img src='?big=true&tuong=" + off.MaTuong + "'>"
                };

                win.Render(this.Form);

            }
        }
        protected void btnLocTuong_Click(object sender, Ex.DirectEventArgs e)
        {
            DB.DBDataContext db = new DB.DBDataContext();
            var rsTuong = from tuong in db.M_Tuongs select tuong;
            //Ten Tuong
            if (!string.IsNullOrEmpty(txtTenTuong.Text.Trim()))
            {
                string strTenTuong = txtTenTuong.Text.Trim();
                rsTuong = from tuong
                          in rsTuong
                          where tuong.TenTuong.ToUpper().Contains(strTenTuong)
                                || tuong.MaTuong.Replace("InitOfficerBow", "CungTien")
                                                .Replace("InitOfficerFan", "QuatPhung")
                                                .Replace("InitOfficerSpear", "ThuongThan")
                                                .Replace("InitOfficerSword", "KiemThanh").ToUpper().Contains(strTenTuong.Replace(" ", string.Empty).ToUpper())
                          select tuong;

            }
            //Nuoc
            if (txtNuoc.SelectedItems.Count > 0)
            {
                List<string> list = new List<string>();
                for (int iIndex = 0; iIndex < txtNuoc.SelectedItems.Count; iIndex++)
                {
                    list.Add(txtNuoc.SelectedItems[iIndex].Value);
                }
                rsTuong = from tuong
                           in rsTuong
                          where list.Contains(tuong.NuocXid.ToString())
                          select tuong;
            }
            //Phai
            if (txtPhai.SelectedItems.Count > 0)
            {
                List<string> list = new List<string>();
                for (int iIndex = 0; iIndex < txtPhai.SelectedItems.Count; iIndex++)
                {
                    list.Add(txtPhai.SelectedItems[iIndex].Value);
                }
                rsTuong = from tuong
                           in rsTuong
                          where list.Contains(tuong.PhaiXid.ToString())
                          select tuong;
            }
            //So sao
            if (txtSoSao.SelectedItems.Count > 0)
            {
                List<string> list = new List<string>();
                for (int iIndex = 0; iIndex < txtSoSao.SelectedItems.Count; iIndex++)
                {
                    list.Add(txtSoSao.SelectedItems[iIndex].Value);
                }
                rsTuong = from tuong
                          in rsTuong
                          where list.Contains(tuong.SoSao.ToString())
                          select tuong;
            }
            //Luc danh
            if (!string.IsNullOrEmpty(txtLucDanh.Text.Trim()))
            {
                int iNumber = 0;
                if (int.TryParse(txtLucDanh.Text.Trim(), out iNumber))
                {
                    rsTuong = from tuong
                              in rsTuong
                              where tuong.LucDanh >= iNumber
                              select tuong;
                }
            }
            //Pham Vi
            if (!string.IsNullOrEmpty(txtPhamVi.Text.Trim()))
            {
                int iNumber = 0;
                if (int.TryParse(txtPhamVi.Text.Trim(), out iNumber))
                {
                    rsTuong = from tuong
                              in rsTuong
                              where tuong.TamDanh >= iNumber
                              select tuong;
                }
            }
            //Toc Danh
            if (!string.IsNullOrEmpty(txtTocDo.Text.Trim()))
            {
                int iNumber = 0;
                if (int.TryParse(txtTocDo.Text.Trim(), out iNumber))
                {
                    rsTuong = from tuong
                              in rsTuong
                              where tuong.TocDoDanh >= iNumber
                              select tuong;
                }
            }
            //Tri Luc
            if (!string.IsNullOrEmpty(txtTriLuc.Text.Trim()))
            {
                int iNumber = 0;
                if (int.TryParse(txtTriLuc.Text.Trim(), out iNumber))
                {
                    rsTuong = from tuong
                              in rsTuong
                              where tuong.TriLuc >= iNumber
                              select tuong;
                }
            }
            var listTuong = from tuong
                            in rsTuong
                            orderby tuong.SoSao descending
                            select new
                            {
                                tuong.Pid,
                                tuong.MaTuong,
                                tuong.TenTuong,
                                tuong.SoSao,
                                tuong.NuocXid
                            }
                        ;
            this.storeTuong.DataSource = listTuong;
            this.storeTuong.DataBind();
        }
    }
}