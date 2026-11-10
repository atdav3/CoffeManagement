using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CoffeeManagement.GUI.Helpers
{
    internal static class CrystalReportsBridge
    {
        public static void Preview(DataTable data, string reportName, IWin32Window owner)
        {
            object report = LoadReport(data, reportName);
            try
            {
                Type viewerType = RequireType("CrystalDecisions.Windows.Forms.CrystalReportViewer", "CrystalDecisions.Windows.Forms");
                Control viewer = Activator.CreateInstance(viewerType) as Control;
                if (viewer == null) throw new InvalidOperationException("Crystal Reports viewer could not be created.");
                PropertyInfo source = viewerType.GetProperty("ReportSource");
                if (source == null) throw new InvalidOperationException("Crystal Reports viewer does not expose ReportSource.");
                source.SetValue(viewer, report, null);
                Form preview = new Form { Text = reportName + " · Crystal Reports", StartPosition = FormStartPosition.CenterParent, WindowState = FormWindowState.Maximized, MinimumSize = new System.Drawing.Size(900, 600) };
                viewer.Dock = DockStyle.Fill; preview.Controls.Add(viewer);
                preview.FormClosed += (s, e) => DisposeReport(report);
                preview.ShowDialog(owner);
            }
            catch { DisposeReport(report); throw; }
        }

        public static void ExportPdf(DataTable data, string reportName, string path)
        {
            object report = LoadReport(data, reportName);
            try
            {
                Type formatType = RequireType("CrystalDecisions.Shared.ExportFormatType", "CrystalDecisions.Shared");
                object pdfFormat = Enum.Parse(formatType, "PortableDocFormat", true);
                MethodInfo export = report.GetType().GetMethod("ExportToDisk", new[] { formatType, typeof(string) });
                if (export == null) throw new InvalidOperationException("Crystal Reports PDF export is unavailable in this runtime.");
                export.Invoke(report, new[] { pdfFormat, path });
            }
            catch (TargetInvocationException ex) { throw ex.InnerException ?? ex; }
            finally { DisposeReport(report); }
        }

        private static object LoadReport(DataTable data, string reportName)
        {
            if (data == null) throw new InvalidOperationException("Run a report before opening Crystal Reports.");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", reportName + ".rpt");
            if (!File.Exists(path)) throw new FileNotFoundException("Crystal Reports template is missing. Add " + reportName + ".rpt to the Reports folder.", path);
            Type reportType = RequireType("CrystalDecisions.CrystalReports.Engine.ReportDocument", "CrystalDecisions.CrystalReports.Engine");
            object report = Activator.CreateInstance(reportType);
            try
            {
                MethodInfo load = Array.Find(reportType.GetMethods(), method => method.Name == "Load" && method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(string));
                if (load == null) throw new InvalidOperationException("Crystal Reports runtime does not expose ReportDocument.Load.");
                load.Invoke(report, new object[] { path });
                MethodInfo setDataSource = reportType.GetMethod("SetDataSource", new[] { typeof(DataTable) });
                if (setDataSource == null) throw new InvalidOperationException("Crystal Reports runtime does not support DataTable data sources.");
                setDataSource.Invoke(report, new object[] { data });
                return report;
            }
            catch (TargetInvocationException ex) { DisposeReport(report); throw ex.InnerException ?? ex; }
            catch { DisposeReport(report); throw; }
        }

        private static Type RequireType(string typeName, string assemblyName)
        {
            Type type = Type.GetType(typeName + ", " + assemblyName, false);
            if (type == null) throw new InvalidOperationException("SAP Crystal Reports runtime is not installed. Install the matching Crystal Reports for Visual Studio runtime before using Crystal preview/export.");
            return type;
        }

        private static void DisposeReport(object report)
        {
            if (report == null) return;
            try { report.GetType().GetMethod("Close", Type.EmptyTypes)?.Invoke(report, null); } catch { }
            try { (report as IDisposable)?.Dispose(); } catch { }
        }
    }
}
