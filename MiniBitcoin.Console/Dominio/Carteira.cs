namespace MiniBitcoin.Dominio;

public class Carteira
{
    public const long SatoshisPorBitcoin = 100_000_000;

    public decimal SaldoBrl { get; private set; }
    public long SaldoSatoshis { get; private set; }
    public decimal SaldoBtc => (decimal)SaldoSatoshis / SatoshisPorBitcoin;

    public Carteira(decimal saldoInicialBrl, long saldoInicialSatoshis = 0)
    {
        if (saldoInicialBrl < 0)
            throw new ArgumentException("O saldo inicial em BRL não pode ser negativo.", nameof(saldoInicialBrl));
        if (saldoInicialSatoshis < 0)
            throw new ArgumentException("O saldo inicial em satoshis não pode ser negativo.", nameof(saldoInicialSatoshis));

        SaldoBrl = saldoInicialBrl;
        SaldoSatoshis = saldoInicialSatoshis;
    }

    public void CreditarBrl(decimal valor)
    {
        ValidarPositivo(valor);
        SaldoBrl += valor;
    }

    public void DebitarBrl(decimal valor)
    {
        ValidarPositivo(valor);
        if (valor > SaldoBrl)
            throw new InvalidOperationException("Saldo em BRL insuficiente.");
        SaldoBrl -= valor;
    }

    public void CreditarSatoshis(long quantidade)
    {
        ValidarPositivo(quantidade);
        SaldoSatoshis += quantidade;
    }

    public void DebitarSatoshis(long quantidade)
    {
        ValidarPositivo(quantidade);
        if (quantidade > SaldoSatoshis)
            throw new InvalidOperationException("Saldo em BTC insuficiente.");
        SaldoSatoshis -= quantidade;
    }

    private static void ValidarPositivo(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor deve ser maior que zero.", nameof(valor));
    }
}