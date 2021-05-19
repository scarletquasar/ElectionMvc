using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoHubCount.Models
{
    public class VotoModel
    {
        public int legenda { get; set; }
        public string data_voto { get; set; }
        public string nome_candidato { get; set; }
        public string nome_vice { get; set; }

    }
}
