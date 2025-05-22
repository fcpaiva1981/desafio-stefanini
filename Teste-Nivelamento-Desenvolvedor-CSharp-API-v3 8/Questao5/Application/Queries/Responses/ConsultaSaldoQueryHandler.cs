using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Infrastructure;

namespace Questao5.Application.Queries.Responses;

public class ConsultaSaldoQueryHandler: IRequestHandler<ConsultaSaldoQuery, decimal>
{
    private readonly ContaCorrenteRepository _repository;

    public ConsultaSaldoQueryHandler(ContaCorrenteRepository repository)
    {
        _repository = repository;
    }

    public async Task<decimal> Handle(ConsultaSaldoQuery request, CancellationToken cancellationToken)
    {
        var conta = await _repository.ObterContaPorNumero(request.NumeroConta);
        if (conta == null) throw new Exception("Conta não encontrada.");

        return conta.Saldo;
    }
}