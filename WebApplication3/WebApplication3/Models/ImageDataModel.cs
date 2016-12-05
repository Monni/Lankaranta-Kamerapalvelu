using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class ImageDataModel
    {
        public int ID { get; set; }
        public DateTime datetime { get; set; }
        public String imagePath { get; set; }
        public Boolean movement { get; set; }
    }
}