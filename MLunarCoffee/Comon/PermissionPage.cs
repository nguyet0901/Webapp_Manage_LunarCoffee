using Microsoft.AspNetCore.Http;
using MLunarCoffee.Comon;
using MLunarCoffee.Models.GlobalPer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Web;

namespace MLunarCoffee.Comon
{
    public class PermissionPage : IDisposable
    {
        public List<PageClass> menutable { get; protected set; }
        public List<PageMushide> menuhusthide { get; protected set; }
        public List<PageUser> permenu { get; protected set; }
        public PermissionPage(List<PageClass> _menutable, List<PageMushide> _menuhusthide
            , List<PageUser> _permenu)
        {
            menutable = _menutable;
            menuhusthide = _menuhusthide;
            permenu = _permenu;

        }
        public bool CheckPermissionPageByMenu(string pageAbsoulte, string ip)
        {
            try
            {
                if (CheckMenuIsHideFromServer(pageAbsoulte))
                {
                    if (CheckCurrentPageIsPageWithoutPermission(pageAbsoulte))
                    {
                        var count = (from m in permenu
                                     where m.MenuURL.TrimEnd('/') == pageAbsoulte || m.CodeURL.TrimEnd('/') == pageAbsoulte
                                     select m).Count();
                        if (count != 0) return false; // do duoc
                        return true; // Khong do duoc
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (CheckMenuIsShowOnlyVTT(pageAbsoulte, ip))
                    {
                        return false;// VTT vo duoc
                    }
                    return true; // Khong do duoc
                }
            }
            catch (Exception)
            {
                return true; // Khong do duoc
            }

        }
        private bool CheckCurrentPageIsPageWithoutPermission(string pageAbsoulte)
        {
            try
            {
                var count = (from m in menutable
                             where m.MenuURL.TrimEnd('/') == pageAbsoulte || m.CodeURL.TrimEnd('/') == pageAbsoulte
                             select m).Count();

                if (count != 0) return true; // trang phan quyen
                return false; // trang khong duoc phan quyen
            }
            catch (Exception)
            {
                return false; // trang khong duoc phan quyen
            }
        }

        private bool CheckMenuIsHideFromServer(string pageAbsoulte)
        {
            try
            {
                var count = (from m in menuhusthide
                             where m.LinkUrlWhenClick.TrimEnd('/') == pageAbsoulte
                             select m).Count();

                if (count != 0) return false; // trang nay thuoc ve menu bi cam tren server
                return true; // trang nay khong bi cam

            }
            catch (Exception)
            {
                return true; // trang nay khong bi cam
            }
        }
        private bool CheckMenuIsShowOnlyVTT(string pageAbsoulte, string ip)
        {
            try
            {
                var count = (from m in menuhusthide
                             where m.IPAllow == ip
                             select m).Count();
                if (count != 0)
                {
                    return true; // vtt duoc vao trang nay
                }
                else
                {
                    return false; //vtt khong vao duoc trang nay
                }

            }
            catch (Exception)
            {
                return false; // trang nay khong duoc vao
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (menutable != null) { menutable.Clear(); menutable = null; }
                if (menuhusthide != null) { menuhusthide.Clear(); menuhusthide = null; }
                if (permenu != null) { permenu.Clear(); permenu = null; }
            }
        }
    }
}