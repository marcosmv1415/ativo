using APINotification.Data;
using APINotification.Models;
using APINotification.Repositories.Contracts;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace APINotification.Repositories
{
    public class AtivoRepository : IAtivoRepository
    {
        IConfiguration _conf;
        BancoContext _banco;
        public AtivoRepository(BancoContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Cadastrar(Ativo ativo)
        {           
            _banco.Add(ativo);
            _banco.SaveChanges();

        }
        public List<Ativo> ObterDadosAtivo(string nomeativo)
        {

            var ativos = _banco.Ativo.AsQueryable();

            return ativos.Where(x => x.Nome_Ativo == nomeativo).ToList();
        }
        public void LimparAtivos(string nomeativo)
        {
            var ativos = _banco.Ativo.Where(a => a.Nome_Ativo == nomeativo);
            _banco.Ativo.RemoveRange(ativos);
            _banco.SaveChanges();
        }

    }
}
