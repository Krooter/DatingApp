using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Dtos
{
    public class PhotoForDetailedDto
    {
        public int Id { get; set; }
        public string UrlPhoto { get; set; }
        public string UrlPhotoAvatar { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public int UserId { get; set; }
    }
}
