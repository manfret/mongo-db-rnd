using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace mongo_db_rnd.Api.Mongo;

public class MongoRepository
{
    private readonly IMongoDatabase _mongoDb;

    public MongoRepository(IMongoDatabase mongoDb)
    {
        _mongoDb = mongoDb;
    }

    public async Task CreateCollectionAsync(string collectionName)
    {
        await _mongoDb.CreateCollectionAsync(collectionName);
    }

    public async Task<PayloadData> PushValueAsync(string collectionName, string value)
    {
        var collection = _mongoDb.GetCollection<PayloadData>(collectionName);
        var payload = new PayloadData(value);
        await collection.InsertOneAsync(payload);
        return payload;
    }

    public async Task<PayloadData> DeleteValueAsync(string collectionName, string value)
    {
        var collection = _mongoDb.GetCollection<PayloadData>(collectionName);
        var deleteOptions = new DeleteOptions { Collation = new Collation("en", caseLevel: false) };
        var res = await collection.DeleteOneAsync(data => data.Value == value, deleteOptions);
        return new PayloadData(res?.DeletedCount.ToString() ?? "null");
    }

    public async Task<PayloadData> ReadAsync(string collectionName)
    {
        var collection = _mongoDb.GetCollection<PayloadData>(collectionName);
        var filter = Builders<PayloadData>.Filter.Empty;
        return (await collection.FindAsync(filter)).FirstOrDefault();
    }
}

[BsonIgnoreExtraElements]
public class PayloadData
{
    public string Value { get; set; }

    public PayloadData(string value)
    {
        Value = value;
    }
}