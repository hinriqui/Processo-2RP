using Processo_2RP.Domains;

namespace Processo_2RP.Interfaces
{
    public interface IUsuarioRepository
    {
        Usuario Login(string email, string senha);
        Usuario BuscarPorId(int idUsuario);
        void Atualizar(Usuario usuario);
    }
}
