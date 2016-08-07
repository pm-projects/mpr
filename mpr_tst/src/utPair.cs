using System;

using System.Data;
using System.Collections; 
using System.Xml;

namespace mpr_tst.src
{
 public  struct  pair  /// хранение фигуры и тиков. 
 {
   const string _nm = "pair";
   public short fgr;             // какая фигура нажата <0 или показана >= 0
   public ulong tm;              // в какое время. 



   public void toXml (XmlDocument doc, XmlNode edge){  /// вывод пары в xml
/*
     XmlNode point, x, y;
     point = doc.CreateElement("Point");
     x = doc.CreateElement("X");
     x.InnerText = (string)Xpr.Clone();
     y = doc.CreateElement("Y");
     y.InnerText = (string)Ypr.Clone();
     edge.AppendChild(point);
     point.AppendChild(x); 
     point.AppendChild(y); 
*/
   }


}}