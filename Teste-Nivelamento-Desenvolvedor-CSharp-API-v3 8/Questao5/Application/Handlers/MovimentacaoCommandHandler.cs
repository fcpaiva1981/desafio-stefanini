using Dapper;
using MediatR;
using System.Data;
using Questao5.Application.Commands;
using Questao5.Infrastructure;

namespace Application.Commands
{
    public class MovimentacaoCommandHandler : IRequestHandler<MovimentacaoCommand, bool>
    {
        private readonly ContaCorrenteRepository _repository;

        public MovimentacaoCommandHandler(ContaCorrenteRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(MovimentacaoCommand request, CancellationToken cancellationToken)
        {
            var conta = await _repository.ObterContaPorNumero(request.NumeroConta);
            if (conta == null) throw new Exception("Conta não encontrada.");

            if (request.Tipo.ToLower() == "saque")
            {
                if (conta.Saldo < request.Valor) throw new Exception("Saldo insuficiente.");
                conta.Saldo -= request.Valor;
            }
            else if (request.Tipo.ToLower() == "deposito")
            {
                conta.Saldo += request.Valor;
            }
            else
            {
                throw new Exception("Tipo inválido. Use 'deposito' ou 'saque'.");
            }

            return await _repository.AtualizarSaldo(conta.Numero, conta.Saldo);
        }
    }
}