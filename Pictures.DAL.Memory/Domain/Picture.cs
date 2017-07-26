using System.Windows.Media.Imaging;

namespace Pictures.DAL.Memory.Domain
{
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public BitmapFrame Image { get; set; }

        public BitmapFrame Thumbnail { get; set; }
    }
}
