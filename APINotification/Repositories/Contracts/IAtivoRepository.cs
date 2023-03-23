using APINotification.Data;
using APINotification.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace APINotification.Repositories.Contracts
{
    public interface IAtivoRepository
    {
        void Cadastrar(Ativo ativo);
        List<Ativo> ObterDadosAtivo(string nomeativo);
        void LimparAtivos(string nomeativo);
    }
}
