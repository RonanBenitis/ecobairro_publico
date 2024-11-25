<img src="_readme_assets/readme_logo.png" alt="Logo">

## Autores
- Frontend: [Henrique Moura](https://github.com/Reyralona)
- Backend e documentação: [Ronan Benitis](https://github.com/RonanBenitis)

## Controle de versionamento
Este projeto tem seu controle de versionamento em um repositório privado. Por conta de conteúdo sigiloso (API Key), optou-se por disponibilizar esta versão como privada publicizando um exemplo de .env.

Mantem-se o repositório principal como privado para preservar o controle de versionamento.

## Sobre o projeto
O **EcoBairro** é um **aplicativo mobile para de gestão comunitária de lixo e entulho** desenvolvido para ajudar a comunidade a reportar e solucionar problemas de descarte irregular de lixo e entulho, com apoio da Prefeitura. Através do aplicativo, moradores podem indicar locais que necessitam de limpeza, e outros cidadão podem "adotar" essas tarefas, contribuindo para um bairro mais limpo e organizado. O objetivo é promover a conscientização ambiental, o engajamento comunitário e o incentivo ao voluntariado, tornando as cidades mais limpas e colaborativas.

## Aviso importante: Inicialização do servidor
O servidor da aplicação está hospedado no serviço de cloud **Render**. Caso o servidor não seja utilizado por alguns minutos, este é desligado e reinicializado quando alguma requisição é feita. Por conta deste processo, a primeira consulta ao servidor, quando desligado, **levará algum tempo para ser concluída (poderá alcançar 5 minutos de aguardo)**.

Por isso, caso venha a utilizar a aplicação após um tempo sem consultas no servidor e a aplicação está tomando um tempo considerável para fazer operações que envolvam consultas (como login, criar pin ou afins), não se espante. Possivelmente será o servidor que está inicializando. Uma vez concluida a consulta, o servidor estará ligado e as demais operações serão rápidas.

## Tecnologias Utilizadas
<table>
  <thead align="center">
    <tr border: none;>
      <td><b>BACKEND</b></td>
      <td><b>FRONTEND</b></td>
      <td><b>BANCO DE DADOS</b></td>
    </tr>
  </thead>
  <tbody align="center">
    <tr>
      <td>
        <img alt="" src="https://img.shields.io/badge/dotnet-sdk8.0-black?style=for-the-badge&logo=dotnet&logoColor=white&labelColor=blue&color=black"/>
        <img alt="" src="https://img.shields.io/badge/Docker-V23.0.3-Black?style=for-the-badge&logo=Docker&logoColor=%23ffffff&labelColor=%230538b3&color=%23000000"/>
        <img alt="" src="https://img.shields.io/badge/Render-Black?style=for-the-badge&logo=Render&logoColor=%23ffffff&labelColor=000000&color=%23000000"/>
      </td>
      <td>
        <img alt="" src="https://img.shields.io/badge/node-v20.16-black?style=for-the-badge&logo=node.js&logoColor=white&labelColor=green&color=black"/>
        <img alt="" src="https://img.shields.io/badge/React_Native-V0.76-Black?style=for-the-badge&logo=React&logoColor=%23ffffff&labelColor=%234ad5ff&color=%23000000"/>
        <img alt="" src="https://img.shields.io/badge/Expo-SDK51.0-Black?style=for-the-badge&logo=Expo&logoColor=%23ffffff&labelColor=%23525252&color=%23000000"/>
      </td>
      <td>
        <img alt="" src="https://img.shields.io/badge/sqlite-black?style=for-the-badge&logo=sqlite&logoColor=white&labelColor=%236cb2e4&color=%236cb2e4"/>
      </td>
    </tr>
  </tbody>
</table>

## Rodando a aplicação
*Os comandos neste README estão no padrão CMD*

- Clone o respositório em sua máquina
  - **Muita atenção:** o caminho do diretório local onde o repositório será clonado **não poderá conter espaços**, e cuidado em ter caminhos muito longos. Isso poderá impactar no momento de inicializar o `expo`.
  - Ou seja, se o projeto estiver dentro da pasta "`Nova Pasta`", sendo seu caminho completo "C:\Nova Pasta\ecobairro", o `expo` não conseguirá rodar, pois este buscará suas dependencias na pasta "`C:\Nova`" e não em "`C:\Nova Pasta`", já que interpreta-se espaços como separadores.
```bash
git clone https://github.com/RonanBenitis/ecobairro.git
```

- Navegue até o diretório `apk`
  - Todos os comandos desta seção devem ser utilizados na pasta raiz da aplicação (**diretório apk**)
```bash
cd ecobairro
cd apk
```

- Instalando as dependências do projeto:
```bash
npm install
```

Para teste da aplicação, recomenda-se a utilização da ferramenta **Expo**, seja para rodá-lo via **Expo Go** - aplicativo do Expo que possibilita rodar esta aplicação em seu celular - ou via **Android Studio**

### Opção 1: Rodando em rede
- Comando para rodar pela rede no aplicativo Expo Go ou Android Studio
```bash
npx expo start -c
```

### Opção 2: Rodando em túnel
- Comando para rodar via USB no aplicativo Expo Go
```bash
npx expo start -c --tunnel
```

## Credenciais de usuário
Existem dois tipos de usuário:
- Municipe
  - Usuário que representa o morador do bairro
  - Único que pode iniciar um pin (ordem de serviço)
- Fiscal
  - Usuário que representa o responsável da Prefeitura para acompanhamentos dos casos
  - Único que pode aprovar o pin (ordem de serviço)
  - Para concluir o pin, o único que pode realizar essa ação **é o fiscal que está acompanhando o pin em específico**

A aplicação inicia com um acesso de exemplo pra cada tipo, na tela de login, utilize as seguintes credenciais para realizar os testes:
- Municipe exemplo
  - Usuário: municipe@email.com
  - Senha: senha123
- Fiscal exemplo:
  - Usuário: fiscal@email.com
  - Senha: senha123

## Documentação das APIs
A documentação das APIs (endpoints) foi realizada através de **Swagger** em ambiente de desenvolvimento, desativando-a no ambiente de produção.

Para conferência assincrona - desconectada do ambiente de produção - armazenamos as ultimas atualizações da documentação Swagger no SwaggerHub e pode ser acessado seguinte link:
- [Link para a documentação das APIs](https://app.swaggerhub.com/apis/ronanbenitis/ecobairro/v0#/)

### Endereço base das rotas da API
Este é o endereço utilizado pela aplicação para a consulta e manipulação dos dados pela a aplicação. Este endereço não faz parte da configuração do ambiente, apenas para base das chamadas à API.
- https://ecobairro.onrender.com/

> Para utilizar das rotas da API, acrescente o endpoint (cumprindo suas exigências) ao endereço base.
> - Exemplo para buscar todos os fiscais:
>   - (Método GET) https://ecobairro.onrender.com/api/fiscal
