using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface ICatalogueRepository
    {
        public Task<BookStock> GetBookStockById(Guid bookStockId);
        
        public List<BookStock> GetCatalogue();

        public List<BookStock> SearchCatalogue(CatalogueSearch search);
    }
}
