# Plano de Desenvolvimento - Sistema de Chat em Tempo Real: Whispr

## Objetivo do Projeto

Desenvolver um **chat em tempo real** robusto, utilizando Blazor (ASP.NET) e .NET 8+, com hospedagem na Azure, que permita a troca de mensagens de texto, imagens e áudios entre usuários logados. O sistema deve incorporar funcionalidades modernas como comandos interativos (ex: comando `/stock` para cotações), moderação de conteúdo e suporte a login social (Google OAuth2). Além de atender aos requisitos especificados, este é um projeto de longo prazo, cobrindo boas práticas de desenvolvimento, segurança e DevOps ao longo de aproximadamente um ano.

## Requisitos Funcionais

1.  **Cadastro e Login de Usuários**  
    O sistema deve permitir o cadastro de usuários com email e senha, utilizando a infraestrutura do ASP.NET Core Identity para gerenciamento de credenciais. Deve-se implementar também autenticação via **Google OAuth2** para oferecer login social, aproveitando os provedores externos suportados pelo Identity (que incluem Google, Facebook, Microsoft, etc.) ([Introduction to Identity on ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0#:~:text=Users%20can%20create%20an%20account,Google%2C%20Microsoft%20Account%2C%20and%20Twitter)). Funções de recuperação de senha (reset via email) e confirmação de email devem ser incluídas, garantindo que usuários possam recuperar acesso de forma segura caso esqueçam sua senha. A utilização do Identity fornece abstrações prontas para contas, roles e tokens, agilizando o desenvolvimento dessas funcionalidades.
    
2.  **Interface de Chat**  
    Após login, os usuários acessam uma sala de **chat público** (um “lobby” visível a todos os usuários autenticados). A interface deve exibir mensagens em tempo real, sem necessidade de refresh manual. Para isso, será utilizado **SignalR**, a biblioteca de comunicação em tempo real da Microsoft. O SignalR permite ao servidor enviar atualizações instantâneas aos clientes via WebSockets, eliminando a necessidade de polling contínuo ([What is Azure SignalR Service? | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-signalr/signalr-overview#:~:text=Azure%20SignalR%20Service%20simplifies%20the,new%20HTTP%20requests%20for%20updates)) ([What is Azure SignalR Service? | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-signalr/signalr-overview#:~:text=instant%20sales%20updates%2C%20multi,email%2C%20games%2C%20and%20travel%20alert)). Cada mensagem exibirá o nome e avatar do remetente para fácil identificação de quem enviou. Os usuários poderão enviar:
    
    -   **Texto:** mensagens tradicionais digitadas.
        
    -   **Imagens:** com pré-visualização inline no chat (thumbnail ou tamanho reduzido). O upload de imagens será armazenado em nuvem (Azure Blob Storage) e exibido na UI do chat.
        
    -   **Áudios:** gravações curtas de áudio, com um player embutido para reprodução. Semelhante às imagens, os arquivos de áudio serão armazenados em Blob Storage e referenciados na conversa.
    -   **Reações:** As mensagens nos chats podem ser reagidas pelos outros usuários no chat (além do autor).
    
    A atualização em tempo real das mensagens será gerenciada pelo SignalR Hub no backend .NET, garantindo que quando um usuário envia uma mensagem, todos os outros vejam-na quase imediatamente. A Microsoft fornece um tutorial oficial cobrindo a integração de SignalR em apps Blazor e construindo um chat básico ([Use ASP.NET Core SignalR with Blazor | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/tutorials/signalr-blazor?view=aspnetcore-9.0#:~:text=Learn%20how%20to%3A)), o que valida a viabilidade dessa abordagem neste projeto.
    
3.  **Comandos Especiais (/stock)**  
    O chat terá suporte a comandos de barra (similar ao Slack/Discord) para funcionalidades extras. Em particular, será implementado o comando **`/stock CODIGO_ACAO`**, onde ao digitar esse comando o servidor fará uma chamada a uma API externa de mercado financeiro para obter o preço atual da ação cujo código foi fornecido. O resultado será retornado como uma mensagem especial no chat, visível a todos os usuários. Importante: essas mensagens geradas por comandos **não devem ser persistidas no banco de dados** (são informações dinâmicas e voláteis). Deve haver um mecanismo no backend para interceptar mensagens começando com `/` antes de salvá-las, tratar o comando (no caso, consultar a API de stocks) e só então enviar o resultado aos clientes via SignalR, sem efetuar insert na tabela de mensagens. Esse design garante que o histórico armazenado não fique poluído com respostas de comandos automatizados.
    
4.  **Moderação de Conteúdo**  
    Para manter um ambiente saudável:
    
    -   Haverá um **filtro de palavras ofensivas**: uma lista negra configurável de termos proibidos. Se um usuário enviar mensagem contendo palavrões ou termos banidos, a mensagem pode ser bloqueada ou substituída (e.g. por asteriscos). Essa lista negra será personalizável pelo administrador.
        
    -   **Validação de uploads**: ao enviar imagens ou áudios, o sistema deve validar o tipo de arquivo (MIME type e extensão) para impedir conteúdo não suportado ou potencialmente malicioso. Apenas formatos específicos serão permitidos (por exemplo, JPEG/PNG para imagens; MP3/ WAV para áudio). Também será definido um limite de tamanho para cada tipo (ex: imagem até 2 MB, áudio até 5 MB) – excedendo isso, o upload é rejeitado. Essas práticas seguem as recomendações de segurança da Microsoft, como permitir somente extensões aprovadas e limitar tamanho de upload ([Upload files in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-9.0#:~:text=,limit%20to%20prevent%20large%20uploads)).
        
    -   **Serviço de moderação da Azure** (opcional): Avalia-se integrar o **Azure Content Moderator** ou o novo **Azure AI Content Safety** para análise automática de conteúdo de texto e imagem. O Content Moderator (um serviço de IA da Azure) é capaz de identificar conteúdo impróprio em texto (como insultos, palavrões) e imagens (conteúdo adulto ou violento) ([Quickstart: Use the Content Moderator client library - Azure AI services | Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-services/content-moderator/client-libraries#:~:text=Content%20Moderator%20is%20an%20AI,intended%20environment%20for%20your%20users)). Observação: O Azure Content Moderator está sendo substituído gradualmente pelo Azure AI Content Safety, mais avançado ([Quickstart: Use the Content Moderator client library - Azure AI services | Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-services/content-moderator/client-libraries#:~:text=Important)). Se viável dentro do escopo do projeto, poderíamos usar esses serviços cloud para uma moderação mais robusta, caso contrário o filtro básico por lista negra e validação de arquivos atenderá inicialmente.
        
5.  **Painel Administrativo (Mínimo Viável)**  
    Será desenvolvido um painel simples para administradores (acessível apenas por usuários com papel _Admin_). Funcionalidades previstas:
    
    -   **Gerenciamento de Usuários:** lista de usuários cadastrados, com opção de banir/desbanir usuários que violem regras. O banimento pode ser implementado marcando um flag no perfil do usuário que impede login/envio de mensagens.
        
    -   **Logs de Comandos:** histórico das execuções de comandos especiais (como o `/stock`), registrando quem solicitou, o código buscado e o resultado retornado, com carimbo de data/hora. Isso ajuda a monitorar abuso de comandos.
        
    -   **Visão de Moderacão:** exibir registros de mensagens bloqueadas pelo filtro de palavras ou pelo serviço de moderação (para que o admin revise se necessário).
        
    
    O painel admin será implementado como uma página Blazor restrita a administradores (ver **Segurança** abaixo sobre roles). Funcionalidades futuras poderiam incluir mais opções (como editar lista de palavras proibidas, gerar relatórios, etc.), mas inicialmente o foco é fornecer controle básico.
    

## Requisitos Não Funcionais

1.  **Plataforma – Blazor Server ou Blazor WebAssembly?**  
    O projeto será desenvolvido em Blazor, mas é importante decidir o modelo de hospedagem: **Server** ou **WebAssembly (WASM)**. Ambos permitem construir a UI em componentes Razor reutilizáveis, porém há diferenças arquiteturais importantes ([ASP.NET Core Blazor hosting models | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-9.0#:~:text=that%20can%20be%20hosted%20in,of%20the%20hosting%20models%20unchanged)) ([ASP.NET Core Blazor hosting models | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-9.0#:~:text=Blazor%20Server)):
    
    ([ASP.NET Core Blazor hosting models | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-9.0)) _Arquitetura Blazor Server — a interface (Razor Components) roda no servidor ASP.NET Core e sincroniza com o DOM do navegador via conexão SignalR._ Em **Blazor Server**, a aplicação executa no servidor (sobre .NET Core), enviando atualizações de UI e eventos ao cliente através de uma conexão SignalR em tempo real ([ASP.NET Core Blazor hosting models | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-9.0#:~:text=With%20the%20Blazor%20Server%20hosting,when%20the%20connection%20is%20lost)). As vantagens incluem carregamento inicial muito rápido (o cliente baixa apenas um pequeno script) e uso pleno do poder de processamento do servidor (o cliente pode ser um device modesto) ([ASP.NET Core Blazor hosting models | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-9.0#:~:text=The%20Blazor%20Server%20hosting%20model,offers%20several%20benefits)). Além disso, o código C# não é exposto ao usuário, permanecendo no servidor. Por outro lado, a latência de interações depende da rede (cada evento de UI é enviado ao server) e a app não funciona offline (se a conexão cair, a interação é interrompida) ([ASP.NET Core Blazor hosting models | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-9.0#:~:text=The%20Blazor%20Server%20hosting%20model,has%20the%20following%20limitations)). Também é necessário escalonar o servidor para muitos usuários, pois cada cliente conectado mantém um circuito ativo consumindo recursos no host. A própria Microsoft recomenda usar o **Azure SignalR Service** em apps Blazor Server de grande porte, para suportar um elevado número de conexões simultâneas com confiabilidade ([ASP.NET Core Blazor hosting models | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-9.0#:~:text=We%20recommend%20using%20the%20Azure,number%20of%20concurrent%20SignalR%20connections)).
    
    ([ASP.NET Core Blazor hosting models | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-9.0)) _Figura: Arquitetura Blazor WebAssembly — os componentes e a runtime .NET são baixados e executados no navegador do cliente, interagindo com o DOM localmente._ Em **Blazor WebAssembly**, toda a aplicação (assemblies .NET, runtime e UI) é carregada no navegador e executada client-side via WebAssembly. As vantagens são: após carregado, o app pode funcionar **mesmo que o servidor caia ou perca conexão**, já que a lógica está no cliente ([ASP.NET Core Blazor hosting models | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-9.0#:~:text=The%20Blazor%20WebAssembly%20hosting%20model,offers%20several%20benefits)); reduz carga do servidor (a CPU do cliente executa a maior parte do trabalho) e permite até cenários offline/PWA. Também não requer servidor .NET constante – pode ser hospedado como arquivos estáticos em um CDN, por exemplo. Em contrapartida, o tamanho do download inicial é bem maior e requer um navegador moderno e hardware capaz de rodar WebAssembly ([ASP.NET Core Blazor hosting models | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-9.0#:~:text=,inspection%20and%20tampering%20by%20users)). A inicialização pode ser lenta em conexões lentas devido ao payload. Além disso, como o código roda no cliente, há questões de segurança: é fácil inspecionar o código compilado, então segredos devem permanecer no servidor (via APIs).
    
    **Recomendação:** para este projeto, optamos inicialmente pelo **Blazor Server**. A razão é a simplicidade para um desenvolvedor iniciante – toda a lógica (UI e backend) roda unificada no ASP.NET Core, facilitando debug e uso de recursos do .NET sem restrições do sandbox do browser. O tempo de carregamento será menor, proporcionando melhor UX no primeiro acesso. Como teremos funcionalidades em tempo real (SignalR) de qualquer forma, o modelo Server se encaixa bem. Contudo, devemos estar cientes das limitações: será preciso escalar o servidor conforme a base de usuários cresce. Futuramente, poderíamos avaliar migrar para Blazor WASM + API caso desejemos suporte offline ou aliviar carga do servidor. No curto prazo, Blazor Server atende plenamente aos requisitos, e podemos mitigar problemas de escala usando o serviço gerenciado Azure SignalR (discutido adiante) e um plano de hospedagem adequado.
    
2.  **Backend – .NET 8+**  
    O projeto usará a versão mais recente do .NET (no mínimo .NET 8). O .NET 8 traz melhorias de performance e recursos atualizados para web (como melhorias no SignalR, EF Core 8, etc.), além de suporte de longo prazo (LTS). Usar a versão atual garante longevidade do projeto e compatibilidade com as últimas bibliotecas. O backend será estruturado seguindo boas práticas do ASP.NET Core, com _Dependency Injection_ configurando serviços como o Hub do SignalR, contextos de banco (EF Core), serviços de autenticação, etc. O uso de .NET também permite escrever testes de unidade e integração para partes críticas (por exemplo, testar o filtro de palavras, o serviço de comandos `/stock`, etc.).
    
3.  **Serviços em Nuvem (Azure)**  
    Para aproveitar o ecossistema Azure e garantir escalabilidade, o sistema integrará vários serviços cloud:
    
    -   **Azure Blob Storage:** armazenamento de mídias (imagens e áudios) de forma segura e escalável. Cada upload de arquivo no chat será salvo em um contêiner de Blob Storage, retornando uma URL acessível para download/leitura. Blobs são ideais para armazenar arquivos binários e oferecem baixo custo por GB. A integração no .NET é feita via SDK do Azure Storage – é possível enviar um stream diretamente para um container ([Upload a blob with .NET - Azure Storage | Microsoft Learn](https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blob-upload#:~:text=This%20article%20shows%20how%20to,upload%20large%20blobs%20in%20blocks)). Será criada uma conta de armazenamento com um container, e possivelmente usadas URLs SAS ou tokens de acesso para controlar permissões de leitura. Assim, as mídias não sobrecarregam o servidor web e podem ser servidas via CDN/Blob diretamente aos clientes.
        
    -   **Azure SignalR Service:** considerando o potencial crescimento do número de usuários conectados simultaneamente no chat, avalia-se usar o serviço do Azure SignalR. Esse serviço gerenciado atua como **backplane** para hubs SignalR, permitindo escalar para _milhares ou milhões de conexões simultâneas_ sem que o servidor web tenha que gerenciar cada socket individualmente ([What is Azure SignalR Service? | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-signalr/signalr-overview#:~:text=connections,Azure%27s%20standard%20compliance%20and%20security)). Em modo **Azure SignalR** integrado, nosso hub SignalR do chat pode ser configurado para usar o serviço como intermediário (as chamadas em tempo real passam pelo Azure SignalR, que cuida da distribuição para os clientes). Isso alivia o App Service de manter conexões WebSocket longas e melhora a resiliência. Como resultado, obteremos comunicação em tempo real altamente escalável, o que é crucial para aplicações de chat de larga escala.
        
    -   **Banco de Dados – Azure PostgreSQL SQL e mongo DB para não relacional:** para persistir os dados do sistema (usuários, mensagens, logs, etc.)
        
    -   **Azure App Service:** será o host da aplicação web (Blazor + APIs). O App Service fornece um ambiente gerenciado para rodar aplicações ASP.NET Core, com suporte integrado a escalonamento, slots de implantação e integração contínua. Podemos optar por um Plano do App Service em Linux para hospedar a aplicação .NET 8. O App Service simplifica a gestão de infraestrutura: a MS cuida de atualizar o SO, aplicar patches de segurança e manter o ambiente, para que possamos focar no código ([Overview of Azure App Service - Azure App Service | Microsoft Learn](https://learn.microsoft.com/en-us/azure/app-service/overview#:~:text=Azure%20App%20Service%20is%20an,based%20environments)) ([Overview of Azure App Service - Azure App Service | Microsoft Learn](https://learn.microsoft.com/en-us/azure/app-service/overview#:~:text=App%20Service%20adds%20the%20power,custom%20domains%2C%20and%20TLS%2FSSL%20certificates)). A publicação da aplicação poderá ser feita via CI/CD (GitHub Actions) automaticamente para o App Service, facilitando o deploy de novas versões.
        
    -   **Azure Content Moderator / AI Content Safety:** conforme mencionado nos requisitos funcionais, caso usemos moderação de conteúdo por IA, teremos um recurso do Azure AI configurado. Isso envolveria criar um recurso do Content Moderator (ou Content Safety) no portal Azure, obter chave e endpoint, e usar o SDK NuGet em nosso projeto. As mensagens e imagens dos usuários poderiam ser enviadas para a API de moderação antes de serem distribuídas, e o resultado (flag de aprovada ou rejeitada) determinaria se a mensagem entra no chat. **Obs:** dado que o uso desse serviço implica custos adicionais e complexidade (chamadas a API externa para cada mensagem), talvez inicialmente o mantenhamos como “futuro incremento” e com uso pontual (ex: admin pode rodar moderação sobre mensagens denunciadas).
        
4.  **Performance e Escalabilidade**  
    Mesmo com poucos usuários no início, é importante projetar pensando em crescimento e eficiência:
    
    -   **Cache com Redis:** Implementaremos um cache em memória distribuído para dados temporários e não críticos. Por exemplo, os resultados do comando `/stock` poderiam ser cacheados por alguns minutos (para evitar chamar a API externa repetidamente para o mesmo código). Outra aplicação: armazenar em cache listas de palavrões ou configurações carregadas do banco, reduzindo consultas frequentes. Para isso, podemos usar o **Azure Cache for Redis**, um serviço gerenciado compatível com Redis que oferece latência baixíssima e alta taxa de transferência ([Azure Cache for Redis Documentation - Azure Cache for Redis | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-cache-for-redis/#:~:text=Azure%20Cache%20for%20Redis%20is,newest%20Redis%20offering%20on%20Azure)). O Azure Cache for Redis atua como um repositório chave-valor in-memory, ótimo para cenários de sessão, cache de conteúdo e pub/sub. Integrar o StackExchange.Redis (cliente .NET) no projeto permitirá ler/escrever facilmente no cache. O uso de cache melhora a responsividade e alivia carga do banco de dados.
        
    -   **Compressão e Otimização de Mídias:** Quando usuários fizerem upload de imagens ou áudios, o servidor pode aplicar compressão para otimizar o tamanho antes de salvar. Por exemplo, reduzir a resolução ou qualidade JPEG de imagens muito grandes, ou converter áudio para um formato comprimido (MP3) se vier em WAV. Isso economiza espaço no Blob Storage e acelera a entrega aos clientes. Bibliotecas como _ImageSharp_ (para imagens) podem ser usadas. Além disso, habilitaremos compressão HTTP nas respostas do servidor (o ASP.NET pode comprimir JSON, etc., embora para WebSockets do SignalR isso não se aplique).
        
    -   **Limites de Tamanho e Rate Limiting:** Conforme citado, serão impostos limites de tamanho de upload (ex.: imagens até 2 MB, áudios 5 MB). O front-end pode avisar isso, mas a validação no servidor é essencial. Limitar tamanho previne abuso e consumo excessivo de banda/storage ([Upload files in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-9.0#:~:text=,before%20the%20file%20is%20stored)). Também podemos introduzir **rate limiting** para certos recursos – por exemplo, evitar que um usuário envie dezenas de mensagens por segundo (para prevenir spam ou ataques DoS). O .NET 8 possui middleware de rate limiting que podemos configurar para o hub ou endpoints específicos.
        
    -   **Escalabilidade Horizontal:** No futuro, se a carga crescer, poderemos escalar a aplicação para múltiplas instâncias do App Service. Graças ao Azure SignalR Service (se habilitado), o estado em memória de cada instância fica sincronizado via backplane, permitindo que usuários conectados a instâncias diferentes ainda troquem mensagens normalmente. Também o banco Azure SQL pode ser escalonado mudando o tier (mais DTUs) ou migrando para uma instância de escala horizontal (Managed Instance ou sharding). O design com serviços desacoplados (App, DB, Redis, SignalR service) segue princípios de **cloud scalability**, onde cada componente pode ser dimensionado independentemente conforme o gargalo observado.
        
5.  **Segurança**  
    Em um sistema de chat multiusuário, vários vetores de segurança precisam de atenção:
    
    -   **Autenticação e Autorização:** Usaremos Identity para autenticação (login) e podemos definir **roles** para diferenciar usuários comuns de administradores. O acesso ao painel admin será protegido por `[Authorize(Roles="Admin")]`. As APIs de comandos ou endpoints sensíveis também checarão autorização. Além disso, todas as páginas de chat exigem usuário logado (podemos aplicar `[Authorize]` globalmente no hub e nas páginas, redirecionando não logados para login).
        
    -   **Proteção contra XSS:** Cross-site scripting é mitigado pelo próprio Blazor, pois o output de variáveis em Razor é automaticamente codificado em HTML. Se um usuário enviar uma mensagem contendo, por exemplo, `<script>alert(1)</script>`, ao exibirmos essa mensagem na interface ela aparecerá como texto literal, não executando código, já que o Blazor renderiza conteúdo de forma segura (escape de HTML) ([Threat mitigation guidance for ASP.NET Core Blazor interactive server-side rendering | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/interactive-server-side-rendering?view=aspnetcore-9.0#:~:text=match%20at%20L687%20,the%20content%20as%20static%20text)). Devemos tomar cuidado de não introduzir manualmente nenhum `MarkupString` não sanitizado. Também evitar usar `@bind` em partes da UI que possam ser manipuladas maliciosamente sem validação. Adicionalmente, implementar o filtro de palavrões e/ou moderação ajuda a evitar que conteúdo ofensivo (inclusive possíveis ataques XSS escondidos em inputs) circule livremente. Podemos considerar políticas de Content Security Policy (CSP) strict no cabeçalho para adicionar camadas de defesa, embora não seja tão crítico em apps Blazor por padrão.
        
    -   **Proteção contra CSRF:** Em Blazor Server, a comunicação é via SignalR (WebSocket), o que já mitiga CSRF em grande parte (não há requisições POST tradicionais que um site externo possa forjar facilmente). Porém, teremos endpoints para upload de arquivos e talvez APIs REST para algumas funções – nesses casos, devemos usar tokens anti-forgery. O ASP.NET Core já suporta antiforgery em formulários e podemos habilitar isso nos uploads (ex.: incluir `ValidateAntiForgeryToken` nos controllers que recebem upload, e garantir que o Blazor ou JS enviem o token). Além disso, configurar o CORS apropriadamente para não permitir que scripts externos chamem nossa API indevidamente.
        
    -   **Validação de Arquivos e Dados:** Qualquer conteúdo recebido do cliente será validado. Arquivos: checar extensão e MIME (como citado), talvez verificar “magic number” dentro do arquivo para confirmar que uma imagem é realmente imagem e não um executável renomeado ([How to protect Net.Core server from malicious content uploads by ...](https://learn.microsoft.com/en-us/answers/questions/1379033/how-to-protect-net-core-server-from-malicious-cont#:~:text=How%20to%20protect%20Net,File%20Size%20Limit)). Mensagens de texto: delimitar tamanho máximo (evitar payloads gigantes), filtrar caracteres estranhos. Também prevenir SQL Injection usando sempre parâmetros (no EF isso já é padrão).
        
    -   **Criptografia e SSL:** Todo o tráfego será sob HTTPS (o App Service já oferece certificado *.azurewebsites.net por default). Dados sensíveis como senhas nunca trafegam em claro (o Identity as hashes). Para armazenamento, podemos habilitar Transparent Data Encryption no Azure SQL e criptografia nos containers de Blob. Backups e dados no Redis também ficam em memória ou criptografados em disco gerenciado pela Azure.
        
    -   **Auditoria e Logs de Segurança:** Registros de logins, falhas de login, tentativas de ações não autorizadas devem ser gravados (ex: via ILogger). Essas informações podem ser enviadas ao Application Insights e/ou ao Azure Monitor para futura auditoria de incidentes.
        
6.  **Boas Práticas de DevOps**  
    Desde cedo no projeto, vamos adotar práticas de DevOps para garantir entregas confiáveis e rastreáveis:
    
    -   **CI/CD com GitHub Actions:** O repositório do código (provavelmente GitHub) terá pipelines de integração contínua e entrega contínua configurados. A cada push ou merge na branch principal, o pipeline irá: compilar e rodar testes; se bem-sucedido, construir a imagem/artifact e implantar no Azure App Service automaticamente ([Deploying .NET to Azure App Service - GitHub Docs](https://docs.github.com/en/actions/use-cases-and-examples/deploying/deploying-net-to-azure-app-service#:~:text=Deploying%20,Service)). Isso garante que novas funcionalidades cheguem rapidamente ao ambiente de teste ou produção. O Actions também pode executar workflows para linting, scanning de vulnerabilidades (usando ferramentas SAST) e outras tarefas de qualidade.
        
    -   **Infraestrutura como Código:** Para padronizar e facilitar a recriação do ambiente, usaremos IaC para os recursos Azure. Podemos escolher **Bicep** (linguagem declarativa nativa do Azure) ou **Terraform** (ferramenta multiplataforma de IaC) conforme familiaridade. Bicep, por exemplo, permitiria descrever em arquivos `.bicep` todos os recursos necessários – Conta de Armazenamento, App Service, Azure SQL, SignalR Service, etc. – e implantar via CLI. Bicep simplifica bastante a definição de recursos Azure em comparação a JSON bruto ([Bicep documentation | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/#:~:text=Bicep%20is%20a%20language%20for,ARM%20templates)). Já Terraform tem a vantagem de ser multicloud e muito usada – poderíamos usar módulos oficiais para Azure. Independentemente da escolha, versionar a infraestrutura junto do código ajuda a manter consistência entre ambientes (dev/prod) e documentar configurações.
        
    -   **Monitoramento e Logs:** Integrar o **Azure Application Insights** para monitorar a aplicação em tempo real. Com o Application Insights, podemos coletar telemetria de requisições, métricas de desempenho, logs customizados e exceções do aplicativo ([Application Insights for ASP.NET Core applications - Azure Monitor | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core#:~:text=Application%20Insights%20can%20collect%20the,NET%20Core%20application)). Configuraremos o SDK do App Insights no projeto .NET para automaticamente rastrear dependências (como chamadas ao Azure SQL, Blob, APIs externas) e tempos de resposta. Ter visibilidade das métricas (número de usuários online, latência de mensagens, taxa de erros) é fundamental tanto para depurar problemas quanto para dimensionar recursos corretamente. Além de Application Insights, usaremos logs estruturados (ILogger) gravando eventos importantes (ex: "Usuário X enviou imagem Y", "Erro ao conectar API stock") – esses logs podem ser consultados no App Insights ou no Log Stream do App Service. Também podemos definir alertas na Azure (ex: alerta se CPU do App Service > 80% por 10 min, ou se taxa de erros SignalR sobe além de X) para agir proativamente.
        
    -   **Backups e Recovery:** Configurar backups automáticos do Azure SQL Database (ponto-in-time restore já é padrão, mas garantir política de retenção adequada). Para Blob Storage, ativar Soft Delete para blobs, assim se um arquivo for apagado por engano, é possível recuperar dentro de um período. Documentar procedimentos de recuperação (DR) caso seja necessário restaurar dados.
        
    -   **Testes Automatizados e Quality Gates:** À medida que o projeto cresce, incluir testes unitários (por exemplo, testar a função de filtro de palavrões com vários inputs) e testes de integração (ex: simular uma troca de mensagens pelo SignalR hub). Integrar esses testes no pipeline CI para evitar regressões. Podemos também usar ferramentas de análise estática (StyleCop, SonarQube) para manter padronização de código. Para um desenvolvedor júnior, isso introduz contato com práticas profissionais de qualidade.
        

## Roadmap de Implementação (por Trimestre)

Para organizar o desenvolvimento ao longo de um ano, o roadmap será dividido em **4 trimestres**, iniciando pelo núcleo funcional e gradativamente adicionando recursos avançados, melhorias e práticas DevOps.

### 1º Trimestre: Fundamentos e MVP do Chat

-   **Setup do Projeto:** Configuração inicial do repositório, solução Blazor Server (.NET 8) criada, integração com Azure DevOps/GitHub (repo, boards) e configuração básica do pipeline CI/CD.
    
-   **Autenticação Básica:** Implementar cadastro/login via Identity (email e senha) e páginas de login/registro no Blazor. Garantir que somente usuários autenticados acessam a página de chat.
    
-   **Chat de Texto em Tempo Real:** Desenvolver o hub SignalR e a página Blazor do chat público. Permitir enviar e receber mensagens de texto instantaneamente entre múltiplos usuários. Nesta fase inicial, armazenar as mensagens em memória ou banco simples (Azure SQL) para persistência básica do histórico.
    
-   **Interface Simples:** Design mínimo da UI mostrando mensagens com nome do usuário e timestamp. Foco na funcionalidade em tempo real mais do que em estilo neste momento.
    
-   **Deploy Inicial:** Fazer deploy do MVP no Azure App Service ao final do trimestre. Validar que várias pessoas conseguem conectar e conversar em tempo real. Ajustar quaisquer problemas de conexão (CORS, websockets, etc.). Esse primeiro deploy serve para aprendizado de publicação e para ter um ambiente de teste acessível.
    

### 2º Trimestre: Recursos de Mídia e Aprimoramentos de UI

-   **Envio de Imagens:** Implementar upload de imagens no chat. Integrar com Azure Blob Storage – ao selecionar uma imagem, o arquivo é enviado ao servidor, validado e salvo no Blob; então uma mensagem com link/preview é enviada no chat. Renderizar a imagem inline na lista de mensagens (tag `<img>` com URL do blob SAS).
    
-   **Envio de Áudios:** Similar às imagens, permitir gravar ou enviar arquivo de áudio. Implementar um pequeno player de áudio nas mensagens (por exemplo, usando elemento `<audio>` HTML5).
    
-   **Melhorias de UI/UX:** Estilizar o chat usando CSS (ou frameworks como Bootstrap). Diferenciar visualmente mensagens do usuário atual vs de outros. Mostrar avatar (foto do perfil ou um ícone com iniciais) ao lado das mensagens. Adicionar notificações sonoras ou visuais quando novas mensagens chegam (especialmente se o usuário estiver em outra aba).
    
-   **Login Social (Google):** Configurar autenticação OAuth2 com Google. Registrar a aplicação no Google Developers Console, obter Client ID/Secret e adicionar no Identity. Permitir que usuários se cadastrem/entrem usando sua conta Google, facilitando o onboarding.
    
-   **Paginação ou Scroll do Histórico:** Se o histórico de mensagens crescer, implementar carregamento progressivo (lazy load) ao rolar a tela para cima, para não sobrecarregar o cliente carregando milhares de mensagens de uma vez.
    
-   **Teste e Feedback:** Conduzir testes com um pequeno grupo de usuários finais para coletar feedback de usabilidade. Ajustar detalhes de layout, desempenho no front-end (por exemplo, otimizar tamanho das imagens enviadas) e quaisquer bugs encontrados. Garantir que o app se comporta bem em diferentes navegadores e dispositivos (desktop/mobile).
    

### 3º Trimestre: Comandos Interativos e Moderação

-   **Comando `/stock`:** Desenvolver o mecanismo de comandos de barra. Implementar especificamente o `/stock`: criar um serviço que consulte uma API financeira (por exemplo, Alpha Vantage, Yahoo Finance ou outra disponível) dado um código de ação. Ao digitar o comando, chamar esse serviço e retornar uma mensagem formatada com o nome da ação, preço atual, variação, etc. Não armazenar essa mensagem no banco (pular lógica de persistência no hub para mensagens que iniciam com `/`). Logar o uso do comando (quem solicitou, código, sucesso/erro).
    
-   **Sistema de Plugins de Comando:** Embora o foco seja o comando de stock, estruturar o código de forma que novos comandos possam ser adicionados facilmente (por exemplo, `/weather`, `/gif`, etc., no futuro). Possivelmente ter uma interface `IChatCommand` e registrar vários comandos.
    
-   **Filtro de Palavras Ofensivas:** Implementar o filtro de texto utilizando uma lista de termos proibidos. Essa lista pode residir em configurações ou tabela no banco para fácil manutenção. Na pipeline de processamento de mensagem (antes de broadcastear via SignalR), verificar se a mensagem contém algo proibido. Se sim, decidir se bloqueia completamente (não envia) ou sanitiza (substituir letras por *, por exemplo). Também registrar um log interno de mensagens bloqueadas.
    
-   **Moderação Automática (texto/imagem):** Explorar a integração com Azure Content Moderator (ou Content Safety). Talvez implementar de forma limitada: por exemplo, verificar texto das mensagens com a API de moderação da Azure _somente_ se a mensagem passou pelo filtro básico (para não chamar API para cada mensagem comum). Avaliar desempenho e custos nessa POC de moderação. Para imagens, usar o Content Moderator para detectar nudez ou violência (o serviço retorna flags para imagens). As mensagens/imagens que forem marcadas como impróprias podem ser removidas antes de chegar aos outros usuários, e o sistema notifica o usuário remetente sobre a violação.
    
-   **Painel Admin Inicial:** Construir a página administrativa listando usuários e permitindo banir/desbanir. Banir poderia simplesmente marcar um campo “IsBanned” no usuário; ajustar o processo de login para negar acesso se usuário banido. Fornecer opção de filtrar/ordenar usuários, e talvez indicar quantas mensagens enviou, data de registro, último login, etc., para ajudar o admin.
    
-   **Logs de Comando no Admin:** No painel admin, adicionar uma seção mostrando os registros de uso de comandos (especialmente `/stock`). Assim, o admin pode monitorar se alguém está abusando (e.g., chamando `/stock` em excesso) ou se houve erros nas integrações de API externas.
    
-   **Aprimoramentos de Segurança:** Revisar a aplicação usando alguma checklist de segurança. Por exemplo, testar manualmente XSS (inserindo scripts), testar upload de arquivo com extensão trocada, tentar floodar mensagens (talvez implementar um limite de mensagens por minuto por usuário). Endurecer configurações conforme necessário (como limitar tamanho de requests no Kestrel, configurar appropriate HTTP headers via middleware de Segurança - e.g. CSP, X-Content-Type-Options, etc.).
    

### 4º Trimestre: Escalabilidade, Desempenho e Extras

-   **Azure SignalR Service:** Se o chat já tiver um número significativo de usuários simultâneos nos testes, configurar o projeto para usar o Azure SignalR Service em produção. Isso envolve provisionar o serviço no Azure e ajustar a configuração do SignalR no ASP.NET Core (UseAzureSignalR). Realizar testes de carga simulada para ver o comportamento (usando, por exemplo, Azure Load Testing ou scripts JMeter/Selenium para múltiplas conexões).
    
-   **Escala do Banco de Dados:** Avaliar o desempenho do Azure SQL com o volume de dados coletado ao longo do ano (usuários, mensagens). Ajustar o tier do banco, índices nas tabelas de mensagens (por exemplo, índice por data para ordenar histórico, etc.). Se necessário, archivar mensagens antigas para não prejudicar consultas recentes – poderíamos mover para outra tabela ou blob as mensagens com mais de X meses.
    
-   **Otimização de Código e Consulta:** Perfil de performance do backend – usar Application Insights ou profiler para identificar métodos lentos. Otimizar o hub de chat (por exemplo, evitar operações desnecessárias a cada mensagem). Verificar uso de Async adequadamente em chamadas de I/O (como blob, db). No front-end, analisar o tamanho do WASM (se fosse WASM) ou a latência dos updates (no Server) e melhorar onde possível.
    
-   **Polimento de Funcionalidades:** Implementar pequenas melhorias solicitadas pelos usuários ou identificadas pelo desenvolvedor, por exemplo:
    
    -   Possibilidade de **editar ou deletar** uma mensagem enviada (e propagar atualização a todos via SignalR).
        
    -   Indicação de "**usuário digitando...**" no chat – usar SignalR para notificar quando alguém está escrevendo (evento onInput).
        
    -   Suporte a **múltiplas salas/canais**: permitir criar salas de chat separadas além do lobby público, e usuários se juntarem a elas. (Isso poderia ser um grande recurso extra, mas factível usando grupos do SignalR).
        
    -   Notificações push ou emails para menções: ex, se alguém menciona @Fulano, enviar notificação se Fulano estiver offline.
        
-   **Teste de Usuários Beta:** Liberar o sistema para um grupo maior de teste (por exemplo, colegas da empresa/escola do desenvolvedor). Coletar feedback abrangente sobre usabilidade, detectar bugs em cenários não previstos. Monitorar pelo App Insights o uso real – número de conexões, memória consumida, etc. – para validar a robustez.
    
-   **Documentação Final:** Escrever documentação de todo o sistema: arquitetura, decisões técnicas, como rodar o ambiente de dev, como fazer deploy, etc. Incluir também um README para o repositório e possivelmente uma pequena **Wiki** com tópicos (por exemplo: “Como adicionar um novo comando?”, “Como moderar usuários?”). Isso consolida o aprendizado e ajuda em futuras manutenções ou handoff.
    
-   **Preparação para o Futuro:** Criar backlog de possíveis próximos passos além do ano 1, caso o desenvolvedor queira continuar evoluindo o projeto (ver seção de Extras a seguir). Avaliar também a saúde do código: refatorar partes confusas, remover código morto, aumentar cobertura de testes. Garantir que o projeto esteja bem estruturado para suportar novas funcionalidades.
    

## Extras e Dicas

### Melhorias Futuras (Extras)

-   **Chats Privados ou Grupos:** Permitir que usuários iniciem conversas privadas (1 a 1) ou criem grupos fechados. Isso envolveria criar salas dinâmicas com apenas determinados membros e possivelmente implementar criptografia ponta-a-ponta para privacidade.
    
-   **Mais Comandos e Bots:** Adicionar novos comandos interativos, como `@bot traduzir <texto>` para traduzir texto, `/weather <cidade>` para clima, ou integrar um bot de FAQ. Esses comandos podem tornar o chat mais divertido e útil, e exercitar integrações com outras APIs.
    
-   **Reações e Menções:** Habilitar reações tipo “like” ou emojis nas mensagens. Implementar menção de usuário (@usuário) que notifica a pessoa mencionada. São recursos comuns em chats modernos.
    
-   **Aplicativo Mobile:** Usar o mesmo backend SignalR para criar um app móvel (talvez usando .NET MAUI ou Flutter) para o chat, permitindo notificações push de novas mensagens. Como alternativa, transformar o Blazor em uma PWA instalável no celular, com suporte offline básico.
    
-   **Suporte a Vídeos ou Streaming:** Expandir o envio de mídia para vídeos curtos ou até streaming ao vivo (mais complexo). Isso exigiria processamento e talvez uso de serviços como Azure Media Services para codificação.
    
-   **Escala Global:** Se pertinente, explorar o uso do Cosmos DB para distribuir dados globalmente e do Azure Front Door/CDN para servir conteúdo estático com menor latência mundial. Esse seria um passo além se o chat tivesse usuários em diferentes continentes.
    
-   **Telemetria de Uso Avançada:** Integrar análise de uso com Power BI ou Azure Monitor Workbooks para visualizar métricas como mensagens por dia, usuários ativos, etc. Isso daria insights para evoluir o produto.
    

### Dicas para o Desenvolvedor

-   **Aprenda com a Documentação Oficial:** Consulte frequentemente a documentação da Microsoft (MS Learn, docs oficiais do Azure e .NET) – muitas soluções e exemplos estão disponíveis (como vimos para Identity, SignalR, etc.). Por exemplo, se tiver dúvidas de como algo funciona, busque por “Microsoft docs [tecnologia]”. A leitura da documentação ajuda a consolidar boas práticas.
    
-   **Evolução Gradual:** Construa o projeto de forma iterativa. Comece pelo básico (MVP de chat texto) e vá incrementando aos poucos. Cada nova funcionalidade deve ser testada e integrada antes de partir para a próxima. Isso evita ficar sobrecarregado e facilita identificar bugs introduzidos recentemente.
    
-   **Boas Práticas de Código:** Mantenha o código organizado – use nomes claros para classes e métodos, escreva comentários em trechos complexos, e evite duplicação (princípio DRY). Separe responsabilidades (ex: lógica de comando em uma classe de serviço, lógica de armazenamento em outro). Isso deixará o código mais legível e facilitará manutenção.
    
-   **Controle de Versão e Histórico:** Faça commits frequentes no Git com mensagens descritivas do que foi feito (“Implementa upload de imagem no chat”, “Corrige bug de login nulo”, etc.). Assim, você mantém um histórico claro e pode reverter algo se der problema. Use branches para desenvolver funcionalidades isoladamente (feature branches) e depois mergear na main via pull requests – mesmo em projeto solo, isso simula um fluxo profissional e te obriga a revisar o código antes de integrar.
    
-   **Testes e Depuração:** Pratique escrever testes unitários para partes lógicas (por exemplo, passe vários textos para o filtro de palavrões e confira se o resultado é esperado). Isso te dá mais confiança ao alterar código no futuro. Ao depurar, use logs e o debugger do Visual Studio/VS Code para inspecionar o estado da aplicação. Ferramentas como Postman ou a própria interface do browser dev (F12) ajudam a testar as APIs e SignalR.
    
-   **Performance e Escalabilidade:** Mesmo que no início poucos usuários usem, tente pensar em como o app se comportaria com muitos. Habitue-se a considerar complexidade de algoritmos (um loop dentro de outro pode ser ok para 100 itens, mas não para 100k). Use cache onde fizer sentido. E monitore a aplicação – por exemplo, ative o Application Insights desde cedo para ter noção de tempos de resposta e erros acontecendo.
    
-   **Não hesite em ajustar o curso:** Este plano é uma diretriz, mas durante o desenvolvimento você aprenderá novas coisas e talvez encontre maneiras melhores de implementar algo. Tudo bem refatorar ou alterar planos conforme ganha conhecimento. Parte do objetivo é **aprender** – então aproveite para experimentar, e caso algo não dê certo, reflita, pesquise (StackOverflow, docs) e tente novamente.
    
-   **Comunidade e Suporte:** Busque apoio na comunidade – fóruns como Stack Overflow, os repositórios de exemplo da Microsoft no GitHub, e comunidades locais (.NET São Paulo, por exemplo). Muitas vezes, dúvidas que você terá já foram respondidas por outros. Só cuidado para não copiar código sem entender; use como referência e adapte ao seu contexto.
    
-   **Documente o Processo:** Além da documentação técnica do projeto, mantenha um diário ou blog de aprendizado (mesmo que pessoal). Anote os desafios que enfrentou e como resolveu. Isso não só reforça seu aprendizado como gera um material que pode ajudar outros no futuro (e serve de portfólio do seu trajeto de desenvolvedor).