using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengePetApi.Models;

[Table("CH_PET")]
public class Pet
{
    [Key]
    [Column("ID_PET")]
    public int Id { get; set; }

    [Column("NM_PET")]
    public string? Nome { get; set; }

    [Column("DS_ESPECIE")]
    public string? Especie { get; set; }

    [Column("NR_IDADE")]
    public int Idade { get; set; }

    [Column("DS_NIVEL_RISCO")]
    public string? NivelRisco { get; set; }
}