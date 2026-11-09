using MiniBitcoin.Dominio;

namespace MiniBitcoin.Tests;

public class CarteiraTests
{
    [Fact]
    public void Construtor_ComSaldosValidos_DefineSaldos()
    {
        var carteira = new Carteira(1000m, 50_000L);

        Assert.Equal(1000m, carteira.SaldoBrl);
        Assert.Equal(50_000L, carteira.SaldoSatoshis);
    }

    [Fact]
    public void Construtor_ComBrlNegativo_LancaExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Carteira(-1m));
    }

    [Fact]
    public void Construtor_ComSatoshisNegativos_LancaExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Carteira(0m, -1L));
    }

    [Fact]
    public void DebitarBrl_ComSaldoSuficiente_ReduzSaldo()
    {
        var carteira = new Carteira(100m);

        carteira.DebitarBrl(40m);

        Assert.Equal(60m, carteira.SaldoBrl);
    }

    [Fact]
    public void DebitarBrl_ComSaldoInsuficiente_LancaExcecaoESaldoNaoMuda()
    {
        var carteira = new Carteira(100m);

        Assert.Throws<InvalidOperationException>(() => carteira.DebitarBrl(150m));
        Assert.Equal(100m, carteira.SaldoBrl);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void DebitarBrl_ComValorNaoPositivo_LancaExcecao(double valor)
    {
        var carteira = new Carteira(100m);

        Assert.Throws<ArgumentException>(() => carteira.DebitarBrl((decimal)valor));
    }

    [Fact]
    public void DebitarSatoshis_ComSaldoInsuficiente_LancaExcecao()
    {
        var carteira = new Carteira(0m, 1_000L);

        Assert.Throws<InvalidOperationException>(() => carteira.DebitarSatoshis(2_000L));
        Assert.Equal(1_000L, carteira.SaldoSatoshis);
    }

    [Fact]
    public void SaldoBtc_ConverteSatoshisParaBitcoin()
    {
        var carteira = new Carteira(0m, 150_000_000L);

        Assert.Equal(1.5m, carteira.SaldoBtc);
    }
}