using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;

namespace CoffeeManagement.GUI.Helpers
{
    internal static class ExcelExporter
    {
        private const string SpreadsheetNs = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        private const string RelationshipNs = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

        public static void Write(DataTable table, string path)
        {
            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            using (ZipArchive archive = new ZipArchive(file, ZipArchiveMode.Create, false, Encoding.UTF8))
            {
                WriteXml(archive, "[Content_Types].xml", writer =>
                {
                    writer.WriteStartElement("Types", "http://schemas.openxmlformats.org/package/2006/content-types");
                    writer.WriteStartElement("Default"); writer.WriteAttributeString("Extension", "rels"); writer.WriteAttributeString("ContentType", "application/vnd.openxmlformats-package.relationships+xml"); writer.WriteEndElement();
                    writer.WriteStartElement("Default"); writer.WriteAttributeString("Extension", "xml"); writer.WriteAttributeString("ContentType", "application/xml"); writer.WriteEndElement();
                    writer.WriteStartElement("Override"); writer.WriteAttributeString("PartName", "/xl/workbook.xml"); writer.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"); writer.WriteEndElement();
                    writer.WriteStartElement("Override"); writer.WriteAttributeString("PartName", "/xl/worksheets/sheet1.xml"); writer.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"); writer.WriteEndElement();
                    writer.WriteEndElement();
                });
                WriteXml(archive, "_rels/.rels", writer =>
                {
                    writer.WriteStartElement("Relationships", "http://schemas.openxmlformats.org/package/2006/relationships");
                    writer.WriteStartElement("Relationship"); writer.WriteAttributeString("Id", "rId1"); writer.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"); writer.WriteAttributeString("Target", "xl/workbook.xml"); writer.WriteEndElement(); writer.WriteEndElement();
                });
                WriteXml(archive, "xl/workbook.xml", writer =>
                {
                    writer.WriteStartElement("workbook", SpreadsheetNs); writer.WriteAttributeString("xmlns", "r", null, RelationshipNs);
                    writer.WriteStartElement("sheets"); writer.WriteStartElement("sheet"); writer.WriteAttributeString("name", "Report"); writer.WriteAttributeString("sheetId", "1"); writer.WriteAttributeString("r", "id", RelationshipNs, "rId1"); writer.WriteEndElement(); writer.WriteEndElement(); writer.WriteEndElement();
                });
                WriteXml(archive, "xl/_rels/workbook.xml.rels", writer =>
                {
                    writer.WriteStartElement("Relationships", "http://schemas.openxmlformats.org/package/2006/relationships");
                    writer.WriteStartElement("Relationship"); writer.WriteAttributeString("Id", "rId1"); writer.WriteAttributeString("Type", RelationshipNs + "/worksheet"); writer.WriteAttributeString("Target", "worksheets/sheet1.xml"); writer.WriteEndElement(); writer.WriteEndElement();
                });
                WriteXml(archive, "xl/worksheets/sheet1.xml", writer =>
                {
                    writer.WriteStartElement("worksheet", SpreadsheetNs); writer.WriteStartElement("sheetData");
                    writer.WriteStartElement("row"); writer.WriteAttributeString("r", "1");
                    for (int column = 0; column < table.Columns.Count; column++) WriteCell(writer, Address(column, 1), table.Columns[column].ColumnName);
                    writer.WriteEndElement();
                    for (int row = 0; row < table.Rows.Count; row++)
                    {
                        int rowNumber = row + 2; writer.WriteStartElement("row"); writer.WriteAttributeString("r", rowNumber.ToString(CultureInfo.InvariantCulture));
                        for (int column = 0; column < table.Columns.Count; column++) WriteCell(writer, Address(column, rowNumber), table.Rows[row][column]);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement(); writer.WriteEndElement();
                });
            }
        }

        private static void WriteCell(XmlWriter writer, string address, object value)
        {
            if (value == null || value == DBNull.Value) return;
            Type type = value.GetType();
            writer.WriteStartElement("c"); writer.WriteAttributeString("r", address);
            if (type == typeof(bool))
            {
                writer.WriteAttributeString("t", "b"); writer.WriteElementString("v", SpreadsheetNs, (bool)value ? "1" : "0");
            }
            else if (type == typeof(byte) || type == typeof(short) || type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double) || type == typeof(decimal))
            {
                writer.WriteElementString("v", SpreadsheetNs, Convert.ToString(value, CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteAttributeString("t", "inlineStr"); writer.WriteStartElement("is"); writer.WriteStartElement("t", SpreadsheetNs);
                string text = value is DateTime ? ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToString(value, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(text) && (char.IsWhiteSpace(text[0]) || char.IsWhiteSpace(text[text.Length - 1]))) writer.WriteAttributeString("xml", "space", "http://www.w3.org/XML/1998/namespace", "preserve");
                writer.WriteString(text); writer.WriteEndElement(); writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private static string Address(int column, int row)
        {
            string letters = ""; int value = column + 1;
            while (value > 0) { int remainder = (value - 1) % 26; letters = (char)('A' + remainder) + letters; value = (value - 1) / 26; }
            return letters + row.ToString(CultureInfo.InvariantCulture);
        }

        private static void WriteXml(ZipArchive archive, string path, Action<XmlWriter> write)
        {
            ZipArchiveEntry entry = archive.CreateEntry(path, CompressionLevel.Optimal);
            using (Stream stream = entry.Open())
            using (XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings { Encoding = new UTF8Encoding(false), Indent = false, CloseOutput = false }))
                write(writer);
        }
    }
}
