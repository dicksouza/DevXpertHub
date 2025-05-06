namespace DevXpertHub.Core.Entities;

/// <summary>
/// Classe base abstrata para entidades do domínio.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Construtor protegido que inicializa a entidade com um novo identificador único (GUID).
    /// </summary>
    protected Entity()
    {
        Id = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Identificador único da entidade.
    /// </summary>
    public string Id { get; set; }
}