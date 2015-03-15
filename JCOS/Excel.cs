using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.VisualBasic;
using System.Data;
using System.Diagnostics;

namespace SCRANTIC
{
  class Excel
  {
    private static string excelheader = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>\r\n<?mso-application progid=\"Excel.Sheet\"?>\r\n<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:html=\"http://www.w3.org/TR/REC-html40\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\">\r\n  <x:ExcelWorkbook>\r\n    <x:ProtectStructure>False</x:ProtectStructure>\r\n    <x:ProtectWindows>False</x:ProtectWindows>\r\n  </x:ExcelWorkbook>\r\n";

    private static string excelstyles = "  <Styles>\r\n    <Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n      <Alignment ss:Vertical=\"Top\"/>\r\n      <Borders/>\r\n      <Font/>\r\n      <Interior/>\r\n      <NumberFormat/>\r\n      <Protection/>\r\n    </Style>\r\n    <Style ss:ID=\"headercell\">\r\n      <Alignment ss:Vertical=\"Top\"/>\r\n      <Borders/>\r\n      <Font ss:Bold=\"1\" />\r\n      <Interior/>\r\n      <NumberFormat/>\r\n      <Protection/>\r\n    </Style>\r\n    <Style ss:ID=\"datecell\">\r\n      <Alignment ss:Vertical=\"Top\"/>\r\n      <Borders/>\r\n      <Font/>\r\n      <Interior/>\r\n      <NumberFormat ss:Format=\"yyyy/mm/dd\\ hh:mm;@\"/>\r\n      <Protection/>\r\n    </Style>\r\n  </Styles>\r\n";
    private string lz(int i)
    {
      if (i > 0 && i < 10)
        return "0" + i.ToString();
      else return i.ToString();
    }

    public string DataTablesToExcelXml(List<DataTable> datatables, bool autofilter)
    {
      System.DateTime d = DateTime.Now;
      StringBuilder sb = new StringBuilder();

      sb.Append(excelheader);
      sb.Append(excelstyles);

      try
      {
        foreach (DataTable dt in datatables)
        {
          StringBuilder sColumns = new StringBuilder();
          sb.Append("  <ss:Worksheet ss:Name=\"" + dt.TableName + "\">\r\n    <ss:Table>\r\n");
          int colcount = 0;
          string[] columns = null;

          foreach (DataColumn dc in dt.Columns)
          {
            sb.Append("      <ss:Column ss:Width=\"81.0\"/>\r\n");
            sColumns.Append("        <ss:Cell ss:StyleID=\"headercell\">\r\n          <Data ss:Type=\"String\"><![CDATA[" + dc.ColumnName.ToString() + "]]></Data>\r\n        </ss:Cell>\r\n");
            Array.Resize(ref columns, colcount + 1);
            columns[colcount] = dc.ColumnName.ToString();
            colcount = colcount + 1;
          }

          sb.Append("      <ss:Row>\r\n");
          sb.Append(sColumns.ToString());
          sb.Append("      </ss:Row>\r\n");


          int rowcount = 0;
          foreach (DataRow dr in dt.Rows)
          {
            sb.Append("      <ss:Row>\r\n");
            for (int i = 0; i <= colcount - 1; i++)
            {
              DataColumn col = dt.Columns[i];
              string data = "";
              string datatype = col.DataType.Name;
              if (datatype == "Int16" | datatype == "Int32" | datatype == "Double" | datatype == "Float" | datatype == "Decimal")
              {
                datatype = "Number";
                if (dr[i] != null)
                {
                  data = dr[i].ToString();
                  data = data.Replace(",", ";").Replace(".", ",").Replace(";", ".");
                }
                else
                {
                  data = "";
                }
              }
              else if (datatype == "Boolean")
              {
                if (dr[i] != null)
                {
                  bool b = (bool)dr[i];
                  if (b)
                    data = "1";
                  else
                    data = "0";
                }
                else
                {
                  data = "";
                }
              }
              else if (datatype == "DateTime")
              {
                if (dr[i] != null)
                {
                  DateTime d1 = (DateTime)dr[i];
                  data = d1.Year + "-" + lz(d1.Month) + "-" + lz(d1.Day) + "T" + lz(d1.Hour) + ":" + lz(d1.Minute) + ":" + lz(d1.Second);
                }
                else
                {
                  data = "";
                }
              }
              else
              {
                datatype = "String";
                if (dr[i] != null)
                {
                  data = dr[i].ToString().Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;");
                }
                else
                {
                  data = "";
                }
              }
              if (datatype == "DateTime")
              {
                sb.Append("        <ss:Cell ss:StyleID=\"datecell\">\r\n");
              }
              else
              {
                sb.Append("        <ss:Cell>\r\n");
              }
              sb.Append("          <Data ss:Type=\"" + datatype + "\"><![CDATA[" + data + "]]></Data>\r\n        </ss:Cell>\r\n");
            }
            sb.Append("      </ss:Row>\r\n");
            rowcount = rowcount + 1;
          }



          sb.Append("    </ss:Table>\r\n    ");
          if (autofilter)
            sb.Append("<AutoFilter x:Range=\"R1C1:R" + rowcount + "C" + colcount + "\" xmlns=\"urn:schemas-microsoft-com:office:excel\">\r\n    </AutoFilter>\r\n  ");
          sb.Append("</ss:Worksheet>\r\n");
        }

      }
      catch
      {
      } sb.Append("</Workbook>\r\n");

      return sb.ToString();
    }
  }
}

