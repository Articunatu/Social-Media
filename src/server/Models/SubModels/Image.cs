using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.SubModels
{
    public class Image
    {
        public Guid Id { get; set; }
        public byte[] Base64 { get; set; }
    }
}
