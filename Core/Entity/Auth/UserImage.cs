namespace Core.Entity.Auth
{
    public class UserImage : BaseEntity
    {
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public string ImagePath { get; set; }
        public string ImageDescription { get; set; }
        public bool IsProfile { get; set; }
        public bool IsDeleted { get; set; }
    }
}
