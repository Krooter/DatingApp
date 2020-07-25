using System;

namespace DatingApp.Core
{
    public class Photo
    {
        public int Id { get; set; }
        public string UrlPhoto { get; set; }
        public string UrlPhotoAvatar { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public string PublicIdAvatar { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}