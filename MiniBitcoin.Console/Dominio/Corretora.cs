namespace MiniBitcoin.Dominio;

public class Corretora
{
    private readonly Carteira _carteira;
    private readonly Cotacao _cotacao;

    public Corretora(Carteira carteira, Cotacao cotacao)
    {
        _carteira = carteira ?? throw new ArgumentNullException(nameof(carteira));
        _cotacao = cotacao ?? throw new ArgumentNullException(nameof(cotacao));
    }

    public Transacao Comprar(decimal valorBrl)
    {
        if (valorBrl <= 0)
            throw new ArgumentException("O valor da compra deve ser maior que zero.", nameof(valorBrl));
        if (valorBrl > _carteira.SaldoBrl)
            throw new InvalidOperationException("Saldo em BRL insuficiente para a compra.");

        var preco = _cotacao.PrecoBtcEmBrl;
        var satoshis = (long)Math.Floor(valorBrl / preco * Carteira.SatoshisPorBitcoin);
        if (satoshis == 0)
            throw new InvalidOperationException("Valor muito baixo para comprar ao menos 1 satoshi.");

        _carteira.DebitarBrl(valorBrl);
        _carteira.CreditarSatoshis(satoshis);

        return new Transacao(TipoTransacao.Compra, satoshis, valorBrl, preco, DateTime.Now);
    }

    public Transacao Vender(long satoshis)
    {
        if (satoshis <= 0)
            throw new ArgumentException("A quantidade vendida deve ser maior que zero.", nameof(satoshis));
        if (satoshis > _carteira.SaldoSatoshis)
            throw new InvalidOperationException("Saldo em BTC insuficiente para a venda.");

        var preco = _cotacao.PrecoBtcEmBrl;
        var valorBrl = Math.Round((decimal)satoshis / Carteira.SatoshisPorBitcoin * preco, 2, MidpointRounding.ToZero);
        if (valorBrl <= 0)
            throw new InvalidOperationException("Quantidade muito baixa para gerar valor em BRL.");

        _carteira.DebitarSatoshis(satoshis);
        _carteira.CreditarBrl(valorBrl);

        return new Transacao(TipoTransacao.Venda, satoshis, valorBrl, preco, DateTime.Now);
    }
} 