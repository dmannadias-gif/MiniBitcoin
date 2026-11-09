namespace MiniBitcoin.Dominio;

public class Cotacao
{
    public decimal PrecoBtcEmBrl { get; private set; }

    public Cotacao(decimal precoInicial)
    {
        Atualizar(precoInicial);
    }

    public void Atualizar(decimal novoPreco)
    {
        if (novoPreco <= 0)
            throw new ArgumentException("A cotação deve ser maior que zero.", nameof(novoPreco));
        PrecoBtcEmBrl = novoPreco;
    }
}