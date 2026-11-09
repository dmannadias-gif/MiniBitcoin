# MiniBitcoin

Aplicação de console em .NET que simula uma carteira de Bitcoin: saldo em BRL e em satoshis, cotação atualizável, compra e venda, extrato e patrimônio consolidado. Desenvolvida com TDD (xUnit) e Conventional Commits.

## Arquitetura

```
MiniBitcoin/
├── MiniBitcoin.Console/
│   ├── Dominio/
│   │   ├── Carteira.cs    # Saldos BRL/satoshis e validações (sem saldo negativo)
│   │   ├── Cotacao.cs     # Preço atual do BTC em BRL (atualizável)
│   │   ├── Corretora.cs   # Regras de compra e venda com base na cotação
│   │   ├── Transacao.cs   # Registro imutável de cada operação
│   │   └── Extrato.cs     # Histórico de operações e cálculo de patrimônio
│   └── Program.cs         # Menu interativo
└── MiniBitcoin.Tests/     # Testes unitários (xUnit)
```

Decisões:
- BRL usa `decimal` e BTC é armazenado em satoshis (`long`, 1 BTC = 100.000.000 sats) para evitar erros de ponto flutuante.
- A compra arredonda os satoshis para baixo; a venda trunca o valor em BRL em 2 casas.
- Operações inválidas lançam `ArgumentException` (valor inválido) ou `InvalidOperationException` (saldo insuficiente), sem alterar os saldos.

## Como executar

Pré-requisito: .NET SDK 8 ou superior.

```bash
git clone <URL_DO_REPOSITORIO>
cd MiniBitcoin
dotnet test
dotnet run --project MiniBitcoin.Console/MiniBitcoin.Console.csproj
```

A carteira começa com R$ 10.000,00 e cotação de R$ 350.000,00 por BTC. Use vírgula para decimais.