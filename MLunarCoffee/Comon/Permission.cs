using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
namespace MLunarCoffee.Comon
{
    public static class Permission
    {
        /// <param name="dtmain">Danh sách control ban đầu</param>
        /// <param name="dtBlockClient">Danh sách control bị dấu do client</param>
        /// <param name="dtBlockClient">Danh sách control bị dấu do group</param>
        /// <param name="linkCurrentPage">Trang hiện tại</param>
        /// <returns></returns>
        public static System.Data.DataTable Get_Control_IsShow_ByCurrentpage(System.Data.DataTable dtmain, System.Data.DataTable dtBlockClient, System.Data.DataTable dtBlockGroup, string linkCurrentPage)
        {
            try
            {
                DataTable dtResulf;
                DataTable blockCurrentpage_Client = Get_ControlIsBlock_FromServer_ByCurrentpage(dtBlockClient, linkCurrentPage);
                if (blockCurrentpage_Client == null) dtResulf = dtmain;
                else
                {
                    // Loc nhung control bi dau tu server
                    var query = from row in dtmain.AsEnumerable()
                                join block in blockCurrentpage_Client.AsEnumerable()
                                on row.Field<String>("ControlName") equals block.Field<String>("ControlName")
                                select row;
                    dtResulf = dtmain.AsEnumerable().Except(query).CopyToDataTable();
                }
                // Loc nhung control bi dau do group
                DataTable blockCurrentpage_Group = Get_ControlIsBlock_FromClient_ByCurrentpage(dtBlockGroup, linkCurrentPage);
                if (blockCurrentpage_Group == null) return dtResulf;
                else
                {
                    var query = from row in dtResulf.AsEnumerable()
                            join block in blockCurrentpage_Group.AsEnumerable()
                            on row.Field<String>("ControlName") equals block.Field<String>("ControlName")
                            select row;
                    query = dtResulf.AsEnumerable().Except(query);

                    dtResulf = query.CopyToDataTable();
                }
                return dtResulf;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Lọc danh sách control bị dấu , từ server 
        /// </summary>
        /// <param name="dtBlock">Danh sách control bị dấu từ phía server , lấy theo client</param>
        /// <param name="linkCurrentPage">Link của page hiện tại</param>
        /// <returns></returns>
        public static System.Data.DataTable Get_ControlIsBlock_FromServer_ByCurrentpage(System.Data.DataTable dtBlock, string linkCurrentPage)
        {
            try
            {
                System.Data.DataRow[] dr = dtBlock.Select("LinkPage='" + linkCurrentPage + "'");
                return dr.CopyToDataTable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Lọc danh sách control bị dấu , từ group 
        /// </summary>
        /// <param name="dtBlock">Danh sách control bị dấu từ phía client , lấy theo clientgroupparam>
        /// <param name="linkCurrentPage">Link của page hiện tại</param>
        /// <returns></returns>
        public static System.Data.DataTable Get_ControlIsBlock_FromClient_ByCurrentpage(System.Data.DataTable dtBlock, string linkCurrentPage)
        {
            try
            {
                if(dtBlock==null) return null;
                System.Data.DataRow[] dr = dtBlock.Select("LinkPage='" + linkCurrentPage + "'");
                return dr.CopyToDataTable();
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">Table chua loc</param>
        /// <param name="type">Key de loc</param>
        /// <returns></returns>
        public static DataTable SelectDataFromKey(DataTable target, string type)
        {
            try
            {
                return target.Select("GroupType='" + type + "'").CopyToDataTable();
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}