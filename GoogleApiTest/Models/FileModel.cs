
namespace GoogleApiTest.Models
{
    internal class FileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public DateTime? UploadDate { get; set; }

        public long? Size { get; set; }

        public string FileType { get; set; }
    }
}
