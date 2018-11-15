using System;
using System.Data;
using System.IO;
using System.Web;
using Aspose.Cells;

namespace DaZhongManagementSystem.Common.Tools
{
    public class UploadHelper
    {
        public bool UploadFile(HttpPostedFileBase Filedata, string fileName, out string path)
        {

            path = string.Empty;
            try
            {
                string rootDir = HttpContext.Current.Server.MapPath("/UpLoad");

                if (!Directory.Exists(rootDir))
                {
                    // 创建根目录下子文件夹
                    Directory.CreateDirectory(rootDir);
                }
                if (Directory.Exists(rootDir))
                {
                    //上传附件
                    path = rootDir + "/" + fileName;
                    Filedata.SaveAs(path);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                //LogHelper.WriteLog(ex.Message);
                return false;
            }
        }

        public DataTable GetDataByExcel(string file, bool isSetTitle = false)
        {
            Workbook workbook = new Workbook(file);
            Cells cells = workbook.Worksheets[0].Cells;
            DataTable dt = null;
            try
            {
                dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, isSetTitle);//noTitle
            }
            catch (Exception ex)
            {
                LogHelper.LogHelper.WriteLog("导入Excel " + ex.ToString());
            }

            return dt;
        }
        public DataTable GetDataByExcelString(string file, bool isSetTitle = false)
        {
            Workbook workbook = new Workbook(file);
            Cells cells = workbook.Worksheets[0].Cells;
            DataTable dt = null;
            try
            {
                //ExportTableOptions option = new ExportTableOptions();
                //option.CheckMixedValueType = false;
                //option.ExportColumnName = isSetTitle;
                //option.SkipErrorValue = true;
                dt = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, isSetTitle);//noTitle
            }
            catch (Exception ex)
            {
                LogHelper.LogHelper.WriteLog("导入Excel " + ex.ToString());
            }

            return dt;
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="templateFileName">模板名称</param>
        /// <param name="fileName">导出文件名称</param>
        /// <param name="dataSource">数据源</param>
        public static void ExportExcels(string templateFileName, string fileName)
        {
            string rootPath = HttpContext.Current.Server.MapPath(string.Format("~/ExerciseTemplate/{0}", templateFileName));
            Workbook wk = new Workbook(rootPath);
            WorkbookDesigner designer = new WorkbookDesigner(wk);
            designer.Process();
            designer.Workbook.Save(HttpContext.Current.Response, fileName, ContentDisposition.Inline, designer.Workbook.SaveOptions);
        }


        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="templateFileName">模板名称</param>
        /// <param name="fileName">导出文件名称</param>
        /// <param name="dataSource">数据源</param>
        public static void ExportExcel(string templateFileName, string fileName)
        {
            string rootPath = HttpContext.Current.Server.MapPath(string.Format("~/PushTemplate/{0}", templateFileName));
            Workbook wk = new Workbook(rootPath);
            WorkbookDesigner designer = new WorkbookDesigner(wk);
            designer.Process();
            designer.Workbook.Save(HttpContext.Current.Response, fileName, ContentDisposition.Inline, designer.Workbook.SaveOptions);
        }

    }
}
