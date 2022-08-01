using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Processo_2RP.Contexts;
using Processo_2RP.Domains;
using Processo_2RP.Interfaces;
using Processo_2RP.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Processo_2RP.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuariosController(AppDbContext context, IUsuarioRepository repo)
        {
            _context = context;
            _usuarioRepository = repo;
        }

        /// <summary>
        /// Lista todos os usuários
        /// </summary>
        /// <returns>Uma lista de usuários</returns>
        // GET: api/Usuarios
        //[Authorize(Roles = "2,3")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> ListarUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        /// <summary>
        /// Lista um usuário específico
        /// </summary>
        /// <param name="id">id do usuário</param>
        /// <returns>um único usuário</returns>
        // GET: api/Usuarios/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> BuscarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        /// <summary>
        /// Atualiza um usuário
        /// </summary>
        /// <param name="usuario">Objeto que recebe os dados atualizados</param>
        /// <param name="arquivo">Foto de perfil</param>
        /// <returns>Retorna o status No content</returns>

        //[Authorize]
        [HttpPatch("image")]
        public IActionResult AtualizarUsuario([FromForm] Usuario usuario, IFormFile arquivo)
        {
            var usuarioBuscado = _usuarioRepository.BuscarPorId(usuario.Id);
            string[] extensoesPermitidas = { "jpg", "png", "jpeg" };

            if (usuarioBuscado == null)
            {
                return NotFound();
            }

            string result = Upload.UploadFile(arquivo, extensoesPermitidas);
            if (result != null && result != "Extensão não permitida")
            {
                usuario.Imagem = result;
            } 
            else
            {
                usuario.Imagem = null;
            }

            _usuarioRepository.Atualizar(usuario);
            return NoContent();
        }

        /// <summary>
        /// Atualiza um usuário
        /// </summary>
        /// <param name="usuario">Objeto que recebe os dados atualizados</param>
        /// <returns>Retorna o status No content</returns>
        [Authorize]
        [HttpPatch]
        public IActionResult AtualizarUsuario(Usuario usuario)
        {
            var usuarioBuscado = _usuarioRepository.BuscarPorId(usuario.Id);
            string[] extensoesPermitidas = { "jpg", "png", "jpeg" };

            if (usuarioBuscado == null)
            {
                return NotFound();
            }

            usuario.Imagem = null;

            _usuarioRepository.Atualizar(usuario);
            return NoContent();
        }

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <param name="usuario">Objeto que leva os dados do usuário a ser cadastrado</param>
        /// <param name="arquivo">Foto do usuario</param>
        /// <returns>retorna o status Ok</returns>
        [Authorize(Roles = "2,3")]
        [HttpPost("image")]
        public async Task<ActionResult<Usuario>> PostUsuario([FromForm] Usuario usuario, IFormFile arquivo)
        {
            string[] extensoesPermitidas = { "jpg", "png", "jpeg" };
            
            string uploadResultado = Upload.UploadFile(arquivo, extensoesPermitidas);
            if (uploadResultado != null && uploadResultado != "Extensão não permitida")
            {
                usuario.Imagem = uploadResultado;
            }
            else
            {
                usuario.Imagem = null;
            }

            // A senha é criptografada antes do cadastro
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <param name="usuario">Objeto que leva os dados do usuário a ser cadastrado</param>
        /// <returns>retorna o status Ok</returns>
        [Authorize(Roles = "2,3")]
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            string[] extensoesPermitidas = { "jpg", "png", "jpeg" };

            // A senha é criptografada antes do cadastro
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            usuario.Imagem = null;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostUsuario", new { id = usuario.Id }, usuario);
        }

        /// <summary>
        /// Deleta um usuário
        /// </summary>
        /// <param name="id">Id do usuário a ser deletado</param>
        /// <returns>Retorna o status no content</returns>
        // DELETE: api/Usuarios/5
        [Authorize(Roles = "3")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            if (usuario.Imagem != null)
            {
                Upload.RemoverArquivo(usuario.Imagem);
            }
            return NoContent();
        }
    }
}
