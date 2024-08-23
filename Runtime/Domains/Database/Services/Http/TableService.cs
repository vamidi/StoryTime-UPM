using Newtonsoft.Json.Linq;

namespace StoryTime.Domains.Database.Services.Http
{
    using StoryTime.Domains.Database.ScriptableObjects;
    public class TableService
    {
        private readonly IAPIService _apiService;

        public TableService(IAPIService apiService)
        {
            _apiService = apiService;
        }
     
        public struct TableResult
        {
            public TableSO Table;
            public JToken Item;
        }
                
        public TableResult CreateTable(string destination, JToken item, TableMetaData tableMetadata)
        {
            return new()
            {
                Table = TableDatabase.Get.AddTable(destination, tableMetadata),
                Item = item
            };
        }
    }
}