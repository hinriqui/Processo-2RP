using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Processo_2RP.Domains
{
    public class Usuario
    {
        public int Id { get; set; }
        [ForeignKey("Tipo")]
        public int IdTipo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Imagem { get; set; }
        public bool? Ativado { get; set; }

        public virtual Tipo IdTipoNavigation { get; set; }
    }
}
