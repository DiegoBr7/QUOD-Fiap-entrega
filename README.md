# Quod Antifraude

## Descrição Geral

O **Quod Antifraude** é uma solução moderna para análise e validação de documentos, com foco em detecção de fraudes, extração e validação de CPF via OCR, biometria digital e detecção facial. O sistema é composto por uma API RESTful desenvolvida em .NET 8, integrando processamento de imagens, inteligência antifraude e persistência de dados em MongoDB. Também inclui um aplicativo desktop para detecção facial utilizando EmguCV.

---

## Requisitos do Sistema

### API (.NET 8)
- .NET 8 SDK
- MongoDB (local ou remoto)
- Sistema operacional: Windows, Linux ou macOS

### Aplicativo Desktop (Detecção Facial)
- .NET Framework 4.8
- Windows 7 ou superior
- Webcam (para detecção facial em tempo real)

---

## Tecnologias Utilizadas

- **.NET 8** – Backend da API
- **.NET Framework 4.8** – Aplicativo desktop de detecção facial
- **ASP .NET Core** – Criação de APIs RESTful
- **MongoDB** – Banco de dados NoSQL
- **SixLabors.ImageSharp** – Processamento de imagens
- **Tesseract OCR** – Extração de texto de imagens
- **EmguCV** – Detecção facial (aplicativo desktop)
- **Swagger (Swashbuckle)** – Documentação interativa da API
- **Microsoft.Extensions.Options** – Configuração tipada
- **Docker** (opcional) – Para execução do MongoDB

---

## Como Rodar o Projeto

### 1. Clonar o Repositório

`git clone https://github.com/DiegoBr7/QUOD-Fiap-entrega.git`


### 2. Configurar o MongoDB

- Instale e inicie o MongoDB localmente **ou** utilize um serviço em nuvem.
- Altere a string de conexão no arquivo `appsettings.json` da API:

```
"MongoSettings": {
  "ConnectionString": "mongodb://localhost:27017",
  "DatabaseName": "QuodAntifraude"
  }
```


### 3. Compilar e Executar a API

`cd src/Quod.Antifraude.Api`
`dotnet build`
`dotnet run`


A API estará disponível em `https://localhost:5001` (ou porta configurada).

### 4. Acessar a Documentação Swagger

Abra no navegador:

`https://localhost:5001/swagger`

Teste os endpoints diretamente pela interface.

### 5. Executar o Aplicativo Desktop (Detecção Facial)

- Abra o projeto `Quod.DeteccaoRostoEmguCV` no Visual Studio 2022.
- Compile e execute (F5).
- Utilize a webcam ou abra uma imagem para testar a detecção facial.

---

## Estrutura do Projeto

src/
├── Quod.Antifraude.Api/           # API principal (.NET 8)
├── Quod.Antifraude.Core/          # Modelos e contratos
├── Quod.Antifraude.Infrastructure/# Repositórios e integração com MongoDB
├── Quod.Antifraude.Services/      # Serviços de negócio, OCR, validação, biometria
├── Quod.DeteccaoRostoEmguCV/      # Aplicativo desktop de detecção facial (.NET Framework 4.8)


---

## Funcionalidades Principais

- **Upload e análise de documentos** (extração e validação de CPF)
- **Consulta de pessoas** cadastradas via CPF
- **Processamento de imagens** para melhoria de OCR
- **Detecção facial** em imagens e webcam (aplicativo desktop)
- **Integração com serviços de notificação** (ex: envio de alertas)
- **Documentação interativa** via Swagger

---

## 👨‍💻 Autores / Contribuidores

- **Gabriel Araújo Ferraz de Melo**
- **Jonas Alves Moreira**
- **Diego Brasileiro Vilela Dias**
- **Paulo Cauê Krüger Costa**
- **Gabriel Paulucci**

  Estudantes de Análise e Desenvolvimento de Sistemas - FIAP
  GitHub: https://github.com/DiegoBr7/QUOD-Fiap-entrega
