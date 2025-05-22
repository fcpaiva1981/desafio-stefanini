using MediatR;

namespace Questao5.Application.Queries.Requests;

public class ConsultaSaldoQuery: IRequest<decimal>
{
    public int NumeroConta { get; set; }

    public ConsultaSaldoQuery(int numeroConta)
    {
        NumeroConta = numeroConta;
    }
}