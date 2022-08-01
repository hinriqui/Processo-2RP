using Processo_2RP.Contexts;
using Processo_2RP.Domains;
using Processo_2RP.Interfaces;
using Processo_2RP.Utils;
using System.Linq;
using System.Text.RegularExpressions;

namespace Processo_2RP.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext ctx;

        public UsuarioRepository(AppDbContext appContext)
        {
            ctx = appContext;
        }

        public Usuario BuscarPorId(int idUsuario)
        {
            return ctx.Usuarios.FirstOrDefault(u => u.Id == idUsuario);
        }

        public Usuario Login(string email, string senha)
        {
            var usuario = ctx.Usuarios.FirstOrDefault(u => u.Email == email);
            var rgx = new Regex(@"^\$\d[a-z]\$\d\d\$.{53}");

            if (usuario == null)
            {
                return null;
            }

            if (rgx.IsMatch(usuario.Senha))
            {
                bool comparado = Criptografia.Comparar(senha, usuario.Senha);
                if (comparado)
                {
                    return usuario;
                }
                return null;
            }
            else
            {
                AtualizarCriptografia(usuario, usuario.Id);
                return Login(usuario.Email, usuario.Senha);
            }
        }

        public void Atualizar(Usuario usuario)
        {
            Usuario usuarioBuscado = BuscarPorId(usuario.Id);

            if (usuario.Imagem != null)
            {
                usuarioBuscado.Imagem = usuario.Imagem;
            }

            if (usuario.IdTipo != 0)
            {
                usuarioBuscado.IdTipo = usuario.IdTipo;
            }

            if (usuario.Email != null)
            {
                usuarioBuscado.Email = usuario.Email;
            }

            if (usuario.Senha != null)
            {
                usuarioBuscado.Senha = usuario.Senha;
            }

            if (usuario.Ativado != null)
            {
                usuarioBuscado.Ativado = usuario.Ativado;
            }

            if (usuario.Nome != null)
            {
                usuarioBuscado.Nome = usuario.Nome;
            }

            ctx.Usuarios.Update(usuarioBuscado);

            ctx.SaveChanges();
        }

        public void AtualizarCriptografia(Usuario usuarioBuscado, int id)
        {
            Usuario usuarioRegistrado = BuscarPorId(id);

            string senhaAtualizada = Criptografia.GerarHash(usuarioBuscado.Senha);

            usuarioRegistrado.Senha = senhaAtualizada;

            ctx.Usuarios.Update(usuarioRegistrado);

            ctx.SaveChanges();
        }
    }
}
