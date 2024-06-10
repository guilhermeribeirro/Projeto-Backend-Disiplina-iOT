namespace WebApplication1.DTOs
{
    public class UsuarioDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public IFormFile Foto { get; set; }
    }

    public class GrupoDto
    {
        public string NomeGrupo { get; set; }
        public int ParticipantesMax { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataRevelacao { get; set; }
        public string Descricao { get; set; }
        public int ID_Administrador { get; set; }
        public bool SorteioRealizado { get; set; }
        public IFormFile Foto { get; set; }
    }
    public class AddParticipanteDto
    {
        public int GrupoID { get; set; }
        public int UsuarioID { get; set; }
    }

}
