using MongoDB.Bson.Serialization.Attributes;
using VarzeaLeague.Domain.Enum;
using VarzeaTeam.Domain.Enum;

namespace VarzeaLeague.Domain.Model.User;

public class UserModel
{

    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDB
    public string Id { get; set; } = string.Empty;

    [BsonElement("UserName")] // Atributo que mapeia essa propriedade para o campo 'UserName' no MongoDB
    public string UserName { get; set; } = string.Empty;

    [BsonElement("Email")] // Atributo que mapeia essa propriedade para o campo 'Email' no MongoDB
    public string Email { get; set; } = string.Empty;

    [BsonElement("Password")] // Atributo que mapeia essa propriedade para o campo 'Password' no MongoDB
    public string Password { get; set; } = string.Empty;

    [BsonElement("Cpf")] // Atributo que mapeia essa propriedade para o campo 'Cpf' no MongoDB
    public string Cpf { get; set; } = string.Empty;

    [BsonElement("DateCreated")] // Atributo que mapeia essa propriedade para o campo 'DateCreated' no MongoDB
    public DateTime DateCreated { get; set; }

    [BsonElement("Role")] // Atributo que mapeia essa propriedade para o campo 'Role' no MongoDB
    public Role Role { get; set; }

    [BsonElement("AccountStatus")] // Atributo que mapeia essa propriedade para o campo 'AccountStatus' no MongoDB
    public AccountStatus AccountStatus { get; set; }
}
