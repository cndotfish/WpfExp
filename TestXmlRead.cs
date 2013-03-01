using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;

namespace WpfExp
{
    public class TestXmlRead
    {
        public static readonly string FilePath = "";
        public static void ReadXmlByXmlDocument()
        {

        }

        public static void ReadXmlByXmlReader()
        {

        }

        public static void ReadXmlBySerialize()
        {

        }

        public static void ReadXmlByLINQ()
        {

        }

        public static void ReadXmlByDataSet()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(TestXmlRead.FilePath);
            DataTable dt = ds.Tables["Person"];
            int age = Convert.ToInt16(dt.Select("Name='Mark'")[0]["Age"]);
            Console.Write(age);
        }

    }
}
