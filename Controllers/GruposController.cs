using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Dados.EntityFramework;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        private readonly Contexto _context;

        public GruposController(Contexto context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("AdicionarGrupo")]
        public async Task<IActionResult> AddGroup([FromForm] GrupoDto groupDto)
        {
            if (ModelState.IsValid)



            {

                byte[] fotoBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await groupDto.Foto.CopyToAsync(memoryStream);
                    fotoBytes = memoryStream.ToArray();
                }
                var adminExists = await _context.Usuarios.AnyAsync(u => u.UsuariosID == groupDto.ID_Administrador);
                if (!adminExists)
                {
                    return BadRequest("O administrador especificado não existe.");
                }

                var group = new Grupos
                {
                    NomeGrupo = groupDto.NomeGrupo,
                    ParticipantesMax = groupDto.ParticipantesMax,
                    Valor = groupDto.Valor,
                    DataRevelacao = groupDto.DataRevelacao,
                    Descricao = groupDto.Descricao,
                    ID_Administrador = groupDto.ID_Administrador,
                    SorteioRealizado = groupDto.SorteioRealizado,
                    Foto = fotoBytes

                };

                if (groupDto.Foto != null && groupDto.Foto.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await groupDto.Foto.CopyToAsync(ms);
                        group.Foto = ms.ToArray();
                    }
                }

                _context.Grupos.Add(group);
                await _context.SaveChangesAsync();
                return Ok(group);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("AdicionarParticipantes")]
        public async Task<IActionResult> AddParticipante([FromForm] AddParticipanteDto participanteDto)
        {
            if (ModelState.IsValid)
            {
                var grupoExists = await _context.Grupos.AnyAsync(g => g.GruposID == participanteDto.GrupoID);
                var usuarioExists = await _context.Usuarios.AnyAsync(u => u.UsuariosID == participanteDto.UsuarioID);

                if (!grupoExists)
                {
                    return BadRequest("O grupo especificado não existe.");
                }

                if (!usuarioExists)
                {
                    return BadRequest("O usuário especificado não existe.");
                }

                var participanteGrupo = new ParticipantesGrupo
                {
                    ID_Grupo = participanteDto.GrupoID,
                    ID_Participante = participanteDto.UsuarioID
                };

                _context.ParticipantesGrupo.Add(participanteGrupo);
                await _context.SaveChangesAsync();
                return Ok(participanteGrupo);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("TodosGrupos")]
        public async Task<IActionResult> GetGroups()
        {
            var groups = await _context.Grupos.ToListAsync();
            return Ok(groups);
        }




        [HttpDelete]
        [Route("DeletarGrupo/{id}")]
        public async Task<IActionResult> DeleteGroups(int id)
        {
            var group = await _context.Grupos.FindAsync(id);
            if (group == null)
            {
                return NotFound(new { Message = "Grupo nao encontrado" });
            }

            _context.Grupos.Remove(group);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Grupo deletado com sucesso" });
        }

    }

    public class Grupos
    {
        public int GruposID { get; set; }
        public string NomeGrupo { get; set; }
        public int ParticipantesMax { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataRevelacao { get; set; }
        public string Descricao { get; set; }
        public int ID_Administrador { get; set; }
        public bool SorteioRealizado { get; set; }

        public byte[] Foto { get; set; }
    }

    public class ParticipantesGrupo
    {
        public int ID_Grupo { get; set; }
        public int ID_Participante { get; set; }
    }
}
