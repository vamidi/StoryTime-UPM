using Newtonsoft.Json.Linq;

using StoryTime.Database;
using StoryTime.Database.ScriptableObjects;

namespace StoryTime.Domains.Database.Service.Http
{
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