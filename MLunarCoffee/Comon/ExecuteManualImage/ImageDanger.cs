using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace MLunarCoffee.Comon.ExecuteManualImage
{
    public class ImageDanger
    {
        #region // Move string to image folder
        private async void SaveImage()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {

                dt = await confunc.ExecuteDataTable("[YYY_sp_Move_ImageCustomer]" ,CommandType.StoredProcedure);
            }
            var path = Path.Combine(Directory.GetCurrentDirectory() ,"wwwroot" ,"UploadedFiles");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int custid = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                string mgbase64 = dt.Rows[i]["Image"].ToString();
                var Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                string filename = Time + Guid.NewGuid() + ".jpg";
                string imgPath = Path.Combine(path ,filename);
                byte[] imageBytes = Convert.FromBase64String(mgbase64);
                System.IO.File.WriteAllBytes(imgPath ,imageBytes);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    await confunc.ExecuteDataTable("[YYY_sp_Move_ImageUpdate]" ,CommandType.StoredProcedure
                        ,"@CustID" ,SqlDbType.Int ,custid
                        ,"@Image" ,SqlDbType.NVarChar ,filename);
                }

                // UPDATE VTT_Customer SET Avatar='https://cdnvttimg.vttechsolution.com/DEMONK/_AvatarCustomer/'+Avatar
                //WHERE Avatar IS NOT NULL
            }

        }


        #endregion

        #region //Chi move image ,khong move file pdf.Branch mac dinh la 1
        private void MoveImageToSQL()
        {
            List<string> listcust = Comon.GetAllDirectoryInFolder("");
            DataTable dt = new DataTable();
            dt.Columns.Add("Index" ,typeof(int));
            dt.Columns.Add("Name" ,typeof(string));
            int i = 0;
            foreach (var cus in listcust)
            {
                DataRow dr = dt.NewRow();
                dr["Index"] = i;
                dr["Name"] = cus;
                dt.Rows.Add(dr);
                i = i + 1;
            }
            //for (int k = 0; k < dt.Rows.Count; k++)
            //{
            //    if (Convert.ToInt32(dt.Rows[k]["Index"]) > 1784)
            //    {
            //        string cus = dt.Rows[k]["Name"].ToString();
            //        var isNumeric = int.TryParse(cus, out int n);
            //        if (isNumeric == true)
            //        {
            //            int custid = Convert.ToInt32(cus);
            //            if (custid > 6296)
            //            {
            //                List<string> listfolder = Comon.Comon.GetAllDirectoryInFolder(custid.ToString());
            //                foreach (var folder in listfolder)
            //                {
            //                    List<string> listimgs = Comon.Comon.GetAllDirectoryInFolder(custid.ToString() + "/" + folder.ToString());
            //                    string imageliststring = "";
            //                    foreach (var imgs in listimgs)
            //                    {
            //                        imageliststring = imageliststring + "," + imgs.ToString();
            //                    }
            //                    InsertImageSQL(custid, folder.ToString(), imageliststring);
            //                }
            //            }
            //        }
            //    }

            //}
            ///
            //foreach (var cus in listcust)
            //{
            //    var isNumeric = int.TryParse(cus, out int n);
            //    if (isNumeric == true)
            //    {
            //        int custid = Convert.ToInt32(cus);
            //        if (custid > 6296)
            //        {
            //            List<string> listfolder = Comon.Comon.GetAllDirectoryInFolder(custid.ToString());
            //            foreach (var folder in listfolder)
            //            {
            //                List<string> listimgs = Comon.Comon.GetAllDirectoryInFolder(custid.ToString() + "/" + folder.ToString());
            //                string imageliststring = "";
            //                foreach (var imgs in listimgs)
            //                {
            //                    imageliststring = imageliststring + "," + imgs.ToString();
            ////                }
            //                InsertImageSQL(custid, folder.ToString(), imageliststring);
            //            }
            //        }
            //    }
            //}

        }
        private async void InsertImageSQL(int custid ,string foldername ,string imagestring)
        {
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("YYY_sp_Move_ImageToSQL" ,CommandType.StoredProcedure
                    ,"@CustID" ,SqlDbType.Int ,custid
                    ,"@FolderName" ,SqlDbType.NVarChar ,foldername
                    ,"@imagestring" ,SqlDbType.NVarChar ,imagestring);
            }

        }
        #endregion

        #region // Giảm dung lương hình ảnh  lại
        public async void ReduceImageGet()
        {
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("YYY_sp_Reduce_GetFolder" ,CommandType.StoredProcedure);

                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    string FolderName = dt.Rows[k]["FolderName"].ToString();
                    string FolderID = dt.Rows[k]["FolderID"].ToString();
                    string CustID = dt.Rows[k]["CustID"].ToString();
                    List<string> listinfolder = Comon.GetAllDirectoryInFolder("/" + CustID + "/" + FolderName);
                    if (listinfolder != null)
                    {
                        ReduceImageFolder_Exe(CustID ,FolderID ,FolderName ,listinfolder);
                    }

                }
            }

        }
        private async void ReduceImageFolder_Exe(string CustID ,string FolderID ,string FolderName ,List<string> listcust)
        {
            try
            {
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_Reduce_GetImg" ,CommandType.StoredProcedure
                        ,"@FolderID" ,SqlDbType.Int ,FolderID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ReduceImageGet_Exe(CustID ,FolderID ,FolderName ,dt ,listcust);
                    }


                }
            }
            catch (Exception ex)
            {

            }
        }


        private async void ReduceImageGet_Exe(string CustID ,string FolderID ,string FolderName ,DataTable dtimg ,List<string> listcust)
        {
            try
            {
                DataTable dt = new DataTable();
                if (listcust != null)
                {

                    foreach (var imgname in listcust)
                    {
                        if (!imgname.Contains(".webp"))
                        {
                            bool imghave = false;
                            for (int i = 0; i < dtimg.Rows.Count; i++)
                            {
                                if (dtimg.Rows[i]["NameImg"].ToString() == imgname)
                                {
                                    imghave = true;
                                    i = dtimg.Rows.Count;
                                }
                            }
                            if (imghave)
                            {
                                string link_old = String.Format(@"{0}{1}/{2}/{3}" ,Global.sys_ServerImageFolder ,CustID ,FolderName ,imgname);
                                Stream fileNew = new MemoryStream();
                                fileNew = GetStreamFromUrl(Global.sys_HTTPImageRoot + CustID + "/" + FolderName + "/" + imgname);
                                string ext = Path.GetExtension(imgname);
                                byte[] fileData = null;
                                using (var ms = new MemoryStream())
                                {
                                    fileNew.Position = 0;
                                    fileNew.CopyTo(ms);
                                    fileData = ms.ToArray();
                                    if (ext != ".svg")
                                    {
                                        fileData = Comon.ResizeImage_Byte(fileData ,2000 ,2000);  
                                    }

                                }
                                fileNew = new MemoryStream(fileData);
                                fileNew = Comon.ConvertImageWebp(fileNew);
                                using (var ms = new MemoryStream())
                                {
                                    fileNew.Position = 0;
                                    fileNew.CopyTo(ms);
                                    fileData = ms.ToArray();
                                }




                                string newname = imgname.Replace(ext ,".webp");
                                string link = String.Format(@"{0}{1}/{2}/{3}" ,Global.sys_ServerImageFolder ,CustID ,FolderName ,newname);
                                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(link);
                                req.UseBinary = true;
                                req.Method = WebRequestMethods.Ftp.UploadFile;
                                req.Credentials = new NetworkCredential(Global.sys_ServerImageUserName ,Global.sys_ServerImagePassword);
                                req.ContentLength = fileData.Length;
                                Stream reqStream = req.GetRequestStream();
                                await reqStream.WriteAsync(fileData ,0 ,fileData.Length);
                                reqStream.Close();

                                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                                {
                                    await confunc.ExecuteDataTable("YYY_sp_Reduce_UpdateImg" ,CommandType.StoredProcedure
                                        ,"@FolderID" ,SqlDbType.Int ,FolderID
                                        ,"@Name" ,SqlDbType.NVarChar ,imgname
                                        ,"@newname" ,SqlDbType.NVarChar ,newname);
                                }
                                DeleteFile(link_old);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        private static Stream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;

            using (var wc = new System.Net.WebClient()) imageData = wc.DownloadData(url);

            return new MemoryStream(imageData);
        }
        private bool DeleteFile(string des)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(des);
                request.Credentials = new NetworkCredential(Global.sys_ServerImageUserName ,Global.sys_ServerImagePassword);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion
    }
}
