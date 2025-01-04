//using System.Runtime.CompilerServices;

//namespace SM.Persistence.Document
//{
//    internal class CosmosSetup
//    {
//        public static void Start(CallConvThiscall callConvThiscall)
//        {
//            builder.Services.AddDbContext<ApplicationDbContext>(options =>
//            {
//                var cosmosSettings = builder.Configuration.GetSection("CosmosDb");
//                options.UseCosmos(
//                    accountEndpoint: cosmosSettings["AccountEndpoint"],
//                    accountKey: cosmosSettings["AccountKey"],
//                    databaseName: cosmosSettings["DatabaseName"]);
//            });
//        }
//    }
//}
