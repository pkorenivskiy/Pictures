using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pictures.UI
{
    public class PictureCollection : ObservableCollection<Dto.Picture>
    {
        public PictureCollection()
        {

        }

        public PictureCollection(IEnumerable<Dto.Picture> pictures)
        {
            Clear();
            foreach (var p in pictures)
                Add(p);
        }

        public void AddPicture(Dto.Picture picture)
        {
            Add(picture);
        }
    }
}
