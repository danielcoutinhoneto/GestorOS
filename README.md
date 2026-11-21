# 🛠️ GestorOS

**Aplicação web para gerenciamento de clientes e ordens de serviço.**

O GestorOS é um projeto em desenvolvimento criado para praticar a construção de aplicações com **ASP.NET Core MVC**, **Entity Framework Core** e **SQL Server**. Seu objetivo é reunir informações de clientes e organizar o acompanhamento de serviços em uma interface simples.

> 🚧 **Status:** projeto em desenvolvimento. O gerenciamento de clientes está implementado; os módulos de ordens de serviço e categorias ainda estão em construção.

---

## 📌 Sobre o projeto

O GestorOS foi desenvolvido como projeto de aprendizado e portfólio, aplicando conceitos de desenvolvimento web com .NET, arquitetura MVC, persistência de dados e validação de formulários.

A aplicação já permite gerenciar clientes e consultar as ordens associadas a cada um. A estrutura de dados para ordens de serviço e categorias também foi criada e servirá de base para as próximas funcionalidades.

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

A apresentação do status utiliza textos e cores para facilitar a identificação. As telas e operações de gerenciamento de ordens ainda estão em desenvolvimento.

### 🗂️ Categorias

O banco possui uma estrutura para categorias de serviço e uma carga inicial com **Manutenção**, **Instalação** e **Consultoria**. As telas de gerenciamento de categorias ainda estão em desenvolvimento.

## 🧰 Tecnologias utilizadas

| Tecnologia | Utilização |
| --- | --- |
| **C# e .NET 10** | Linguagem e plataforma da aplicação |
| **ASP.NET Core MVC** | Organização dos controladores, modelos e views |
| **Entity Framework Core** | Acesso aos dados e migrations |
| **SQL Server** | Banco de dados relacional |
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
- SQL Server ou SQL Server Express.
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

O projeto usa **User Secrets** para a conexão local em ambiente de desenvolvimento.

No Visual Studio, clique com o botão direito no projeto **GestorOS** e selecione **Gerenciar Segredos do Usuário**. No arquivo aberto, configure a conexão:

```json
{
  "ConnectionStrings": {
    "GestorOsConnection": "Server=SEU_SERVIDOR;Database=GestorOS;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
  }
}
```

Substitua `SEU_SERVIDOR` pelo endereço ou nome da sua instância do SQL Server. Se a instância for informada no formato `SERVIDOR\INSTANCIA`, use duas barras invertidas no JSON: `SERVIDOR\\INSTANCIA`.

O valor em `appsettings.json` é apenas uma orientação e não é uma conexão válida. Não publique senhas ou conexões reais no repositório.

> 🔐 User Secrets é destinado ao desenvolvimento local e não criptografa os valores. Em produção, utilize o mecanismo de gerenciamento de segredos do ambiente de hospedagem.

### 4. Prepare o banco de dados

Se ainda não tiver a ferramenta `dotnet-ef`, instale uma versão compatível:

```bash
dotnet tool install --global dotnet-ef --version 10.0.15
```

Se ela já estiver instalada, atualize-a, se necessário:

```bash
dotnet tool update --global dotnet-ef --version 10.0.15
```

> ⚠️ O modelo atual possui alterações ainda não registradas em uma migration. Antes de atualizar o banco, é necessário criar e revisar uma nova migration. Isso gera arquivos no projeto; em um trabalho colaborativo, normalmente a migration é criada e versionada pelo responsável pela alteração do modelo.

```bash
dotnet ef migrations add AtualizarModeloCliente --project GestorOS/GestorOS.csproj
dotnet ef database update --project GestorOS/GestorOS.csproj
```

A migration inicial cria as tabelas e cadastra as categorias iniciais.

### 5. Execute a aplicação

```bash
dotnet run --project GestorOS/GestorOS.csproj --launch-profile https
```

Abra o endereço HTTPS informado no terminal. Na configuração atual, o perfil de execução utiliza o ambiente `Development` e carrega os User Secrets.

## 🗺️ Próximos passos

- Implementar o gerenciamento de ordens de serviço.
- Implementar as telas de categorias de serviço.
- Evoluir a página inicial da aplicação.
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
