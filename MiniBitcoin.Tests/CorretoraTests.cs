using MiniBitcoin.Dominio;

namespace MiniBitcoin.Tests;

public class CorretoraTests
{
    private const decimal Preco = 500_000m;

    [Fact]
    public void Cotacao_ComPrecoNaoPositivo_LancaExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Cotacao(0m));
    }

    [Fact]
    public void Cotacao_Atualizar_AlteraPreco()
    {
        var cotacao = new Cotacao(Preco);

        cotacao.Atualizar(600_000m);

        Assert.Equal(600_000m, cotacao.PrecoBtcEmBrl);
    }

    [Fact]
    public void Comprar_ComSaldoSuficiente_DebitaBrlECreditaSatoshis()
    {
        var carteira = new Carteira(1000m);
        var corretora = new Corretora(carteira, new Cotacao(Preco));

        var transacao = corretora.Comprar(500m);

        Assert.Equal(500m, carteira.SaldoBrl);
        Assert.Equal(100_000L, carteira.SaldoSatoshis);
        Assert.Equal(TipoTransacao.Compra, transacao.Tipo);
    }

    [Fact]
    public void Comprar_UsaCotacaoAtual()
    {
        var carteira = new Carteira(1000m);
        var cotacao = new Cotacao(Preco);
        var corretora = new Corretora(carteira, cotacao);

        cotacao.Atualizar(1_000_000m);
        corretora.Comprar(500m);

        Assert.Equal(50_000L, carteira.SaldoSatoshis);
    }

    [Fact]
    public void Comprar_ComSaldoInsuficiente_LancaExcecaoESaldosNaoMudam()
    {
        var carteira = new Carteira(100m);
        var corretora = new Corretora(carteira, new Cotacao(Preco));

        Assert.Throws<InvalidOperationException>(() => corretora.Comprar(200m));
        Assert.Equal(100m, carteira.SaldoBrl);
        Assert.Equal(0L, carteira.SaldoSatoshis);
    }

    [Fact]
    public void Comprar_ComValorZero_LancaExcecao()
    {
        var corretora = new Corretora(new Carteira(100m), new Cotacao(Preco));

        Assert.Throws<ArgumentException>(() => corretora.Comprar(0m));
    }

    [Fact]
    public void Vender_ComSaldoSuficiente_DebitaSatoshisECreditaBrl()
    {
        var carteira = new Carteira(0m, 200_000L);
        var corretora = new Corretora(carteira, new Cotacao(Preco));

        var transacao = corretora.Vender(100_000L);

        Assert.Equal(100_000L, carteira.SaldoSatoshis);
        Assert.Equal(500m, carteira.SaldoBrl);
        Assert.Equal(TipoTransacao.Venda, transacao.Tipo);
    }

    [Fact]
    public void Vender_ComSaldoInsuficiente_LancaExcecaoESaldosNaoMudam()
    {
        var carteira = new Carteira(0m, 1_000L);
        var corretora = new Corretora(carteira, new Cotacao(Preco));

        Assert.Throws<InvalidOperationException>(() => corretora.Vender(5_000L));
        Assert.Equal(1_000L, carteira.SaldoSatoshis);
        Assert.Equal(0m, carteira.SaldoBrl);
    }
}