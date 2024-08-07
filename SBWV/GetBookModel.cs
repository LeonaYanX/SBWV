using SBWV.Models.ViewModels;

namespace SBWV
{
    public static class GetBookModel
    {
        public static IEnumerable<Picture> GetImages(ICollection<Galary> galaries)
        {
            return galaries
                .Where(g => g.Photo != null)
                .Select(g => new Picture() { Id = g.Id, Url= $"/Home/GetImageById?idGalary={g.Id}" });

        }
        /* .Select(g => new Picture() { Id = g.Id, Src = "data:image/png;base64, " + Convert.ToBase64String(g.Photo) , Url= $"/Home/GetImageById?idGalary={g.Id}" });
         * 
         * public static string GetBase64Image(byte[] bytes)
          {
              if (bytes == null)
                  return String.Empty;

              return "data:image/png;base64, " + Convert.ToBase64String(bytes);
          }*/
        public static BookVM GetBookVM(Book book)
        {

            BookVM bookVM = new BookVM()
            {
                Id = book.Id,
                Author = book?.Author,
                Category = book?.IdCatalogNavigation?.Value.ToString()
                ,
                Info = book?.Info,
                Price = book?.Price,
                Swap = book.Swap == 1 ? true : false,
                Title = book?.Title,
                Pictures = GetImages(book.Galaries).ToArray()

            };



            return bookVM;
        }


    }
}
