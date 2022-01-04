using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;

namespace DAL
{
    class XMLTools
    {
        static string dir = @"xml\";

        static XMLTools()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }    
        
        public static void SaveListToXMLSerializer<T>(List<T> list,string filePath)
        {
            try
            {
                FileStream file = new FileStream(dir + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch(Exception ex)
            {
          //      throw new DAL.XMLFileLoadCreateException(filePath, $"file to create xml file: {filePath}", ex);
            }
        }

        public static IEnumerable<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(dir + filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                // throw new DO.XMLFileLoadeCreateException(filePath, $"fail to load xml file: {filePath}", ex);
                return null;
            }

        }

        public class XmlStation
        {
            XElement stationRoot;
            string stationsPath = @"StationsXml.xml";
            public void SaveStationListLinq(List<DalXml.DO.Station> stationList)
            {
                stationRoot = new XElement("stations",
                                           from p in stationList
                                           select new XElement("station",
                                           new XElement("id", p.Id),
                                           new XElement("name", p.Name),
                                           new XElement("longitude", p.Longitude),
                                           new XElement("latitude", p.Latitude),
                                           new XElement("chargeSlots", p.ChargeSlots),
                                           new XElement("exists", p.Exists)));
                stationRoot.Save(stationsPath);
            }
        }

    }
}
