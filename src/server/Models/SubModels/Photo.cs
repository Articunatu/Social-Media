using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.SubModels
{
    public class Photo: Image
    {
        public bool IsProfilePhoto { get; set; }
        public bool IsBackgroundPhoto { get; set; }
    }
}
