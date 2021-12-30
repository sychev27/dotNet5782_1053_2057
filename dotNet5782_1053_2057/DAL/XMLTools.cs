using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
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

    }
}
