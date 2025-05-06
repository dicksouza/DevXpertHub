namespace DevXpertHub.Core.Dtos.Posts;

public record ComentarioCreateDto(
    string Texto,
    string UsuarioId,
    string PostId
);