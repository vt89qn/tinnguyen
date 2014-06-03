/*
 * Create by Khoan Nguyen
 * */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SD = System.Data;
using System.Text;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Drawing;


namespace Vnit.Excel
{
    /// <summary>
    /// Working with excel application
    /// </summary>
    public class ExcelEngine
    {
        //2010.05.18 Dung Add Start
        #region EVENT
        public event EventHandler OnBeforeSave;
        public event EventHandler OnQuit;
        public event EventHandler OnOpen;
        #endregion
        //2010.05.18 Dung Add End

        private object oMissing = System.Reflection.Missing.Value;
        public ApplicationClass ExcelAp = null;
        public Workbooks ExcelWkbks = null;
        public Workbook Excelbk = null;
        public Worksheet ActiveSheet = null;
        private CultureInfo m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
        private string _filedestination = "";
        public static string _filedestination_temp = "";
        private int rowCount = 0;

        /// <summary>
        /// Total rows which to be used in excel file
        /// </summary>
        public int RowCount
        {
            get { return rowCount; }
            set { rowCount = value; }
        }

        private int columnCount = 0;
        /// <summary>
        ///  Total columns which to be used in excel file
        /// </summary>
        public int ColumnCount
        {
            get { return columnCount; }
            set { columnCount = value; }
        }

        private Range usedRange = null;
        /// <summary>
        /// The range to be used in excel file
        /// </summary>
        public Range UsedRange
        {
            get { return usedRange; }
            set { usedRange = value; }
        }

        private string _fileName = "";
        /// <summary>
        /// Name of current excel file
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private bool allowVisible = false;
        /// <summary>
        /// Gets or sets value indicating whether allow show excel app
        /// </summary>
        public bool AllowVisible
        {
            get { return allowVisible; }
            set { allowVisible = value; }
        }

        /// <summary>
        /// Init excel application
        /// </summary>
        /// <param name="fileName">filename to save</param>
        /// <param name="fileTemplate">filename template</param>
        /// <returns>True : init succesfull</returns>
        public bool CreateObject(string fileName, string fileTemplate)
        {
            try
            {
                string file = System.IO.Path.GetFileName(fileName);
                string path = System.IO.Path.GetDirectoryName(fileName);
                string fileNameTemp = System.IO.Path.Combine(path, file);
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                if (System.IO.File.Exists(fileNameTemp))
                {
                    System.IO.File.Delete(fileNameTemp);
                }

                copy(fileTemplate, fileNameTemp);

                _filedestination = fileName;
                _filedestination_temp = fileNameTemp;

                _fileName = fileNameTemp;

                ExcelAp = new ApplicationClass();
                // Dung 2010.05.18 Add Start
                ExcelAp.WorkbookBeforeSave += new AppEvents_WorkbookBeforeSaveEventHandler(ExcelAp_WorkbookBeforeSave);
                ExcelAp.WorkbookBeforeClose += new AppEvents_WorkbookBeforeCloseEventHandler(ExcelAp_WorkbookBeforeClose);
                ExcelAp.WorkbookOpen += new AppEvents_WorkbookOpenEventHandler(ExcelAp_WorkbookOpen);
                // Dung 2010.05.18 Add End                
                ExcelWkbks = ExcelAp.Workbooks;

                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                Excelbk = ExcelWkbks.Open(fileNameTemp, 3, false, 3, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                                    ";", false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                ActiveSheet = (Worksheet)Excelbk.ActiveSheet;

                if (!allowVisible)
                {
                    ExcelAp.ScreenUpdating = true;
                    Excelbk.Saved = true;

                    //ExcelAp.DisplayAlerts = false;
                }
                //Get RowCount and ColumnCount
                Range allCells = ActiveSheet.get_Range("A1", "Z500");
                usedRange = GetUsedRange(allCells);
                //range = ActiveSheet.UsedRange;               

                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Init excel application
        /// </summary>
        /// <param name="fileName">filename to save</param>        
        /// <returns>True : init succesfull</returns>
        public bool CreateObject(string fileName)
        {
            try
            {
                _filedestination = fileName;
                ExcelAp = new ApplicationClass();
                // Dung 2010.05.18 Add Start
                ExcelAp.WorkbookBeforeSave += new AppEvents_WorkbookBeforeSaveEventHandler(ExcelAp_WorkbookBeforeSave);
                ExcelAp.WorkbookBeforeClose += new AppEvents_WorkbookBeforeCloseEventHandler(ExcelAp_WorkbookBeforeClose);
                ExcelAp.WorkbookOpen += new AppEvents_WorkbookOpenEventHandler(ExcelAp_WorkbookOpen);
                // Dung 2010.05.18 Add End
                ExcelWkbks = ExcelAp.Workbooks;

                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                Excelbk = ExcelWkbks.Open(fileName, 3, false, 3, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                                    ";", true, true, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                ActiveSheet = (Worksheet)Excelbk.ActiveSheet;

                if (!allowVisible)
                {
                    ExcelAp.ScreenUpdating = true;
                    Excelbk.Saved = true;

                    //ExcelAp.DisplayAlerts = false;
                }

                //Get RowCount and ColumnCount
                Range allCells = ActiveSheet.get_Range("A1", "Z500");
                usedRange = GetUsedRange(allCells);
                //range = ActiveSheet.UsedRange;

                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Init new excel file
        /// </summary>
        /// <param name="fileName">filename to save</param>        
        /// <returns>True : init succesfull</returns>
        public bool CreateNewObject(string fileName)
        {
            try
            {
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }

                string path = System.IO.Path.GetDirectoryName(fileName);
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                // Create the Excel Application object
                ApplicationClass excelApp = new ApplicationClass();

                // Create a new Excel Workbook
                Workbook excelWorkbook = excelApp.Workbooks.Add(Type.Missing);

                // Save and Close the Workbook
                excelWorkbook.SaveAs(fileName, XlFileFormat.xlWorkbookNormal,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    XlSaveAsAccessMode.xlExclusive,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing);

                excelWorkbook.Close(true, Type.Missing, Type.Missing);
                excelWorkbook = null;
                _filedestination = fileName;
                // Release the Application object
                excelApp.Quit();
                excelApp = null;

                // Collect the unreferenced objects
                GC.Collect();
                GC.WaitForPendingFinalizers();


                ExcelAp = new ApplicationClass();
                ExcelWkbks = ExcelAp.Workbooks;

                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                Excelbk = ExcelWkbks.Open(fileName, 3, false, 3, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                                    ";", false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                ActiveSheet = (Worksheet)Excelbk.ActiveSheet;

                ExcelAp.ScreenUpdating = true;
                Excelbk.Saved = true;

                ExcelAp.DisplayAlerts = false;

                //Get RowCount and ColumnCount
                Range allCells = ActiveSheet.get_Range("A1", "Z500");
                usedRange = GetUsedRange(allCells);
                //range = ActiveSheet.UsedRange;

                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch
            {
                return false;
            }
            return true;
        }

        // Dung 2010.05.18 Add Start       
        /// <summary>
        /// When Excel is open
        /// </summary>
        /// <param name="Wb"></param>
        private void ExcelAp_WorkbookOpen(Workbook Wb)
        {
            if (OnOpen != null) OnOpen(Wb, EventArgs.Empty);
        }

        /// <summary>
        /// When Excel close
        /// </summary>
        /// <param name="Wb"></param>
        /// <param name="Cancel"></param>                 
        private void ExcelAp_WorkbookBeforeSave(Workbook Wb, bool SaveAsUI, ref bool Cancel)
        {
            if (OnBeforeSave != null)
            {
                OnBeforeSave(Wb, EventArgs.Empty);
            }
        }

        /// <summary>
        /// When Excel save
        /// </summary>
        /// <param name="Wb"></param>
        /// <param name="SaveAsUI"></param>
        /// <param name="Cancel"></param>
        private void ExcelAp_WorkbookBeforeClose(Workbook Wb, ref bool Cancel)
        {
            if (OnQuit != null) OnQuit(Wb, EventArgs.Empty);
        }
        // Dung 2010.05.18 Add End

        // Dung 2010.04.26 Add Start
        /// <summary>
        /// Funtion write content into sheet input
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileTemplate"></param>
        /// <returns></returns>
        public bool WriteOnSheet(string strFilePath, int iSheet)
        {
            try
            {
                ExcelAp = new ApplicationClass();
                ExcelWkbks = ExcelAp.Workbooks;

                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                Excelbk = ExcelWkbks.Open(strFilePath, 3, false, 3, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                                    ";", false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                ActiveSheet = (Worksheet)Excelbk.Sheets[iSheet];

                ExcelAp.ScreenUpdating = true;
                Excelbk.Saved = true;

                ExcelAp.DisplayAlerts = false;

                //Get RowCount and ColumnCount
                Range allCells = ActiveSheet.get_Range("A1", "Z500");

                usedRange = GetUsedRange(allCells);
                //range = ActiveSheet.UsedRange;

                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Function set name for sheet
        /// </summary>
        /// <param name="strSheetName"></param>
        public void SetSheetName(string strSheetName)
        {
            ActiveSheet.Name = strSheetName.Trim();
        }

        /// <summary>
        /// Function set name for sheet
        /// </summary>
        /// <param name="strSheetName"></param>
        public void SetActiveSheet(int SheetIdx)
        {
            ActiveSheet = (Worksheet)Excelbk.Sheets[SheetIdx];
            ActiveSheet.Select(Type.Missing);
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        public void AddNewSheet()
        {
            Excelbk.Sheets.Add();
        }
        /// <summary>
        /// Save as excel file
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public void SaveAs(string strFilePath)
        {
            try
            {
                string path = Path.GetDirectoryName(strFilePath);
                ExcelAp.DisplayAlerts = false;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Excelbk.SaveAs(strFilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange,
                                                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch
            {
            }
        }

        // Dung 2010.04.26 Add End

        // Dung 2010.05.11 Add Start

        /// <summary>
        /// Funtion copy sheet active
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileTemplate"></param>
        /// <returns></returns>
        public bool CopySheet()
        {
            try
            {
                ActiveSheet = (Worksheet)Excelbk.Sheets[1];
                ActiveSheet.Copy(ActiveSheet, Type.Missing);
                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Funtion copy sheet index
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileTemplate"></param>
        /// <returns></returns>
        public bool CopySheet(int index)
        {
            try
            {
                ActiveSheet = (Worksheet)Excelbk.Sheets[index];
                ActiveSheet.Copy(ActiveSheet, Type.Missing);
                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Funtion delete sheet active
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileTemplate"></param>
        /// <returns></returns>
        public bool DeleteSheet()
        {
            try
            {
                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                ActiveSheet = (Worksheet)Excelbk.ActiveSheet;
                ExcelAp.DisplayAlerts = false;
                ActiveSheet.Delete();
                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Funtion delete sheet index
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileTemplate"></param>
        /// <returns></returns>
        public bool DeleteSheet(int index)
        {
            try
            {
                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                ActiveSheet = (Worksheet)Excelbk.Sheets[index];
                ExcelAp.DisplayAlerts = false;
                ActiveSheet.Delete();
                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch
            {
                return false;
            }
            return true;
        }
        // Dung 2010.05.11 Add End                

        /// <summary>
        /// Finish write data to excel
        /// </summary>
        public void End_Write()
        {
            try
            {
                if (!allowVisible)
                {
                    m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                    Excelbk.Saved = true;
                    ExcelAp.DisplayAlerts = false;

                    try
                    {
                        Excelbk.SaveAs(_filedestination, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange,
                                                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }
                    catch
                    {
                        // File.Delete(_filedestination);
                    }
                    //Excelbk.Close(false, _filedestination, Type.Missing);
                    ExcelWkbks.Close();
                    ExcelAp.Quit();
                    //ExcelAp.Workbooks.Close();
                    releaseNativeReference(usedRange);
                    releaseNativeReference(ActiveSheet);
                    releaseNativeReference(Excelbk);
                    foreach (Workbook book in ExcelWkbks)
                    {
                        book.Close(false, _filedestination, Type.Missing);
                        releaseNativeReference(book);
                    }
                    releaseNativeReference(ExcelWkbks);
                    releaseNativeReference(ExcelAp);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
                }
                else
                {
                    ExcelAp.Visible = true;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Write value to specified cell based on row index and column index
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <param name="colIndex">Column index</param>
        /// <param name="value"></param>
        public void SetValue(int rowIndex, int colIndex, object value)
        {
            if (value != null && value is string)
            {
                value = value.ToString().Replace(Environment.NewLine, '\n'.ToString());
            }
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range objrange = get_Range(rowIndex, colIndex);
            objrange.Value2 = value;
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }
        //2012/8/20 Chien Add Start
        /// <summary>
        /// Convert Value to input string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <param name="value"></param>
        public void SetValue(int rowIndex, int colIndex, string key, object value)
        {
            if (value != null && value is string)
            {
                value = value.ToString().Replace(Environment.NewLine, '\n'.ToString());
            }
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range objrange = get_Range(rowIndex, colIndex);
            objrange.Value2 = key;
            objrange.Replace(key, value, XlLookAt.xlPart, XlSearchOrder.xlByRows, false, false, false, false);
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }
        //2012/8/20 Chien Add End
        //2013/05/15 Chien Add Start
        public void SetCellCustom(int rowIndex, int colIndex, string value)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range objrange = get_Range(rowIndex, colIndex);
            objrange.NumberFormat = value;
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }
        //2013/05/15 Chien Add End
        //2011.05.13 Thanh add start
        /// <summary>
        /// clear format range
        /// </summary>
        /// <param name="iRow"></param>
        /// <param name="iColumn"></param>
        public void formatCell(int iRow, int iColumn)
        {
            try
            {
                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Range range = get_Range(iRow, iColumn);
                range.Clear();

                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //2011.05.13 Thanh add end
        /// <summary>
        /// Copy format cell
        /// </summary>
        /// <param name="iRow"></param>
        /// <param name="iColumn"></param>
        public void CopyformatCell(int rowc, int colc, int rowp, int colp)
        {
            try
            {
                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Range range = get_Range(rowc, colc);
                range.Copy(range);

                Range range2 = get_Range(rowp, colp);

                range2.PasteSpecial(XlPasteType.xlPasteFormats, XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Write value to specified cell based on keyword
        /// </summary>
        public void SetValue(string key, object value2, bool matchCase)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            if (value2 != null && value2 is string)
            {
                value2 = value2.ToString().Replace(Environment.NewLine, '\n'.ToString());
            }
            //Get RowCount and ColumnCount
            Range allCells = ActiveSheet.get_Range("A1", "Z500");
            usedRange = GetUsedRange(allCells);
            bool displayAlert = ExcelAp.DisplayAlerts;
            ExcelAp.DisplayAlerts = false;
            if (usedRange == null) return;
            try
            {
                usedRange.Replace(string.Format("[{0}]", key), value2, XlLookAt.xlPart, XlSearchOrder.xlByRows, false, false, false, false);
            }
            catch { }
            finally
            {
                ExcelAp.DisplayAlerts = displayAlert;
                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
        }
        //2011.12.12 thanh add start-> create hyperlink open a sheet
        public void setHyperlink(int row, int col, string value, string sheetName)
        {
            if (ActiveSheet != null)
            {
                try
                {
                    m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                    string address = "[ExportSearchHotel.xls]" + sheetName + "!A1";
                    //ActiveSheet.Hyperlinks.Add(ActiveSheet.get_Range("A1", oMissing), address, oMissing, "Tip", value);

                    Range rangeToHoldHyperlink = get_Range(row, col);
                    string hyperlinkTargetAddress = sheetName + "!A1";

                    ActiveSheet.Hyperlinks.Add(
                        rangeToHoldHyperlink,
                        string.Empty,
                        hyperlinkTargetAddress,
                        "Screen Tip Text",
                        value);

                    Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
                }
                catch { }
            }
        }
        //2011.12.12 thanh add end
        public void setHyperlink(int row, int col, string value)
        {
            if (ActiveSheet != null)
            {
                try
                {
                    m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                    //ActiveSheet.Hyperlinks.Add(ActiveSheet.get_Range("A1", oMissing), address, oMissing, "Tip", value);

                    Range rangeToHoldHyperlink = get_Range(row, col);
                    string address = value;
                    if (!address.Contains("http"))
                    {
                        address = "http://" + address;
                    }

                    ActiveSheet.Hyperlinks.Add(
                        rangeToHoldHyperlink,
                       address,
                        "",
                       value,
                        value);

                    Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
                }
                catch { }
            }
        }
        /// <summary>
        /// Write value with lenght > 250 character
        /// </summary>
        public void SetValue(string key, object value2)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            if (value2 != null && value2 is string)
            {
                value2 = value2.ToString().Replace(Environment.NewLine, '\n'.ToString());
            }
            Range rangeBookmark = FindRange(key);

            bool displayAlert = ExcelAp.DisplayAlerts;
            ExcelAp.DisplayAlerts = false;
            if (rangeBookmark == null) return;
            try
            {
                int row = rangeBookmark.Row;
                int col = rangeBookmark.Column;

                SetValue(row, col, "");
                SetValue(row, col, value2);
            }
            catch { }
            finally
            {
                ExcelAp.DisplayAlerts = displayAlert;
            }
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        /// <summary>
        /// Add hyperlink in sheet
        /// </summary>
        public void AddHyperlink(int rowIndex, int colIndex, string link)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            bool displayAlert = ExcelAp.DisplayAlerts;
            ExcelAp.DisplayAlerts = false;
            try
            {
                Range range = get_Range(rowIndex, colIndex);
                Hyperlink hyperlink = (Hyperlink)range.Hyperlinks.Add(range, link, Type.Missing, Type.Missing, Type.Missing);
            }
            catch { }
            finally
            {
                ExcelAp.DisplayAlerts = displayAlert;
            }
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        /// <summary>
        /// Add hyperlink in sheet
        /// </summary>
        public void AddHyperlink(int rowIndex, int colIndex, string filePath, string sheetName)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            bool displayAlert = ExcelAp.DisplayAlerts;
            ExcelAp.DisplayAlerts = false;
            try
            {
                Range range = get_Range(rowIndex, colIndex);
                string link = filePath + "#'" + sheetName + "'!A1";
                Hyperlink hyperlink = (Hyperlink)range.Hyperlinks.Add(range, link, Type.Missing, Type.Missing, Type.Missing);
            }
            catch
            {
            }
            finally
            {
                ExcelAp.DisplayAlerts = displayAlert;
            }
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        /// <summary>
        /// Add hyperlink in sheet
        /// </summary>
        public void AddHyperlink(string key, string textDisplay, string link)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            bool displayAlert = ExcelAp.DisplayAlerts;
            ExcelAp.DisplayAlerts = false;
            try
            {
                Range range = FindRange(key);
                SetValue(range.Row, range.Column, textDisplay);
                Hyperlink hyperlink = (Hyperlink)range.Hyperlinks.Add(range, link, Type.Missing, Type.Missing, Type.Missing);
            }
            catch { }
            finally
            {
                ExcelAp.DisplayAlerts = displayAlert;
            }
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        /// <summary>
        /// Add hyperlink in sheet
        /// </summary>
        public void AddHyperlink(string key, string textDisplay, string filePath, string sheetName)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            bool displayAlert = ExcelAp.DisplayAlerts;
            ExcelAp.DisplayAlerts = false;
            try
            {
                Range range = FindRange(key);
                SetValue(range.Row, range.Column, textDisplay);
                string link = filePath + "#'" + sheetName + "'!A1";
                Hyperlink hyperlink = (Hyperlink)range.Hyperlinks.Add(range, link, Type.Missing, Type.Missing, Type.Missing);
            }
            catch { }
            finally
            {
                ExcelAp.DisplayAlerts = displayAlert;
            }
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        /// <summary>
        /// Find range based on keyword
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Range FindRange(string key)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range allCells = ActiveSheet.get_Range("A1", "Z500");
            usedRange = GetUsedRange(allCells);
            Range cells = (Range)usedRange.Cells[1, 1];
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture; //2014/03/19 Chien Add # 890
            if (cells == null) return null;
            return Find(usedRange, cells, key);
            //Thread.CurrentThread.CurrentCulture = m_CurrentCulture; //2014/03/19 Chien Del #890
        }

        /// <summary>
        /// Find key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Range FindKey(string key, object RowStart, object ColStart)
        {
            Range cells = (Range)usedRange.Cells[RowStart, ColStart];
            if (cells == null) return null;
            return Find(usedRange, cells, key);
        }

        /// <summary>
        /// Find a range after a range based on keyword
        /// </summary>
        /// <param name="range"></param>
        /// <param name="after"></param>
        /// <param name="findWhat"></param>
        /// <returns></returns>
        public Range Find(Range range, object after, object findWhat)
        {
            try
            {
                return range.Find(findWhat,
                                    after,
                                    XlFindLookIn.xlValues,
                                    XlLookAt.xlPart,
                                    XlSearchOrder.xlByColumns,
                                    XlSearchDirection.xlNext,
                                    false,
                                    false,
                                    false
                            );

            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get used rage of active work sheet
        /// </summary>
        /// <param name="allCells">The Range to get</param>
        /// <returns></returns>
        private Range GetUsedRange(Range allCells)
        {
            Range rangedCells = null;

            if (allCells != null)
            {
                Range lastCell = null;

                try
                {
                    // Get the last cell to find out the number of
                    // columns and rows
                    lastCell = allCells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);

                    // Get the last row and column information
                    //rowCount = lastCell.Row;
                    rowCount = lastCell.Row;
                    //columnCount = lastCell.Column;
                    columnCount = lastCell.Column;

                    lastCell = null;

                    if ((rowCount > 0) && (columnCount > 0))
                    {
                        // find the used range
                        // User range string should be something like "A1:D4" etc.
                        // To find the last cell index, we do the following thing.
                        string lastColumn = MapColIndexToColName(columnCount);
                        rangedCells = ActiveSheet.get_Range("A1", lastColumn + rowCount.ToString());
                    }
                }
                catch
                {
                    return null;
                }
            }
            return rangedCells;
        }

        /// <summary>
        /// Open result excel file on excel application
        /// </summary>
        public void ViewFile()
        {
            if (System.IO.File.Exists(_filedestination_temp))
            {
                System.IO.File.Delete(_filedestination_temp);
            }
            _filedestination_temp = "";
            System.Diagnostics.Process.Start(_filedestination);
        }

        // Dung 2010.04.26 Add Start
        /// <summary>
        /// Open result excel file on excel application
        /// </summary>
        public void ViewFileExport()
        {
            System.Diagnostics.Process.Start(_filedestination);
        }
        /// <summary>
        /// Delete temporary file
        /// </summary>
        public static void DeleteFileTmp()
        {
            try
            {
                if (System.IO.File.Exists(_filedestination_temp))
                {
                    System.IO.File.Delete(_filedestination_temp);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Copy file from source to destination
        /// </summary>
        private void copy(string fileSource, string fileDestination)
        {

            try
            {
                System.IO.File.Copy(fileSource, fileDestination, true);
                FileInfo file = new FileInfo(fileDestination);
                if (file.IsReadOnly)
                {
                    file.IsReadOnly = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Set column width
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="value"></param>
        public void SetWidth(int colIndex, int value)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            string colName = MapColIndexToColName(colIndex);
            Range objRange = get_Range(1, colIndex);
            objRange.ColumnWidth = value;
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        /// <summary>
        /// Set row height
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="value"></param>
        public void SetRowHeight(int rowIndex, int value)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range objRange = (Range)usedRange.Cells[rowIndex, 1];
            objRange.RowHeight = value;
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }
        /// <param name="value"></param>
        public void SetRowHeight(int rowIndex, double value)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range objRange = (Range)usedRange.Cells[rowIndex, 1];
            objRange.RowHeight = value;
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }
        /// <summary>
        /// Format text in column (Bold, Italic, Underline)
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="value"></param>
        public void SetValueWithFormat(int rowIndex, int colIndex, object value, bool Bold, bool Italic, bool Underline)
        {
            try
            {
                if (value != null && value is string)
                {
                    value = value.ToString().Replace(Environment.NewLine, '\n'.ToString());
                }
                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                Range objRange = get_Range(rowIndex, colIndex);
                objRange.Font.Bold = Bold;
                objRange.Font.Italic = Italic;
                objRange.Font.Underline = Underline;

                objRange.Value2 = value;
                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //dai add start 2011.4.14
        /// <summary>
        /// Format text in column (Bold, Italic, Underline)
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="value"></param>
        public void SetValueWithFormatText(int rowIndex, int colIndex, object value, bool Bold, bool Italic, bool Underline, int align)
        {
            try
            {
                if (align > 3 || align < 1)
                {
                    align = 1;
                }
                if (value != null && value is string)
                {
                    value = value.ToString().Replace(Environment.NewLine, '\n'.ToString());
                }
                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                Range objRange = get_Range(rowIndex, colIndex);
                objRange.Font.Bold = Bold;
                objRange.Font.Italic = Italic;
                objRange.Font.Underline = Underline;
                objRange.HorizontalAlignment = align;
                objRange.Value2 = value;
                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //2013.05.08 Tinnv add start
        public void setForeColor(string cell1, string cell2, System.Drawing.Color colorFill)
        {
            Range range = get_Range(cell1, cell2);
            if (range != null)
            {
                range.Font.Color = ColorTranslator.ToOle(colorFill);
            }
        }
        public void setForeColor(int rowIndex, int colIndex, System.Drawing.Color colorFill)
        {
            Range range = get_Range(rowIndex, colIndex);
            if (range != null)
            {
                range.Font.Color = ColorTranslator.ToOle(colorFill);
            }
        }
        /// <summary>
        /// Format text in column (Bold, Italic, Underline,Strikethrough)
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="value"></param>
        public void SetValueWithFormat(int rowIndex, int colIndex, object value, bool Bold, bool Italic, bool Underline, bool Strikethrough)
        {
            try
            {
                if (value != null && value is string)
                {
                    value = value.ToString().Replace(Environment.NewLine, '\n'.ToString());
                }
                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                Range objRange = get_Range(rowIndex, colIndex);
                objRange.Font.Bold = Bold;
                objRange.Font.Italic = Italic;
                objRange.Font.Underline = Underline;
                objRange.Font.Strikethrough = Strikethrough;
                objRange.Value2 = value;
                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SetPageBreak(int rowIndex)
        {
            Range objRange = (Range)ActiveSheet.Rows[rowIndex];
            objRange.PageBreak = -4135;
        }

        //dai add start
        public void setAlignAndValign(int rowIndex, int colIndex, int valign, int align)
        {
            if (align > 3 || align < 1)
            {
                align = 1;
            }
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Range objRange = get_Range(rowIndex, colIndex);
            objRange.HorizontalAlignment = align;
            objRange.VerticalAlignment = valign;


        }
        public void setAlignAndValign(int rowIndex, int colIndex, Microsoft.Office.Interop.Excel.XlHAlign halign, Microsoft.Office.Interop.Excel.XlVAlign valign)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Range objRange = get_Range(rowIndex, colIndex);
            objRange.HorizontalAlignment = halign;
            objRange.VerticalAlignment = valign;
        }
        public void setAlignAndValign(string cell1, string cell2, Microsoft.Office.Interop.Excel.XlHAlign halign, Microsoft.Office.Interop.Excel.XlVAlign valign)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Range objRange = get_Range(cell1, cell2);
            objRange.HorizontalAlignment = halign;
            objRange.VerticalAlignment = valign;
        }
        //dai add end
        /// <summary>
        /// Format text in column (Bold, Italic, Underline)
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="value"></param>
        public void SetValueWithFormatCell(int rowIndex, int colIndex, object value, bool Bold, bool Italic, bool Underline, object backcolor, object forecolor)
        {
            try
            {

                if (value != null && value is string)
                {
                    value = value.ToString().Replace(Environment.NewLine, '\n'.ToString());
                }
                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                Range objRange = get_Range(rowIndex, colIndex);
                objRange.Font.Bold = Bold;
                objRange.Font.Italic = Italic;
                objRange.Font.Underline = Underline;
                if (backcolor.GetType().Equals(typeof(Color)))
                {
                    objRange.Interior.Color = ColorTranslator.ToOle((Color)backcolor);
                }
                else
                {
                    if (backcolor.Equals(Color.Olive.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.Olive);
                    }
                    else if (backcolor.Equals(Color.MediumVioletRed.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.MediumVioletRed);
                    }
                    else if (backcolor.Equals(Color.Fuchsia.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.Fuchsia);
                    }
                    else if (backcolor.Equals(Color.Lime.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.Lime);
                    }
                    else if (backcolor.Equals(Color.LightBlue.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                    }
                    else if (backcolor.Equals(Color.Yellow.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    }
                    else if (backcolor.Equals(Color.Red.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.Red);// Color.Red.ToArgb();
                    }
                    else if (backcolor.Equals(Color.MidnightBlue.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.MidnightBlue);

                    }
                    else if (backcolor.Equals(Color.MidnightBlue.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.MidnightBlue);

                    }
                    else if (backcolor.Equals(Color.YellowGreen.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.YellowGreen);

                    }
                    else if (backcolor.Equals(Color.HotPink.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.HotPink);

                    }
                    else if (backcolor.Equals(Color.Wheat.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.Wheat);

                    }
                    else if (backcolor.Equals(Color.SlateGray.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.SlateGray);

                    }
                    else if (backcolor.Equals(Color.PaleGreen.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.PaleGreen);

                    }
                    else if (backcolor.Equals(Color.DeepSkyBlue.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.DeepSkyBlue);

                    }
                    else if (backcolor.Equals(Color.DarkGreen.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.DarkGreen);

                    }
                    else if (backcolor.Equals(Color.DarkOrange.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.DarkOrange);

                    }
                    else if (backcolor.Equals(Color.OliveDrab.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.OliveDrab);

                    }
                    else if (backcolor.Equals(Color.Blue.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.Blue);

                    }
                    else if (backcolor.Equals(Color.MediumSeaGreen.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.MediumSeaGreen);

                    }
                    else if (backcolor.Equals(Color.Indigo.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.Indigo);

                    }
                    else if (backcolor.Equals(Color.LightGray.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.LightGray);

                    }
                    else if (backcolor.Equals(Color.Black.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.Black);
                        //objRange.Font.Color  = ColorTranslator.ToOle(Color.White);
                    }
                    else if (backcolor.Equals(Color.PeachPuff.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.PeachPuff);
                    }
                    else if (backcolor.Equals(Color.Violet.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.Violet);
                    }

                    else if (backcolor.Equals(Color.LightCyan.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.LightCyan);
                    }
                    else if (backcolor.Equals(Color.RoyalBlue.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.RoyalBlue);
                    }
                    else if (backcolor.Equals(Color.Aquamarine.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.Aquamarine);
                    }
                    else if (backcolor.Equals(Color.Cyan.Name.ToString()))
                    {
                        objRange.Interior.Color = ColorTranslator.ToOle(Color.Cyan);
                    }
                }
                if (forecolor.GetType().Equals(typeof(Color)))
                {
                    objRange.Font.Color = ColorTranslator.ToOle((Color)forecolor);
                }
                else
                {
                    if (forecolor.Equals(Color.Olive.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.Olive);
                    }
                    else if (forecolor.Equals(Color.MediumVioletRed.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.MediumVioletRed);
                    }
                    else if (forecolor.Equals(Color.Fuchsia.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.Fuchsia);
                    }
                    else if (forecolor.Equals(Color.Lime.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.Lime);
                    }
                    else if (forecolor.Equals(Color.LightBlue.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.LightBlue);
                    }
                    else if (forecolor.Equals(Color.Yellow.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.Yellow);
                    }
                    else if (forecolor.Equals(Color.Red.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.Red);// Color.Red.ToArgb();
                    }
                    else if (forecolor.Equals(Color.MidnightBlue.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.MidnightBlue);

                    }
                    else if (forecolor.Equals(Color.MidnightBlue.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.MidnightBlue);

                    }
                    else if (forecolor.Equals(Color.YellowGreen.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.YellowGreen);

                    }
                    else if (forecolor.Equals(Color.HotPink.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.HotPink);

                    }
                    else if (forecolor.Equals(Color.Wheat.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.Wheat);

                    }
                    else if (forecolor.Equals(Color.SlateGray.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.SlateGray);

                    }
                    else if (forecolor.Equals(Color.PaleGreen.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.PaleGreen);

                    }
                    else if (forecolor.Equals(Color.DeepSkyBlue.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.DeepSkyBlue);

                    }
                    else if (forecolor.Equals(Color.DarkGreen.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.DarkGreen);

                    }
                    else if (forecolor.Equals(Color.DarkOrange.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.DarkOrange);

                    }
                    else if (forecolor.Equals(Color.OliveDrab.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.OliveDrab);

                    }
                    else if (forecolor.Equals(Color.Blue.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.Blue);

                    }
                    else if (forecolor.Equals(Color.MediumSeaGreen.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.MediumSeaGreen);

                    }
                    else if (forecolor.Equals(Color.Indigo.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.Indigo);

                    }
                    else if (forecolor.Equals(Color.LightGray.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.LightGray);

                    }
                    else if (forecolor.Equals(Color.Black.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.Black);


                    }
                    else if (forecolor.Equals(Color.White.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.White);

                    }
                    else if (forecolor.Equals(Color.Green.Name.ToString()))
                    {
                        objRange.Font.Color = ColorTranslator.ToOle(Color.Green);
                    }
                }
                objRange.Value2 = value;
                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //dai add end

        private char[] chars = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        /// <summary>
        /// Mapping column from index to name
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public string MapColIndexToColName(int index)
        {
            //int ascii = idx + 64;
            //if (ascii > 90)
            //{
            //    int other = ascii - 90 + 64;
            //    return "A" + char.ConvertFromUtf32(other);
            //}
            //return char.ConvertFromUtf32(idx + 64);

            index -= 1; //adjust so it matches 0-indexed array rather than 1-indexed column

            int quotient = index / 26;
            if (quotient > 0)
                return MapColIndexToColName(quotient) + chars[index % 26].ToString();
            else
                return chars[index % 26].ToString();
        }

        /// <summary>
        /// Release COM object
        /// </summary>
        /// <param name="o"></param>
        private void releaseNativeReference(object o)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            catch { ;}
            finally
            {
                o = null;
            }
        }

        /// <summary>
        /// Get range based on row index and column index
        /// </summary>
        private Range get_Range(int rowIndex, int colIndex)
        {
            try
            {
                return (Range)ActiveSheet.Cells[rowIndex, colIndex];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get range based on first cell and last cell
        /// </summary>
        private Range get_Range(string cell1, string cell2)
        {
            try
            {
                return ActiveSheet.get_Range(cell1, cell2);
            }
            catch
            {
                return null;
            }
        }

        public bool CopyRange(string cell11, string cell12, string cell21, string cell22)
        {
            try
            {
                Range range1 = get_Range(int.Parse(cell11), int.Parse(cell12));
                if (range1 != null)
                {
                    Range range2 = get_Range(int.Parse(cell21), int.Parse(cell22));
                    range1.Copy(range2);
                    return true;

                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// Border range from cell1 to cell2
        /// </summary>
        /// <param name="cell1"></param>
        /// <param name="cell2"></param>
        public void Border_Range(string cell1, string cell2, System.Drawing.Color color)
        {
            Range range = get_Range(cell1, cell2);
            if (range != null)
            {
                Microsoft.Office.Interop.Excel.Borders boders = range.Borders;//[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical];
                boders.LineStyle = XlLineStyle.xlContinuous;
                boders.Weight = XlBorderWeight.xlThin;
                boders.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                boders.Color = color.ToArgb();
            }
        }

        /// <summary>
        /// Set properties of font object
        /// </summary>
        public void SetFont(string cell1, string cell2, System.Drawing.Font font)
        {
            Range range = get_Range(cell1, cell2);
            if (range != null && UsedRange.Font != null)
            {
                UsedRange.Font.Strikethrough = font.Strikeout;
                UsedRange.Font.Size = font.Size;
                UsedRange.Font.Italic = font.Italic;
                UsedRange.Font.Bold = font.Bold;
                UsedRange.Font.Name = font.Name;
                UsedRange.Font.Underline = font.Underline;
            }
        }

        /// <summary>
        /// Set properties of font object
        /// </summary>
        public void SetFont(string cell1, string cell2, float size)
        {
            Range range = get_Range(cell1, cell2);
            if (range != null && range.Font != null)
            {
                range.Font.Size = size;

            }
        }

        /// <summary>
        /// Set wordwrap text property of spcecified cell based on cell1, cell2
        /// </summary>
        /// <param name="cell1"></param>
        /// <param name="cell2"></param>
        /// <param name="wr"></param>
        public void SetWrapText(string cell1, string cell2, bool wr)
        {
            Range range = get_Range(cell1, cell2);
            if (range != null)
            {
                range.WrapText = wr;
            }
        }

        /// <summary>
        /// Insert new row based on row index and row count
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="countRow"></param>
        public void Insert(int rowIndex, int rowCount)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Range objRange = (Range)ActiveSheet.Rows[rowIndex, Type.Missing];
            for (int i = 0; i < rowCount; i++)
            {
                objRange.Insert(Type.Missing, 1);
            }

            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        /// <summary>
        /// Insert new row based on row index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="countRow"></param>
        public void InsertRow(int rowIndex)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Range objRange = (Range)ActiveSheet.Rows[rowIndex, Type.Missing];
            objRange.Insert(Type.Missing, true);

            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        /// <summary>
        /// Insert new col based on col index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="countRow"></param>
        public void InsertCol(int colIndex)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Range objRange = (Range)ActiveSheet.Columns[Type.Missing, colIndex];
            objRange.Insert(Type.Missing, true);

            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        /// <summary>
        /// Delete row based on row index and row count
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="countRow"></param>
        public void DeleteRow(int rowIndex)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Range objRange = (Range)ActiveSheet.Rows[rowIndex, Type.Missing];
            objRange.Delete(Type.Missing);

            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        /// <summary>
        /// Delete cold based on col index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="countRow"></param>
        public void DeleteCol(int colIndex)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Range objRange = (Range)ActiveSheet.Rows[Type.Missing, colIndex];
            objRange.Delete(Type.Missing);

            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        /// <summary>
        /// Insert new column based on column index and column count
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="countRow"></param>
        public void InsertColumn(int colIndex)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Range objRange = (Range)ActiveSheet.Columns[colIndex, Type.Missing];
            objRange.Insert(XlInsertShiftDirection.xlShiftToRight, true);

            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;

        }

        /// <summary>
        /// Insert new column based on column index and column count
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="countRow"></param>
        public void Merge(string cell1, string cell2)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Range objRange = get_Range(cell1, cell2);
            objRange.Merge(Type.Missing);

            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }


        //2011.08.17 Thanh add start        
        public string GetColumnName(int col)
        {
            string column = null;
            int mode = 0;
            while ((col > 0))
            {
                mode = (col - 1) % 26;
                column = Convert.ToChar((65 + mode)) + column;
                col = (int)((col - mode) / 26);
            }
            return column;
        }
        //2011.08.17 Thanh add end
        /// <summary>
        /// Delete column based on column index and column count
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="countRow"></param>
        public void DeleteColumn(int colIndex)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Range objRange = (Range)ActiveSheet.Columns[colIndex, Type.Missing];
            objRange.Delete(XlInsertShiftDirection.xlShiftToRight);

            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        /// <summary>
        /// Set color for specified range
        /// </summary>
        public void SetColor(int rowIndex, int colIndex, System.Drawing.Color color)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Range objRange = get_Range(rowIndex, colIndex);
            objRange.Interior.Color = color.ToArgb();

            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        // Dung 2010.04.26 Add Start
        /// <summary>
        /// Set properties of color object
        /// </summary>
        public void SetColor(string cell1, string cell2, System.Drawing.Color colorFill, System.Drawing.Color colorBorder)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range range = get_Range(cell1, cell2);
            if (range != null)
            {
                range.Interior.Color = colorFill.ToArgb();
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;
                range.Borders.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                range.Borders.Color = colorBorder.ToArgb();
            }
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }
        // Dung 2010.04.26 Add End
        //dai add start 2011.4.26
        public void SetBackgroundColor(int cell1, int cell2, System.Drawing.Color color)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range range = get_Range(cell1, cell2);
            if (range != null)
            {

                range.Interior.Color = ColorTranslator.ToOle(color);

            }
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }
        //2013.10.21 Tinnv add start
        public void SetBackgroundColor(string cell1, string cell2, System.Drawing.Color color)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range range = get_Range(cell1, cell2);
            if (range != null)
            {

                range.Interior.Color = ColorTranslator.ToOle(color);

            }
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }
        //2013.10.21 Tinnv add end
        public void SetBorderColor(int cell1, int cell2, System.Drawing.Color colorBorder)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range range = get_Range(cell1, cell2);
            if (range != null)
            {
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;
                range.Borders.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                range.Borders.Color = ColorTranslator.ToOle(colorBorder);
                range.WrapText = true;
            }
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }
        public void SetComment(int cell1, int cell2, string comment)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range range = get_Range(cell1, cell2);
            if (range != null)
            {
                if (comment != string.Empty)
                {
                    range.AddComment(comment);
                }
                else
                {
                    range.ClearComments();
                }
            }
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }
        //dai add end
        //Cuong 2010.08.25 add start
        /// <summary>
        /// Set properties of color object
        /// </summary>
        public void SetColor(string cell1, string cell2, System.Drawing.Color colorFill)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range range = get_Range(cell1, cell2);
            if (range != null)
            {
                range.Interior.Color = ColorTranslator.ToOle(colorFill);
            }
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }
        //Cuong 2010.08.25 add end

        //2010.04.28 Khoan add start
        /// <summary>
        /// Convert Excel to PDF 
        /// </summary>
        /// <param name="sourceBookPath"></param>
        /// <param name="targetFilePath"></param>
        /// <param name="targetFormat"></param>
        public static void ConvertWorkbookToPDF(string sourceBookPath, string targetFilePath)
        {
            // Make sure the source document exists.
            if (!System.IO.File.Exists(sourceBookPath))
                throw new Exception("The specified source workbook does not exist.");

            object targetFormat = 0;

            // Create an instance of the Excel ApplicationClass object.          
            ApplicationClass excelApplication = new ApplicationClass();

            // Declare a variable to hold the reference to the workbook.
            Workbook excelWorkBook = null;

            // Declare variables for the Workbooks.Open and ApplicationClass.Quit method parameters. 
            string paramSourceBookPath = sourceBookPath;
            object paramMissing = Type.Missing;

            // Declare variables for the Workbook.ExportAsFixedFormat method parameters.
            //string paramExportFilePath = @"D:\Dev\Work\Akona\How To's\HowTos\Excel\ConvertingSheetToPDFXPS\test.pdf";
            //XlFixedFormatType paramExportFormat = XlFixedFormatType.xlTypePDF;

            // To save the file in XPS format using the following for the paramExportFilePath
            // and paramExportFormat variables:
            //
            string paramExportFilePath = targetFilePath;
            object paramExportFormat = targetFormat;

            XlFixedFormatQuality paramExportQuality = XlFixedFormatQuality.xlQualityStandard;
            bool paramOpenAfterPublish = false;
            bool paramIncludeDocProps = true;
            bool paramIgnorePrintAreas = true;
            object paramFromPage = Type.Missing;
            object paramToPage = Type.Missing;
            CultureInfo m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            try
            {
                // Open the source workbook.
                excelWorkBook = excelApplication.Workbooks.Open(paramSourceBookPath, paramMissing, paramMissing, paramMissing,
                    paramMissing, paramMissing, paramMissing, paramMissing, paramMissing, paramMissing,
                    paramMissing, paramMissing, paramMissing, paramMissing, paramMissing);

                // Save it in the target format.
                if (excelWorkBook != null)
                {
                    //excelWorkBook.ExportAsFixedFormat(targetFormat, targetFilePath, paramExportQuality, paramIncludeDocProps,
                    //    paramIgnorePrintAreas, paramFromPage, paramToPage, paramOpenAfterPublish, paramMissing);
                    object[] m_params = new object[]{targetFormat, targetFilePath, paramExportQuality, paramIncludeDocProps,
                        paramIgnorePrintAreas, paramFromPage, paramToPage, paramOpenAfterPublish, paramMissing};
                    excelWorkBook.GetType().InvokeMember("ExportAsFixedFormat", BindingFlags.InvokeMethod, null, excelWorkBook, m_params);
                }
            }
            catch (Exception ex)
            {
                // Respond to the error.
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Close the workbook object.
                if (excelWorkBook != null)
                {
                    excelWorkBook.Close(false, paramMissing, paramMissing);
                    excelWorkBook = null;
                }

                // Close the ApplicationClass object.
                if (excelApplication != null)
                {
                    excelApplication.Quit();
                    excelApplication = null;
                }

                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        //2010.04.28 Khoan add end

        public static SD.DataTable GetDataTableByExcelSheet(string pathFile, string sheetName)
        {
            OleDbConnection connect = new OleDbConnection();
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            connect = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathFile + "';Extended Properties=Excel 8.0;");
            adapter = new OleDbDataAdapter("select * from [" + sheetName + "$]", connect);
            adapter.TableMappings.Add("Table", sheetName);
            SD.DataTable tbl = new SD.DataTable();
            adapter.Fill(tbl);
            adapter.Dispose();
            connect.Close();
            return tbl;
        }

        public static void InsertDataToExcel(string pathFile, string sheetName, SD.DataTable tbl)
        {
            System.Data.OleDb.OleDbConnection MyConnection;

            string sql = null;
            MyConnection = new System.Data.OleDb.OleDbConnection(string.Format("provider=Microsoft.Jet.OLEDB.4.0;Data Source='{0}';Extended Properties=Excel 8.0;", pathFile));
            try
            {
                MyConnection.Open();

                foreach (SD.DataRow item in tbl.Rows)
                {
                    string s = "";
                    System.Collections.ArrayList arr = new System.Collections.ArrayList();
                    foreach (SD.DataColumn col in tbl.Columns)
                    {
                        s += "?,";
                        arr.Add(item[col]);
                    }
                    if (s != "") s = s.Substring(0, s.Length - 1);
                    sql = string.Format("Insert into [{0}$] values({1})", sheetName, s);
                    System.Data.OleDb.OleDbCommand myCommand = CreateCommand(arr.ToArray());

                    myCommand.CommandText = sql;
                    myCommand.Connection = MyConnection;
                    myCommand.ExecuteNonQuery();
                }
                MyConnection.Close();
                MyConnection.Dispose();
            }
            catch (Exception)
            {

            }
            finally
            {
                if (MyConnection != null) MyConnection.Close();
            }
        }
        //thanh add start
        public void setBackColor(int rowIndex, int colIndex, object backcolor)
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Range objRange = get_Range(rowIndex, colIndex);
            Color fc = Color.White;
            try
            {
                ColorConverter ccon = new ColorConverter();
                if (backcolor.GetType().Equals(typeof(Color)))
                {
                    fc = (Color)backcolor;
                }
                else
                {
                    if (backcolor.ToString() != "0" && backcolor.ToString() != string.Empty)
                    {
                        fc = (Color)ccon.ConvertFromString(backcolor.ToString());
                    }
                }
            }
            catch
            {
                //just	leave	it	default	on	fail	
            }
            objRange.Interior.Color = ColorTranslator.ToOle(fc);
            Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }
        public void SetValueWithFormatCell(int rowIndex, int colIndex, object value, bool Bold, bool Italic, bool Underline, bool Strikeout, object backcolor, object forecolor)
        {
            try
            {
                if (value != null && value is string)
                {
                    value = value.ToString().Replace(Environment.NewLine, '\n'.ToString());
                }
                m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                Range objRange = get_Range(rowIndex, colIndex);
                objRange.Font.Bold = Bold;
                objRange.Font.Italic = Italic;
                objRange.Font.Underline = Underline;
                objRange.Font.Strikethrough = Strikeout;
                //dai modify 
                Color fc = Color.White;
                try
                {
                    ColorConverter ccon = new ColorConverter();
                    if (backcolor.GetType().Equals(typeof(Color)))
                    {
                        fc = (Color)backcolor;
                    }
                    else
                    {
                        if (backcolor.ToString() != "0" && backcolor.ToString() != string.Empty)
                        {
                            fc = (Color)ccon.ConvertFromString(backcolor.ToString());
                        }
                    }
                }
                catch
                {
                    //just	leave	it	default	on	fail	
                }
                objRange.Interior.Color = ColorTranslator.ToOle(fc);
                Color fc2 = Color.Black;
                try
                {
                    ColorConverter ccon = new ColorConverter();
                    if (forecolor.GetType().Equals(typeof(Color)))
                    {
                        fc2 = (Color)forecolor;
                    }
                    else
                    {
                        fc2 = (Color)ccon.ConvertFromString(forecolor.ToString());
                    }
                }
                catch
                {
                    //just	leave	it	default	on	fail	
                }
                objRange.Font.Color = ColorTranslator.ToOle(fc2);

                //if (backcolor.Equals(Color.Olive.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.Olive);
                //}
                //else if (backcolor.Equals(Color.MediumVioletRed.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.MediumVioletRed);
                //}
                //else if (backcolor.Equals(Color.Fuchsia.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.Fuchsia);
                //}
                //else if (backcolor.Equals(Color.Lime.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.Lime);
                //}
                //else if (backcolor.Equals(Color.LightBlue.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                //}
                //else if (backcolor.Equals(Color.Yellow.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                //}
                //else if (backcolor.Equals(Color.Red.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.Red);// Color.Red.ToArgb();
                //}
                //else if (backcolor.Equals(Color.MidnightBlue.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.MidnightBlue);

                //}
                //else if (backcolor.Equals(Color.MidnightBlue.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.MidnightBlue);

                //}
                //else if (backcolor.Equals(Color.YellowGreen.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.YellowGreen);

                //}
                //else if (backcolor.Equals(Color.HotPink.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.HotPink);

                //}
                //else if (backcolor.Equals(Color.Wheat.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.Wheat);

                //}
                //else if (backcolor.Equals(Color.SlateGray.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.SlateGray);

                //}
                //else if (backcolor.Equals(Color.PaleGreen.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.PaleGreen);

                //}
                //else if (backcolor.Equals(Color.DeepSkyBlue.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.DeepSkyBlue);

                //}
                //else if (backcolor.Equals(Color.DarkGreen.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.DarkGreen);

                //}
                //else if (backcolor.Equals(Color.DarkOrange.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.DarkOrange);

                //}
                //else if (backcolor.Equals(Color.OliveDrab.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.OliveDrab);

                //}
                //else if (backcolor.Equals(Color.Blue.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.Blue);

                //}
                //else if (backcolor.Equals(Color.MediumSeaGreen.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.MediumSeaGreen);

                //}
                //else if (backcolor.Equals(Color.Indigo.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.Indigo);

                //}
                //else if (backcolor.Equals(Color.LightGray.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.LightGray);

                //}
                //else if (backcolor.Equals(Color.Black.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.Black);
                //    //objRange.Font.Color  = ColorTranslator.ToOle(Color.White);
                //}
                //else if (backcolor.Equals(Color.PeachPuff.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.PeachPuff);
                //}
                //else if (backcolor.Equals(Color.Violet.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.Violet);
                //}

                //else if (backcolor.Equals(Color.LightCyan.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.LightCyan);
                //}
                //else if (backcolor.Equals(Color.RoyalBlue.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.RoyalBlue);
                //}
                //else if (backcolor.Equals(Color.Aquamarine.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.Aquamarine);
                //}
                //else if (backcolor.Equals(Color.Cyan.Name.ToString()))
                //{
                //    objRange.Interior.Color = ColorTranslator.ToOle(Color.Cyan);
                //}
                /////
                //if (forecolor.Equals(Color.Olive.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.Olive);
                //}
                //else if (forecolor.Equals(Color.MediumVioletRed.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.MediumVioletRed);
                //}
                //else if (forecolor.Equals(Color.Fuchsia.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.Fuchsia);
                //}
                //else if (forecolor.Equals(Color.Lime.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.Lime);
                //}
                //else if (forecolor.Equals(Color.LightBlue.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.LightBlue);
                //}
                //else if (forecolor.Equals(Color.Yellow.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.Yellow);
                //}
                //else if (forecolor.Equals(Color.Red.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.Red);// Color.Red.ToArgb();
                //}
                //else if (forecolor.Equals(Color.MidnightBlue.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.MidnightBlue);

                //}
                //else if (forecolor.Equals(Color.MidnightBlue.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.MidnightBlue);

                //}
                //else if (forecolor.Equals(Color.YellowGreen.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.YellowGreen);

                //}
                //else if (forecolor.Equals(Color.HotPink.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.HotPink);

                //}
                //else if (forecolor.Equals(Color.Wheat.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.Wheat);

                //}
                //else if (forecolor.Equals(Color.SlateGray.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.SlateGray);

                //}
                //else if (forecolor.Equals(Color.PaleGreen.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.PaleGreen);

                //}
                //else if (forecolor.Equals(Color.DeepSkyBlue.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.DeepSkyBlue);

                //}
                //else if (forecolor.Equals(Color.DarkGreen.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.DarkGreen);

                //}
                //else if (forecolor.Equals(Color.DarkOrange.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.DarkOrange);

                //}
                //else if (forecolor.Equals(Color.OliveDrab.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.OliveDrab);

                //}
                //else if (forecolor.Equals(Color.Blue.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.Blue);

                //}
                //else if (forecolor.Equals(Color.MediumSeaGreen.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.MediumSeaGreen);

                //}
                //else if (forecolor.Equals(Color.Indigo.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.Indigo);

                //}
                //else if (forecolor.Equals(Color.LightGray.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.LightGray);

                //}
                //else if (forecolor.Equals(Color.Black.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.Black);


                //}
                //else if (forecolor.Equals(Color.White.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.White);

                //}
                //else if (forecolor.Equals(Color.Green.Name.ToString()))
                //{
                //    objRange.Font.Color = ColorTranslator.ToOle(Color.Green);
                //}
                objRange.Value2 = value;
                Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Thanh add end
        /// <summary>
        /// This method retrieves the excel sheet names from 
        /// an excel workbook.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <returns>String[]</returns>
        public String[] GetExcelSheetNames(string excelFile)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {
                // Connection String. Change the excel file to the file you
                // will search.
                String connString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                  "Data Source=" + excelFile + ";Extended Properties=Excel 8.0;";
                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
                // Get the data table containg the schema guid.
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;

                // Add the sheet name to the string array.
                foreach (SD.DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }

                // Loop through all of the sheets if you want too...
                for (int j = 0; j < excelSheets.Length; j++)
                {
                    // Query each excel sheet.
                }

                return excelSheets;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }

        /// <summary>
        /// Create SqlCommand based on inputted parameters
        /// </summary>
        /// <param name="lstData"></param>
        /// <returns></returns>
        private static OleDbCommand CreateCommand(object[] lstData)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (lstData != null)
            {
                for (int i = 0; i < lstData.Length; i++)
                {
                    OleDbParameter param = new OleDbParameter();
                    param.Value = lstData[i];
                    param.ParameterName = string.Format("Param{0}", i + 1);
                    cmd.Parameters.Add(param);
                }
            }
            return cmd;
        }
    }


    public enum XlFixedFormatQuality
    {
        xlQualityStandard = 0,
        xlQualityMinimum = 1,
    }

    public enum XlFixedFormatType
    {
        xlTypePDF = 0,
        xlTypeXPS = 1,
    }

}
