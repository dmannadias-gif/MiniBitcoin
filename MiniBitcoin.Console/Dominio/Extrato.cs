namespace MiniBitcoin.Dominio;

public class Extrato
{
    private readonly List<Transacao> _transacoes = new();

    public IReadOnlyList<Transacao> Transacoes => _transacoes;

    public void Registrar(Transacao transacao)
    {
        ArgumentNullException.ThrowIfNull(transacao);
        _transacoes.Add(transacao);
    }

    public static decimal CalcularPatrimonio(Carteira carteira, Cotacao cotacao)
    {
        return carteira.SaldoBrl + carteira.SaldoBtc * cotacao.PrecoBtcEmBrl;
    }
}