using Aspose.Cells;
using System.Data;
using System.Web;
using Aspose.Pdf.Drawing;
using System.IO;
using System;

namespace DaZhongManagementSystem.Common
{
    public class ExportExcel
    {

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="templateFileName">模板名称</param>
        /// <param name="fileName">导出文件名称</param>
        /// <param name="dataSource">数据源</param>
        public static string ExportExcelsTo(string templateFileName, string fileName, DataTable dataSource)
        {
            string rootPath = HttpContext.Current.Server.MapPath(string.Format("~/ReportTemplate/{0}", templateFileName));
            string folderPath = HttpContext.Current.Server.MapPath("~/Temp");
            if (System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            string filePath = System.IO.Path.Combine(folderPath, fileName);
            Workbook wk = new Workbook(rootPath);
            WorkbookDesigner designer = new WorkbookDesigner(wk);
            designer.SetDataSource(dataSource);
            designer.Process();
            designer.Workbook.Save(filePath);
            //designer.Workbook.Save(HttpContext.Current.Response, fileName, ContentDisposition.Attachment, designer.Workbook.SaveOptions);
            return fileName;
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="templateFileName">模板名称</param>
        /// <param name="fileName">导出文件名称</param>
        /// <param name="dataSource">数据源</param>
        public static void ExportExcels(string templateFileName, string fileName, DataTable dataSource)
        {
            string rootPath = HttpContext.Current.Server.MapPath(string.Format("~/ReportTemplate/{0}", templateFileName));
            Workbook wk = new Workbook(rootPath);
            WorkbookDesigner designer = new WorkbookDesigner(wk);
            designer.SetDataSource(dataSource);
            designer.Process();
            designer.Workbook.Save(HttpContext.Current.Response, fileName, ContentDisposition.Attachment, designer.Workbook.SaveOptions);
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="templateFileName">模板名称</param>
        /// <param name="fileName">导出文件名称</param>
        /// <param name="dataSource">数据源</param>
        public static void ExportExcels(string templateFileName, string fileName, DataSet dataSource)
        {
            string rootPath = HttpContext.Current.Server.MapPath(string.Format("~/ReportTemplate/{0}", templateFileName));
            Workbook wk = new Workbook(rootPath);
            WorkbookDesigner designer = new WorkbookDesigner(wk);
            designer.SetDataSource(dataSource);
            designer.Process();
            designer.Workbook.Save(HttpContext.Current.Response, fileName, ContentDisposition.Attachment, designer.Workbook.SaveOptions);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="templateFileName">模板名称</param>
        /// <param name="fileName">导出文件名称</param>
        /// <param name="dataSource">数据源</param>
        public static void ExportExcels(string fileName, DataTable dataSource)
        {
            string rootPath = BuildExportTemplate(dataSource);
            Workbook wk = new Workbook(rootPath);
            WorkbookDesigner designer = new WorkbookDesigner(wk);
            designer.SetDataSource(dataSource);
            designer.Process();
            designer.Workbook.Save(HttpContext.Current.Response, fileName, ContentDisposition.Attachment, designer.Workbook.SaveOptions);
            try
            {
                File.Delete(rootPath);
            }
            catch (Exception)
            {

            }
        }

        public static string BuildExportTemplate(DataTable dataSource)
        {
            string fileName = GetImportTemplateTempFile();
            Workbook workbook = new Workbook(FileFormatType.Xlsx);
            workbook.Worksheets.Clear();
            Style styleHeader = workbook.Styles[workbook.Styles.Add()];//列头样式 
            styleHeader.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            styleHeader.VerticalAlignment = TextAlignmentType.Center;
            styleHeader.Font.Name = "宋体";//文字字体 
            styleHeader.Font.Size = 10;//文字大小 
            styleHeader.Font.IsBold = true;//粗体
            System.Drawing.Color color = System.Drawing.Color.FromArgb(0, 0, 158, 251);
            styleHeader.BackgroundColor = color;
            styleHeader.ForegroundColor = color;
            styleHeader.Pattern = BackgroundType.Solid;
            styleHeader.Font.Color = System.Drawing.Color.White;
            styleHeader.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleHeader.Borders[BorderType.RightBorder].Color = System.Drawing.Color.White;
            styleHeader.IsTextWrapped = true;

            Style styleData = workbook.Styles[workbook.Styles.Add()];//列头样式 
            styleData.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            styleData.VerticalAlignment = TextAlignmentType.Center;
            styleData.IsTextWrapped = true;

            Worksheet sheet = workbook.Worksheets.Add("sheet1");
            Cells cells = sheet.Cells;
            cells.SetRowHeight(0, 20);//设置DID行为隐藏行;
            cells.SetColumnWidth(0,28);//设置DID行为隐藏行;
            cells[0, 0].PutValue("公司/部门");
            cells[0, 0].SetStyle(styleHeader); ;
            cells[1, 0].PutValue("&=[table].OrganizationName");
            cells[1, 0].SetStyle(styleData);
            cells.SetColumnWidth(1, 12);//设置DID行为隐藏行;
            cells[0, 1].PutValue("姓名");
            cells[0, 1].SetStyle(styleHeader); ;
            cells[1, 1].PutValue("&=[table].Name");
            cells[1,1].SetStyle(styleData);
            cells.SetColumnWidth(2, 20);//设置DID行为隐藏行;
            cells[0, 2].PutValue("身份证号码");
            cells[0, 2].SetStyle(styleHeader); ;
            cells[1, 2].PutValue("&=[table].IDNumber");
            cells[1, 2].SetStyle(styleData);
            for (int i = 3; i < dataSource.Columns.Count; i++)
            {
                cells[0, i].PutValue(dataSource.Columns[i].ColumnName);
                cells[0, i].SetStyle(styleHeader); ;
                cells[1, i].PutValue("&=[table]." + dataSource.Columns[i].ColumnName);
                cells[1, i].SetStyle(styleData); ;
                cells.SetColumnWidth(i, 10);//设置DID行为隐藏行;
            }
            workbook.Save(fileName);
            return fileName;
        }

        /// <summary>
        /// 获取创建文件的文件名称
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static string GetImportTemplateTempFile()
        {
            string filePath = string.Empty;
            string fileName = string.Empty;
            do
            {
                fileName = string.Format("{0}_{1}.xlsx", Guid.NewGuid().ToString(), DateTime.Now.ToString("yyyyMMddHHmmss"));
                filePath = System.IO.Path.Combine("", fileName);

            } while (File.Exists(filePath));
            return filePath;
        }
    }
}
