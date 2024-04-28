using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaLeague.Domain.Model;

public interface IEntity
{
    public string Id { get; set; }

    public DateTime DateCreated { get; set; }
}
