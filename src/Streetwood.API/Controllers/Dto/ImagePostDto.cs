using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Streetwood.API.Controllers.Dto
{
    public class ImagePostDto
    {
        public IFormFile File { get; set; }

        public int Id { get; set; }

        public bool IsMain { get; set; }

    }
}
