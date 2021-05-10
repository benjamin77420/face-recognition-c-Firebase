using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace openCVWork_with_files_
{
    class Worker
    {
        public string FullName { get; set; }
        public string ID { get; set; }
        public List<string> imges {get; set;}

    }  
}
