using Microsoft.Azure.Cosmos;
using Models.Models;
using Models.SubModels;

namespace Core.Service
{
    public class AccountRepository : IAccountRepository
    {
        readonly Container _container;
        readonly string containerName = "Account";

        public AccountRepository(CosmosClient client, string databaseName)
        {
            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task Create(Account created)
        {
            created.Id = Guid.NewGuid();
            await _container.CreateItemAsync(created);
        }
        public async Task<IEnumerable<Account>> ReadAll()
        {
            string query = "SELECT * FROM c";

            var selected = _container.GetItemQueryIterator<Account>(new QueryDefinition(query));

            List<Account> result = new();
            while (selected.HasMoreResults)
            {
                var response = await selected.ReadNextAsync();
                result.AddRange(response);
            }

            return result.ToArray();
        }

        public async Task<Account> ReadSingle(Guid id)
        {
            await _container.ReadItemAsync<Account>(id.ToString(), new PartitionKey(id.ToString()));
            // Read existing item from container
            //var account = (await ReadAll(id)).FirstOrDefault(a => a.Id.Equals(id));
            //return account;
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT TOP 1 id, firstName, lastName, tag FROM Account a WHERE a.id = @partitionKey")
                .WithParameter("@partitionKey", id);

            // Query multiple items from container
            using FeedIterator<Account> filteredFeed = _container.GetItemQueryIterator<Account>(
                queryDefinition: parameterizedQuery
            );

            Account? result = new();

            // Iterate query result pages
            while (filteredFeed.HasMoreResults)
            {
                FeedResponse<Account> response = await filteredFeed.ReadNextAsync();
                result = response.FirstOrDefault() ?? result;
            }
            return result;
        }

        public async Task Update(Account updated)
        {
            await _container.UpsertItemAsync(updated);
        }

        public async Task Delete(Guid id)
        {
            var newId = id.ToString();
            await _container.DeleteItemAsync<Account>(newId, new PartitionKey(newId));
        }

        public async Task<IEnumerable<AccountDto>> GetTop20PagedFollowers(Guid accountId)
        {
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT TOP 10 Followers FROM Account a WHERE a.id = @partitionKey")
                .WithParameter("@partitionKey", accountId);

            // Query multiple items from container
            using FeedIterator<AccountDto> filteredFeed = _container.GetItemQueryIterator<AccountDto>(
                queryDefinition: parameterizedQuery
            );

            List<AccountDto> result = new();
            while (filteredFeed.HasMoreResults)
            {
                var response = await filteredFeed.ReadNextAsync();
                result.AddRange(response);
            }

            return result.ToArray();
        }

        public Task<IEnumerable<AccountDto>> GetTop20PagedFollowing(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetAccount(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AccountDto>> GetTop20PagedMutuals(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMutualsCount(Guid accountId)
        {
            throw new NotImplementedException();
        }
    }
}

//private static async Task<KeyValuePair<string, IEnumerable<CeleryTask>>> QueryDocumentsByPage(int pageNumber, int pageSize, string continuationToken)
//{
//    DocumentClient documentClient = new DocumentClient(new Uri("https://{CosmosDB/SQL Account Name}.documents.azure.com:443/"), "{CosmosDB/SQL Account Key}");

//    var feedOptions = new FeedOptions
//    {
//        MaxItemCount = pageSize,
//        EnableCrossPartitionQuery = true,

//        // IMPORTANT: Set the continuation token (NULL for the first ever request/page)
//        RequestContinuation = continuationToken
//    };

//    IQueryable<CeleryTask> filter = documentClient.CreateDocumentQuery<CeleryTask>("dbs/{Database Name}/colls/{Collection Name}", feedOptions);
//    IDocumentQuery<CeleryTask> query = filter.AsDocumentQuery();

//    FeedResponse<CeleryTask> feedRespose = await query.ExecuteNextAsync<CeleryTask>();

//    List<CeleryTask> documents = new List<CeleryTask>();
//    foreach (CeleryTask t in feedRespose)
//    {
//        documents.Add(t);
//    }

//    // IMPORTANT: Ensure the continuation token is kept for the next requests
//    return new KeyValuePair<string, IEnumerable<CeleryTask>>(feedRespose.ResponseContinuation, documents);
//}

//private static async Task QueryPageByPage()
//{
//    // Number of documents per page
//    const int PAGE_SIZE = 3;

//    int currentPageNumber = 1;
//    int documentNumber = 1;

//    // Continuation token for subsequent queries (NULL for the very first request/page)
//    string continuationToken = null;

//    do
//    {
//        Console.WriteLine($"----- PAGE {currentPageNumber} -----");

//        // Loads ALL documents for the current page
//        KeyValuePair<string, IEnumerable<CeleryTask>> currentPage = await QueryDocumentsByPage(currentPageNumber, PAGE_SIZE, continuationToken);

//        foreach (CeleryTask celeryTask in currentPage.Value)
//        {
//            Console.WriteLine($"[{documentNumber}] {celeryTask.Id}");
//            documentNumber++;
//        }

//        // Ensure the continuation token is kept for the next page query execution
//        continuationToken = currentPage.Key;
//        currentPageNumber++;
//    } while (continuationToken != null);

//    Console.WriteLine("\n--- END: Finished Querying ALL Dcuments ---");
//}
