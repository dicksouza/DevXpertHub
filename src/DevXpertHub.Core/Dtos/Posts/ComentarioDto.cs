namespace DevXpertHub.Core.Dtos.Posts;

public record ComentarioDto(
    string Id,
    string Texto,
    string UsuarioId,
    DateTime DataCriacao
);