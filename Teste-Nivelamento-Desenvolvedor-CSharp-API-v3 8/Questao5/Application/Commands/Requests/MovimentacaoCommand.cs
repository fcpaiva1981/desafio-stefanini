using MediatR;

namespace Questao5.Application.Commands;

public class MovimentacaoCommand : IRequest<bool>
{
    public int NumeroConta { get; set; }
    public decimal Valor { get; set; }
    public string Tipo { get; set; } // "deposito" ou "saque"
}