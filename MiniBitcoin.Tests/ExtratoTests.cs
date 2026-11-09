using MiniBitcoin.Dominio;

namespace MiniBitcoin.Tests;

public class ExtratoTests
{
    [Fact]
    public void NovoExtrato_ComecaVazio()
    {
        var extrato = new Extrato();

        Assert.Empty(extrato.Transacoes);
    }

    [Fact]
    public void Registrar_CompraEVenda_MantemOrdem()
    {
        var carteira = new Carteira(1000m);
        var corretora = new Corretora(carteira, new Cotacao(500_000m));
        var extrato = new Extrato();

        extrato.Registrar(corretora.Comprar(500m));
        extrato.Registrar(corretora.Vender(50_000L));

        Assert.Equal(2, extrato.Transacoes.Count);
        Assert.Equal(TipoTransacao.Compra, extrato.Transacoes[0].Tipo);
        Assert.Equal(TipoTransacao.Venda, extrato.Transacoes[1].Tipo);
    }

    [Fact]
    public void Registrar_TransacaoNula_LancaExcecao()
    {
        var extrato = new Extrato();

        Assert.Throws<ArgumentNullException>(() => extrato.Registrar(null!));
    }

    [Fact]
    public void CalcularPatrimonio_SomaBrlEBtcConvertido()
    {
        var carteira = new Carteira(1000m, 50_000_000L);
        var cotacao = new Cotacao(400_000m);

        var patrimonio = Extrato.CalcularPatrimonio(carteira, cotacao);

        Assert.Equal(201_000m, patrimonio);
    }

    [Fact]
    public void CalcularPatrimonio_AposCompra_MantemValorTotal()
    {
        var carteira = new Carteira(1000m);
        var cotacao = new Cotacao(500_000m);
        new Corretora(carteira, cotacao).Comprar(500m);

        Assert.Equal(1000m, Extrato.CalcularPatrimonio(carteira, cotacao));
    }
}