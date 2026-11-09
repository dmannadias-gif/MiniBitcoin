using System.Globalization;
using MiniBitcoin.Dominio;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var br = CultureInfo.GetCultureInfo("pt-BR");

var carteira = new Carteira(saldoInicialBrl: 10_000m);
var cotacao = new Cotacao(precoInicial: 350_000m);
var corretora = new Corretora(carteira, cotacao);
var extrato = new Extrato();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("===== MiniBitcoin =====");
    Console.WriteLine("1 - Consultar cotação");
    Console.WriteLine("2 - Atualizar cotação");
    Console.WriteLine("3 - Ver saldos");
    Console.WriteLine("4 - Comprar Bitcoin");
    Console.WriteLine("5 - Vender Bitcoin");
    Console.WriteLine("6 - Exibir extrato");
    Console.WriteLine("0 - Sair");
    Console.Write("Opção: ");
    var opcao = Console.ReadLine();

    try
    {
        switch (opcao)
        {
            case "1":
                Console.WriteLine($"Cotação atual: {cotacao.PrecoBtcEmBrl.ToString("C", br)} por BTC");
                break;
            case "2":
                cotacao.Atualizar(LerDecimal("Novo preço do BTC em BRL: "));
                Console.WriteLine("Cotação atualizada.");
                break;
            case "3":
                Console.WriteLine($"Saldo BRL: {carteira.SaldoBrl.ToString("C", br)}");
                Console.WriteLine($"Saldo BTC: {carteira.SaldoBtc.ToString("0.00000000", br)} ({carteira.SaldoSatoshis} sats)");
                break;
            case "4":
                var compra = corretora.Comprar(LerDecimal("Valor em BRL a investir: "));
                extrato.Registrar(compra);
                Console.WriteLine($"Compra realizada: {compra.Satoshis} satoshis por {compra.ValorBrl.ToString("C", br)}.");
                break;
            case "5":
                var btc = LerDecimal("Quantidade de BTC a vender (ex.: 0,001): ");
                var venda = corretora.Vender((long)(btc * Carteira.SatoshisPorBitcoin));
                extrato.Registrar(venda);
                Console.WriteLine($"Venda realizada: {venda.Satoshis} satoshis por {venda.ValorBrl.ToString("C", br)}.");
                break;
            case "6":
                if (extrato.Transacoes.Count == 0)
                    Console.WriteLine("Nenhuma transação registrada.");
                foreach (var t in extrato.Transacoes)
                    Console.WriteLine($"{t.Data:dd/MM/yyyy HH:mm} | {t.Tipo,-6} | {t.Satoshis,12} sats | {t.ValorBrl.ToString("C", br),14} | cotação {t.PrecoBtc.ToString("C", br)}");
                Console.WriteLine($"Patrimônio total: {Extrato.CalcularPatrimonio(carteira, cotacao).ToString("C", br)}");
                break;
            case "0":
            case null:
                return;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or FormatException)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

decimal LerDecimal(string mensagem)
{
    Console.Write(mensagem);
    var entrada = Console.ReadLine();
    if (!decimal.TryParse(entrada, NumberStyles.Number, br, out var valor))
        throw new FormatException("Valor inválido. Use vírgula para decimais (ex.: 150,50).");
    return valor;
}