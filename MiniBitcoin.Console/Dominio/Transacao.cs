namespace MiniBitcoin.Dominio;

public enum TipoTransacao
{
    Compra,
    Venda
}

public record Transacao(
    TipoTransacao Tipo,
    long Satoshis,
    decimal ValorBrl,
    decimal PrecoBtc,
    DateTime Data);