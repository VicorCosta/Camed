using Camed.SSC.Core.Entities;

namespace Camed.SSC.Domain.Entities
{
    public class Cidade : EntityBase
    {
        //Todo: O Campo na tabela tb_cidade está como decimal. Lembrar de fazer um alter table na para o Id ficar como int
        public string Nome { get; set; }
        public string UF { get; set; }

    }
}
