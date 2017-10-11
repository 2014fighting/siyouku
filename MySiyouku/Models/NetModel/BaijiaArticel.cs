using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySiyouku.Models.NetModel
{
    public class BaijiaArticel
    {
        public int errno { get; set; }
        public Tdata data { get; set; }
    }
    
    public class TArticel
    {
        public string ID { get; set; }
        public string m_create_time { get; set; }
        public string m_title { get; set; }
        public string m_summary { get; set; }
        public string hotcount { get; set; }
        public List<MlableName> m_label_names { get; set; }
        public string m_image_url { get; set; }
        public string m_display_url { get; set; }
        public string m_writer_name { get; set; }
        public string m_writer_url { get; set; }
        public string m_writer_account_type { get; set; }
        public string m_attr_exclusive { get; set; }
        public string m_attr_first_pub { get; set; }
    }

    public class MlableName
    {
        public string m_id { get; set; }
        public string m_name { get; set; }
    }
    public class Tdata
    {
        public int total { get; set; }
        public List<TArticel> list { get; set; }
    }
}