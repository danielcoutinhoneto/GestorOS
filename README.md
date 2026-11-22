# 🛠️ GestorOS

**Aplicação web para gerenciamento de clientes e ordens de serviço.**

O GestorOS é um projeto em desenvolvimento criado para praticar a construção de aplicações com **ASP.NET Core MVC** e **Entity Framework Core**. Atualmente, a aplicação está configurada para usar **PostgreSQL** e também pode ser adaptada para **SQL Server**. Seu objetivo é reunir informações de clientes e organizar o acompanhamento de serviços em uma interface simples.

> 🚧 **Status:** projeto em desenvolvimento, com gerenciamento de clientes, categorias e ordens de serviço.

---

## 📌 Sobre o projeto

O GestorOS foi desenvolvido como projeto de aprendizado e portfólio, aplicando conceitos de desenvolvimento web com .NET, arquitetura MVC, persistência de dados e validação de formulários.

A aplicação permite gerenciar clientes, categorias e ordens de serviço, além de acompanhar indicadores e consultar as ordens associadas a cada cliente.

### 🎬 Origem e evolução

O projeto foi baseado em uma [playlist de videoaulas no YouTube](https://youtube.com/playlist?list=PLG_9Iucxssf8&si=-dXmir95SsFr1uxk), criada por **Crislaine D'Paula**. Agradeço à autora pelo conteúdo que serviu de base para o aprendizado e para a construção inicial da aplicação.

- [LinkedIn de Crislaine D'Paula](https://www.linkedin.com/in/crislaine-luana-15a2222b0/)
- [Canal CrisTech20 no YouTube](https://www.youtube.com/@CrisTech20)

A proposta é continuar evoluindo o GestorOS com melhorias e funcionalidades além das apresentadas na playlist, explorando novos desafios e aprofundando a prática de desenvolvimento com .NET.

## ✨ Funcionalidades

### 👥 Clientes

- Cadastro de clientes.
- Listagem com busca pelo nome.
- Consulta dos dados e das ordens associadas ao cliente.
- Edição e exclusão de clientes.
- Validação de nome, e-mail e telefone.
- Impedimento da exclusão de clientes que possuem ordens associadas.

### 📋 Ordens de serviço

A estrutura de dados inclui:

- Descrição e observações.
- Datas de abertura e conclusão.
- Valor do serviço.
- Cliente e categoria associados.
- Status: aberta, em andamento, concluída ou cancelada.

A aplicação permite cadastrar, listar, filtrar, editar, excluir, consultar e imprimir ordens, além de alterar seu status. A apresentação do status utiliza textos e cores para facilitar a identificação.

### 🗂️ Categorias

É possível gerenciar categorias de serviço. O banco também possui uma carga inicial com **Manutenção**, **Instalação** e **Consultoria**.

## 🧰 Tecnologias utilizadas

| Tecnologia | Utilização |
| --- | --- |
| **C# e .NET 10** | Linguagem e plataforma da aplicação |
| **ASP.NET Core MVC** | Organização dos controladores, modelos e views |
| **Entity Framework Core** | Acesso aos dados e migrations |
| **PostgreSQL e Npgsql** | Banco de dados relacional e provedor para Entity Framework Core |
| **SQL Server** | Alternativa de banco de dados, mediante adaptação da configuração e das consultas |
| **Razor** | Renderização das páginas |
| **Bootstrap 5** | Componentes e estrutura visual |
| **Font Awesome** | Ícones da interface |
| **HTML e CSS** | Estrutura e estilos das páginas |

## 🏗️ Organização do projeto

```text
GestorOS/
├── Controllers/       # Controladores e ações da aplicação
├── Data/              # Contexto e configuração do Entity Framework Core
├── Helpers/           # Métodos auxiliares de apresentação
├── Migrations/        # Histórico de alterações do banco de dados
├── Models/            # Entidades, enumerações e validações
├── Properties/        # Perfis de execução
├── Views/             # Páginas Razor
├── wwwroot/           # Arquivos estáticos, como CSS e JavaScript
├── appsettings.json   # Configurações gerais
└── Program.cs         # Inicialização e configuração da aplicação
```

## 🚀 Como executar localmente

### Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0).
- PostgreSQL 17, instalado localmente ou executado em Docker, para a configuração atual; ou SQL Server, caso adapte o projeto para esse provedor.
- Um editor, como Visual Studio ou Visual Studio Code.
- Ferramenta de linha de comando `dotnet-ef`.

### 1. Clone o repositório

```bash
git clone https://github.com/danielcoutinhoneto/GestorOS.git
cd GestorOS
```

### 2. Restaure as dependências

```bash
dotnet restore
```

### 3. Configure a conexão com o banco

O projeto usa **User Secrets** para a conexão local em ambiente de desenvolvimento. Escolha uma das opções abaixo conforme o banco utilizado.

No Visual Studio, clique com o botão direito no projeto **GestorOS** e selecione **Gerenciar Segredos do Usuário**. No arquivo aberto, configure a conexão.

#### PostgreSQL — configuração atual

```json
{
  "ConnectionStrings": {
    "GestorOsConnection": "Host=localhost;Port=5432;Database=GestorOS;Username=postgres;Password=SUA_SENHA"
  }
}
```

Substitua os valores pelos dados do seu PostgreSQL. O nome do banco deve corresponder exatamente ao existente no servidor, incluindo letras maiúsculas e minúsculas. Com a aplicação no computador e o PostgreSQL em um contêiner com a porta 5432 publicada, use `localhost` como host.

#### SQL Server — configuração alternativa

Para uma instância local com autenticação integrada do Windows, o formato da conexão é:

```json
{
  "ConnectionStrings": {
    "GestorOsConnection": "Server=SEU_SERVIDOR;Database=GestorOS;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
  }
}
```

Substitua `SEU_SERVIDOR` pelo endereço da instância. Se usar `SERVIDOR\INSTANCIA`, escreva `SERVIDOR\\INSTANCIA` no JSON. Ajuste a autenticação e o certificado ao seu ambiente; `TrustServerCertificate=True` é um exemplo para desenvolvimento local.

**Trocar apenas a string de conexão não é suficiente.** Para usar SQL Server, também é necessário:

- Em `Program.cs`, configurar o contexto com `UseSqlServer(...)` no lugar de `UseNpgsql(...)`. O pacote do provedor SQL Server já está referenciado no projeto.
- Adaptar as consultas que usam `EF.Functions.ILike` e `EF.Functions.Unaccent`, pois são específicas do PostgreSQL. Escolha uma collation do SQL Server adequada para buscas sem distinção de maiúsculas e acentos.
- Criar e manter migrations próprias para SQL Server. As migrations atuais foram geradas para PostgreSQL e não devem ser aplicadas diretamente ao SQL Server. Revise a estratégia de migrations antes de aplicá-las em um banco existente.

Essas adaptações não estão implementadas na configuração atual; os passos seguintes descrevem o uso com PostgreSQL.

O valor em `appsettings.json` é apenas uma orientação e não é uma conexão válida. Não publique senhas ou conexões reais no repositório.

> 🔐 User Secrets é destinado ao desenvolvimento local e não criptografa os valores. Em produção, utilize o mecanismo de gerenciamento de segredos do ambiente de hospedagem.

### 4. Prepare o banco PostgreSQL

Se ainda não tiver a ferramenta `dotnet-ef`, instale uma versão compatível:

```bash
dotnet tool install --global dotnet-ef --version 10.0.15
```

Se ela já estiver instalada, atualize-a, se necessário:

```bash
dotnet tool update --global dotnet-ef --version 10.0.15
```

Verifique se há alterações no modelo ainda não registradas em uma migration:

```bash
dotnet ef migrations has-pending-model-changes --project GestorOS/GestorOS.csproj
```

Se houver alterações pendentes, crie e revise uma migration antes de continuar:

```bash
dotnet ef migrations add AtualizarModelo --project GestorOS/GestorOS.csproj
```

Em seguida, aplique as migrations:

```bash
dotnet ef database update --project GestorOS/GestorOS.csproj
```

As migrations existentes criam as tabelas e cadastram as categorias iniciais. Se o banco ainda não existir, o usuário precisa ter permissão para criá-lo; caso contrário, crie-o previamente. Em um trabalho colaborativo, compartilhe as migrations geradas junto com as alterações do modelo.

As buscas de clientes usam a extensão PostgreSQL `unaccent` para ignorar acentos, em conjunto com `ILIKE` para ignorar diferenças entre maiúsculas e minúsculas. Habilite-a **no banco configurado na conexão**, por exemplo, executando no cliente SQL:

```sql
CREATE EXTENSION IF NOT EXISTS unaccent;
```

Para um contêiner chamado `postgres_db`, com usuário `postgres` e banco `GestorOS`, execute:

```powershell
docker exec -it postgres_db psql -U postgres -d GestorOS -c "CREATE EXTENSION IF NOT EXISTS unaccent;"
docker exec -it postgres_db psql -U postgres -d GestorOS -c "SELECT unaccent('João');"
```

O resultado do teste deve ser `Joao`. Ajuste o nome do contêiner, usuário e banco conforme sua instalação. O usuário precisa ter permissão para criar a extensão; repita o procedimento em cada banco utilizado, inclusive quando um banco for recriado.

As datas de abertura e conclusão usam UTC e são armazenadas em colunas `timestamp with time zone`. Para apresentar um horário local, converta-o para o fuso desejado; as variáveis `TZ` e `PGTZ` do contêiner não substituem essa conversão na aplicação.

### 5. Execute a aplicação

```bash
dotnet run --project GestorOS/GestorOS.csproj --launch-profile https
```

Abra o endereço HTTPS informado no terminal. Na configuração atual, o perfil de execução utiliza o ambiente `Development` e carrega os User Secrets.

## 🗺️ Próximos passos

- Evoluir o gerenciamento de ordens de serviço e o painel inicial.
- Implementar controle de acesso e autenticação.
- Criar perfis de acesso com permissões adequadas para cada usuário.
- Implementar o envio de cobranças por e-mail e WhatsApp.
- Explorar outras funcionalidades e melhorias conforme a evolução do projeto.
- Manter as migrations sincronizadas com as alterações dos modelos.
- Adicionar testes para os principais fluxos.

## 👨‍💻 Autor

Desenvolvido por [Daniel Coutinho Neto](https://github.com/danielcoutinhoneto).

[LinkedIn](https://www.linkedin.com/in/daniel-coutinho-neto/)

Projeto criado para aprendizado, prática e evolução no desenvolvimento com .NET.
